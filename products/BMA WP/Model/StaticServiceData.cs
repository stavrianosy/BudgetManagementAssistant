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
        private const string STATIC_NOTIFICATION_FOLDER = "Static_Notification";
        private const string STATIC_TYPEFREQUENCY_FOLDER = "Static_TypeFrequency";
        private const string STATIC_TYPEINTERVAL_FOLDER = "Static_TypeInterval";
        private const string STATIC_RECURRENCE_FOLDER = "Static_Recurrence";
        private const string STATIC_BUDGETTHRESHOLD_FOLDER = "Static_BudgetThreshold";
        private const string STATIC_USER_FOLDER = "Static_User";

        private string folderFormat = "{0}_{1}";
        private string User_Category_Folder { get { return string.Format("{0}_{1}", STATIC_CATEGORY_FOLDER, App.Instance.User.UserId); } }
        private string User_TypeTransaction_Folder { get { return string.Format("{0}_{1}", STATIC_TYPETRANSACTION_FOLDER, App.Instance.User.UserId); } }
        private string User_TypeTransactionReason_Folder { get { return string.Format("{0}_{1}", STATIC_TYPETRANSACTIONREASON_FOLDER, App.Instance.User.UserId); } }
        private string User_Notification_Folder { get { return string.Format("{0}_{1}", STATIC_NOTIFICATION_FOLDER, App.Instance.User.UserId); } }
        private string User_TypeFrequency_Folder { get { return string.Format("{0}_{1}", STATIC_TYPEFREQUENCY_FOLDER, App.Instance.User.UserId); } }
        private string User_TypeInterval_Folder { get { return string.Format("{0}_{1}", STATIC_TYPEINTERVAL_FOLDER, App.Instance.User.UserId); } }
        private string User_Recurrence_Folder { get { return string.Format("{0}_{1}", STATIC_RECURRENCE_FOLDER, App.Instance.User.UserId); } }
        private string User_BudgetThrushold_Folder { get { return string.Format("{0}_{1}", STATIC_BUDGETTHRESHOLD_FOLDER, App.Instance.User.UserId); } }
        
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
                if (App.Instance.IsOnline)
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
                else
                {
                    result = ServerStatus.Error;
                    callback(result);
                }
            }
            catch (Exception)
            {
                //throw;
                result = ServerStatus.Error;
            }
            return result;
        }

        public void LoadStaticData(Action<Exception> callback)
        {
            var categories = false;
            var typeTransReasons = false;
            var typeTrans= false;
            var notifications = false;
            var typeFrequencies= false;
            var typeIntervals= false;
            var recurrenceRules= false;
            var budgetThrushold= false;

            LoadCategories(error => 
            {
                categories = true;
                if(LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules, budgetThrushold))
                    callback(error);
            });
            LoadTypeTransactionReasons(error =>
            {
                typeTransReasons = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules, budgetThrushold))
                    callback(error);
            });
            LoadTypeTransactions(error =>
            {
                typeTrans = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules, budgetThrushold))
                    callback(error);
            });
            LoadNotifications(error =>
            {
                notifications = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules, budgetThrushold))
                    callback(error);
            });
            LoadTypeFrequencies(error =>
            {
                typeFrequencies = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules, budgetThrushold))
                    callback(error);
            });
            LoadTypeIntervals(error =>
            {
                typeIntervals = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules, budgetThrushold))
                    callback(error);
            });
            LoadRecurrenceRules(error =>
            {
                recurrenceRules = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules, budgetThrushold))
                    callback(error);
            });
            LoadBudgetThresholds(error =>
            {
                budgetThrushold = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules, budgetThrushold))
                    callback(error);
            });

        }

        private bool LoadStaticDataDone(bool categories, bool typeTransReasons, bool typeTrans, bool notifications, bool typeFrequencies, 
                                    bool typeIntervals, bool recurrenceRules, bool budgetThrushold)
        {
            return categories && typeTrans && typeTransReasons && notifications && typeFrequencies && typeIntervals && recurrenceRules && budgetThrushold;
        }

        public void LoadCategories(Action<Exception> callback)
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                LoadCachedCategories((categoryList, error) => callback(error));
            else
                LoadLiveCategories(error => callback(error));
        }

        public void LoadTypeTransactionReasons(Action<Exception> callback)
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                LoadCachedTypeTransactionReasons((typeTransactionReasonList, error) => callback(error));
            else
                LoadLiveTypeTransactionReasons(error => callback(error));
        }

        public void LoadTypeTransactions(Action<Exception> callback)
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                LoadCachedTypeTransactions((typeTransactionList, error) => callback(error));
            else
                LoadLiveTypeTransactions(error => callback(error));
        }

        public void LoadNotifications(Action<Exception> callback)
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                LoadCachedNotifications((notificationsList, error) => callback(error));
            else
                LoadLiveNotifications(error => callback(error));
        }

        public void LoadTypeFrequencies(Action<Exception> callback)
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                LoadCachedTypeFrequencies((typeFrequenciesList, error) => callback(error));
            else
                LoadLiveTypeFrequencies(error => callback(error));
        }

        public void LoadTypeIntervals(Action<Exception> callback)
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                LoadCachedTypeIntervals((typeIntervalsList, error) => callback(error));
            else
                LoadLiveTypeIntervals(error => callback(error));
        }

        public void LoadRecurrenceRules(Action<Exception> callback)
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                LoadCachedRecurrenceRules((recurrenceRuleList, error) => callback(error));
            else
                LoadLiveRecurrenceRules(error => callback(error));
        }

        public void LoadBudgetThresholds(Action<Exception> callback)
        {
            if (App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok)
                LoadCachedBudgetThresholds((budgetThresholdsList, error) => callback(error));
            else
                LoadLiveBudgetThresholds(error => callback(error));
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
                        UserFound(existing);
                        await UpdateCacheUserData(existing);

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

                StorageUtility.SaveItem(User_Category_Folder, item, item.CategoryId, App.Instance.User.UserId);
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

                StorageUtility.SaveItem(User_TypeTransactionReason_Folder, item, item.TypeTransactionReasonId, App.Instance.User.UserId);
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

                StorageUtility.SaveItem(User_TypeTransaction_Folder, item, item.TypeTransactionId, App.Instance.User.UserId);
            }
            var ord = TypeTransactionList.OrderBy(x => x.Name).ToList();
            TypeTransactionList.Clear();
            ord.ForEach(x => TypeTransactionList.Add(x));
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

                StorageUtility.SaveItem(User_Notification_Folder, item, item.NotificationId, App.Instance.User.UserId);
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

                StorageUtility.SaveItem(User_TypeFrequency_Folder, item, item.TypeFrequencyId, App.Instance.User.UserId);
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

                StorageUtility.SaveItem(User_TypeInterval_Folder, item, item.TypeIntervalId, App.Instance.User.UserId);
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

                StorageUtility.SaveItem(User_Recurrence_Folder, item, item.RecurrenceRuleId, App.Instance.User.UserId);
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

                StorageUtility.SaveItem(User_BudgetThrushold_Folder, item, item.BudgetThresholdId, App.Instance.User.UserId);
            }
            var ord = BudgetThresholdList.OrderByDescending(x => x.Amount).ToList();
            BudgetThresholdList.Clear();
            ord.ForEach(x => BudgetThresholdList.Add(x));
        }
        #endregion

        #region Load Cached Data

        private async void LoadCachedCategories(Action<CategoryList, Exception> callback)
        {
            var retVal = new CategoryList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(User_Category_Folder, App.Instance.User.UserId))
                {

                    var staticType = await StorageUtility.RestoreItem<Category>(User_Category_Folder, item, App.Instance.User.UserId);
                    //retVal.Add(staticType);
                    CategoryList.Add(staticType);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //callback(null, ex);
            }
            //SetupTypeCategoryData(retVal, false);

            callback(CategoryList, null);
        }

        private void RemoveInsertedCategories()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = CategoryList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.CategoryId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                CategoryList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<Category>(User_Category_Folder, x.Item, x.Item.CategoryId);
            }
        }

        private async void LoadCachedTypeTransactionReasons(Action<TypeTransactionReasonList, Exception> callback)
        {
            var retVal = new TypeTransactionReasonList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(User_TypeTransactionReason_Folder, App.Instance.User.UserId))
                {

                    var staticType = await StorageUtility.RestoreItem<TypeTransactionReason>(User_TypeTransactionReason_Folder, item, App.Instance.User.UserId);
                    //retVal.Add(staticType);
                    TypeTransactionReasonList.Add(staticType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //callback(null, ex);
            }
            
            //SetupTypeTransactionReasonData(retVal, false);

            callback(TypeTransactionReasonList, null);
        }

        private void RemoveInsertedTypeTransactionReasons()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = TypeTransactionReasonList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeTransactionReasonId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                TypeTransactionReasonList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<TypeTransactionReason>(User_TypeTransactionReason_Folder, x.Item, x.Item.TypeTransactionReasonId);
            }
        }

        private async void LoadCachedTypeTransactions(Action<TypeTransactionList, Exception> callback)
        {
            var retVal = new TypeTransactionList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(User_TypeTransaction_Folder, App.Instance.User.UserId))
                {
                    var staticType = await StorageUtility.RestoreItem<TypeTransaction>(User_TypeTransaction_Folder, item, App.Instance.User.UserId);
                    //retVal.Add(staticType);
                    TypeTransactionList.Add(staticType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //callback(null, ex);
            }
            
            //SetupTypeTransactionData(retVal, false);

            callback(TypeTransactionList, null);
        }

        private void RemoveInsertedTypeTransactions()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = TypeTransactionList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeTransactionId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                TypeTransactionList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<TypeTransaction>(User_TypeTransaction_Folder, x.Item, x.Item.TypeTransactionId);
            }
        }

        private async void LoadCachedNotifications(Action<NotificationList, Exception> callback)
        {
            var retVal = new NotificationList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(User_Notification_Folder, App.Instance.User.UserId))
                {

                    var staticType = await StorageUtility.RestoreItem<Notification>(User_Notification_Folder, item, App.Instance.User.UserId);
                    //retVal.Add(staticType);
                    NotificationList.Add(staticType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //callback(null, ex);
            }
            
            //SetupNotificationData(retVal, false);

            callback(NotificationList, null);
        }

        private void RemoveInsertedNotifications()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = NotificationList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.NotificationId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                NotificationList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<Notification>(User_Notification_Folder, x.Item, x.Item.NotificationId);
            }
        }

        private async void LoadCachedTypeFrequencies(Action<TypeFrequencyList, Exception> callback)
        {
            var retVal = new TypeFrequencyList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(User_TypeFrequency_Folder, App.Instance.User.UserId))
                {

                    var staticType = await StorageUtility.RestoreItem<TypeFrequency>(User_TypeFrequency_Folder, item, App.Instance.User.UserId);
                    //retVal.Add(staticType);
                    TypeFrequencyList.Add(staticType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //callback(null, ex);
            }

            //SetupTypeFrequencyData(retVal, false);

            callback(TypeFrequencyList, null);
        }

        private void RemoveInsertedTypeFrequencies()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = TypeFrequencyList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeFrequencyId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                TypeFrequencyList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<TypeFrequency>(User_TypeFrequency_Folder, x.Item, x.Item.TypeFrequencyId);
            }
        }

        private async void LoadCachedTypeIntervals(Action<TypeIntervalList, Exception> callback)
        {
            var retVal = new TypeIntervalList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(User_TypeInterval_Folder, App.Instance.User.UserId))
                {
                    var staticType = await StorageUtility.RestoreItem<TypeInterval>(User_TypeInterval_Folder, item, App.Instance.User.UserId);
                    //retVal.Add(staticType);
                    IntervalList.Add(staticType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //callback(null, ex);
            }
            
            //SetupIntervalData(retVal, false);

            callback(IntervalList, null);
        }

        private void RemoveInsertedTypeIntervals()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = IntervalList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeIntervalId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                IntervalList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<TypeInterval>(User_TypeInterval_Folder, x.Item, x.Item.TypeIntervalId);
            }
        }

        private async void LoadCachedRecurrenceRules(Action<RecurrenceRuleList, Exception> callback)
        {
            var retVal = new RecurrenceRuleList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(User_Recurrence_Folder, App.Instance.User.UserId))
                {
                    var staticType = await StorageUtility.RestoreItem<RecurrenceRule>(User_Recurrence_Folder, item, App.Instance.User.UserId);
                    //retVal.Add(staticType);
                    RecurrenceRuleList.Add(staticType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //callback(null, ex);
            }

            //SetupRecurrenceRuleData(retVal, false);

            callback(RecurrenceRuleList, null);
        }

        private void RemoveInsertedRecurrenceRules()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = RecurrenceRuleList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.RecurrenceRuleId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                RecurrenceRuleList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<RecurrenceRule>(User_Recurrence_Folder, x.Item, x.Item.RecurrenceRuleId);
            }
        }

        private async void LoadCachedBudgetThresholds(Action<BudgetThresholdList, Exception> callback)
        {
            var retVal = new BudgetThresholdList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(User_BudgetThrushold_Folder, App.Instance.User.UserId))
                {

                    var staticType = await StorageUtility.RestoreItem<BudgetThreshold>(User_BudgetThrushold_Folder, item, App.Instance.User.UserId);
                    //retVal.Add(staticType);
                    BudgetThresholdList.Add(staticType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //callback(null, ex);
            }

            //SetupBudgetThresholdData(retVal, false);

            callback(BudgetThresholdList, null);
        }

        private void RemoveInsertedBudgetThresholds()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = BudgetThresholdList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.BudgetThresholdId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                BudgetThresholdList.RemoveAt(x.Index);
                StorageUtility.DeleteItem<BudgetThreshold>(User_BudgetThrushold_Folder, x.Item, x.Item.BudgetThresholdId);
            }
        }

        private async Task<List<User>> LoadCachedUser()
        {
            var retVal = new List<User>();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_USER_FOLDER, App.Instance.User.UserId))
                {
                    var staticType = await StorageUtility.RestoreItem<User>(STATIC_USER_FOLDER, item, App.Instance.User.UserId);
                    retVal.Add(staticType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }


            //SetupUser(retVal, false);

            return retVal;
        }

        #endregion

        #region Load Live Data
        public void LoadLiveCategories(Action<Exception> callback)
        {
            var retVal = new CategoryList();

            var client = new BMAStaticDataService.StaticClient();
            try
            {
                latestState = Guid.NewGuid().ToString();
                client.GetAllCategoriesAsync(App.Instance.User.UserId);
                client.GetAllCategoriesCompleted += async (o, e) =>
                {
                    if(e.Error == null)
                        SetupTypeCategoryData(e.Result, true);

                    callback(e.Error);
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadLiveTypeTransactionReasons(Action<Exception> callback)
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllTypeTransactionReasonsAsync(App.Instance.User.UserId);
            client.GetAllTypeTransactionReasonsCompleted += (o, e) =>
            {
                if (e.Error == null)
                    SetupTypeTransactionReasonData(e.Result, true);

                callback(e.Error);
            };
        }

        private void LoadLiveTypeTransactions(Action<Exception> callback)
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllTypeTransactionsAsync(App.Instance.User.UserId);
            client.GetAllTypeTransactionsCompleted += (o, e) =>
            {
                if (e.Error == null)
                    SetupTypeTransactionData(e.Result, true);

                callback(e.Error);
            };
        }

        private void LoadLiveNotifications(Action<Exception> callback)
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllNotificationsAsync(App.Instance.User.UserId);
            client.GetAllNotificationsCompleted += (o, e) =>
            {
                if (e.Error == null)
                    SetupNotificationData(e.Result, true);

                callback(e.Error);
            };
        }

        private void LoadLiveTypeFrequencies(Action<Exception> callback)
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllTypeFrequenciesAsync(App.Instance.User.UserId);
            client.GetAllTypeFrequenciesCompleted += (o, e) =>
            {
                if (e.Error == null)
                    SetupTypeFrequencyData(e.Result, true);

                callback(e.Error);
            };
        }

        private void LoadLiveTypeIntervals(Action<Exception> callback)
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllTypeIntervalsAsync(App.Instance.User.UserId);
            client.GetAllTypeIntervalsCompleted += (o, e) =>
            {
                if (e.Error == null)
                    SetupIntervalData(e.Result, true);

                callback(e.Error);
            };
        }

        private void LoadLiveRecurrenceRules(Action<Exception> callback)
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllRecurrenceRulesAsync();
            client.GetAllRecurrenceRulesCompleted += (o, e) =>
            {
                if (e.Error == null)
                    SetupRecurrenceRuleData(e.Result, true);

                callback(e.Error);
            };
        }

        private void LoadLiveBudgetThresholds(Action<Exception> callback)
        {
            latestState = Guid.NewGuid().ToString();

            var client = new BMAStaticDataService.StaticClient();

            client.GetAllBudgetThresholdsAsync(App.Instance.User.UserId);
            client.GetAllBudgetThresholdsCompleted += (o, e) =>
            {
                if (e.Error == null)
                    SetupBudgetThresholdData(e.Result, true);

                callback(e.Error);
            };
        }
        #endregion

        #region Save
        public async Task SaveCategory(ObservableCollection<Category> categories, Action<Exception> callback)
        {
            try
            {
                await App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
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
                                //** TEST IF I CAN SEND NULL TO THE CALLBACK
                                LoadTypeTransactionReasons(null);

                                SetupTypeCategoryData(completedEventArgs.Result, true);

                                App.Instance.IsSync = true;

                                callback(null);
                            }
                            else
                                callback(completedEventArgs.Error);

                        };
                    }
                });
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
                await App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
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
                                //** CHECK IF NULL IS OK
                                LoadCategories(null);

                                SetupTypeTransactionReasonData(completedEventArgs.Result, true);

                                App.Instance.IsSync = true;

                                callback(null);
                            }
                            else
                                callback(completedEventArgs.Error);

                        };
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveTypeInterval(ObservableCollection<TypeInterval> typeInterval, Action<Exception> callback)
        {
            try
            {
                var result = this.IntervalList.ToObservableCollection();

                await App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        try
                        {
                            foreach (var item in IntervalList.Where(x => x.HasChanges))
                                item.HasChanges = false;

                            SetupIntervalData(typeInterval, false);

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
                        var client = new BMAStaticDataService.StaticClient();
                        client.SaveTypeIntervalsAsync(typeInterval, App.Instance.User);
                        client.SaveTypeIntervalsCompleted += async (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                SetupIntervalData(completedEventArgs.Result, true);

                                App.Instance.IsSync = true;

                                callback(null);
                            }
                            else
                                callback(completedEventArgs.Error);
                        };
                    }
                });
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
            await StorageUtility.SaveItem(STATIC_USER_FOLDER, user, user.UserId, App.Instance.User.UserId);

            //var test = await StorageUtility.ListItems(STATIC_USER_FOLDER);
        }

        private async Task UpdateCacheBudgetThresholds(ICollection<BudgetThreshold> budgetThresholdList)
        {
            await StorageUtility.Clear(User_BudgetThrushold_Folder);
            foreach (var item in budgetThresholdList)
                await StorageUtility.SaveItem(User_BudgetThrushold_Folder, item, item.BudgetThresholdId, App.Instance.User.UserId);
        }

        #endregion
    }
}
