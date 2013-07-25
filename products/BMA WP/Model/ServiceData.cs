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
            const string TRANSACTIONS_FOLDER = "Transactions";
            const string TRANSACTIONIMAGES_FOLDER = "TransactionImagess";
            const string BUDGETS_FOLDER = "Budgets";

            static readonly string Utf8ByteOrderMark =
            Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble(), 0, Encoding.UTF8.GetPreamble().Length);

            string latestState;
            
            #region Constructor
            public ServiceData()
            {
                TransactionList = new TransactionList();
                TransactionImageList = new TransactionImageList();
                BudgetList = new BudgetList();
            }

            #endregion

            #region Events
            
            void client_SyncBudgetsCompleted(object sender, BMAService.SyncBudgetsCompletedEventArgs e)
            {
                try
                {
                    var result = e.Result;

                    ApplicationData.Current.LocalSettings.Values["IsSync"] = true;

                    UpdateCacheBudgets(result);
                    SetupBudgetList(result);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            void client_GetTransactionsForBudgetCompleted(object sender, BMAService.GetTransactionsForBudgetCompletedEventArgs e)
            {
                try
                {
                    //retVal.AddRange(result);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            void client_GetAllBudgetsCompleted(object sender, BMAService.GetAllBudgetsCompletedEventArgs e)
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
            }
            #endregion

            #region Private Methods

            #endregion

            #region Load
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

                if (!App.Instance.IsOnline)
                {
                    existing = await LoadCachedTransactions();
                }
                else
                {
                    existing = await LoadLiveTransactions(budgetId);
                }
            }

            public async Task LoadAllTransactionImages(int transactionId, Action<Exception> callback)
            {
                ICollection<TransactionImage> existing = null;

                if (!App.Instance.IsOnline)
                {
                    try
                    {
                        existing = await LoadCachedTransactionImages();
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
                            SetupTransactionImageList(completedEventArgs.Result);

                            UpdateCacheTransactionImages();

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

            private void SetupTransactionList(ICollection<Transaction> existing)
            {
                //sync logic
                
                //REMOVE all inserted records as they will be added with a new Id
                var newItems = TransactionList.Select((x,i) => new {Item=x, Index=i}).Where(x=>x.Item.TransactionId <= 0).OrderByDescending(x=>x.Index).ToList();
                newItems.ForEach(x=>TransactionList.RemoveAt(x.Index));

                foreach (var item in existing)
                {
                    var query = TransactionList.Select((x, i) => new {trans = x, Index = i}).Where(x => x.trans.TransactionId == item.TransactionId).FirstOrDefault();
                    
                    //INSERT
                    if (query == null)
                        TransactionList.Add(item);
                    //UPDATE
                    else
                        TransactionList[query.Index] = item;
                }
            }

            private void SetupTransactionImageList(ICollection<TransactionImage> existing)
            {
                existing = existing ?? new List<TransactionImage>();

                TransactionImageList.Clear();

                foreach (var item in existing)
                {
                    var trans = TransactionList.FirstOrDefault(x=>x.TransactionId == item.Transaction.TransactionId);
                    if (trans != null)
                    {
                        if (trans.TransactionImages == null)
                            trans.TransactionImages = new TransactionImageList();

                        trans.TransactionImages.Add(item);
                    }
                    //TransactionImageList.Add(trans);
                }
            }

            private void SetupBudgetList(ICollection<Budget> existing)
            {
                existing = existing ?? new ObservableCollection<Budget>();

                BudgetList.Clear();

                foreach (var trans in existing)
                    BudgetList.Add(trans);
            }

            public async Task LoadAllBudgets()
            {
                ICollection<Budget> existing = null;

                if (!App.Instance.IsOnline)
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

            private async Task<ICollection<Transaction>> LoadLiveTransactions()
            {
                return await LoadLiveTransactions(0);
            }
            
            private async Task<ICollection<Transaction>> LoadLiveTransactions(int budgetId)
            {

                var retVal = new List<Transaction>();

                if (!App.Instance.IsOnline)
                {
                    return retVal;
                }

                try
                {
                    var client = new BMAService.MainClient();
                    if (budgetId == 0)
                    {
                        latestState = Guid.NewGuid().ToString();

                        client.GetLatestTransactionsCompleted += (sender, completedEventArgs) =>
                        {
                            SetupTransactionList(completedEventArgs.Result);

                            UpdateCacheTransactions();
                        };
                        client.GetLatestTransactionsAsync(latestState);
                    }
                    else
                    {
                        client.GetTransactionsForBudgetCompleted += new EventHandler<BMAService.GetTransactionsForBudgetCompletedEventArgs>(client_GetTransactionsForBudgetCompleted);
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
                var info = NetworkInformation.GetInternetConnectionProfile();

                if (!App.Instance.IsOnline)
                {
                    return retVal;
                }

                try
                {
                    var client = new BMAService.MainClient();
                    client.GetAllBudgetsCompleted += client_GetAllBudgetsCompleted;
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

                if (!App.Instance.IsOnline)
                {
                    return retVal;
                }

                try
                {
                    var client = new BMAService.MainClient();
                        latestState = Guid.NewGuid().ToString();

                        client.GetImagesForTransactionCompleted += (sender, completedEventArgs) =>
                        {
                            SetupTransactionImageList(completedEventArgs.Result);

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

                SetupTransactionList(retVal);

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

                SetupTransactionImageList(retVal);

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

            public TransactionImageList TransactionImageList { get; set; }
            public TransactionList TransactionList { get; set; }
            public BudgetList BudgetList { get; set; }

            #endregion

            #region Save
            public async Task SyncTransactions()
            {
                try
                {
                    var transactions = await LoadCachedTransactions();
                    
                    var client = new BMAService.MainClient();
                    client.SyncTransactionsCompleted += async (o, e) => {
                        try
                        {
                            var result = e.Result;
                            await UpdateCacheTransactions();
                            SetupTransactionList(result);

                            App.Instance.IsSync = true;

                        }
                        catch(Exception) { throw; }
                    };
                    client.SyncTransactionsAsync(transactions.ToObservableCollection());
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public async Task SaveTransactionChanges()
            {
                await SaveTransaction(TransactionList.Where(i => i.HasChanges).ToObservableCollection(), (error) => {});
            }

            public async Task SaveTransaction(ObservableCollection<Transaction> transactions, Action<Exception> callback)
            {
                try
                {
                    if (!App.Instance.IsOnline)
                    {
                        try
                        {

                            foreach (var item in transactions)
                                item.OptimizeOnSecondLevel(true);

                            foreach (var item in TransactionList.Where(x => x.HasChanges))
                                item.HasChanges = false;

                            await UpdateCacheTransactions();

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
                            item.OptimizeOnSecondLevel(true);

                        client.SaveTransactionsAsync(transactions);
                        client.SaveTransactionsCompleted += async (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                //1. change save method to return only the saved records
                                //2. When saving, remove childs of categories and reasons because when you choose the Other option for both, 
                                //    you get an error that the object is too large

                                SetupTransactionList(completedEventArgs.Result);
                                await UpdateCacheTransactions();
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
                    if (!App.Instance.IsOnline)
                    {
                        foreach (var item in TransactionImageList.Where(x => x.HasChanges))
                            item.HasChanges = false;

                        await UpdateCacheTransactionImages();

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
                                //await UpdateCacheTransactionImages(completedEventArgs.Result);
                                //SetupTransactionImageList(completedEventArgs.Result);
                            }

                        };
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public async Task SyncBudgets()
            {
                try
                {
                    var budgets = await LoadCachedBudgets();

                    var client = new BMAService.MainClient();
                    client.SyncBudgetsCompleted += async (o, e) => {
                        try
                        {
                            var result = e.Result;
                            await UpdateCacheBudgets(result);
                            SetupBudgetList(result);

                            App.Instance.IsSync = true;

                        }
                        catch (Exception){throw;}
                    };
                    client.SyncBudgetsAsync(budgets.ToObservableCollection());
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public async Task SaveBudgetsChanges()
            {
                await SaveBudgets(BudgetList.Where(i => i.HasChanges).ToObservableCollection());
            }

            public async Task SaveBudgets(ObservableCollection<Budget> budgets)
            {
                try
                {
                    var result = this.BudgetList.ToObservableCollection();

                    if (!App.Instance.IsOnline)
                    {
                        result = result.Where(i => !i.IsDeleted).ToObservableCollection();
                        ApplicationData.Current.LocalSettings.Values["IsSync"] = false;
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
