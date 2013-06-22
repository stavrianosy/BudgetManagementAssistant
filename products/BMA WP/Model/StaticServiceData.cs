using BMA.BusinessLogic;
using BMA_WP.BMAStaticDataService;
using BMA_WP.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Storage;

namespace BMA_WP.Model
{
    public class StaticServiceData
    {
        #region Privat Memebrs
        private const string STATIC_CATEGORY_FOLDER = "Static_Category";
        private const string STATIC_TYPETRANSACTION_FOLDER = "Static_TypeTransaction";
        private const string STATIC_TYPETRANSACTIONREASON_FOLDER = "Static_TypeTransactionReason";
        private const string STATIC_TYPESAVINGFREQUENCY_FOLDER = "Static_TypeSavingsFrequency";
        private const string STATIC_NOTIFICATION_FOLDER = "Static_Notification";
        private const string STATIC_TYPEFREQUENCY_FOLDER = "Static_TypeFrequency";
        private const string STATIC_TYPEINTERVAL_FOLDER = "Static_TypeInterval";
        private const string STATIC_BUDGETTHRESHOLD_FOLDER = "Static_BudgetThreshold";
        private const string STATIC_USER_FOLDER = "Static_User";

        static readonly string Utf8ByteOrderMark =
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
        public StaticServiceData()
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

        #region Events
        #endregion

        #region Private methods
        #endregion

        #region Public methods
        public async Task LoadStaticData()
        {
            StaticTypeList existing = new StaticTypeList();

            if (!App.Instance.IsOnline)
            {
                existing.TypeTransactions = await LoadCachedTypeTransaction(STATIC_TYPETRANSACTION_FOLDER);
                existing.Categories = await LoadCachedCategory(STATIC_CATEGORY_FOLDER);
                existing.TypeTransactionReasons = await LoadCachedTypeTransactionReason(STATIC_TYPETRANSACTIONREASON_FOLDER);
                existing.TypeSavingsDencities = await LoadCachedTypeSavingsDencity(STATIC_TYPESAVINGFREQUENCY_FOLDER);
                existing.Notifications = await LoadCachedNotification(STATIC_NOTIFICATION_FOLDER);
                existing.TypeFrequencies = await LoadCachedTypeFrequency(STATIC_TYPEFREQUENCY_FOLDER);
                existing.TypeIntervals = await LoadCachedInterval(STATIC_TYPEINTERVAL_FOLDER);
                existing.BudgetThresholds = await LoadCachedBudgetThreshold(STATIC_BUDGETTHRESHOLD_FOLDER);

                SetupAllData(existing);
            }
            else
            {
                var client = new BMAStaticDataService.StaticClient();
                client.GetAllStaticDataAsync();
                client.GetAllCategoriesAsync();
                client.GetAllTypeTransactionReasonsAsync();
                client.GetAllStaticDataCompleted += async (o, e) =>
                {
                    try
                    {
                        var result = e.Result;

                        App.Instance.IsSync = true;

                        await UpdateCacheStaticData(result);

                        SetupTypeTransactionData(result.TypeTransactions);
                        SetupTypeSavingsDencityData(result.TypeSavingsDencities);
                        SetupNotificationData(result.Notifications);
                        SetupTypeFrequencyData(result.TypeFrequencies);
                        SetupIntervalData(result.TypeIntervals);
                        SetupBudgetThresholdData(result.BudgetThresholds);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
                client.GetAllCategoriesCompleted += async (o, e) =>
                {
                    try
                    {
                        var result = e.Result;

                        App.Instance.IsSync = true;

                        await UpdateCacheCategory(result);
                        SetupTypeCategoryData(result);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
                client.GetAllTypeTransactionReasonsCompleted += async (o, e) =>
                {
                    try
                    {
                        var result = e.Result;

                        App.Instance.IsSync = true;

                        await UpdateCacheTypeTransactionReason(result);
                        SetupTypeTransactionReasonData(result);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                };
            }

        }

        public async Task LoadUser(User user, Action<User, Exception> callback)
        {
            User existing = new User();
            if (!App.Instance.IsOnline)
            {
                var query = await LoadCachedUser(STATIC_USER_FOLDER);
                existing = query.Where(i => i.UserName == user.UserName).Single();
            }
            else
            {
                var client = new BMAStaticDataService.StaticClient();
                client.AuthenticateUserAsync(user, callback);
                client.AuthenticateUserCompleted += async (sender,eventargs) => {
                    try
                    {
                        var userCallback = eventargs.UserState as Action<User, Exception>;
                        if (userCallback == null)
                            return;

                        if (eventargs.Error != null)
                        {
                            userCallback(null, eventargs.Error);
                            return;
                        }
                        userCallback(eventargs.Result, null);

                        existing = eventargs.Result;
                        await UpdateCacheUserData(existing);
                        if (existing.UserId > 0)
                        {
                            App.Instance.User.UserId = existing.UserId;
                            App.Instance.User.Email = existing.Email;

                            App.Instance.IsUserAuthenticated = true;
                        }
                        else
                        {
                            App.Instance.IsUserAuthenticated = false;

                            throw new Exception("User has no authentication");
                        }
                    }
                    catch (Exception){throw;}
                };                
            }            
        }
        #endregion

        #region Private Methods

        private void SetupAllData(StaticTypeList existing)
        {
            SetupTypeTransactionData(existing.TypeTransactions);
            SetupTypeCategoryData(existing.Categories);
            SetupTypeSavingsDencityData(existing.TypeSavingsDencities);
            SetupTypeTransactionReasonData(existing.TypeTransactionReasons);
            SetupNotificationData(existing.Notifications);
            SetupTypeFrequencyData(existing.TypeFrequencies);
            SetupIntervalData(existing.TypeIntervals);
            SetupBudgetThresholdData(existing.BudgetThresholds);
        }

        private void SetupTypeTransactionData(ICollection<TypeTransaction> existing)
        {
            existing = existing ?? new List<TypeTransaction>();

            TypeTransactionList.Clear();

            foreach (var item in existing)
                TypeTransactionList.Add(item);

        }

        private void SetupTypeCategoryData(ICollection<Category> existing)
        {
            existing = existing ?? new List<Category>();

            CategoryList.Clear();

            foreach (var item in existing)
                CategoryList.Add(item);
        }

        private void SetupTypeSavingsDencityData(ICollection<TypeSavingsDencity> existing)
        {
            existing = existing ?? new List<TypeSavingsDencity>();

            TypeSavingsDencityList.Clear();

            foreach (var item in existing)
                TypeSavingsDencityList.Add(item);
        }

        private void SetupTypeTransactionReasonData(ICollection<TypeTransactionReason> existing)
        {
            existing = existing ?? new List<TypeTransactionReason>();

            TypeTransactionReasonList.Clear();

            foreach (var item in existing)
                TypeTransactionReasonList.Add(item);
        }

        private void SetupNotificationData(ICollection<Notification> existing)
        {
            existing = existing ?? new List<Notification>();

            NotificationList.Clear();

            foreach (var item in existing)
                NotificationList.Add(item);
        }

        private void SetupTypeFrequencyData(ICollection<TypeFrequency> existing)
        {
            existing = existing ?? new List<TypeFrequency>();

            TypeFrequencyList.Clear();

            foreach (var item in existing)
                TypeFrequencyList.Add(item);
        }

        private void SetupIntervalData(ICollection<TypeInterval> existing)
        {
            existing = existing ?? new List<TypeInterval>();

            IntervalList.Clear();

            foreach (var item in existing)
                IntervalList.Add(item);
        }

        private void SetupBudgetThresholdData(ICollection<BudgetThreshold> existing)
        {
            existing = existing ?? new List<BudgetThreshold>();

            BudgetThresholdList.Clear();

            foreach (var item in existing)
                BudgetThresholdList.Add(item);
        }
        #endregion

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

        #region Save
        public async Task SaveCategory(ObservableCollection<Category> categories)
        {
            try
            {
                var result = this.CategoryList.ToObservableCollection();

                if (!App.Instance.IsOnline)
                {
                    result = result.Where(i => !i.IsDeleted).ToObservableCollection();
                    ApplicationData.Current.LocalSettings.Values["IsSync"] = false;
                }
                else
                {
                    var client = new BMAStaticDataService.StaticClient();
                    client.SaveCategoriesAsync(categories, App.Instance.User);
                    client.SaveCategoriesCompleted += async (sender, completedEventArgs) =>
                    {
                        if (completedEventArgs.Error == null)
                        {
                            await UpdateCacheCategory(completedEventArgs.Result);
                            SetupTypeCategoryData(completedEventArgs.Result);
                        }

                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveTransactionReason(ObservableCollection<TypeTransactionReason> transactionReason)
        {
            try
            {
                var result = this.TypeTransactionReasonList.ToObservableCollection();

                if (!App.Instance.IsOnline)
                {
                    result = result.Where(i => !i.IsDeleted).ToObservableCollection();
                    ApplicationData.Current.LocalSettings.Values["IsSync"] = false;
                }
                else
                {
                    var client = new BMAStaticDataService.StaticClient();
                    client.SaveTypeTransactionReasonsAsync(transactionReason);
                    client.SaveTypeTransactionReasonsCompleted += async (sender, completedEventArgs) =>
                    {
                        if (completedEventArgs.Error == null)
                        {
                            await UpdateCacheTypeTransactionReason(completedEventArgs.Result);
                            SetupTypeTransactionReasonData(completedEventArgs.Result);
                        }

                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task RegisterUser(User user, Action<User, Exception> callback)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                client.RegisterUserAsync(user, callback);
                client.RegisterUserCompleted += (sender, eventArgs) =>
                {
                    try
                    {
                        var userCallback = eventArgs.UserState as Action<User, Exception>;
                        if (userCallback == null)
                            return;

                        if (eventArgs.Error != null)
                        {
                            userCallback(null, eventArgs.Error);
                            return;
                        }

                        user.HasChanges = false;
                        user.UserId = eventArgs.Result.UserId;

                        userCallback(eventArgs.Result, null);
                    }
                    catch (Exception) { throw; }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ForgotPassword(User user, Action<User, Exception> callback)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                client.ForgotPasswordAsync(user, callback);
                client.ForgotPasswordCompleted += (sender, eventArgs) =>
                {
                    try
                    {
                        var userCallback = eventArgs.UserState as Action<User, Exception>;
                        if (userCallback == null)
                            return;

                        if (eventArgs.Error != null)
                        {
                            userCallback(null, eventArgs.Error);
                            return;
                        }

                        userCallback(eventArgs.Result, null);
                    }
                    catch (Exception) { throw; }
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ChangePassword(User user, Action<User, Exception> callback)
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                client.ChangePasswordAsync(user, callback);
                client.ChangePasswordCompleted += (sender, eventArgs) =>
                {
                    try
                    {
                        var userCallback = eventArgs.UserState as Action<User, Exception>;
                        if (userCallback == null)
                            return;

                        if (eventArgs.Error != null)
                        {
                            userCallback(null, eventArgs.Error);
                            return;
                        }

                        user.HasChanges = false;
                        user.Password = eventArgs.Result.Password;

                        userCallback(eventArgs.Result, null);
                    }
                    catch (Exception) { throw; }
                };
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
            //await StorageUtility.Clear(STATIC_CATEGORY_FOLDER);
            await StorageUtility.Clear(STATIC_TYPESAVINGFREQUENCY_FOLDER);
            await StorageUtility.Clear(STATIC_TYPETRANSACTION_FOLDER);
            //await StorageUtility.Clear(STATIC_TYPETRANSACTIONREASON_FOLDER);

            //foreach (var item in staticDataList.Categories)
            //    await StorageUtility.SaveItem(STATIC_CATEGORY_FOLDER, item, item.CategoryId);

            foreach (var item in staticDataList.TypeTransactions)
                await StorageUtility.SaveItem(STATIC_TYPETRANSACTION_FOLDER, item, item.TypeTransactionId);

            //foreach (var item in staticDataList.TypeTransactionReasons)
            //    await StorageUtility.SaveItem(STATIC_TYPETRANSACTIONREASON_FOLDER, item, item.TypeTransactionReasonId);

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
