using BMA.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.Web.Syndication;

namespace BMA.DataModel
{
    public class TransDataSource
    {
        const string GROUP_FOLDER = "Groups";
        const string TRANSACTIONS_FOLDER = "Transactions";
        const string BUDGETS_FOLDER = "Budgets";

        static readonly string Utf8ByteOrderMark =
            Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble(), 0, Encoding.UTF8.GetPreamble().Length);

        #region Constructor
        public TransDataSource()
        {
            TransactionList = new TransactionList();
            BudgetList = new ObservableCollection<Budget>();
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
                await UpdateCacheTransactions(existing);
            }

            SetupTransactionList(existing);

        }

        private void SetupTransactionList(ICollection<Transaction> existing)
        {
            existing = existing ?? new TransactionList();

            TransactionList.Clear();

            foreach (var trans in existing)
                TransactionList.Add(trans);
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

        private static async Task<ICollection<Transaction>> LoadLiveTransactions()
        {
            return await LoadLiveTransactions(0); 
        }
        
        private static async Task<ICollection<Transaction>> LoadLiveTransactions(int budgetId)
        {

            var retVal = new List<Transaction>();
            var info = NetworkInformation.GetInternetConnectionProfile();

            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                return retVal;
            }

            try
            {
                var client = new BMAService.MainClient();
                var result = budgetId == 0 ? await client.GetLatestTransactionsAsync() : await client.GetTransactionsForBudgetAsync(budgetId);
                retVal.AddRange(result);
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message));
                throw;
            }
            return retVal;
        }

        private static async Task<ICollection<Budget>> LoadLiveBudgets()
        {

            var retVal = new List<Budget>();
            var info = NetworkInformation.GetInternetConnectionProfile();

            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                return retVal;
            }

            try
            {
                var client = new BMAService.MainClient();
                var result = await client.GetAllBudgetsAsync();

                    retVal.AddRange(result);
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message));
                throw;
            }
            return retVal;
        }

        private async Task<IList<Budget>> LoadCachedGroups()
        {
            var retVal = new List<Budget>();
            foreach (var item in await StorageUtility.ListItems(GROUP_FOLDER))
            {
                try
                {
                    var group = await StorageUtility.RestoreItem<Budget>(GROUP_FOLDER, item);
                    retVal.Add(group);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
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

        public TransactionList TransactionList { get; set; }

        public ObservableCollection<Budget> BudgetList { get; set; }

        public StartupInfo LoadCounts { get; set; }

        #endregion

        #region Save
        public async Task SyncTransactions()
        {
            try
            {
                var transactions = await LoadCachedTransactions();

                var client = new BMAService.MainClient();
                var result = await client.SyncTransactionsAsync(transactions.ToObservableCollection());

                ApplicationData.Current.LocalSettings.Values["IsSync"] = true;

                await UpdateCacheTransactions(result);
                SetupTransactionList(result);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task SaveTransactionChanges()
        {
            await SaveTransaction(TransactionList.Where(i => i.HasChanges).ToObservableCollection());
        }

        public async Task SaveTransaction(ObservableCollection<Transaction> transactions)
        {
            try
            {
                var result = this.TransactionList.ToObservableCollection();
                
                if (!App.Instance.IsOnline)
                {
                    result = result.Where(i => !i.IsDeleted).ToObservableCollection();
                    ApplicationData.Current.LocalSettings.Values["IsSync"] = false;
                }
                else
                {
                    var client = new BMAService.MainClient();
                    result = await client.SaveTransactionsAsync(transactions);
                    ApplicationData.Current.LocalSettings.Values["IsSync"] = true;
                }

                await UpdateCacheTransactions(result);
                SetupTransactionList(result);
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
                var result = await client.SyncBudgetsAsync(budgets.ToObservableCollection());

                ApplicationData.Current.LocalSettings.Values["IsSync"] = true;

                await UpdateCacheBudgets(result);
                SetupBudgetList(result);
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
                
                if(!App.Instance.IsOnline)
                {
                    result = result.Where(i => !i.IsDeleted).ToObservableCollection();
                    ApplicationData.Current.LocalSettings.Values["IsSync"] = false;
                }
                else
                {
                    var client = new BMAService.MainClient();
                    result = await client.SaveBudgetsAsync(budgets);
                    ApplicationData.Current.LocalSettings.Values["IsSync"] = true;
                }
                
                await UpdateCacheBudgets(result);
                SetupBudgetList(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private async Task UpdateCacheBudgets(ICollection<Budget> budgetList)
        {
            await StorageUtility.Clear(BUDGETS_FOLDER);
            foreach (var item in budgetList)
                await StorageUtility.SaveItem(BUDGETS_FOLDER, item, item.BudgetId);
        }

        private async Task UpdateCacheTransactions(ICollection<Transaction> transList)
        {
            await StorageUtility.Clear(TRANSACTIONS_FOLDER);
            foreach (var item in transList)
                await StorageUtility.SaveItem(TRANSACTIONS_FOLDER, item, item.TransactionId);
        }
    }
}
