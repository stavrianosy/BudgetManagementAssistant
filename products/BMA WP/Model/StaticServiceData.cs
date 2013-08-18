using BMA.BusinessLogic;
using BMA_WP.Common;
using BMA_WP.Resources;
//using BMA.Proxy.BMAStaticDataService;
using BMA_WP.BMAStaticDataService;
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
        private const string STATIC_TYPEINTERVALCONFIG_FOLDER = "Static_TypeIntervalConfiguration";
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
        public TypeFrequencyList TypeFrequencyList { get; set; }
        public TypeIntervalList IntervalList { get; set; }
        public TypeIntervalConfiguration IntervalConfiguration { get; set; }
        public RecurrenceRuleList RecurrenceRuleList { get; set; }
        public NotificationList NotificationList { get; set; }
        #endregion

        #region Constructor
        public StaticServiceData()
        {
            CategoryList = new CategoryList();
            TypeTransactionList = new TypeTransactionList();
            TypeTransactionReasonList = new TypeTransactionReasonList();
            NotificationList = new NotificationList();
            TypeFrequencyList = new TypeFrequencyList();
            IntervalList = new TypeIntervalList();
            RecurrenceRuleList = new RecurrenceRuleList();
        }
        #endregion

        #region Events
        #endregion

        #region Private methods
        #endregion

        #region Public methods
        public ServerStatus SetServerStatus(Action<ServerStatus> callback)
        {
            var result = ServerStatus.Communicating;
            try
            {
                if (App.Instance.IsOnline)
                {
                    var client = new StaticClient();
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
            var typeIntervals= true;
            var recurrenceRules= false;

            LoadCategories(error => 
            {
                categories = true;
                if(LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules))
                    callback(error);
            });
            LoadTypeTransactionReasons(error =>
            {
                typeTransReasons = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules))
                    callback(error);
            });
            LoadTypeTransactions(error =>
            {
                typeTrans = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules))
                    callback(error);
            });
            LoadNotifications(error =>
            {
                notifications = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules))
                    callback(error);
            });
            LoadTypeFrequencies(error =>
            {
                typeFrequencies = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules))
                    callback(error);
            });
            
            //LoadTypeIntervals(error =>
            //{
            //    typeIntervals = true;
            //    if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules))
            //        callback(error);
            //});

            LoadRecurrenceRules(error =>
            {
                recurrenceRules = true;
                if (LoadStaticDataDone(categories, typeTransReasons, typeTrans, notifications, typeFrequencies, typeIntervals, recurrenceRules))
                    callback(error);
            });
        }

        private bool LoadStaticDataDone(bool categories, bool typeTransReasons, bool typeTrans, bool notifications, bool typeFrequencies,
                                    bool typeIntervals, bool recurrenceRules)
        {
            return categories && typeTrans && typeTransReasons && notifications && typeFrequencies && typeIntervals && recurrenceRules;
        }

        public void LoadCategories(Action<Exception> callback)
        {
            App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedCategories((categoryList, error) => callback(error));
                    else
                        LoadLiveCategories(error => callback(error));
                });
        }

        public void LoadTypeTransactionReasons(Action<Exception> callback)
        {
            App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedTypeTransactionReasons((typeTransactionReasonList, error) => callback(error));
                    else
                        LoadLiveTypeTransactionReasons(error => callback(error));
                });
        }

        public void LoadTypeTransactions(Action<Exception> callback)
        {
            App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedTypeTransactions((typeTransactionList, error) => callback(error));
                    else
                        LoadLiveTypeTransactions(error => callback(error));
                });
        }

        public void LoadNotifications(Action<Exception> callback)
        {
            App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedNotifications((notificationsList, error) => callback(error));
                    else
                        LoadLiveNotifications(error => callback(error));
                });
        }

        public void LoadTypeFrequencies(Action<Exception> callback)
        {
            App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedTypeFrequencies((typeFrequenciesList, error) => callback(error));
                    else
                        LoadLiveTypeFrequencies(error => callback(error));
                });
        }

        public void LoadTypeIntervals(Action<Exception> callback)
        {
            App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedTypeIntervals((typeIntervalsList, error) => callback(error));
                    else
                        LoadLiveTypeIntervals(error => callback(error));
                });
        }

        public void LoadTypeIntervalConfiguration(Action<Exception> callback)
        {
            App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedTypeIntervalConfiguration((typeIntervalsConfiguration, error) => callback(error));
                    else
                        LoadLiveTypeIntervalConfiguration(error => callback(error));
                });
        }

        
        public void LoadRecurrenceRules(Action<Exception> callback)
        {
            App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    if (status != StaticServiceData.ServerStatus.Ok)
                        LoadCachedRecurrenceRules((recurrenceRuleList, error) => callback(error));
                    else
                        LoadLiveRecurrenceRules(error => callback(error));
                });
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
                var client = new StaticClient();
                client.AuthenticateUserAsync(user, callback);
                client.AuthenticateUserCompleted += async (sender,eventargs) => {
                    try
                    {
                        if (eventargs.Error == null)
                        {
                            existing = eventargs.Result;
                            UserFound(existing);
                            await UpdateCacheUserData(existing);
                        }

                        callback(eventargs.Error);
                    }
                    catch (Exception){throw;}
                };
            }
        }

        private void UserFound(User existing)
        {
            if (existing.UserName != null && existing.UserName.Length > 0)
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
        }

        private async void SetupTypeCategoryData(ICollection<Category> existing, bool removeNew)
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

                await StorageUtility.SaveItem(STATIC_CATEGORY_FOLDER, item, item.CategoryId, App.Instance.User.UserName);
            }
            var ord = CategoryList.OrderBy(x => x.Name).ToList();
            CategoryList.Clear();
            ord.ForEach(x => CategoryList.Add(x));
        }

        private async void SetupTypeTransactionReasonData(ICollection<TypeTransactionReason> existing, bool removeNew)
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

                await StorageUtility.SaveItem(STATIC_TYPETRANSACTIONREASON_FOLDER, item, item.TypeTransactionReasonId, App.Instance.User.UserName);
            }
            var ord = TypeTransactionReasonList.OrderBy(x => x.Name).ToList();
            TypeTransactionReasonList.Clear();
            ord.ForEach(x => TypeTransactionReasonList.Add(x));
        }

        private async void SetupTypeTransactionData(ICollection<TypeTransaction> existing, bool removeNew)
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

                await StorageUtility.SaveItem(STATIC_TYPETRANSACTION_FOLDER, item, item.TypeTransactionId, App.Instance.User.UserName);
            }
            var ord = TypeTransactionList.OrderBy(x => x.Name).ToList();
            TypeTransactionList.Clear();
            ord.ForEach(x => TypeTransactionList.Add(x));
        }

        private async void SetupNotificationData(ICollection<Notification> existing, bool removeNew)
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

                await StorageUtility.SaveItem(STATIC_NOTIFICATION_FOLDER, item, item.NotificationId, App.Instance.User.UserName);
            }
            var ord = NotificationList.OrderBy(x => x.Name).ToList();
            NotificationList.Clear();
            ord.ForEach(x => NotificationList.Add(x));
        }

        private async void SetupTypeFrequencyData(ICollection<TypeFrequency> existing, bool removeNew)
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

                await StorageUtility.SaveItem(STATIC_TYPEFREQUENCY_FOLDER, item, item.TypeFrequencyId, App.Instance.User.UserName);
            }
            var ord = TypeFrequencyList.OrderBy(x => x.Name).ToList();
            TypeFrequencyList.Clear();
            ord.ForEach(x => TypeFrequencyList.Add(x));
        }

        private async void SetupIntervalData(ICollection<TypeInterval> existing, bool removeNew)
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

                StorageUtility.SaveItem(STATIC_TYPEINTERVAL_FOLDER, item, item.TypeIntervalId, App.Instance.User.UserName);
            }
            var ord = IntervalList.OrderBy(x => x.Name).ToList();
            IntervalList.Clear();
            ord.ForEach(x => IntervalList.Add(x));
        }

        private async void SetupIntervalConfigurationData(BMA.BusinessLogic.TypeIntervalConfiguration existing, bool removeNew)
        {
            //existing = existing ?? new BMA.BusinessLogic.TypeIntervalConfiguration();

            if(existing != null)
                StorageUtility.SaveItem(STATIC_TYPEINTERVAL_FOLDER, existing, existing.TypeIntervalConfigurationId, App.Instance.User.UserName);

            IntervalConfiguration = existing;
            
        }

        private async void SetupRecurrenceRuleData(ICollection<RecurrenceRule> existing, bool removeNew)
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

                await StorageUtility.SaveItem(STATIC_RECURRENCE_FOLDER, item, item.RecurrenceRuleId, App.Instance.User.UserName);
            }
            var ord = RecurrenceRuleList.OrderBy(x => x.Name).ToList();
            RecurrenceRuleList.Clear();
            ord.ForEach(x => RecurrenceRuleList.Add(x));
        }

        private async void SetupTypeIntervalConfigData(TypeIntervalConfiguration existing, bool removeNew)
        {
            var item = existing;
            
            App.Instance.StaticServiceData.IntervalConfiguration = item;
            
            StorageUtility.SaveItem(STATIC_TYPEINTERVALCONFIG_FOLDER, item, item.TypeIntervalConfigurationId, App.Instance.User.UserName);
        }

        #endregion

        #region Load Cached Data

        private async void LoadCachedCategories(Action<CategoryList, Exception> callback)
        {
            var retVal = new CategoryList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_CATEGORY_FOLDER, App.Instance.User.UserName))
                {

                    var staticType = await StorageUtility.RestoreItem<Category>(STATIC_CATEGORY_FOLDER, item, App.Instance.User.UserName);
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

        private async void RemoveInsertedCategories()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = CategoryList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.CategoryId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                CategoryList.RemoveAt(x.Index);
                await StorageUtility.DeleteItem<Category>(STATIC_CATEGORY_FOLDER, x.Item, x.Item.CategoryId);
            }
        }

        private async void LoadCachedTypeTransactionReasons(Action<TypeTransactionReasonList, Exception> callback)
        {
            var retVal = new TypeTransactionReasonList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_TYPETRANSACTIONREASON_FOLDER, App.Instance.User.UserName))
                {

                    var staticType = await StorageUtility.RestoreItem<TypeTransactionReason>(STATIC_TYPETRANSACTIONREASON_FOLDER, item, App.Instance.User.UserName);
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

        private async void RemoveInsertedTypeTransactionReasons()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = TypeTransactionReasonList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeTransactionReasonId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                TypeTransactionReasonList.RemoveAt(x.Index);
                await StorageUtility.DeleteItem<TypeTransactionReason>(STATIC_TYPETRANSACTIONREASON_FOLDER, x.Item, x.Item.TypeTransactionReasonId);
            }
        }

        private async void LoadCachedTypeTransactions(Action<TypeTransactionList, Exception> callback)
        {
            var retVal = new TypeTransactionList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_TYPETRANSACTION_FOLDER, App.Instance.User.UserName))
                {
                    var staticType = await StorageUtility.RestoreItem<TypeTransaction>(STATIC_TYPETRANSACTION_FOLDER, item, App.Instance.User.UserName);
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

        private async void RemoveInsertedTypeTransactions()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = TypeTransactionList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeTransactionId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                TypeTransactionList.RemoveAt(x.Index);
                await StorageUtility.DeleteItem<TypeTransaction>(STATIC_TYPETRANSACTION_FOLDER, x.Item, x.Item.TypeTransactionId);
            }
        }

        private async void LoadCachedNotifications(Action<NotificationList, Exception> callback)
        {
            var retVal = new NotificationList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_NOTIFICATION_FOLDER, App.Instance.User.UserName))
                {

                    var staticType = await StorageUtility.RestoreItem<Notification>(STATIC_NOTIFICATION_FOLDER, item, App.Instance.User.UserName);
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

        private async void RemoveInsertedNotifications()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = NotificationList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.NotificationId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                NotificationList.RemoveAt(x.Index);
                await StorageUtility.DeleteItem<Notification>(STATIC_NOTIFICATION_FOLDER, x.Item, x.Item.NotificationId);
            }
        }

        private async void LoadCachedTypeFrequencies(Action<TypeFrequencyList, Exception> callback)
        {
            var retVal = new TypeFrequencyList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_TYPEFREQUENCY_FOLDER, App.Instance.User.UserName))
                {

                    var staticType = await StorageUtility.RestoreItem<TypeFrequency>(STATIC_TYPEFREQUENCY_FOLDER, item, App.Instance.User.UserName);
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

        private async void RemoveInsertedTypeFrequencies()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = TypeFrequencyList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeFrequencyId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                TypeFrequencyList.RemoveAt(x.Index);
                await StorageUtility.DeleteItem<TypeFrequency>(STATIC_TYPEFREQUENCY_FOLDER, x.Item, x.Item.TypeFrequencyId);
            }
        }

        private async void LoadCachedTypeIntervals(Action<TypeIntervalList, Exception> callback)
        {
            var retVal = new TypeIntervalList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_TYPEINTERVAL_FOLDER, App.Instance.User.UserName))
                {
                    var staticType = await StorageUtility.RestoreItem<TypeInterval>(STATIC_TYPEINTERVAL_FOLDER, item, App.Instance.User.UserName);
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

        private async void LoadCachedTypeIntervalConfiguration(Action<BMA.BusinessLogic.TypeIntervalConfiguration, Exception> callback)
        {
            //var retVal = new BMA.BusinessLogic.TypeIntervalConfiguration();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_TYPEINTERVAL_FOLDER, App.Instance.User.UserName))
                {
                    var staticType = await StorageUtility.RestoreItem<BMA.BusinessLogic.TypeIntervalConfiguration>(STATIC_TYPEINTERVALCONFIG_FOLDER, item, App.Instance.User.UserName);
                    //retVal.Add(staticType);
                    IntervalConfiguration = staticType;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //callback(null, ex);
            }

            //SetupIntervalData(retVal, false);

            callback(IntervalConfiguration, null);
        }

        private async void RemoveInsertedTypeIntervals()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = IntervalList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.TypeIntervalId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                IntervalList.RemoveAt(x.Index);
                await StorageUtility.DeleteItem<TypeInterval>(STATIC_TYPEINTERVAL_FOLDER, x.Item, x.Item.TypeIntervalId);
            }
        }

        private async void LoadCachedRecurrenceRules(Action<RecurrenceRuleList, Exception> callback)
        {
            var retVal = new RecurrenceRuleList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_RECURRENCE_FOLDER, App.Instance.User.UserName))
                {
                    var staticType = await StorageUtility.RestoreItem<RecurrenceRule>(STATIC_RECURRENCE_FOLDER, item, App.Instance.User.UserName);
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

        private async void RemoveInsertedRecurrenceRules()
        {
            //REMOVE all inserted records as they will be added with a new Id
            var newItems = RecurrenceRuleList.Select((x, i) => new { Item = x, Index = i }).Where(x => x.Item.RecurrenceRuleId <= 0).OrderByDescending(x => x.Index).ToList();
            foreach (var x in newItems)
            {
                RecurrenceRuleList.RemoveAt(x.Index);
                await StorageUtility.DeleteItem<RecurrenceRule>(STATIC_RECURRENCE_FOLDER, x.Item, x.Item.RecurrenceRuleId);
            }
        }

        private async Task<List<User>> LoadCachedUser()
        {
            var retVal = new List<User>();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_USER_FOLDER, App.Instance.User.UserName))
                {
                    var staticType = await StorageUtility.RestoreItem<User>(STATIC_USER_FOLDER, item, App.Instance.User.UserName);
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

            var client = new StaticClient();
            try
            {
                latestState = Guid.NewGuid().ToString();
                client.GetAllCategoriesAsync(App.Instance.User.UserId);
                client.GetAllCategoriesCompleted += (o, e) =>
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

            var client = new StaticClient();

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

            var client = new StaticClient();

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

            var client = new StaticClient();

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

            var client = new StaticClient();

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

            var client = new StaticClient();

            client.GetAllTypeIntervalsAsync(App.Instance.User.UserId);
            client.GetAllTypeIntervalsCompleted += (o, e) =>
            {
                if (e.Error == null)
                    SetupIntervalData(e.Result, true);

                callback(e.Error);
            };
        }

        private void LoadLiveTypeIntervalConfiguration(Action<Exception> callback)
        {
            latestState = Guid.NewGuid().ToString();

            var client = new StaticClient();

            client.GetTypeIntervalConfigurationAsync(App.Instance.User.UserId);
            client.GetTypeIntervalConfigurationCompleted += (o, e) =>
            {
                if (e.Error == null)
                    SetupIntervalConfigurationData(e.Result, true);

                callback(e.Error);
            };
        }

        private void LoadLiveRecurrenceRules(Action<Exception> callback)
        {
            latestState = Guid.NewGuid().ToString();

            var client = new StaticClient();

            client.GetAllRecurrenceRulesAsync();
            client.GetAllRecurrenceRulesCompleted += (o, e) =>
            {
                if (e.Error == null)
                    SetupRecurrenceRuleData(e.Result, true);

                callback(e.Error);
            };
        }
        #endregion

        #region Save

        public void SaveCachedTypeTransaction(ObservableCollection<TypeTransaction> typeTransactions, Action<Exception> callback)
        {
            try
            {
                foreach (var item in TypeTransactionList.Where(x => x.HasChanges))
                    item.HasChanges = false;

                SetupTypeTransactionData(typeTransactions, false);

                App.Instance.IsSync = false;

                callback(null);
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }

        public void SaveCachedTypeFrequency(ObservableCollection<TypeFrequency> typeFrequencies, Action<Exception> callback)
        {
            try
            {
                foreach (var item in TypeFrequencyList.Where(x => x.HasChanges))
                    item.HasChanges = false;

                SetupTypeFrequencyData(typeFrequencies, false);

                App.Instance.IsSync = false;

                callback(null);
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }

        public void SaveCachedRecurrenceRule(ObservableCollection<RecurrenceRule> recurrenceRules, Action<Exception> callback)
        {
            try
            {
                foreach (var item in RecurrenceRuleList.Where(x => x.HasChanges))
                    item.HasChanges = false;

                SetupRecurrenceRuleData(recurrenceRules, false);

                App.Instance.IsSync = false;

                callback(null);
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }
        
        public void SaveCategory(ObservableCollection<Category> categories, Action<Exception> callback)
        {
            try
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        SaveCachedCategory(categories, callback);
                    }
                    else
                    {
                        foreach (var item in categories)
                            item.OptimizeOnTopLevel();

                        var client = new StaticClient();

                        client.SaveCategoriesAsync(categories, App.Instance.User);

                        client.SaveCategoriesCompleted +=  (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                //** TEST IF I CAN SEND NULL TO THE CALLBACK
                                LoadTypeTransactionReasons(null);

                                SetupTypeCategoryData(completedEventArgs.Result, true);

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

        public void SaveCachedCategory(ObservableCollection<Category> categories, Action<Exception> callback)
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

        public void SaveTransactionReason(ObservableCollection<TypeTransactionReason> transactionReasons, Action<Exception> callback)
        {
            try
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        SaveCachedTransactionReason(transactionReasons, callback);
                    }
                    else
                    {
                        foreach (var item in transactionReasons)
                            item.OptimizeOnTopLevel();

                        var client = new StaticClient();
                        client.SaveTypeTransactionReasonsAsync(transactionReasons);
                        client.SaveTypeTransactionReasonsCompleted +=  (sender, completedEventArgs) =>
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

        private void SaveCachedTransactionReason(ObservableCollection<TypeTransactionReason> transactionReasons, Action<Exception> callback)
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

        public void SaveTypeInterval(ObservableCollection<TypeInterval> typeInterval, Action<Exception> callback)
        {
            try
            {
                 App.Instance.StaticServiceData.SetServerStatus(status =>
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
                        var client = new StaticClient();
                        client.SaveTypeIntervalsAsync(typeInterval, App.Instance.User);
                        client.SaveTypeIntervalsCompleted += (sender, completedEventArgs) =>
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

        public void SaveNotification(ObservableCollection<Notification> notifications, Action<Exception> callback)
        {
            try
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        try
                        {
                            foreach (var item in NotificationList.Where(x => x.HasChanges))
                                item.HasChanges = false;

                            SetupNotificationData(notifications, false);

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
                        var client = new StaticClient();

                        client.SaveNotificationsAsync(notifications, App.Instance.User);

                        client.SaveNotificationsCompleted +=  (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                SetupNotificationData(completedEventArgs.Result, true);

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

        public void SaveTypeIntervalConfiguration(TypeIntervalConfiguration typeIntervalConfig, Action<Exception> callback)
        {
            try
            {
                App.Instance.StaticServiceData.SetServerStatus(status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        try
                        {
                            SetupTypeIntervalConfigData(typeIntervalConfig, false);

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
                        var client = new StaticClient();
                        client.SaveTypeIntervalConfigAsync(typeIntervalConfig);
                        client.SaveTypeIntervalConfigCompleted += (sender, completedEventArgs) =>
                        {
                            if (completedEventArgs.Error == null)
                            {
                                SetupTypeIntervalConfigData(completedEventArgs.Result, true);

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

        /// <summary>
        /// Online only
        /// </summary>
        /// <param name="user"></param>
        /// <param name="callback"></param>
        public void RegisterUser(User user, Action<User, Exception> callback)
        {
            try
            {
                App.Instance.StaticServiceData.SetServerStatus(async status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        try
                        {
                            Exception networkError = new Exception(AppResources.NetworkError);
                            callback(null, networkError);
                            return;

                            await UpdateCacheUserData(user);

                            App.Instance.IsSync = false;

                            var isCatDone = false;
                            var isTransReasonDone = false;
                            var isTypeTransDone = false;
                            var isTypeFreqDone = false;
                            var isRecRuleDone = false;


                            SaveCachedCategory(InitialData.InitializeCategories(), error =>
                            {
                                isCatDone = true;
                                if (CheckIfDone(isCatDone, isTransReasonDone, isTypeTransDone, isTypeFreqDone, isRecRuleDone))
                                    callback(user, null);
                            });
                            SaveCachedTransactionReason(InitialData.InitializeTypeTransactionReasons(), error =>
                            {
                                isTransReasonDone = true;
                                if (CheckIfDone(isCatDone, isTransReasonDone, isTypeTransDone, isTypeFreqDone, isRecRuleDone))
                                    callback(user, null);
                            });
                            SaveCachedTypeTransaction(InitialData.InitializeTypeTransactions(), error =>
                            {
                                isTypeTransDone = true;
                                if (CheckIfDone(isCatDone, isTransReasonDone, isTypeTransDone, isTypeFreqDone, isRecRuleDone))
                                    callback(user, null);
                            });
                            SaveCachedTypeFrequency(InitialData.InitializeTypeFrequencies(), error =>
                            {
                                isTypeFreqDone = true;
                                if (CheckIfDone(isCatDone, isTransReasonDone, isTypeTransDone, isTypeFreqDone, isRecRuleDone))
                                    callback(user, null);
                            });
                            SaveCachedRecurrenceRule(InitialData.InitializeRecurrenceRule(), error =>
                            {
                                isRecRuleDone = true;
                                if (CheckIfDone(isCatDone, isTransReasonDone, isTypeTransDone, isTypeFreqDone, isRecRuleDone))
                                    callback(user, null);
                            });

                            
                        }
                        catch (Exception ex)
                        {
                            callback(null, ex);
                        }
                    }
                    else
                    {
                        var client = new StaticClient();
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
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool CheckIfDone(bool isCatDone, bool isTransReasonDone, bool isTypeTransDone, bool isTypeFreqDone, bool isRecRuleDone)
        {
            return isCatDone && isTransReasonDone && isTypeTransDone && isTypeFreqDone && isRecRuleDone;
        }

        public void UpdateUser(User user, Action<Exception> callback)
        {
            try
            {
                App.Instance.StaticServiceData.SetServerStatus(async status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        try
                        {
                            App.Instance.User.HasChanges = false;
                            App.Instance.User.Update(user);

                            await UpdateCacheUserData(user);
                            
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
                        var client = new StaticClient();
                        client.UpdateUserAsync(user, callback);
                        client.UpdateUserCompleted += async (sender, eventArgs) =>
                        {
                            try
                            {
                                if (eventArgs.Error != null)
                                {
                                    callback(eventArgs.Error);
                                    return;
                                }

                                App.Instance.User.HasChanges = false;
                                App.Instance.User.Update(user);

                                await UpdateCacheUserData(user);

                                callback(null);
                            }
                            catch (Exception) { throw; }
                        };
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ForgotPassword(User user, Action<User, Exception> callback)
        {
            try
            {
                var client = new StaticClient();
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

        public void ChangePassword(User user, Action<Exception> callback)
        {
            try
            {
                App.Instance.StaticServiceData.SetServerStatus(async status =>
                {
                    //continue with local if status is ok but is pending Sync
                    if (status != Model.StaticServiceData.ServerStatus.Ok || !App.Instance.IsSync)
                    {
                        try
                        {
                            App.Instance.User.HasChanges = false;
                            App.Instance.User.Update(user);

                            await UpdateCacheUserData(user);

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
                        var client = new StaticClient();
                        client.ChangePasswordAsync(user, callback);
                        client.ChangePasswordCompleted += async (sender, eventArgs) =>
                        {
                            try
                            {
                                if (eventArgs.Error != null)
                                {
                                    callback(eventArgs.Error);
                                    return;
                                }

                                App.Instance.User.HasChanges = false;
                                App.Instance.User.Password = eventArgs.Result.Password;

                                await UpdateCacheUserData(eventArgs.Result);

                                callback(null);
                            }
                            catch (Exception) { throw; }
                        };
                    }
                });
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
            await StorageUtility.SaveItem(STATIC_USER_FOLDER, user, user.UserId, App.Instance.User.UserName);

            //var test = await StorageUtility.ListItems(STATIC_USER_FOLDER);
        }


        #endregion
    }
}
