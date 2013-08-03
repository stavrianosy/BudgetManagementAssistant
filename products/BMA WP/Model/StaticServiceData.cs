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
        #region Enumerators
        public enum ServerStatus
        {
            Communicating,
            Ok,
            Error
        }
        #endregion

        #region Privat Memebrs
        private const string STATIC_CATEGORY_FOLDER = "Static_Category";
        private const string STATIC_TYPETRANSACTION_FOLDER = "Static_TypeTransaction";
        private const string STATIC_TYPETRANSACTIONREASON_FOLDER = "Static_TypeTransactionReason";
        private const string STATIC_TYPESAVINGFREQUENCY_FOLDER = "Static_TypeSavingsFrequency";
        private const string STATIC_NOTIFICATION_FOLDER = "Static_Notification";
        private const string STATIC_TYPEFREQUENCY_FOLDER = "Static_TypeFrequency";
        private const string STATIC_TYPEINTERVAL_FOLDER = "Static_TypeInterval";
        private const string STATIC_RECURRENCE_FOLDER = "Static_Recurrence";
        private const string STATIC_BUDGETTHRESHOLD_FOLDER = "Static_BudgetThreshold";
        private const string STATIC_USER_FOLDER = "Static_User";

        static readonly string Utf8ByteOrderMark =
            Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble(), 0, Encoding.UTF8.GetPreamble().Length);

        string latestState;

        #endregion

        #region Public members
        public CategoryList CategoryList { get; set; }
        public TypeTransactionList TypeTransactionList { get; set; }
        public TypeTransactionReasonList TypeTransactionReasonList { get; set; }
        public TypeSavingsDencityList TypeSavingsDencityList { get; set; }
        public TypeFrequencyList TypeFrequencyList { get; set; }
        public TypeIntervalList IntervalList { get; set; }
        public RecurrenceRuleList RecurrenceRuleList { get; set; }
        public NotificationList NotificationList { get; set; }
        public BudgetThresholdList BudgetThresholdList { get; set; }
        #endregion

        #region Constructor
        public StaticServiceData()
        {
            CategoryList = new CategoryList();
            TypeTransactionList = new TypeTransactionList();
            TypeTransactionReasonList = new TypeTransactionReasonList();
            TypeSavingsDencityList = new TypeSavingsDencityList();
            NotificationList = new NotificationList();
            TypeFrequencyList = new TypeFrequencyList();
            IntervalList = new TypeIntervalList();
            RecurrenceRuleList = new RecurrenceRuleList();
            BudgetThresholdList = new BudgetThresholdList();
        }
        #endregion

        #region Events
        #endregion

        #region Private methods
        #endregion

        #region Public methods
        public async Task<ServerStatus> SetServerStatus(Action<ServerStatus> callback)
        {
            var result = ServerStatus.Communicating;
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                client.GetDBStatusAsync();
                client.GetDBStatusCompleted += (sender, e) =>
                {
                    if (e.Error != null || !e.Result)
                        result = ServerStatus.Error;
                    else if (e.Result)
                        result = ServerStatus.Ok;

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
        
        public async Task LoadStaticData()
        {
            StaticTypeList existing = new StaticTypeList();

            await LoadCategories();
            await LoadTypeTransactionReasons();

            await LoadTypeTransactions();
            await LoadTypeSavingsDencities();
            await LoadNotifications();
            await LoadTypeFrequencies();
            await LoadTypeIntervals();
            await LoadRecurrenceRules();
            await LoadBudgetThresholds();

            //if (App.Instance.StaticDataOnlineStatus != ServerStatus.Ok)
            //{
            //    SetupAllData(existing);
            //}
            //else
            //{
            //    var client = new BMAStaticDataService.StaticClient();
            //    client.GetAllStaticDataAsync(App.Instance.User.UserId);

            //    client.GetAllStaticDataCompleted += async (o, e) =>
            //    {
            //        try
            //        {
            //            var result = e.Result;

            //            App.Instance.IsSync = true;

            //            SetupTypeTransactionData(result.TypeTransactions, true);
            //            SetupTypeSavingsDencityData(result.TypeSavingsDencities, true);
            //            SetupNotificationData(result.Notifications, true);
            //            SetupTypeFrequencyData(result.TypeFrequencies, true);
            //            SetupIntervalData(result.TypeIntervals, true);
            //            SetupRecurrenceRuleData(result.RecurrenceRules, true);
            //            SetupBudgetThresholdData(result.BudgetThresholds, true);
            //        }
            //        catch (Exception)
            //        {
            //            throw;
            //        }
            //    };
            //}

        }

        public async Task LoadCategories()
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                await LoadCachedCategories();
            else
                await LoadLiveCategories();
        }

        public async Task LoadTypeTransactionReasons()
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                await LoadCachedTypeTransactionReasons();
            else
                await LoadLiveTypeTransactionReasons();
        }

        public async Task LoadTypeTransactions()
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                await LoadCachedTypeTransactions();
            else
                await LoadLiveTypeTransactions();
        }

        public async Task LoadTypeSavingsDencities()
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                await LoadCachedTypeSavingsDencities();
            else
                await LoadLiveTypeSavingsDencities();
        }

        public async Task LoadNotifications()
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                await LoadCachedNotifications();
            else
                await LoadLiveNotifications();
        }

        public async Task LoadTypeFrequencies()
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                await LoadCachedTypeFrequencies();
            else
                await LoadLiveTypeFrequencies();
        }

        public async Task LoadTypeIntervals()
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                await LoadCachedTypeIntervals();
            else
                await LoadLiveTypeIntervals();
        }

        public async Task LoadRecurrenceRules()
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                await LoadCachedRecurrenceRules();
            else
                await LoadLiveRecurrenceRules();
        }

        public async Task LoadBudgetThresholds()
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                await LoadCachedBudgetThresholds();
            else
                await LoadLiveBudgetThresholds();
        }

        public async Task LoadUser(User user, Action<Exception> callback)
        {
            User existing = new User();
            if (App.Instance.StaticDataOnlineStatus != ServerStatus.Ok)
            {
                try
                {
                    var query = await LoadCachedUser();
                    existing = query.Where(i => i.UserName == user.UserName).Single();
                    UserFound(existing);
                    
                    callback(null);
                }
                catch (Exception) { throw new Exception("Username or Password is incorrect.\nThis might be due to that offline mode is not updated.\nPlease connect to the live system get get the latest data."); }
            }
            else
            {
                var client = new BMAStaticDataService.StaticClient();
                client.AuthenticateUserAsync(user, callback);
                client.AuthenticateUserCompleted += async (sender,eventargs) => {
                    try
                    {
                        existing = eventargs.Result;
                        await UpdateCacheUserData(existing);
                        UserFound(existing);

                        callback(eventargs.Error);
                    }
                    catch (Exception){throw;}
                };
            }
        }

        private void UserFound(User existing)
        {
            if (existing.UserId > 0)
            {
                App.Instance.User.Update(existing);

                App.Instance.IsUserAuthenticated = true;
            }
            else
            {
                App.Instance.IsUserAuthenticated = false;

                throw new Exception("User has no authentication");
            }
        }
        #endregion

        #region Private Methods

        private void SetupAllData(StaticTypeList existing)
        {
            SetupTypeTransactionData(existing.TypeTransactions, true);
            SetupTypeSavingsDencityData(existing.TypeSavingsDencities, true);
            SetupNotificationData(existing.Notifications, true);
            SetupTypeFrequencyData(existing.TypeFrequencies, true);
            SetupIntervalData(existing.TypeIntervals, true);
            SetupBudgetThresholdData(existing.BudgetThresholds, true);
        }

        private void SetupTypeCategoryData(ICollection<Category> existing, bool removeNew)
        {
            existing = existing ?? new CategoryList();

            //sync logic
            if (removeNew)
                RemoveInsertedCategories();

            foreach (var item in existing)
            {
                var query = CategoryList.Select((x, i) => new { trans = x, Index = i }).Where(x => x.trans.CategoryId == item.CategoryId).FirstOrDefault();

                //INSERT
                if (query == null)
                    CategoryList.Add(item);
                //UPDATE
                else
                    CategoryList[query.Index] = item;

                StorageUtility.SaveItem(STATIC_CATEGORY_FOLDER, item, item.CategoryId);
            }
            var ord = CategoryList.OrderBy(x => x.Name).ToList();
            CategoryList.Clear();
            ord.ForEach(x => CategoryList.Add(x));
        }

        private void SetupTypeTransactionReasonData(ICollection<TypeTransactionReason> existing, bool removeNew)
        {
            existing = existing ?? new TypeTransactionReasonList();

            //sync logic
            if (removeNew)
                RemoveInsertedTypeTransactionReasons();

            foreach (var item in existing)
            {
                var query = TypeTransactionReasonList.Select((x, i) => new { trans = x, Index = i }).Where(x => x.trans.TypeTransactionReasonId == item.TypeTransactionReasonId).FirstOrDefault();

                //INSERT
                if (query == null)
                    TypeTransactionReasonList.Add(item);
                //UPDATE
                else
                    TypeTransactionReasonList[query.Index] = item;

                StorageUtility.SaveItem(STATIC_TYPETRANSACTIONREASON_FOLDER, item, item.TypeTransactionReasonId);
            }
            var ord = TypeTransactionReasonList.OrderBy(x => x.Name).ToList();
            TypeTransactionReasonList.Clear();
            ord.ForEach(x => TypeTransactionReasonList.Add(x));
        }

        private void SetupTypeTransactionData(ICollection<TypeTransaction> existing, bool removeNew)
        {
            existing = existing ?? new TypeTransactionList();

            //sync logic
            if (removeNew)
                RemoveInsertedTypeTransactions();

            foreach (var item in existing)
            {
                var query = TypeTransactionList.Select((x, i) => new { trans = x, Index = i }).Where(x => x.trans.TypeTransactionId == item.TypeTransactionId).FirstOrDefault();

                //INSERT
                if (query == null)
                    TypeTransactionList.Add(item);
                //UPDATE
                else
                    TypeTransactionList[query.Index] = item;

                StorageUtility.SaveItem(STATIC_TYPETRANSACTION_FOLDER, item, item.TypeTransactionId);
            }
            var ord = TypeTransactionList.OrderBy(x => x.Name).ToList();
            TypeTransactionList.Clear();
            ord.ForEach(x => TypeTransactionList.Add(x));
        }

        private void SetupTypeSavingsDencityData(ICollection<TypeSavingsDencity> existing, bool removeNew)
        {
            existing = existing ?? new TypeSavingsDencityList();

            //sync logic
            if (removeNew)
                RemoveInsertedTypeSavingsDencities();

            foreach (var item in existing)
            {
                var query = TypeSavingsDencityList.Select((x, i) => new { trans = x, Index = i }).Where(x => x.trans.TypeSavingsDencityId == item.TypeSavingsDencityId).FirstOrDefault();

                //INSERT
                if (query == null)
                    TypeSavingsDencityList.Add(item);
                //UPDATE
                else
                    TypeSavingsDencityList[query.Index] = item;

                StorageUtility.SaveItem(STATIC_TYPESAVINGFREQUENCY_FOLDER, item, item.TypeSavingsDencityId);
            }
            var ord = TypeSavingsDencityList.OrderBy(x => x.Name).ToList();
            TypeSavingsDencityList.Clear();
            ord.ForEach(x => TypeSavingsDencityList.Add(x));
        }

        private void SetupNotificationData(ICollection<Notification> existing, bool removeNew)
        {
            existing = existing ?? new NotificationList();

            //sync logic
            if (removeNew)
                RemoveInsertedNotifications();

            foreach (var item in existing)
            {
                var query = NotificationList.Select((x, i) => new { trans = x, Index = i }).Where(x => x.trans.NotificationId == item.NotificationId).FirstOrDefault();

                //INSERT
                if (query == null)
                    NotificationList.Add(item);
                //UPDATE
                else
                    NotificationList[query.Index] = item;

                StorageUtility.SaveItem(STATIC_NOTIFICATION_FOLDER, item, item.NotificationId);
            }
            var ord = NotificationList.OrderBy(x => x.Name).ToList();
            NotificationList.Clear();
            ord.ForEach(x => NotificationList.Add(x));
        }

        private void SetupTypeFrequencyData(ICollection<TypeFrequency> existing, bool removeNew)
        {
            existing = existing ?? new TypeFrequencyList();

            //sync logic
            if (removeNew)
                RemoveInsertedTypeFrequencies();

            foreach (var item in existing)
            {
                var query = TypeFrequencyList.Select((x, i) => new { trans = x, Index = i }).Where(x => x.trans.TypeFrequencyId == item.TypeFrequencyId).FirstOrDefault();

                //INSERT
                if (query == null)
                    TypeFrequencyList.Add(item);
                //UPDATE
                else
                    TypeFrequencyList[query.Index] = item;

                StorageUtility.SaveItem(STATIC_TYPEFREQUENCY_FOLDER, item, item.TypeFrequencyId);
            }
            var ord = TypeFrequencyList.OrderBy(x => x.Name).ToList();
            TypeFrequencyList.Clear();
            ord.ForEach(x => TypeFrequencyList.Add(x));
        }

        private void SetupIntervalData(ICollection<TypeInterval> existing, bool removeNew)
        {
            existing = existing ?? new TypeIntervalList();

            //sync logic
            if (removeNew)
                RemoveInsertedTypeIntervals();

            foreach (var item in existing)
            {
                var query = IntervalList.Select((x, i) => new { trans = x, Index = i }).Where(x => x.trans.TypeIntervalId == item.TypeIntervalId).FirstOrDefault();

                //INSERT
                if (query == null)
                    IntervalList.Add(item);
                //UPDATE
                else
                    IntervalList[query.Index] = item;

                StorageUtility.SaveItem(STATIC_TYPEINTERVAL_FOLDER, item, item.TypeIntervalId);
            }
            var ord = IntervalList.OrderBy(x => x.Name).ToList();
            IntervalList.Clear();
            ord.ForEach(x => IntervalList.Add(x));
        }

        private void SetupRecurrenceRuleData(ICollection<RecurrenceRule> existing, bool removeNew)
        {
            existing = existing ?? new RecurrenceRuleList();

            //sync logic
            if (removeNew)
                RemoveInsertedRecurrenceRules();

            foreach (var item in existing)
            {
                var query = RecurrenceRuleList.Select((x, i) => new { trans = x, Index = i }).Where(x => x.trans.RecurrenceRuleId == item.RecurrenceRuleId).FirstOrDefault();

                //INSERT
                if (query == null)
                    RecurrenceRuleList.Add(item);
                //UPDATE
                else
                    RecurrenceRuleList[query.Index] = item;

                StorageUtility.SaveItem(STATIC_RECURRENCE_FOLDER, item, item.RecurrenceRuleId);
            }
            var ord = RecurrenceRuleList.OrderBy(x => x.Name).ToList();
            RecurrenceRuleList.Clear();
            ord.ForEach(x => RecurrenceRuleList.Add(x));
        }

        private void SetupBudgetThresholdData(ICollection<BudgetThreshold> existing, bool removeNew)
        {
            existing = existing ?? new BudgetThresholdList();

            //sync logic
            if (removeNew)
                RemoveInsertedBudgetThresholds();

            foreach (var item in existing)
            {
                var query = BudgetThresholdList.Select((x, i) => new { trans = x, Index = i }).Where(x => x.trans.BudgetThresholdId == item.BudgetThresholdId).FirstOrDefault();

                //INSERT
                if (query == null)
                    BudgetThresholdList.Add(item);
                //UPDATE
                else
                    BudgetThresholdList[query.Index] = item;

                StorageUtility.SaveItem(STATIC_BUDGETTHRESHOLD_FOLDER, item, item.BudgetThresholdId);
            }
            var ord = BudgetThresholdList.OrderByDescending(x => x.Amount).ToList();
            BudgetThresholdList.Clear();
            ord.ForEach(x => BudgetThresholdList.Add(x));
        }
        #endregion

        #region Load Cached Data

        private async Task<CategoryList> LoadCachedCategories()
        {
            var retVal = new CategoryList();
            foreach (var item in await StorageUtility.ListItems(STATIC_CATEGORY_FOLDER))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<Category>(STATIC_CATEGORY_FOLDER, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            SetupTypeCategoryData(retVal, false);

            return retVal;
        }

        private void RemoveInsertedCategories()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = CategoryList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.CategoryId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                CategoryList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<Transaction>(STATIC_CATEGORY_FOLDER, x.Item.CategoryId.GetHashCode().ToString());
            }
        }

        private async Task<TypeTransactionReasonList> LoadCachedTypeTransactionReasons()
        {
            var retVal = new TypeTransactionReasonList();
            foreach (var item in await StorageUtility.ListItems(STATIC_TYPETRANSACTIONREASON_FOLDER))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeTransactionReason>(STATIC_TYPETRANSACTIONREASON_FOLDER, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            SetupTypeTransactionReasonData(retVal, false);

            return retVal;
        }

        private void RemoveInsertedTypeTransactionReasons()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = TypeTransactionReasonList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeTransactionReasonId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                TypeTransactionReasonList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<TypeTransactionReason>(STATIC_TYPETRANSACTIONREASON_FOLDER, x.Item.TypeTransactionReasonId.GetHashCode().ToString());
            }
        }

        private async Task<TypeTransactionList> LoadCachedTypeTransactions()
        {
            var retVal = new TypeTransactionList();
            foreach (var item in await StorageUtility.ListItems(STATIC_TYPETRANSACTION_FOLDER))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeTransaction>(STATIC_TYPETRANSACTION_FOLDER, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            
            SetupTypeTransactionData(retVal, false);

            return retVal;
        }

        private void RemoveInsertedTypeTransactions()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = TypeTransactionList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeTransactionId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                TypeTransactionList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<TypeTransaction>(STATIC_TYPETRANSACTION_FOLDER, x.Item.TypeTransactionId.GetHashCode().ToString());
            }
        }

        private async Task<TypeSavingsDencityList> LoadCachedTypeSavingsDencities()
        {
            var retVal = new TypeSavingsDencityList();
            foreach (var item in await StorageUtility.ListItems(STATIC_TYPESAVINGFREQUENCY_FOLDER))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeSavingsDencity>(STATIC_TYPESAVINGFREQUENCY_FOLDER, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            SetupTypeSavingsDencityData(retVal, false);

            return retVal;
        }

        private void RemoveInsertedTypeSavingsDencities()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = TypeSavingsDencityList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeSavingsDencityId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                TypeSavingsDencityList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<TypeSavingsDencity>(STATIC_TYPESAVINGFREQUENCY_FOLDER, x.Item.TypeSavingsDencityId.GetHashCode().ToString());
            }
        }

        private async Task<NotificationList> LoadCachedNotifications()
        {
            var retVal = new NotificationList();
            foreach (var item in await StorageUtility.ListItems(STATIC_NOTIFICATION_FOLDER))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<Notification>(STATIC_NOTIFICATION_FOLDER, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            SetupNotificationData(retVal, false);

            return retVal;
        }

        private void RemoveInsertedNotifications()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = NotificationList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.NotificationId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                NotificationList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<Notification>(STATIC_NOTIFICATION_FOLDER, x.Item.NotificationId.GetHashCode().ToString());
            }
        }

        private async Task<TypeFrequencyList> LoadCachedTypeFrequencies()
        {
            var retVal = new TypeFrequencyList();
            foreach (var item in await StorageUtility.ListItems(STATIC_TYPEFREQUENCY_FOLDER))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeFrequency>(STATIC_TYPEFREQUENCY_FOLDER, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            SetupTypeFrequencyData(retVal, false);

            return retVal;
        }

        private void RemoveInsertedTypeFrequencies()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = TypeFrequencyList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeFrequencyId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                TypeFrequencyList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<TypeFrequency>(STATIC_TYPEFREQUENCY_FOLDER, x.Item.TypeFrequencyId.GetHashCode().ToString());
            }
        }

        private async Task<TypeIntervalList> LoadCachedTypeIntervals()
        {
            var retVal = new TypeIntervalList();
            foreach (var item in await StorageUtility.ListItems(STATIC_TYPEINTERVAL_FOLDER))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeInterval>(STATIC_TYPEINTERVAL_FOLDER, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            
            SetupIntervalData(retVal, false);

            return retVal;
        }

        private void RemoveInsertedTypeIntervals()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = IntervalList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeIntervalId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                IntervalList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<TypeInterval>(STATIC_TYPEINTERVAL_FOLDER, x.Item.TypeIntervalId.GetHashCode().ToString());
            }
        }

        private async Task<RecurrenceRuleList> LoadCachedRecurrenceRules()
        {
            var retVal = new RecurrenceRuleList();
            foreach (var item in await StorageUtility.ListItems(STATIC_RECURRENCE_FOLDER))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<RecurrenceRule>(STATIC_RECURRENCE_FOLDER, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            SetupRecurrenceRuleData(retVal, false);

            return retVal;
        }

        private void RemoveInsertedRecurrenceRules()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = RecurrenceRuleList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.RecurrenceRuleId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                RecurrenceRuleList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<RecurrenceRule>(STATIC_RECURRENCE_FOLDER, x.Item.RecurrenceRuleId.GetHashCode().ToString());
            }
        }

        private async Task<BudgetThresholdList> LoadCachedBudgetThresholds()
        {
            var retVal = new BudgetThresholdList();
            foreach (var item in await StorageUtility.ListItems(STATIC_BUDGETTHRESHOLD_FOLDER))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<BudgetThreshold>(STATIC_BUDGETTHRESHOLD_FOLDER, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            SetupBudgetThresholdData(retVal, false);

            return retVal;
        }

        private void RemoveInsertedBudgetThresholds()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = BudgetThresholdList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.BudgetThresholdId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                BudgetThresholdList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<BudgetThreshold>(STATIC_BUDGETTHRESHOLD_FOLDER, x.Item.BudgetThresholdId.GetHashCode().ToString());
            }
        }

        private async Task<List<User>> LoadCachedUser()
        {
            var retVal = new List<User>();
            foreach (var item in await StorageUtility.ListItems(STATIC_USER_FOLDER))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<User>(STATIC_USER_FOLDER, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            //SetupUser(retVal, false);

            return retVal;
        }

        #endregion

        #region Load Live Data
        public async Task LoadLiveCategories()
        {
            var retVal = new CategoryList();

            var client = new BMAStaticDataService.StaticClient();
            try
            {
                latestState = Guid.NewGuid().ToString();
                client.GetAllCategoriesAsync(App.Instance.User.UserId);
                client.GetAllCategoriesCompleted += async (o, e) =>
                {
                    var result = e.Result;

                    SetupTypeCategoryData(result, true);
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task LoadLiveTypeTransactionReasons()
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllTypeTransactionReasonsAsync(App.Instance.User.UserId);
            client.GetAllTypeTransactionReasonsCompleted += (o, e) =>
            {
                SetupTypeTransactionReasonData(e.Result, true);
            };
        }

        private async Task LoadLiveTypeTransactions()
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllTypeTransactionsAsync(App.Instance.User.UserId);
            client.GetAllTypeTransactionsCompleted += (o, e) =>
            {
                SetupTypeTransactionData(e.Result, true);
            };
        }

        private async Task LoadLiveTypeSavingsDencities()
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllTypeSavingsDencitiesAsync(App.Instance.User.UserId);
            client.GetAllTypeSavingsDencitiesCompleted += (o, e) =>
            {
                SetupTypeSavingsDencityData(e.Result, true);
            };
        }

        private async Task LoadLiveNotifications()
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllNotificationsAsync(App.Instance.User.UserId);
            client.GetAllNotificationsCompleted += (o, e) =>
            {
                SetupNotificationData(e.Result, true);
            };
        }

        private async Task LoadLiveTypeFrequencies()
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllTypeFrequenciesAsync(App.Instance.User.UserId);
            client.GetAllTypeFrequenciesCompleted += (o, e) =>
            {
                SetupTypeFrequencyData(e.Result, true);
            };
        }

        private async Task LoadLiveTypeIntervals()
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllTypeIntervalsAsync(App.Instance.User.UserId);
            client.GetAllTypeIntervalsCompleted += (o, e) =>
            {
                SetupIntervalData(e.Result, true);
            };
        }

        private async Task LoadLiveRecurrenceRules()
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllRecurrenceRulesAsync(App.Instance.User.UserId);
            client.GetAllRecurrenceRulesCompleted += (o, e) =>
            {
                SetupRecurrenceRuleData(e.Result, true);
            };
        }

        private async Task LoadLiveBudgetThresholds()
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllBudgetThresholdsAsync(App.Instance.User.UserId);
            client.GetAllBudgetThresholdsCompleted += (o, e) =>
            {
                SetupBudgetThresholdData(e.Result, true);
            };
        }
        #endregion

        #region Save
        public async Task SaveCategory(ObservableCollection<Category> categories, Action<Exception> callback)
        {
            try
            {
                if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                {
                    try
                    {
                        foreach (var item in categories)
                            item.OptimizeOnTopLevel();

                        foreach (var item in CategoryList.Where(x => x.HasChanges))
                            item.HasChanges = false;

                        SetupTypeCategoryData(categories, false);

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
                    foreach (var item in categories)
                        item.OptimizeOnTopLevel();

                    var client = new BMAStaticDataService.StaticClient();
                    
                    client.SaveCategoriesAsync(categories, App.Instance.User);
                    
                    client.SaveCategoriesCompleted += async (sender, completedEventArgs) =>
                    {
                        if (completedEventArgs.Error == null)
                        {
                            await LoadTypeTransactionReasons();

                            SetupTypeCategoryData(completedEventArgs.Result, true);

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

        public async Task SaveTransactionReason(ObservableCollection<TypeTransactionReason> transactionReasons, Action<Exception> callback)
        {
            try
            {
                if (App.Instance.StaticDataOnlineStatus != ServerStatus.Ok)
                {
                    try
                    {
                        foreach (var item in transactionReasons)
                            item.OptimizeOnTopLevel();

                        foreach (var item in TypeTransactionReasonList.Where(x => x.HasChanges))
                            item.HasChanges = false;

                        SetupTypeTransactionReasonData(transactionReasons, false);

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
                    foreach (var item in transactionReasons)
                        item.OptimizeOnTopLevel();

                    var client = new BMAStaticDataService.StaticClient();
                    client.SaveTypeTransactionReasonsAsync(transactionReasons);
                    client.SaveTypeTransactionReasonsCompleted += async (sender, completedEventArgs) =>
                    {
                        if (completedEventArgs.Error == null)
                        {
                            await LoadCategories();

                            await UpdateCacheTypeTransactionReason(completedEventArgs.Result);
                            SetupTypeTransactionReasonData(completedEventArgs.Result, true);

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

        public async Task SaveTypeInterval(ObservableCollection<TypeInterval> typeInterval)
        {
            try
            {
                var result = this.IntervalList.ToObservableCollection();

                if (App.Instance.StaticDataOnlineStatus != ServerStatus.Ok)
                {
                    result = result.Where(i => !i.IsDeleted).ToObservableCollection();
                    ApplicationData.Current.LocalSettings.Values["IsSync"] = false;
                }
                else
                {
                    var client = new BMAStaticDataService.StaticClient();
                    client.SaveTypeIntervalsAsync(typeInterval, App.Instance.User);
                    client.SaveTypeIntervalsCompleted += async (sender, completedEventArgs) =>
                    {
                        if (completedEventArgs.Error == null)
                        {
                            await UpdateCacheTypeInterval(completedEventArgs.Result);
                            SetupIntervalData(completedEventArgs.Result, true);
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
      //     await StorageUtility.Clear(STATIC_USER_FOLDER);
            await StorageUtility.SaveItem(STATIC_USER_FOLDER, user, user.UserId);

            //var test = await StorageUtility.ListItems(STATIC_USER_FOLDER);
        }

        private async Task UpdateCacheStaticData(StaticTypeList staticDataList)
        {
            await StorageUtility.Clear(STATIC_TYPESAVINGFREQUENCY_FOLDER);
            await StorageUtility.Clear(STATIC_TYPETRANSACTION_FOLDER);

            foreach (var item in staticDataList.TypeTransactions)
                await StorageUtility.SaveItem(STATIC_TYPETRANSACTION_FOLDER, item, item.TypeTransactionId);

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

        private async Task UpdateCacheRecurrenceRule(ICollection<RecurrenceRule> recurrenceRuleList)
        {
            await StorageUtility.Clear(STATIC_RECURRENCE_FOLDER);
            foreach (var item in RecurrenceRuleList)
                await StorageUtility.SaveItem(STATIC_RECURRENCE_FOLDER, item, item.RecurrenceRuleId);
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
