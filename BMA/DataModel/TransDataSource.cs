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
        private const string GROUP_FOLDER = "Groups";
        private const string TRANSACTIONS_FOLDER = "Transactions";
        private const string BUDGETS_FOLDER = "Budgets";

        private static readonly string Utf8ByteOrderMark =
            Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble(), 0, Encoding.UTF8.GetPreamble().Length);

        #region Constructor
        public TransDataSource()
        {
            GroupList = new ObservableCollection<Budget>();
            TransactionList = new ObservableCollection<Transaction>();
            BudgetList = new ObservableCollection<Budget>();

            if (DesignMode.DesignModeEnabled)
            {
                LoadTestGroups();
            }
        }
        #endregion

        #region Load
        public async Task LoadTransactions()
        {
            if (Config.TestOnly)
                LoadTestGroups();
            else
                await LoadAllTransactions(0);
        }

        public async Task LoadBudgets()
        {
            if (Config.TestOnly)
                LoadTestGroups();
            else
                await LoadAllBudgets();
        }


        public async Task LoadTransactionsForBudget(int budgetId)
        {
            await LoadAllTransactions(budgetId);
        }

        public async Task LoadAllGroups()
        {
            var existing = await LoadCachedGroups();
            var live = await LoadLiveGroups();

            //foreach (var liveGroup in live
            //    .Where(liveGroup => !existing.Contains(liveGroup, new BaseItemComparer())))
            //{
            //    existing.Add(liveGroup);
            //    await StorageUtility.SaveItem(GROUP_FOLDER, liveGroup);
            //}

            //foreach (var group in existing.OrderBy(e => e.Title))
            //    foreach (var group in live.OrderBy(e => e.Name))
            //    {
            //        GroupList.Add(group);
            //    }
            LoadCounts = live;
        }

        public async Task LoadAllTransactions(int budgetId)
        {
            ICollection<Transaction> existing = null;

            var info = NetworkInformation.GetInternetConnectionProfile();
            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                existing = await LoadCachedTransactions();                
            }
            else
            {
                existing = await LoadLiveTransactions(budgetId);
                await UpdateCacheTransactions(existing);
            }

            foreach (var trans in existing)
            {
                if (TransactionList.Where(t => t.TransactionId == trans.TransactionId).Count() == 0)
                    TransactionList.Add(trans);
            }    
        }

        public async Task LoadAllBudgets()
        {
            ICollection<Budget> existing = null;


            var info = NetworkInformation.GetInternetConnectionProfile();
            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                existing = await LoadCachedBudgets();
            }
            else
            {
                existing = await LoadLiveBudgets();
                await UpdateCacheBudgets(existing);
            }

            foreach (var budget in existing)
            {
                if (BudgetList.Where(b => b.BudgetId == budget.BudgetId).Count() == 0)
                    BudgetList.Add(budget);
            }
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

                //foreach (var item in result)
                //{
                //    var transList = item.Value.Where(c => c.Category.CategoryId == item.Key.CategoryId).ToList();
                //    item.Key.Transactions = new List<Transaction>();
                //    item.Key.Transactions.AddRange(transList);
                //    retVal.Add(item.Key);
                //}
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

        private static async Task<StartupInfo> LoadLiveGroups()
        {

            //var retVal2 = new List<TransGroup>();
            var retVal = new StartupInfo();
            var info = NetworkInformation.GetInternetConnectionProfile();

            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                return retVal;
            }

            try
            {
                var client = new BMAService.MainClient();
                var result = await client.LoadItemCountsAsync();

                //foreach (var item in result)
                //{
                //    var group = new Budget
                //    {
                //        BudgetId = item.BudgetId,
                //        Name = item.Name,
                //        Amount = item.Amount,
                //        Comments = item.Comments,
                //        FromDate = item.FromDate
                //    };

                //retVal.Add(group);
                //retVal.Add(item);
                //}
                retVal = result;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message));
                //throw;
            }

            return retVal;
        }

        private static async Task<ICollection<TransGroup>> LoadLiveGroupsRSS()
        {
            var retVal = new List<TransGroup>();
            var info = NetworkInformation.GetInternetConnectionProfile();

            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                return retVal;
            }

            var content = await PathIO.ReadTextAsync("ms-appx:///Assets/Transactions.js");
            content = content.Trim(Utf8ByteOrderMark.ToCharArray());
            var transactions = JsonArray.Parse(content);

            foreach (JsonValue item in transactions)
            {
                MessageDialog dialog = null;

                try
                {
                    var uri = item.GetObject()["TransUri"].GetString();
                    //var group = new BlogGroup
                    //{
                    //    Id = uri,
                    //    RssUri = new Uri(uri, UriKind.Absolute)
                    //};

                    //var client = GetSyndicationClient();
                    //var feed = await client.RetrieveFeedAsync(group.RssUri);

                    //group.Title = feed.Title.Text;

                    //retVal.Add(group);
                }
                catch (Exception ex)
                {
                    dialog = new MessageDialog(ex.Message);
                }
                if (dialog != null)
                {
                    await dialog.ShowAsync();
                }
            }

            return retVal;
        }

        private void LoadTestGroups()
        {
        }

        private static async Task<IList<TransItem>> LoadCachedItems(Budget group)
        {
            var retVal = new List<TransItem>();

            var groupFolder = group.BudgetId.GetHashCode().ToString();

            foreach (var item in await StorageUtility.ListItems(groupFolder))
            {
                try
                {
                    var post = await StorageUtility.RestoreItem<TransItem>(groupFolder, item);
                    //post.Group = group;
                    retVal.Add(post);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return retVal;
        }

        public async Task LoadAllItems(Budget group)
        {
            var cachedItems = await LoadCachedItems(group);
        }

        public ObservableCollection<Budget> GroupList { get; set; }

        public ObservableCollection<Transaction> TransactionList { get; set; }

        public ObservableCollection<Budget> BudgetList { get; set; }

        public StartupInfo LoadCounts { get; set; }

        private static SyndicationClient GetSyndicationClient()
        {
            var client = new SyndicationClient
            {
                BypassCacheOnRetrieve = false
            };
            //client.SetRequestHeader("user-agent", USER_AGENT);
            return client;
        }
        
        private static HttpClient GetClient()
        {
            var retVal = new HttpClient
            {
                MaxResponseContentBufferSize = 999999
            };
            //retVal.DefaultRequestHeaders.Add("user-agent", USER_AGENT);
            return retVal;
        }
        
        #endregion

        #region Save
        public async Task SaveTransaction(ObservableCollection<Transaction> transactions)
        {
            var client = new BMAService.MainClient();
            var result = await client.SaveTransactionsAsync(transactions);
            await UpdateCacheTransactions(result);
        }

        public async Task SaveBudgets(ObservableCollection<Budget> budgets)
        {
            var client = new BMAService.MainClient();
            //var result = await client.(budgets);
            //await UpdateCacheBudgets(result);
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
