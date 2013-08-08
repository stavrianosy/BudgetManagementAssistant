using BMA.BusinessLogic;
using BMA_WP.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace BMA_WP.Model
{
        public class ServiceData
        {
            #region Enumerators
            public enum ServerStatus
            {
                Communicating,
                Ok,
                Error
            }
            #endregion

            #region Constant
            const string TRANSACTIONS_FOLDER = "Transactions";
            const string TRANSACTIONIMAGES_FOLDER = "TransactionImagess";
            const string BUDGETS_FOLDER = "Budgets";
            #endregion

            #region Private Memebrs
            static readonly string Utf8ByteOrderMark =
            Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble(), 0, Encoding.UTF8.GetPreamble().Length);

            string latestState;
            #endregion

            #region Public Proeprties
            public TransactionImageList TransactionImageList { get; set; }
            public TransactionList TransactionList { get; set; }
            public BudgetList BudgetList { get; set; }
            #endregion

            #region Constructors
            public ServiceData()
            {
                TransactionList = new TransactionList();
                TransactionImageList = new TransactionImageList();
                BudgetList = new BudgetList();
            }

            #endregion

            #region Events
            #endregion

            #region Private Methods
            #region Load Live Data
            private void LoadLiveTransactions(Action<Exception> callback)
            {
                LoadLiveTransactions(0, (error) => callback(error));
            }

            private void LoadLiveTransactions(int budgetId, Action<Exception> callback)
            {
                try
                {
                    var client = new BMAService.MainClient();
                    if (budgetId == 0)
                    {
                        latestState = Guid.NewGuid().ToString();

                        client.GetLatestTransactionsCompleted += (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                SetupTransactionList(completedEventArgs.Result, true);
                                callback(null);
                            }
                            else
                                callback(completedEventArgs.Error);
                        };
                        client.GetLatestTransactionsAsync(App.Instance.User.UserId, latestState);
                    }
                    else
                    {
                        //client.GetTransactionsForBudgetCompleted += new EventHandler<BMAService.GetTransactionsForBudgetCompletedEventArgs>(client_GetTransactionsForBudgetCompleted);
                        client.GetTransactionsForBudgetAsync(budgetId);
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message);
                    throw;
                }
            }

            private void LoadLiveBudgets(Action<Exception> callback)
            {
                try
                {
                    latestState = Guid.NewGuid().ToString();
                    var client = new BMAService.MainClient();
                    client.GetAllBudgetsCompleted += (sender, e) =>
                        {
                            if(e.Error == null)
                                SetupBudgetList(e.Result, true);

                            callback(e.Error);
                        };
                    client.GetAllBudgetsAsync(App.Instance.User.UserId);
                }
                catch (Exception ex)
                {
                    var msg = string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message);
                    throw;
                }
            }

            private void LoadLiveTransactionImages(int transactionId)
            {
                try
                {
                    var client = new BMAService.MainClient();
                    latestState = Guid.NewGuid().ToString();

                    client.GetImagesForTransactionCompleted += (sender, completedEventArgs) =>
                    {
                        SetupTransactionImageList(completedEventArgs.Result, transactionId);

                        //UpdateCacheTransactionImages();
                    };
                    client.GetImagesForTransactionAsync(transactionId, latestState);
                }
                catch (Exception ex)
                {
                    var msg = string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message);
                    throw;
                }
            }
            #endregion

            #region Load Cached Data
            public async void LoadCachedTransactions(Action<TransactionList, Exception> callback)
            {
                var retVal = new TransactionList();
                try
                {
                    foreach (var item in await StorageUtility.ListItems(TRANSACTIONS_FOLDER, App.Instance.User.UserId))
                    {
                        var trans = await StorageUtility.RestoreItem<Transaction>(TRANSACTIONS_FOLDER, item, App.Instance.User.UserId);
                        //retVal.Add(trans);
                        TransactionList.Add(trans);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    //callback(null, ex);
                }

                TransactionList.OrderByDescending(x => x.TransactionDate);
                //SetupTransactionList(retVal, false, false);

                callback(TransactionList, null);
            }

            private async void LoadCachedTransactionImages(Action<TransactionImageList, Exception> callback)
            {
                var retVal = new TransactionImageList();
                try
                {
                    foreach (var item in await StorageUtility.ListItems(TRANSACTIONIMAGES_FOLDER, App.Instance.User.UserId))
                    {
                        var trans = await StorageUtility.RestoreItem<TransactionImage>(TRANSACTIONIMAGES_FOLDER, item, App.Instance.User.UserId);
                        //retVal.Add(trans);
                        TransactionImageList.Add(trans);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    //callback(null, ex);
                }
                
                //SetupTransactionImageList(retVal);

                callback(TransactionImageList, null);
            }

            private async void LoadCachedBudgets(Action<BudgetList, Exception> callback)
            {
                var retVal = new BudgetList();
                try
                {
                    foreach (var item in await StorageUtility.ListItems(BUDGETS_FOLDER, App.Instance.User.UserId))
                    {
                        var budget = await StorageUtility.RestoreItem<Budget>(BUDGETS_FOLDER, item, App.Instance.User.UserId);
                        //retVal.Add(budget);
                        BudgetList.Add(budget);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    //callback(null, ex);
                }

                //SetupBudgetList(retVal, false);
                BudgetList.OrderByDescending(x => x.Name);

                callback(BudgetList, null);
            }
            #endregion

            private async void SetupTransactionList(ICollection<Transaction> existing, bool removeNew)
            {
                existing = existing ?? new TransactionList();

                //sync logic
                if (removeNew)
                    RemoveInsertedTransactions();

                foreach (var item in existing)
                {
                    var query = TransactionList.Select((x, i) => new { trans = x, Index = i }).Where(x => x.trans.TransactionId == item.TransactionId).FirstOrDefault();

                    //INSERT
                    if (query == null)
                        TransactionList.Add(item);
                    //UPDATE
                    else
                        TransactionList[query.Index] = item;

                    //if(updateCache)
                        await StorageUtility.SaveItem(TRANSACTIONS_FOLDER, item, item.TransactionId, App.Instance.User.UserId);
                }
                TransactionList.OrderByDescending(x => x.TransactionDate);
            }

            private async void SetupTransactionImageList(ICollection<TransactionImage> existing, int transactionId)
            {
                existing = existing ?? new TransactionImageList();

                TransactionImageList.Clear();
                var trans = TransactionList.FirstOrDefault(x => x.TransactionId == transactionId);

                if (trans != null)
                    trans.TransactionImages = new TransactionImageList();

                foreach (var item in existing)
                    trans.TransactionImages.Add(item);

                trans.HasChanges = false;

                await StorageUtility.SaveItem<Transaction>(TRANSACTIONS_FOLDER, trans, trans.TransactionId, App.Instance.User.UserId);
            }

            private async void SetupBudgetList(ICollection<Budget> existing, bool removeNew)
            {
                existing = existing ?? new ObservableCollection<Budget>();

                //sync logic
                if (removeNew)
                    RemoveInsertedTransactions();

                foreach (var item in existing)
                {
                    var query = BudgetList.Select((x, i) => new { budget = x, Index = i }).Where(x => x.budget.BudgetId == item.BudgetId).FirstOrDefault();

                    //INSERT
                    if (query == null)
                        BudgetList.Add(item);
                    //UPDATE
                    else
                        BudgetList[query.Index] = item;

                    await StorageUtility.SaveItem(BUDGETS_FOLDER, item, item.BudgetId, App.Instance.User.UserId);
                }
                BudgetList.OrderByDescending(x => x.Name);
            }

            private async void RemoveInsertedTransactions()
            {
                //REMOVE all inserted records as they will be added with a new Id
                var newItems = TransactionList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TransactionId <= 0).OrderByDescending(x => x.Index).ToList();
                foreach (var x in newItems)
                {
                    TransactionList.RemoveAt(x.Index);
                    await StorageUtility.DeleteItem<Transaction>(TRANSACTIONS_FOLDER, x.Item, x.Item.TransactionId);
                }
            }

            #endregion

            #region Load

            public ServerStatus SetServerStatus(Action<ServerStatus> callback)
            {
                var result = ServerStatus.Communicating;
                try
                {
                    var client = new BMAService.MainClient();
                    client.GetDBStatusAsync();
                    client.GetDBStatusCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                            result = ServerStatus.Error;
                        else if (e.Result)
                            result = ServerStatus.Ok;
                        else
                            result = ServerStatus.Error;

                        callback(result);
                    };
                }
                catch (Exception)
                {
                    //throw;
                    result = ServerStatus.Error;
                }
                return result;
            }

            public void LoadTransactions(Action<Exception> callback)
            {
                LoadAllTransactions(0, error => callback(error));
            }

            public void LoadBudgets(Action<Exception> callback)
            {
                LoadAllBudgets(error => callback(error));
            }

            public void LoadTransactionsForBudget(int budgetId, Action<Exception> callback)
            {
                LoadAllTransactions(budgetId, error => callback(error));
            }

            public void LoadAllTransactions(int budgetId, Action<Exception> callback)
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                            LoadCachedTransactions((transactionList, error) => callback(error));
                        else
                            LoadLiveTransactions(budgetId, error => callback(error));
                });
            }

            public void LoadAllTransactionImages(int transactionId, Action<Exception> callback)
            {
                //ICollection<TransactionImage> existing = null;

                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                    {
                        try
                        {
                            //## doesnt need to get anything from cache. Images is retrieved by the transaction
                            //existing = await LoadCachedTransactionImages();
                            callback(null);
                        }
                        catch (Exception) { throw new Exception("Username"); }
                    }
                    else
                    {
                        try
                        {
                            var client = new BMAService.MainClient();
                            latestState = Guid.NewGuid().ToString();

                            client.GetImagesForTransactionCompleted += (sender, completedEventArgs) =>
                            {
                                if (completedEventArgs.Error == null)
                                    SetupTransactionImageList(completedEventArgs.Result, transactionId);

                                callback(completedEventArgs.Error);
                            };
                            client.GetImagesForTransactionAsync(transactionId, latestState);
                        }
                        catch (Exception ex)
                        {
                            var msg = string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message);
                            throw;
                        }
                    }
                });
            }

            public void LoadAllBudgets(Action<Exception> callback)
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedBudgets((budgetList, error) => callback(error));
                    else
                        LoadLiveBudgets(error => callback(error));
                });
            }

            public void GetLatestTransactionDateDouble(Action<double, Exception> callback)
            {
                try
                {
                    var client = new BMAService.MainClient();
                    client.GetLatestTransactionDateDoubleAsync(App.Instance.User.UserId);

                    client.GetLatestTransactionDateDoubleCompleted += (sender, e) =>
                    {
                        callback(e.Result, e.Error);
                    };
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public void GetLatestTransactionDate(Action<DateTime, Exception> callback)
            {
                try
                {
                    var client = new BMAService.MainClient();
                    client.GetLatestTransactionDateAsync();

                    client.GetLatestTransactionDateCompleted += (sender, e) =>
                    {
                        if (e.Error == null)
                            callback(e.Result, null);
                        else
                            callback(DateTime.Now, e.Error);
                    };
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public void SyncTransactions(Action<Exception> callback)
            {
                try
                {
                    App.Instance.ServiceData.LoadCachedTransactions((cachedTransactions, error) =>
                        {
                            var transList = cachedTransactions.Where(x => x.ModifiedDate > App.Instance.LastSyncDate).ToObservableCollection();

                            var client = new BMAService.MainClient();

                            client.SyncTransactionsAsync(transList);

                            client.SyncTransactionsCompleted += (sender, e) => callback(e.Error == null ? null : e.Error);
                        });
                }
                catch (Exception)
                {

                    throw;
                }
            }

            public void SyncBudgets(Action<Exception> callback)
            {
                try
                {
                    App.Instance.ServiceData.LoadCachedBudgets((cachedBudgets, error) =>
                        {
                            var budgetList = cachedBudgets.Where(x => x.ModifiedDate > App.Instance.LastSyncDate).ToObservableCollection();

                            var client = new BMAService.MainClient();

                            client.SyncBudgetsAsync(budgetList);

                            client.SyncBudgetsCompleted += (sender, e) => callback(e.Error == null ? null : e.Error);
                        });
                }
                catch (Exception)
                {

                    throw;
                }
            }

            #endregion

            #region Save

            public void SaveTransaction(ObservableCollection<Transaction> transactions, Action<Exception> callback)
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        try
                        {
                            foreach (var item in transactions)
                                item.OptimizeOnTopLevel(Transaction.ImageRemovalStatus.None);

                            foreach (var item in TransactionList.Where(x => x.HasChanges))
                                item.HasChanges = false;

                            SetupTransactionList(transactions, false);

                            App.Instance.IsSync = false;

                            callback(null);
                        }
                        catch (Exception ex)
                        {
                            callback(ex);
                        }
                    }
                    else
                    {
                        var client = new BMAService.MainClient();

                        foreach (var item in transactions)
                            item.OptimizeOnTopLevel(Transaction.ImageRemovalStatus.Unchanged);

                        client.SaveTransactionsAsync(transactions);
                        client.SaveTransactionsCompleted +=  (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                SetupTransactionList(completedEventArgs.Result, true);

                                App.Instance.IsSync = true;

                                callback(null);
                            }
                            else
                                callback(completedEventArgs.Error);

                        };
                    }
                });
            }
            
            public void SaveTransactionImages(ObservableCollection<TransactionImage> transactionImages)
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        foreach (var item in TransactionImageList.Where(x => x.HasChanges))
                            item.HasChanges = false;

                        //await UpdateCacheTransactionImages();

                        App.Instance.IsSync = false;
                    }
                    else
                    {
                        var client = new BMAService.MainClient();
                        client.SaveTransactionImagesAsync(transactionImages);
                        client.SaveTransactionImagesCompleted += (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                //SetupTransactionImageList(completedEventArgs.Result);
                                App.Instance.IsSync = true;
                            }

                        };
                    }
                });
            }
            
            public void SaveBudgets(ObservableCollection<Budget> budgets, Action<Exception> callback)
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        try
                        {
                            foreach (var item in BudgetList.Where(x => x.HasChanges))
                                item.HasChanges = false;

                            SetupBudgetList(budgets, false);

                            App.Instance.IsSync = false;

                            callback(null);
                        }
                        catch (Exception ex)
                        {
                            callback(ex);
                        }
                    }
                    else
                    {
                        var client = new BMAService.MainClient();
                        client.SaveBudgetsAsync(budgets);
                        client.SaveBudgetsCompleted +=  (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                SetupBudgetList(completedEventArgs.Result, true);

                                App.Instance.IsSync = true;

                                callback(null);
                            }
                            else
                                callback(completedEventArgs.Error);
                        };
                    }
                });
            }

            #endregion

            #region Sync

            #endregion

            #region Update Cache
            
            #endregion
        }

}
