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
            private async Task<ICollection<Transaction>> LoadLiveTransactions()
            {
                return await LoadLiveTransactions(0);
            }

            private async Task<ICollection<Transaction>> LoadLiveTransactions(int budgetId)
            {
                var retVal = new List<Transaction>();

                if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                    return retVal;

                try
                {
                    var client = new BMAService.MainClient();
                    if (budgetId == 0)
                    {
                        latestState = Guid.NewGuid().ToString();

                        client.GetLatestTransactionsCompleted += (sender, completedEventArgs) =>
                        {
                            SetupTransactionList(completedEventArgs.Result, true);
                        };
                        client.GetLatestTransactionsAsync(latestState);
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
                return retVal;
            }

            private async Task<ICollection<Budget>> LoadLiveBudgets()
            {

                var retVal = new List<Budget>();

                if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                {
                    return retVal;
                }

                try
                {
                    var client = new BMAService.MainClient();
                    client.GetAllBudgetsCompleted += (sender, e) =>
                        {
                            try
                            {
                                foreach (var item in e.Result)
                                    BudgetList.Add(item);
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        };
                    client.GetAllBudgetsAsync();
                }
                catch (Exception ex)
                {
                    var msg = string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message);
                    throw;
                }
                return retVal;
            }

            private async Task<ICollection<TransactionImage>> LoadLiveTransactionImages(int transactionId)
            {

                var retVal = new List<TransactionImage>();

                if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                {
                    return retVal;
                }

                try
                {
                    var client = new BMAService.MainClient();
                    latestState = Guid.NewGuid().ToString();

                    client.GetImagesForTransactionCompleted += (sender, completedEventArgs) =>
                    {
                        SetupTransactionImageList(completedEventArgs.Result, transactionId);

                        UpdateCacheTransactionImages();
                    };
                    client.GetImagesForTransactionAsync(transactionId, latestState);
                }
                catch (Exception ex)
                {
                    var msg = string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message);
                    throw;
                }
                return retVal;
            }
            #endregion

            #region Load Cached Data
            private async Task<ICollection<Transaction>> LoadCachedTransactions()
            {
                var retVal = new List<Transaction>();
                foreach (var item in await StorageUtility.ListItems(TRANSACTIONS_FOLDER))
                {
                    try
                    {
                        var trans = await StorageUtility.RestoreItem<Transaction>(TRANSACTIONS_FOLDER, item);
                        retVal.Add(trans);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }

                SetupTransactionList(retVal, false);

                return retVal;
            }

            private async Task<ICollection<TransactionImage>> LoadCachedTransactionImages()
            {
                var retVal = new List<TransactionImage>();
                foreach (var item in await StorageUtility.ListItems(TRANSACTIONIMAGES_FOLDER))
                {
                    try
                    {
                        var trans = await StorageUtility.RestoreItem<TransactionImage>(TRANSACTIONIMAGES_FOLDER, item);
                        retVal.Add(trans);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }

                //SetupTransactionImageList(retVal);

                return retVal;
            }

            private async Task<ICollection<Budget>> LoadCachedBudgets()
            {
                var retVal = new List<Budget>();
                foreach (var item in await StorageUtility.ListItems(BUDGETS_FOLDER))
                {
                    try
                    {
                        var budget = await StorageUtility.RestoreItem<Budget>(BUDGETS_FOLDER, item);
                        retVal.Add(budget);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                    }
                }
                return retVal;
            }
            #endregion

            private async void SetupTransactionList(ICollection<Transaction> existing, bool removeNew)
            {
                existing = existing ?? new List<Transaction>();

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

                    await StorageUtility.SaveItem(TRANSACTIONS_FOLDER, item, item.TransactionId);
                }
            }

            private void SetupTransactionImageList(ICollection<TransactionImage> existing, int transactionId)
            {
                existing = existing ?? new List<TransactionImage>();

                TransactionImageList.Clear();
                var trans = TransactionList.FirstOrDefault(x => x.TransactionId == transactionId);

                if (trans != null)
                    trans.TransactionImages = new TransactionImageList();

                foreach (var item in existing)
                    trans.TransactionImages.Add(item);

                StorageUtility.SaveItem<Transaction>(TRANSACTIONS_FOLDER, trans, trans.TransactionId);
            }

            private void SetupBudgetList(ICollection<Budget> existing)
            {
                existing = existing ?? new ObservableCollection<Budget>();

                BudgetList.Clear();

                foreach (var trans in existing)
                    BudgetList.Add(trans);
            }

            private async void RemoveInsertedTransactions()
            {
                //REMOVE all inserted records as they will be added with a new Id
                var newItems = TransactionList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TransactionId <= 0).OrderByDescending(x => x.Index).ToList();
                foreach (var x in newItems)
                {
                    TransactionList.RemoveAt(x.Index);
                    await StorageUtility.DeleteItem<Transaction>(TRANSACTIONS_FOLDER, x.Item.TransactionId.GetHashCode().ToString());
                }
            }

            #endregion

            #region Load

            public async Task<ServerStatus> SetServerStatus(Action<ServerStatus> callback)
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

            public async Task LoadTransactions()
            {
                await LoadAllTransactions(0);
            }

            public async Task LoadBudgets()
            {
                await LoadAllBudgets();
            }

            public async Task LoadTransactionsForBudget(int budgetId)
            {
                await LoadAllTransactions(budgetId);
            }

            public async Task LoadAllTransactions(int budgetId)
            {
                ICollection<Transaction> existing = null;

                if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                    existing = await LoadCachedTransactions();
                else
                    existing = await LoadLiveTransactions(budgetId);
            }

            public async Task LoadAllTransactionImages(int transactionId, Action<Exception> callback)
            {
                ICollection<TransactionImage> existing = null;

                if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
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
                            if (completedEventArgs.Error != null)
                            {
                                callback(completedEventArgs.Error);
                                return;
                            }
                            SetupTransactionImageList(completedEventArgs.Result, transactionId);

                            //UpdateCacheTransactionImages();

                            callback(null);

                        };
                        client.GetImagesForTransactionAsync(transactionId, latestState);
                    }
                    catch (Exception ex)
                    {
                        var msg = string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message);
                        throw;
                    }
                }
            }

            public async Task LoadAllBudgets()
            {
                ICollection<Budget> existing = null;

                if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                {
                    existing = await LoadCachedBudgets();
                }
                else
                {
                    existing = await LoadLiveBudgets();
                    await UpdateCacheBudgets(existing);
                }

                SetupBudgetList(existing);
            }

            #endregion

            #region Save
            
            public async Task SaveTransaction(ObservableCollection<Transaction> transactions, Action<Exception> callback)
            {
                try
                {
                    if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                    {
                        try
                        {

                            //foreach (var item in transactions)
                            //    item.OptimizeOnTopLevel(Transaction.ImageRemovalStatus.Unchanged);

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
                        client.SaveTransactionsCompleted += async (sender, completedEventArgs) =>
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
                }
                catch (Exception)
                {
                    throw;
                }
            }
            
            public async Task SaveTransactionImages(ObservableCollection<TransactionImage> transactionImages)
            {
                try
                {
                    if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
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
                        client.SaveTransactionImagesCompleted += async (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                //SetupTransactionImageList(completedEventArgs.Result);
                                App.Instance.IsSync = true;
                            }

                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public async Task SaveBudgets(ObservableCollection<Budget> budgets)
            {
                try
                {
                    var result = this.BudgetList.ToObservableCollection();

                    if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                    {
                        result = result.Where(i => !i.IsDeleted).ToObservableCollection();
                        App.Instance.IsSync = false;
                    }
                    else
                    {
                        var client = new BMAService.MainClient();
                        client.SaveBudgetsCompleted += async (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                var reult = completedEventArgs.Result;
                                await UpdateCacheBudgets(result);
                                SetupBudgetList(result);
                                App.Instance.IsSync = true;
                            }
                        };
                        client.SaveBudgetsAsync(budgets);                        
                    }                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
            
            #endregion

            #region Sync

            public async Task SyncBudgets()
            {
                try
                {
                    var budgets = await LoadCachedBudgets();

                    var client = new BMAService.MainClient();
                    client.SyncBudgetsCompleted += async (o, e) =>
                    {
                        try
                        {
                            var result = e.Result;
                            await UpdateCacheBudgets(result);
                            SetupBudgetList(result);

                            App.Instance.IsSync = true;

                        }
                        catch (Exception) { throw; }
                    };
                    client.SyncBudgetsAsync(budgets.ToObservableCollection());
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public async Task SyncTransactions()
            {
                try
                {
                    var transactions = await LoadCachedTransactions();

                    var client = new BMAService.MainClient();
                    client.SyncTransactionsCompleted += async (o, e) =>
                    {
                        try
                        {
                            var result = e.Result;
                            //await UpdateCacheTransactions();
                            SetupTransactionList(result, true);

                            App.Instance.IsSync = true;

                        }
                        catch (Exception) { throw; }
                    };
                    client.SyncTransactionsAsync(transactions.ToObservableCollection());
                }
                catch (Exception)
                {
                    throw;
                }
            }

            #endregion

            #region Update Cache
            private async Task UpdateCacheBudgets(ICollection<Budget> budgetList)
            {
                await StorageUtility.Clear(BUDGETS_FOLDER);
                foreach (var item in budgetList)
                    await StorageUtility.SaveItem(BUDGETS_FOLDER, item, item.BudgetId);
            }

            private async Task UpdateCacheTransactions()
            {
                await StorageUtility.Clear(TRANSACTIONS_FOLDER);

                foreach (var item in TransactionList)
                    await StorageUtility.SaveItem(TRANSACTIONS_FOLDER, item, item.TransactionId);
            }

            private async Task UpdateCacheTransactionImages()
            {
                await StorageUtility.Clear(TRANSACTIONIMAGES_FOLDER);
                foreach (var item in TransactionImageList)
                    await StorageUtility.SaveItem(TRANSACTIONIMAGES_FOLDER, item, item.TransactionImageId);
            }
            #endregion
        }

}
