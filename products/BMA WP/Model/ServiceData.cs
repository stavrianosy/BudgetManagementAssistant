using BMA.BusinessLogic;
using BMA_WP.Common;
//using BMA.Proxy.BMAService;
using BMA_WP.BMAService;
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
            public TransactionList IntervalTransactionList { get; set; }
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
                    var client = new MainClient();
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
                        //client.GetTransactionsForBudgetCompleted += new EventHandler<GetTransactionsForBudgetCompletedEventArgs>(client_GetTransactionsForBudgetCompleted);
                        client.GetTransactionsForBudgetAsync(budgetId);
                    }
                }
                catch (Exception ex)
                {
                    //var msg = string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message);
                    callback(ex);
                }
            }

            private void LoadLiveBudgets(Action<Exception> callback)
            {
                try
                {
                    latestState = Guid.NewGuid().ToString();
                    var client = new MainClient();
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
                    //var msg = string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message);
                    callback(ex);
                }
            }

            #endregion

            #region Load Cached Data
            public async void LoadCachedTransactions(Action<Exception> callback)
            {
                try
                {
                    foreach (var item in await StorageUtility.ListItems(TRANSACTIONS_FOLDER, App.Instance.User.UserName))
                    {
                        var trans = await StorageUtility.RestoreItem<Transaction>(TRANSACTIONS_FOLDER, item, App.Instance.User.UserName);
                        TransactionList.Add(trans);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    //callback(null, ex);
                }

                callback(null);
            }

            private async void LoadCachedTransactionImages(Action<TransactionImageList, Exception> callback)
            {
                var retVal = new TransactionImageList();
                try
                {
                    foreach (var item in await StorageUtility.ListItems(TRANSACTIONIMAGES_FOLDER, App.Instance.User.UserName))
                    {
                        var trans = await StorageUtility.RestoreItem<TransactionImage>(TRANSACTIONIMAGES_FOLDER, item, App.Instance.User.UserName);
                        TransactionImageList.Add(trans);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    //callback(null, ex);
                }
                
                //SetupTransactionImageList(retVal);
                //TransactionImageList = retVal;

                callback(TransactionImageList, null);
            }

            private async void LoadCachedBudgets(Action<BudgetList, Exception> callback)
            {
                var retVal = new BudgetList();
                try
                {
                    foreach (var item in await StorageUtility.ListItems(BUDGETS_FOLDER, App.Instance.User.UserName))
                    {
                        var budget = await StorageUtility.RestoreItem<Budget>(BUDGETS_FOLDER, item, App.Instance.User.UserName);
                        BudgetList.Add(budget);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    //callback(null, ex);
                }

                //SetupBudgetList(retVal, false);
                BudgetList = retVal;

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
                        await StorageUtility.SaveItem(TRANSACTIONS_FOLDER, item, item.TransactionId, App.Instance.User.UserName);
                }
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

                await StorageUtility.SaveItem<Transaction>(TRANSACTIONS_FOLDER, trans, trans.TransactionId, App.Instance.User.UserName);
            }

            private async void SetupBudgetList(ICollection<Budget> existing, bool removeNew)
            {
                existing = existing ?? new ObservableCollection<Budget>();

                //sync logic
                if (removeNew)
                    RemoveInsertedBudgets();

                foreach (var item in existing)
                {
                    var query = BudgetList.Select((x, i) => new { budget = x, Index = i }).Where(x => x.budget.BudgetId == item.BudgetId).FirstOrDefault();

                    //INSERT
                    if (query == null)
                        BudgetList.Add(item);
                    //UPDATE
                    else
                        BudgetList[query.Index] = item;

                    await StorageUtility.SaveItem(BUDGETS_FOLDER, item, item.BudgetId, App.Instance.User.UserName);
                }
            }

            private async void RemoveInsertedTransactions()
            {
                //REMOVE all inserted records as they will be added with a new Id
                var newItems = TransactionList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TransactionId <= 0).OrderByDescending(x => x.Index).ToList();

                foreach (var x in newItems)
                    TransactionList.RemoveAt(x.Index);

                await StorageUtility.DeleteNewItems<Transaction>(TRANSACTIONS_FOLDER, App.Instance.User.UserName);
            }

            private async void RemoveInsertedBudgets()
            {
                //REMOVE all inserted records as they will be added with a new Id
                var newItems = BudgetList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.BudgetId <= 0).OrderByDescending(x => x.Index).ToList();

                foreach (var x in newItems)
                    BudgetList.RemoveAt(x.Index);

                await StorageUtility.DeleteNewItems<Transaction>(BUDGETS_FOLDER, App.Instance.User.UserName);
            }

            #endregion

            #region Load

            public ServerStatus SetServerStatus(Action<ServerStatus> callback)
            {
                var result = ServerStatus.Communicating;
                try
                {
                    var client = new MainClient();
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
                //Clean the list before fetch the new data
                TransactionList = new BMA.BusinessLogic.TransactionList();

                if (!App.Instance.IsOnline)
                    LoadCachedTransactions(error => callback(error));
                else
                    LoadLiveTransactions(budgetId, error => callback(error));
            }

            public void LoadAllTransactionImages(int transactionId, Action<Exception> callback)
            {
                //ICollection<TransactionImage> existing = null;

                App.Instance.StaticDataOnlineStatus = App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != Model.StaticServiceData.ServerStatus.Ok)
                    {
                        //## doesn't need to get anything from cache. Images are retrieved by the transaction
                        callback(null);
                    }
                    else
                    {
                        try
                        {
                            var client = new MainClient();
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
                            //var msg = string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message);
                            callback(ex);
                        }
                    }
                });
            }

            public void LoadAllBudgets(Action<Exception> callback)
            {
                
                    //Clean the list before fetch the new data
                    BudgetList = new BMA.BusinessLogic.BudgetList();

                    if (!App.Instance.IsOnline)
                        LoadCachedBudgets((budgetList, error) => callback(error));
                    else
                        LoadLiveBudgets(error => callback(error));
            }

            public void GetLatestTransactionDateDouble(Action<double, Exception> callback)
            {
                try
                {
                    var client = new MainClient();
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
                    var client = new MainClient();
                    client.GetLatestTransactionDateAsync();

                    client.GetLatestTransactionDateCompleted += (sender, e) =>
                    {
                        if (e.Error == null)
                            callback(e.Result, null);
                        else
                            callback(DateTime.Now, e.Error);
                    };
                }
                catch (Exception ex)
                {
                    callback(DateTime.Now, ex);
                }
            }

            public void SyncTransactions(Action<Exception> callback)
            {
                try
                {
                    App.Instance.ServiceData.LoadCachedTransactions(error =>
                        {
                            var transList = TransactionList.Where(x => x.ModifiedDate > App.Instance.LastSyncDate).ToObservableCollection();

                            var client = new MainClient();

                            client.SyncTransactionsAsync(transList);

                            client.SyncTransactionsCompleted += (sender, e) => callback(e.Error == null ? null : e.Error);
                        });
                }
                catch (Exception ex)
                {
                    callback(ex);
                }
            }

            public void SyncBudgets(Action<Exception> callback)
            {
                try
                {
                    App.Instance.ServiceData.LoadCachedBudgets((cachedBudgets, error) =>
                        {
                            var budgetList = cachedBudgets.Where(x => x.ModifiedDate > App.Instance.LastSyncDate).ToObservableCollection();

                            var client = new MainClient();

                            client.SyncBudgetsAsync(budgetList);

                            client.SyncBudgetsCompleted += (sender, e) => callback(e.Error == null ? null : e.Error);
                        });
                }
                catch (Exception ex)
                {
                    callback(ex);
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
                        var client = new MainClient();

                        foreach (var item in transactions)
                            item.OptimizeOnTopLevel(Transaction.ImageRemovalStatus.Unchanged);

                        client.SaveTransactionsAsync(transactions);
                        client.SaveTransactionsCompleted +=  (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                SetupTransactionList(completedEventArgs.Result, true);

                                //Only update sync when offline and in login and main pages
                                //App.Instance.IsSync = true;

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
                        var client = new MainClient();
                        client.SaveTransactionImagesAsync(transactionImages);
                        client.SaveTransactionImagesCompleted += (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                //SetupTransactionImageList(completedEventArgs.Result);
                                
                                //Only update sync when offline and in login and main pages
                                //App.Instance.IsSync = true;
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
                        var client = new MainClient();
                        client.SaveBudgetsAsync(budgets);
                        client.SaveBudgetsCompleted +=  (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                SetupBudgetList(completedEventArgs.Result, true);

                                //Only update sync when offline and in login and main pages
                                //App.Instance.IsSync = true;

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

            #region Reports
            public void ReportTransactionAmount(DateTime dateFrom, DateTime dateTo, int transTypeId, double amountFrom, double amountTo,
                                                Action<ObservableCollection<Transaction>, Exception> callback)
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedReportTransactionAmount(dateFrom, dateTo, transTypeId, amountFrom, amountTo,
                                                        (result, error) => callback(result, error));
                    else
                        LoadLiveReportTransactionAmount(dateFrom, dateTo, transTypeId, amountFrom, amountTo,
                                                        (result, error) => callback(result, error));
                });

            }

            public void ReportTransactionCategory(DateTime dateFrom, DateTime dateTo, int transTypeId,
                                                Action<Dictionary<Category, double>, Exception> callback)
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedReportTransactionCategory(dateFrom, dateTo, transTypeId,
                                                        (result, error) => callback(result, error));
                    else
                        LoadLiveReportTransactionCategory(dateFrom, dateTo, transTypeId,
                                                        (result, error) => callback(result, error));
                });

            }

            public void ReportTransactionReason(DateTime dateFrom, DateTime dateTo, int transTypeId,
                                                Action<Dictionary<TypeTransactionReason, double>, Exception> callback)
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedReportTransactionReason(dateFrom, dateTo, transTypeId, 
                                                        (result, error) => callback(result, error));
                    else
                        LoadLiveReportTransactionReason(dateFrom, dateTo, transTypeId, 
                                                        (result, error) => callback(result, error));
                });

            }

            public void ReportTransactionNameOfPlace(DateTime dateFrom, DateTime dateTo, int transTypeId,
                                                Action<Dictionary<string, double>, Exception> callback)
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedReportTransactionNameOfPlace(dateFrom, dateTo, transTypeId,
                                                        (result, error) => callback(result, error));
                    else
                        LoadLiveReportTransactionNameOfPlace(dateFrom, dateTo, transTypeId,
                                                        (result, error) => callback(result, error));
                });

            }

            public void ReportTransactionByPeriod(DateTime dateFrom, DateTime dateTo, int transTypeId, Const.ReportPeriod period,
                                                Action<Dictionary<int, double>, Exception> callback)
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedReportTransactionByPeriod(dateFrom, dateTo, transTypeId, period,
                                                        (result, error) => callback(result, error));
                    else
                        LoadLiveReportTransactionByPeriod(dateFrom, dateTo, transTypeId, period,
                                                        (result, error) => callback(result, error));
                });

            }

            private void LoadLiveReportTransactionAmount(DateTime dateFrom, DateTime dateTo, int transTypeId, double amountFrom, double amountTo,
                                                            Action<ObservableCollection<Transaction>, Exception> callback)
            {
                try
                {
                    var client = new MainClient();
                    client.ReportTransactionAmountAsync(dateFrom, dateTo, transTypeId, amountFrom, amountTo, App.Instance.User.UserId);

                    client.ReportTransactionAmountCompleted += (sender, e) =>
                    {
                        if (e.Error == null)
                            callback(e.Result, null);
                        else
                            callback(null, e.Error);
                    };
                }
                catch (Exception)
                {

                    throw;
                }
            }

            private void LoadLiveReportTransactionCategory(DateTime dateFrom, DateTime dateTo, int transTypeId,
                                                            Action<Dictionary<Category, double>, Exception> callback)
            {
                try
                {
                    var client = new MainClient();
                    client.ReportTransactionCategoryAsync(dateFrom, dateTo, transTypeId, App.Instance.User.UserId);

                    client.ReportTransactionCategoryCompleted += (sender, e) =>
                    {
                        if (e.Error == null)
                            callback(e.Result, null);
                        else
                            callback(null, e.Error);
                    };
                }
                catch (Exception)
                {

                    throw;
                }
            }

            private void LoadLiveReportTransactionReason(DateTime dateFrom, DateTime dateTo, int transTypeId,
                                                            Action<Dictionary<TypeTransactionReason, double>, Exception> callback)
            {
                try
                {
                    var client = new MainClient();
                    client.ReportTransactionReasonAsync(dateFrom, dateTo, transTypeId, App.Instance.User.UserId);

                    client.ReportTransactionReasonCompleted += (sender, e) =>
                    {
                        if (e.Error == null)
                            callback(e.Result, null);
                        else
                            callback(null, e.Error);
                    };
                }
                catch (Exception)
                {

                    throw;
                }
            }

            private void LoadLiveReportTransactionNameOfPlace(DateTime dateFrom, DateTime dateTo, int transTypeId,
                                                            Action<Dictionary<string, double>, Exception> callback)
            {
                try
                {
                    var client = new MainClient();
                    client.ReportTransactionNameOfPlaceAsync(dateFrom, dateTo, transTypeId, App.Instance.User.UserId);

                    client.ReportTransactionNameOfPlaceCompleted += (sender, e) =>
                    {
                        if (e.Error == null)
                            callback(e.Result, null);
                        else
                            callback(null, e.Error);
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private void LoadLiveReportTransactionByPeriod(DateTime dateFrom, DateTime dateTo, int transTypeId, Const.ReportPeriod period,
                                                            Action<Dictionary<int, double>, Exception> callback)
            {
                try
                {
                    var client = new MainClient();
                    client.ReportTransactionByPeriodAsync(dateFrom, dateTo, transTypeId, period, App.Instance.User.UserId);

                    client.ReportTransactionByPeriodCompleted += (sender, e) =>
                    {
                        if (e.Error == null)
                            callback(e.Result, null);
                        else
                            callback(null, e.Error);
                    };
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private ObservableCollection<Transaction> GetAllTransactionsWithCriteria(DateTime dateFrom, DateTime dateTo, int transTypeId, double amountFrom, double amountTo)
            {
                var result = new ObservableCollection<Transaction>();

                result = (from i in TransactionList
                          where i.TransactionDate >= dateFrom && i.TransactionDate <= dateTo &&
                          i.TransactionType.TypeTransactionId == transTypeId &&
                          (amountFrom <= 0 || i.Amount >= amountFrom) && (amountTo <= 0 || i.Amount <= amountTo) &&
                          !i.IsDeleted && i.ModifiedUser.UserId == App.Instance.User.UserId
                          orderby i.Amount descending
                          select i).ToObservableCollection();
                
                return result;
            }

            private void LoadCachedReportTransactionAmount(DateTime dateFrom, DateTime dateTo, int transTypeId, double amountFrom, double amountTo,
                                                            Action<ObservableCollection<Transaction>, Exception> callback)
            {
                var result = GetAllTransactionsWithCriteria(dateFrom, dateTo, transTypeId, amountFrom, amountTo);

                callback(result, null);

            }

            private void LoadCachedReportTransactionCategory(DateTime dateFrom, DateTime dateTo, int transTypeId,
                                                            Action<Dictionary<Category, double>, Exception> callback)
            {
                var result = new Dictionary<Category, double>();

                var query = (from i in GetAllTransactionsWithCriteria(dateFrom, dateTo, transTypeId, -1, -1).GroupBy(x => x.Category)
                             select new
                             {
                                 item = i.FirstOrDefault().Category,
                                 sum = i.Sum(x => x.Amount)
                             }).OrderByDescending(x => x.sum).ToList();

                query.ForEach(x =>
                {
                    result.Add(x.item, x.sum);
                });

                callback(result, null);
            }

            private void LoadCachedReportTransactionReason(DateTime dateFrom, DateTime dateTo, int transTypeId,
                                                            Action<Dictionary<TypeTransactionReason, double>, Exception> callback)
            {
                var result = new Dictionary<TypeTransactionReason, double>();

                var query = (from i in GetAllTransactionsWithCriteria(dateFrom, dateTo, transTypeId, -1, -1).GroupBy(x => x.TransactionReasonType)
                             select new
                             {
                                 item = i.FirstOrDefault().TransactionReasonType,
                                 sum = i.Sum(x => x.Amount)
                             }).OrderByDescending(x => x.sum).ToList();

                query.ForEach(x =>
                {
                    result.Add(x.item, x.sum);
                });

                callback(result, null);
            }

            private void LoadCachedReportTransactionNameOfPlace(DateTime dateFrom, DateTime dateTo, int transTypeId,
                                                            Action<Dictionary<string, double>, Exception> callback)
            {
                var result = new Dictionary<string, double>();

                var query = (from i in GetAllTransactionsWithCriteria(dateFrom, dateTo, transTypeId, -1, -1).GroupBy(x => x.NameOfPlace)
                             select new
                             {
                                 item = i.FirstOrDefault().NameOfPlace,
                                 sum = i.Sum(x => x.Amount)
                             }).OrderByDescending(x => x.sum).ToList();

                query.ForEach(x =>
                {
                    result.Add(x.item, x.sum);
                });

                callback(result, null);
            }

            private void LoadCachedReportTransactionByPeriod(DateTime dateFrom, DateTime dateTo, int transTypeId, Const.ReportPeriod period,
                                                            Action<Dictionary<int, double>, Exception> callback)
            {
                var result = new Dictionary<int, double>();

                string periodStr = "";

                switch (period)
                {
                    case Const.ReportPeriod.Monthly:
                        periodStr = "yyyyMM";
                        break;
                    case Const.ReportPeriod.Yearly:
                        periodStr = "yyyy";
                        break;
                    default:
                        periodStr = "yyyyMMdd";
                        break;
                }


                var query = (from i in GetAllTransactionsWithCriteria(dateFrom, dateTo, transTypeId, -1, -1)
                                 .GroupBy(x => x.TransactionDate.ToString(periodStr))
                             select new
                             {
                                 item = i.Key,
                                 sum = i.Sum(x => x.Amount)
                             }).OrderByDescending(x => x.sum).ToList();

                query.ForEach(x =>
                {
                    result.Add(int.Parse(x.item), x.sum);
                });

                callback(result, null);
            }
            #endregion
        }

}
