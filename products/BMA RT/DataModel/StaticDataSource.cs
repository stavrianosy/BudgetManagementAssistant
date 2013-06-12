using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel;

using BMA.BusinessLogic;
using BMA.Common;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace BMA.DataModel
{
    public class StaticDataSource:LayoutAwarePage
    {
        #region Private members
        private const string STATIC_FOLDER = "Static";
        private const string STATIC_CATEGORY_FOLDER = "Static_Category";
        private const string STATIC_TYPETRANSACTION_FOLDER = "Static_TypeTransaction";
        private const string STATIC_TYPETRANSACTIONREASON_FOLDER = "Static_TypeTransactionReason";
        private const string STATIC_TYPESAVINGFREQUENCY_FOLDER = "Static_TypeSavingsFrequency";
        private const string STATIC_NOTIFICATION_FOLDER = "Static_Notification";
        private const string STATIC_TYPEFREQUENCY_FOLDER = "Static_TypeFrequency";
        private const string STATIC_TYPEINTERVAL_FOLDER = "Static_TypeInterval";
        private const string STATIC_BUDGETTHRESHOLD_FOLDER = "Static_BudgetThreshold";
        private const string STATIC_USER_FOLDER = "Static_User";

        private static readonly string Utf8ByteOrderMark =
                    Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble(), 0, Encoding.UTF8.GetPreamble().Length);

        #endregion

        #region Public members
        public ObservableCollection<Category> CategoryList { get; set; }
        public ObservableCollection<TypeTransaction> TypeTransactionList { get; set; }
        public ObservableCollection<TypeTransactionReason> TypeTransactionReasonList { get; set; }
        public ObservableCollection<TypeSavingsDencity> TypeSavingsDencityList { get; set; }
        public ObservableCollection<TypeFrequency> TypeFrequencyList { get; set; }
        public ObservableCollection<TypeInterval> IntervalList { get; set; }
        public ObservableCollection<Notification> NotificationList { get; set; }
        public ObservableCollection<BudgetThreshold> BudgetThresholdList { get; set; }
        #endregion

        #region Constructor
        public StaticDataSource()
        {
            CategoryList = new ObservableCollection<Category>();
            TypeTransactionList = new ObservableCollection<TypeTransaction>();
            TypeTransactionReasonList = new ObservableCollection<TypeTransactionReason>();
            TypeSavingsDencityList = new ObservableCollection<TypeSavingsDencity>();
            NotificationList = new ObservableCollection<Notification>();
            TypeFrequencyList = new ObservableCollection<TypeFrequency>();
            IntervalList = new ObservableCollection<TypeInterval>();
            BudgetThresholdList = new ObservableCollection<BudgetThreshold>();
        }
        #endregion

        #region Public methods
        public async Task LoadStaticData()
        {
            StaticTypeList existing = new StaticTypeList();
            var info = NetworkInformation.GetInternetConnectionProfile();
            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                existing.TypeTransactions = await LoadCachedTypeTransaction(STATIC_TYPETRANSACTION_FOLDER);
                existing.Categories = await LoadCachedCategory(STATIC_CATEGORY_FOLDER);
                existing.TypeTransactionReasons = await LoadCachedTypeTransactionReason(STATIC_TYPETRANSACTIONREASON_FOLDER);
                existing.TypeSavingsDencities = await LoadCachedTypeSavingsDencity(STATIC_TYPESAVINGFREQUENCY_FOLDER);
                existing.Notifications = await LoadCachedNotification(STATIC_NOTIFICATION_FOLDER);
                existing.TypeFrequencies = await LoadCachedTypeFrequency(STATIC_TYPEFREQUENCY_FOLDER);
                existing.TypeIntervals = await LoadCachedInterval(STATIC_TYPEINTERVAL_FOLDER);
                existing.BudgetThresholds = await LoadCachedBudgetThreshold(STATIC_BUDGETTHRESHOLD_FOLDER);                
            }
            else
            {
                existing = await LoadLiveStaticData();
                await UpdateCacheStaticData(existing);
            }
//            var live = await LoadLiveStaticData();

            LoadTypeTransactionData(existing.TypeTransactions);
            LoadTypeCategoryData(existing.Categories);
            LoadTypeSavingsDencityData(existing.TypeSavingsDencities);
            LoadTypeTransactionReasonData(existing.TypeTransactionReasons);
            LoadNotificationData(existing.Notifications);
            LoadTypeFrequencyData(existing.TypeFrequencies);
            LoadIntervalData(existing.TypeIntervals);
            LoadBudgetThresholdData(existing.BudgetThresholds);
            
        }

        public void LoadTypeTransactionData(ICollection<TypeTransaction> existing)
        {
            existing = existing ?? new List<TypeTransaction>();

            TypeTransactionList.Clear();

            foreach (var item in existing)
                TypeTransactionList.Add(item);

        }
        
        public void LoadTypeCategoryData(ICollection<Category> existing)
        {
            existing = existing ?? new List<Category>();

            CategoryList.Clear();

            foreach (var item in existing)
                CategoryList.Add(item);
        }

        public void LoadTypeSavingsDencityData(ICollection<TypeSavingsDencity> existing)
        {
            existing = existing ?? new List<TypeSavingsDencity>();

            TypeSavingsDencityList.Clear();

            foreach (var item in existing)
                TypeSavingsDencityList.Add(item);
        }

        public void LoadTypeTransactionReasonData(ICollection<TypeTransactionReason> existing)
        {
            existing = existing ?? new List<TypeTransactionReason>();

            TypeTransactionReasonList.Clear();

            foreach (var item in existing)
                TypeTransactionReasonList.Add(item);
        }

        public void LoadNotificationData(ICollection<Notification> existing)
        {
            existing = existing ?? new List<Notification>();

            NotificationList.Clear();

            foreach (var item in existing)
                NotificationList.Add(item);
        }

        public void LoadTypeFrequencyData(ICollection<TypeFrequency> existing)
        {
            existing = existing ?? new List<TypeFrequency>();

            TypeFrequencyList.Clear();

            foreach (var item in existing)
                TypeFrequencyList.Add(item);
        }

        public void LoadIntervalData(ICollection<TypeInterval> existing)
        {
            existing = existing ?? new List<TypeInterval>();

            IntervalList.Clear();

            foreach (var item in existing)
                IntervalList.Add(item);
        }

        public void LoadBudgetThresholdData(ICollection<BudgetThreshold> existing)
        {
            existing = existing ?? new List<BudgetThreshold>();

            BudgetThresholdList.Clear();

            foreach (var item in existing)
                BudgetThresholdList.Add(item);
        }

        public async Task LoadUser(User user)
        {
            User existing = new User();
            var info = NetworkInformation.GetInternetConnectionProfile();
            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                var query = await LoadCachedUser(STATIC_USER_FOLDER);
                existing = query.Where(i => i.UserName == user.UserName).Single();
            }
            else 
            {
                existing = await LoadLiveUser(user);
                await UpdateCacheUserData(existing);
            }

            if (existing.UserId > 0)
            {
                App.Instance.User.UserId = existing.UserId;
                App.Instance.User.Email = existing.Email;
            }
            else
                throw new Exception("User has no authentication");
        }
#endregion

        #region Private methods
        private static async Task<StaticTypeList> LoadLiveStaticData()
        {
            var retVal = new StaticTypeList();
            var info = NetworkInformation.GetInternetConnectionProfile();

            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                return retVal;
            }

            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.GetAllStaticDataAsync();

                retVal = result;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message));
                throw;
            }

            return retVal;
        }

        private static async Task<User> LoadLiveUser(User user)
        {
            User retVal = null;
            var info = NetworkInformation.GetInternetConnectionProfile();

            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                return retVal;
            }

            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.AuthenticateUserAsync(user);

                retVal = result;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message));
                throw;
            }

            return retVal;
        }

        #region Load Cached Data
        private async Task<List<Category>> LoadCachedCategory(string folder)
        {
            var retVal = new List<Category>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<Category>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<TypeTransaction>> LoadCachedTypeTransaction(string folder)
        {
            var retVal = new List<TypeTransaction>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeTransaction>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<TypeTransactionReason>> LoadCachedTypeTransactionReason(string folder)
        {
            var retVal = new List<TypeTransactionReason>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeTransactionReason>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<TypeSavingsDencity>> LoadCachedTypeSavingsDencity(string folder)
        {
            var retVal = new List<TypeSavingsDencity>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeSavingsDencity>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<Notification>> LoadCachedNotification(string folder)
        {
            var retVal = new List<Notification>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<Notification>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<TypeFrequency>> LoadCachedTypeFrequency(string folder)
        {
            var retVal = new List<TypeFrequency>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeFrequency>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<TypeInterval>> LoadCachedInterval(string folder)
        {
            var retVal = new List<TypeInterval>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeInterval>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<BudgetThreshold>> LoadCachedBudgetThreshold(string folder)
        {
            var retVal = new List<BudgetThreshold>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<BudgetThreshold>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<User>> LoadCachedUser(string folder)
        {
            var retVal = new List<User>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<User>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }
        #endregion

        #endregion
        
        #region Save

        public async Task SyncStaticData()
        {
            try
            {
                var staticData = new StaticTypeList();

                staticData.BudgetThresholds = await LoadCachedBudgetThreshold(STATIC_BUDGETTHRESHOLD_FOLDER);
                staticData.Categories = await LoadCachedCategory(STATIC_CATEGORY_FOLDER);
                staticData.TypeIntervals = await LoadCachedInterval(STATIC_TYPEINTERVAL_FOLDER);
                staticData.Notifications = await LoadCachedNotification(STATIC_NOTIFICATION_FOLDER);
                staticData.TypeFrequencies = await LoadCachedTypeFrequency(STATIC_TYPEFREQUENCY_FOLDER);
                //staticData.TypeSavingsDencities = await LoadCachedTypeSavingsDencity(STATIC_TYPESAVINGFREQUENCY_FOLDER);
                staticData.TypeTransactions = await LoadCachedTypeTransaction(STATIC_TYPETRANSACTION_FOLDER);
                staticData.TypeTransactionReasons = await LoadCachedTypeTransactionReason(STATIC_TYPETRANSACTIONREASON_FOLDER);

                var client = new BMAStaticDataService.StaticClient();
                var result = await client.SyncStaticDataAsync(staticData);

                ApplicationData.Current.LocalSettings.Values["IsSync"] = true;

                await UpdateCacheBudgetThresholds(result.BudgetThresholds);
                await UpdateCacheCategory(result.Categories);
                await UpdateCacheNotification(result.Notifications);
                await UpdateCacheTypeFrequency(result.TypeFrequencies);
                await UpdateCacheTypeInterval(result.TypeIntervals);
                //await UpdateCacheTypeSavingsDencity(result.TypeSavingsDencities);
                await UpdateCacheTypeTransaction(result.TypeTransactions);
                await UpdateCacheTypeTransactionReason(result.TypeTransactionReasons);

                LoadBudgetThresholdData(result.BudgetThresholds);
                LoadTypeCategoryData(result.Categories);
                LoadTypeFrequencyData(result.TypeFrequencies);
                LoadTypeSavingsDencityData(result.TypeSavingsDencities);
                LoadTypeTransactionData(result.TypeTransactions);
                LoadTypeTransactionReasonData(result.TypeTransactionReasons);
                LoadNotificationData(result.Notifications);
                LoadIntervalData(result.TypeIntervals);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveCategory(ObservableCollection<Category> categories)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.SaveCategoriesAsync(categories);

                await UpdateCacheCategory(result);
                LoadTypeCategoryData(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveTypeTransactionReason(ObservableCollection<TypeTransactionReason> typeTransactionReason)
        {
            try
            {
            var client = new BMAStaticDataService.StaticClient();
            var result = await client.SaveTypeTransactionReasonsAsync(typeTransactionReason);

            await UpdateCacheTypeTransactionReason(result);
            LoadTypeTransactionReasonData(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveNotification(ObservableCollection<Notification> notification)
        {
            try
            {
            var client = new BMAStaticDataService.StaticClient();
            var result = await client.SaveNotificationsAsync(notification);

            await UpdateCacheNotification(result);
            LoadNotificationData(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveTypeTransaction(ObservableCollection<TypeTransaction> typeTransaction)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.SaveTypeTransactionsAsync(typeTransaction);

                await UpdateCacheTypeTransaction(result);
                LoadTypeTransactionData(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveTypeFrequency(ObservableCollection<TypeFrequency> typeFrequency)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.SaveTypeFrequenciesAsync(typeFrequency);

                await UpdateCacheTypeFrequency(result);
                LoadTypeFrequencyData(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveInterval(ObservableCollection<TypeInterval> typeInterval)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.SaveTypeIntervalsAsync(typeInterval);

                await UpdateCacheTypeInterval(result);
                LoadIntervalData(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveBudgetThreshold(ObservableCollection<BudgetThreshold> budgetThreshold)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.SaveBudgetThresholdsAsync(budgetThreshold);

                await UpdateCacheBudgetThresholds(result);
                LoadBudgetThresholdData(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RegisterUser(User user)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.RegisterUserAsync(user);

                user.HasChanges = false;
                user.UserId = result.UserId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ChangePassword(User user)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.ChangePasswordAsync(user);

                user.HasChanges = false;
                user.Password = result.Password;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ForgotPassword(User user)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.ForgotPasswordAsync(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Update Cache Data
        private async Task UpdateCacheUserData(User user)
        {
            await StorageUtility.Clear(STATIC_USER_FOLDER);
            await StorageUtility.SaveItem(STATIC_USER_FOLDER, user, user.UserId);
        }
        
        private async Task UpdateCacheStaticData(StaticTypeList staticDataList)
        {
            await StorageUtility.Clear(STATIC_CATEGORY_FOLDER);
            await StorageUtility.Clear(STATIC_TYPESAVINGFREQUENCY_FOLDER);
            await StorageUtility.Clear(STATIC_TYPETRANSACTION_FOLDER);
            await StorageUtility.Clear(STATIC_TYPETRANSACTIONREASON_FOLDER);

            foreach (var item in staticDataList.Categories)
                await StorageUtility.SaveItem(STATIC_CATEGORY_FOLDER, item, item.CategoryId);

            foreach (var item in staticDataList.TypeTransactions)
                await StorageUtility.SaveItem(STATIC_TYPETRANSACTION_FOLDER, item, item.TypeTransactionId);

            foreach (var item in staticDataList.TypeTransactionReasons)
                await StorageUtility.SaveItem(STATIC_TYPETRANSACTIONREASON_FOLDER, item, item.TypeTransactionReasonId);

            foreach (var item in staticDataList.TypeSavingsDencities)
                await StorageUtility.SaveItem(STATIC_TYPESAVINGFREQUENCY_FOLDER, item, item.TypeSavingsDencityId);
        }

        private async Task UpdateCacheBudgetThresholds(ICollection<BudgetThreshold> budgetThresholdList)
        {
            await StorageUtility.Clear(STATIC_BUDGETTHRESHOLD_FOLDER);
            foreach (var item in budgetThresholdList)
                await StorageUtility.SaveItem(STATIC_BUDGETTHRESHOLD_FOLDER, item, item.BudgetThresholdId);
        }

        private async Task UpdateCacheCategory(ICollection<Category> categoryList)
        {
            await StorageUtility.Clear(STATIC_CATEGORY_FOLDER);
            foreach (var item in categoryList)
                await StorageUtility.SaveItem(STATIC_CATEGORY_FOLDER, item, item.CategoryId);
        }

        private async Task UpdateCacheNotification(ICollection<Notification> notificationList)
        {
            await StorageUtility.Clear(STATIC_NOTIFICATION_FOLDER);
            foreach (var item in notificationList)
                await StorageUtility.SaveItem(STATIC_NOTIFICATION_FOLDER, item, item.NotificationId);
        }

        private async Task UpdateCacheTypeFrequency(ICollection<TypeFrequency> typeFrequencyList)
        {
            await StorageUtility.Clear(STATIC_TYPEFREQUENCY_FOLDER);
            foreach (var item in typeFrequencyList)
                await StorageUtility.SaveItem(STATIC_TYPEFREQUENCY_FOLDER, item, item.TypeFrequencyId);
        }

        private async Task UpdateCacheTypeInterval(ICollection<TypeInterval> typeIntervalList)
        {
            await StorageUtility.Clear(STATIC_TYPEINTERVAL_FOLDER);
            foreach (var item in typeIntervalList)
                await StorageUtility.SaveItem(STATIC_TYPEINTERVAL_FOLDER, item, item.TypeIntervalId);
        }

        private async Task UpdateCacheTypeSavingsDencity(ICollection<TypeSavingsDencity> typeSavingsDencityList)
        {
            await StorageUtility.Clear(STATIC_TYPESAVINGFREQUENCY_FOLDER);
            foreach (var item in typeSavingsDencityList)
                await StorageUtility.SaveItem(STATIC_TYPESAVINGFREQUENCY_FOLDER, item, item.TypeSavingsDencityId);
        }

        private async Task UpdateCacheTypeTransaction(ICollection<TypeTransaction> typeTransactionList)
        {
            await StorageUtility.Clear(STATIC_TYPETRANSACTION_FOLDER);
            foreach (var item in typeTransactionList)
                await StorageUtility.SaveItem(STATIC_TYPETRANSACTION_FOLDER, item, item.TypeTransactionId);
        }

        private async Task UpdateCacheTypeTransactionReason(ICollection<TypeTransactionReason> typeTransactionReasonList)
        {
            await StorageUtility.Clear(STATIC_TYPETRANSACTIONREASON_FOLDER);
            foreach (var item in typeTransactionReasonList)
                await StorageUtility.SaveItem(STATIC_TYPETRANSACTIONREASON_FOLDER, item, item.TypeTransactionReasonId);
        }
        #endregion
    }
}
