using BMA.BusinessLogic;
using BMA.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;

namespace BMAServiceLib
{
    [Serializable]
    public class Static : IStatic
    {
        private const string ADMIN_USERNAME = "admin";
        private const int SYSTEM_USER_ID = 2;
        public bool GetDBStatus()
        {
            try
            {
                var result = false;
                using (var context = new EntityContext())
                {
                    var user = context.User.Take(1);
                    result = user != null;
                }
                return result;
            }
            catch(Exception)
            {
                throw;
            }
        }

        #region Load
        public StaticTypeList GetAllStaticData(int userId)
        {
            try
            {
                StaticTypeList typeData = new StaticTypeList();
                using (EntityContext context = new EntityContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    var typeTrans = (from i in context.TypeTransaction
                                     .Include(i => i.CreatedUser)
                                     .Include(i => i.ModifiedUser)
                                     where !i.IsDeleted && i.ModifiedUser.UserId == userId
                                     select i).ToList();

                    var typeSD = (from i in context.TypeSavingsDencity
                                  .Include(i => i.CreatedUser)
                                  .Include(i => i.ModifiedUser)
                                  where !i.IsDeleted && i.ModifiedUser.UserId == userId
                                  select i).ToList();

                    
                    var notice = (from i in context.Notification
                                  .Include(i => i.CreatedUser)
                                  .Include(i => i.ModifiedUser)
                                  where !i.IsDeleted && i.ModifiedUser.UserId == userId
                                  select i).ToList();

                    var typeF = (from i in context.TypeFrequency
                                 .Include(i => i.CreatedUser)
                                 .Include(i => i.ModifiedUser)
                                 where !i.IsDeleted && i.ModifiedUser.UserId == userId
                                 select i).ToList();

                    var inter = GetAllTypeIntervals(userId);

                    var budgetTH = (from i in context.BudgetThreshold
                                    .Include(i => i.CreatedUser)
                                    .Include(i => i.ModifiedUser)
                                    where !i.IsDeleted && i.ModifiedUser.UserId == userId
                                    select i).ToList();

                    var field = context.FieldType.ToList();

                    var recRule = (from i in context.RecurrenceRule
                                    .Include(i => i.RuleParts)
                                    select i).ToList();


                    typeData.TypeTransactions = typeTrans;
                    typeData.TypeSavingsDencities = typeSD;
                    typeData.Notifications = notice;
                    typeData.TypeFrequencies = typeF;
                    //typeData.TypeIntervals = inter;
                    typeData.BudgetThresholds = budgetTH;
                    typeData.RecurrenceRules = recRule;

                    return typeData;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CategoryList GetAllCategories(int userId)
        {
            try
            {
                var result = new CategoryList();
                using (EntityContext context = new EntityContext())
                {
                    var query = (from i in context.Category
                                .Include(x => x.TypeTransactionReasons)
                                .Include(i => i.ModifiedUser)
                                .Include(i => i.CreatedUser)
                                 orderby i.Name ascending
                                 where !i.IsDeleted && (i.ModifiedUser.UserId == SYSTEM_USER_ID || i.ModifiedUser.UserId == userId)
                                 select new
                                 {
                                     Category = i,
                                     CreatedUser = i.CreatedUser,
                                     ModifiedUser = i.ModifiedUser,
                                     Reasons = i.TypeTransactionReasons.Where(x => !x.IsDeleted)
                                 }).ToList();

                    query.ForEach(x =>
                    {
                        var reasons = x.Reasons.ToList();

                        reasons.ForEach(z =>
                            {
                                z = context.TransactionReason
                                        .Include(i => i.CreatedUser)
                                        .Include(i => i.ModifiedUser)
                                        .FirstOrDefault(k => k.TypeTransactionReasonId == z.TypeTransactionReasonId);
                                z.HasChanges = false; 
                            });

                        x.Category.HasChanges = false;

                        x.Category.TypeTransactionReasons = reasons.Where(z => z.ModifiedUser.UserId == SYSTEM_USER_ID || z.ModifiedUser.UserId == userId).ToList();

                        result.Add(x.Category);
                    });

                    result.PrepareForServiceSerialization();

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TypeTransactionReasonList GetAllTypeTransactionReasons(int userId)
        {
            try
            {
                var result = new TypeTransactionReasonList();
                using (EntityContext context = new EntityContext())
                {
                    var query = (from i in context.TransactionReason
                                .Include(x => x.CreatedUser)
                                .Include(x => x.ModifiedUser)
                                .Include(x => x.Categories)
                                 orderby i.Name ascending
                                 where !i.IsDeleted && (i.ModifiedUser.UserId == SYSTEM_USER_ID || i.ModifiedUser.UserId == userId)
                                 select new {
                                     TransReason = i,
                                     CreatedUser = i.CreatedUser,
                                     ModifiedUser = i.ModifiedUser,
                                     Categories = i.Categories.Where(x => !x.IsDeleted)// && (x.ModifiedUser.UserId == SYSTEM_USER_ID || x.ModifiedUser.UserId == userId))
                                 }).ToList();

                    query.ForEach(x =>
                        {
                            var cats = x.Categories.ToList();

                            cats.ForEach(z =>
                            {
                                z = context.Category
                                        .Include(i => i.CreatedUser)
                                        .Include(i => i.ModifiedUser)
                                        .FirstOrDefault(k => k.CategoryId == z.CategoryId);
                                z.HasChanges = false;
                            });

                            x.TransReason.HasChanges = false;

                            x.TransReason.Categories = cats;
                            
                            result.Add(x.TransReason);
                        });

                    result.PrepareForServiceSerialization();

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Notification> GetAllNotifications(int userId)
        {
            return GetDataGeneric<Notification>(userId);
        }

        public List<TypeSavingsDencity> GetAllTypeSavingsDencities(int userId)
        {
            return GetDataGeneric<TypeSavingsDencity>(userId);
        }

        public RecurrenceRuleList GetAllRecurrenceRules()
        {
            try
            {
                var result = new RecurrenceRuleList();
                using (EntityContext context = new EntityContext())
                {
                    var recRule = (from i in context.RecurrenceRule
                                        .Include(i => i.RuleParts)
                                   select i).ToList();

                    var fields = (from i in context.FieldType select i).ToList();
                    //recRule.ForEach(x=>x.ruleParts.ForEach(
                    //recRule.ForEach(x => x.RuleParts.ForEach(z =>
                    //    {
                    //        z.FieldType = context.FieldType.FirstOrDefault(k=>k.RulePartId == z.RulePartId);
                    //    }));

                    recRule.ForEach(x=>result.Add(x));

                    result.PrepareForServiceSerialization();

                    return result;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TypeTransaction> GetAllTypeTransactions(int userId)
        {
            return GetDataGeneric<TypeTransaction>(userId);
        }

        public List<TypeFrequency> GetAllTypeFrequencies(int userId)
        {
            return GetDataGeneric<TypeFrequency>(userId);
        }

        public List<BudgetThreshold> GetAllBudgetThresholds(int userId)
        {
            return GetDataGeneric<BudgetThreshold>(userId);
        }

        public TypeIntervalList GetAllTypeIntervals(int userId)
        {
            try
            {
                var result = new TypeIntervalList();

                using (EntityContext context = new EntityContext())
                {
                    var query = (from i in context.TypeInterval
                                 .Include(i => i.TransactionType)
                                 .Include(i => i.TransactionReasonType)
                                 .Include(i => i.Category)
                                 .Include(i => i.RecurrenceRuleValue.RecurrenceRule)
                                 .Include(i => i.RecurrenceRuleValue.RulePartValueList)
                                 .Include(i => i.RecurrenceRangeRuleValue.RecurrenceRule)
                                 .Include(i => i.RecurrenceRangeRuleValue.RulePartValueList)
                                 .Include(i => i.CreatedUser)
                                 .Include(i => i.ModifiedUser)
                                 where !i.IsDeleted && (i.ModifiedUser.UserId == SYSTEM_USER_ID || i.ModifiedUser.UserId == userId)
                     select i).ToList();


                    query.ForEach(x =>
                    {
                        if (x.RecurrenceRuleValue != null && x.RecurrenceRuleValue.RulePartValueList != null)
                        {
                            x.RecurrenceRuleValue.RecurrenceRule.RuleParts = new List<RulePart>();
                            x.RecurrenceRuleValue.RulePartValueList.ForEach(z =>
                            {
                                var tt = context.RulePartValue.Include(i => i.RulePart).Include(i => i.RulePart.FieldType).Where(k => k.RulePartValueId == z.RulePartValueId).ToList();
                                x.RecurrenceRuleValue.RecurrenceRule.RuleParts.Add(z.RulePart);

                            });
                        }
                    });

                    query.ForEach(x =>
                    {
                        if (x.RecurrenceRangeRuleValue != null && x.RecurrenceRangeRuleValue.RulePartValueList != null)
                        {
                            x.RecurrenceRangeRuleValue.RecurrenceRule.RuleParts = new List<RulePart>();
                            x.RecurrenceRangeRuleValue.RulePartValueList.ForEach(z =>
                            {
                                var tt = context.RulePartValue.Include(i => i.RulePart).Include(i => i.RulePart.FieldType).Where(k => k.RulePartValueId == z.RulePartValueId).ToList();
                                x.RecurrenceRangeRuleValue.RecurrenceRule.RuleParts.Add(z.RulePart);

                            });
                        }
                        result.Add(x);
                    });

                    result.PrepareForServiceSerialization();

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public User AuthenticateUser(User user)
        {
            User result = null;
            try
            {
                using(EntityContext context = new EntityContext())
                {
                    var query = context.User.FirstOrDefault(i => i.UserName == user.UserName && !i.IsDeleted);

                    if (query == null)
                        throw new Exception("Username cannot be found");
                    else if (query != null && query.Password != user.Password)
                        throw new Exception("Username and password don't match");
                    else
                        result = query;
                }
            }
            catch (Exception)
            {                
                throw;
            }
            return result;
        }

        public List<Notification> GetUpcomingNotifications(DateTime clientTime, int userId)
        {
            try
            {
                using (EntityContext context= new EntityContext())
                {
                    List<Notification> result = new List<Notification>();
                    var query = GetAllNotifications(userId);

                    foreach(var item in query)
                    {
                        clientTime = new DateTime(item.Time.Year, item.Time.Month, item.Time.Day, clientTime.Hour, clientTime.Minute, 0);
                        DateTime dtRange = clientTime.AddMinutes(15);

                        if (item.Time >= clientTime && item.Time <= dtRange)
                            result.Add(item);
                    }
                        
                        //var     
                        //return i.Time >= clientTime && i.Time <= dtRange;


                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TypeIntervalConfiguration GetTypeIntervalConfiguration(int userId)
        {
            return GetDataGeneric<TypeIntervalConfiguration>(userId).FirstOrDefault();
        }

        public List<T> GetDataGeneric<T>(int userId) where T : class
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var dynTable = context.Set<T>();
                    var query = (from i in dynTable
                                .Include("ModifiedUser")
                                .Include("CreatedUser")
                                .Where("IsDeleted == false AND (ModifiedUser.UserId = @0 OR ModifiedUser.UserId = @1)", SYSTEM_USER_ID, userId)
                                 select i).ToList();

                    query.ForEach(x =>
                        {
                            var temp = (x as BaseItem);
                            temp.HasChanges = false;
                        });

                    return query;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Save / Add
        //public StaticTypeList SyncStaticData(StaticTypeList staticData, int userId)
        //{
        //    var result = new StaticTypeList();

        //    result.Categories = SyncCategories(staticData.Categories);
        //    result.BudgetThresholds = SyncBudgetThresholds(staticData.BudgetThresholds);
        //    result.Notifications = SyncNotifications(staticData.Notifications);
        //    result.TypeFrequencies = SyncTypeFrequencies(staticData.TypeFrequencies);
        //    result.TypeIntervals = SyncTypeIntervals(staticData.TypeIntervals, userId);
        //    result.TypeTransactionReasons = SyncTypeTransactionReasons(staticData.TypeTransactionReasons);
        //    result.TypeTransactions = SyncTypeTransactions(staticData.TypeTransactions);

        //    return result;
        //}

        //public List<Category> SyncCategories(List<Category> categories)
        //{
        //    try
        //    {
        //        using (EntityContext context = new EntityContext())
        //        {
        //            var categoryList = new List<Category>();
        //            foreach (var item in GetAllCategories())
        //            {
        //                if (categories.Where(i => i.CategoryId == item.CategoryId).Count() == 0)
        //                {
        //                    item.IsDeleted = true;
        //                    item.ModifiedDate = DateTime.Now;
        //                    categoryList.Add(item);
        //                }
        //            }
        //            foreach (var item in categories.Where(i => i.HasChanges))
        //            {
        //                item.ModifiedDate = DateTime.Now;
        //                categoryList.Add(item);
        //            }

        //            return SaveCategories(categoryList);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public List<TypeTransactionReason> SyncTypeTransactionReasons(List<TypeTransactionReason> typeTransactionReasons)
        //{
        //    try
        //    {
        //        using (EntityContext context = new EntityContext())
        //        {
        //            var typeTransactionReasonList = new List<TypeTransactionReason>();
        //            foreach (var item in GetAllTypeTransactionReasons())
        //            {
        //                if (typeTransactionReasons.Where(i => i.TypeTransactionReasonId == item.TypeTransactionReasonId).Count() == 0)
        //                {
        //                    item.IsDeleted = true;
        //                    item.ModifiedDate = DateTime.Now;
        //                    typeTransactionReasonList.Add(item);
        //                }
        //            }
        //            foreach (var item in typeTransactionReasons.Where(i => i.HasChanges))
        //            {
        //                item.ModifiedDate = DateTime.Now;
        //                typeTransactionReasonList.Add(item);
        //            }

        //            return SaveTypeTransactionReasons(typeTransactionReasonList);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public List<TypeTransaction> SyncTypeTransactions(List<TypeTransaction> typeTransactions)
        //{
        //    try
        //    {
        //        using (EntityContext context = new EntityContext())
        //        {
        //            var typeTransactionList = new List<TypeTransaction>();
        //            foreach (var item in GetAllTypeTransactions())
        //            {
        //                if (typeTransactions.Where(i => i.TypeTransactionId == item.TypeTransactionId).Count() == 0)
        //                {
        //                    item.IsDeleted = true;
        //                    item.ModifiedDate = DateTime.Now;
        //                    typeTransactionList.Add(item);
        //                }
        //            }
        //            foreach (var item in typeTransactions.Where(i => i.HasChanges))
        //            {
        //                item.ModifiedDate = DateTime.Now;
        //                typeTransactionList.Add(item);
        //            }

        //            return SaveTypeTransactions(typeTransactionList);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public List<Notification> SyncNotifications(List<Notification> notifications)
        //{
        //    try
        //    {
        //        using (EntityContext context = new EntityContext())
        //        {
        //            var notificationList = new List<Notification>();
        //            foreach (var item in GetAllNotifications())
        //            {
        //                if (notifications.Where(i => i.NotificationId == item.NotificationId).Count() == 0)
        //                {
        //                    item.IsDeleted = true;
        //                    item.ModifiedDate = DateTime.Now;
        //                    notificationList.Add(item);
        //                }
        //            }
        //            foreach (var item in notifications.Where(i => i.HasChanges))
        //            {
        //                item.ModifiedDate = DateTime.Now;
        //                notificationList.Add(item);
        //            }

        //            return SaveNotifications(notificationList);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public List<TypeFrequency> SyncTypeFrequencies(List<TypeFrequency> typeFrequencies)
        //{
        //    try
        //    {
        //        using (EntityContext context = new EntityContext())
        //        {
        //            var typeFrequencyList = new List<TypeFrequency>();
        //            foreach (var item in GetAllTypeFrequencies())
        //            {
        //                if (typeFrequencies.Where(i => i.TypeFrequencyId == item.TypeFrequencyId).Count() == 0)
        //                {
        //                    item.IsDeleted = true;
        //                    item.ModifiedDate = DateTime.Now;
        //                    typeFrequencyList.Add(item);
        //                }
        //            }
        //            foreach (var item in typeFrequencies.Where(i => i.HasChanges))
        //            {
        //                item.ModifiedDate = DateTime.Now;
        //                typeFrequencyList.Add(item);
        //            }

        //            return SaveTypeFrequencies(typeFrequencyList);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public List<TypeInterval> SyncTypeIntervals(List<TypeInterval> typeIntervals, int userId)
        //{
        //    try
        //    {
        //        using (EntityContext context = new EntityContext())
        //        {
        //            var typeIntervalList = new List<TypeInterval>();
        //            foreach (var item in GetAllTypeIntervals(userId))
        //            {
        //                if (typeIntervals.Where(i => i.TypeIntervalId == item.TypeIntervalId).Count() == 0)
        //                {
        //                    item.IsDeleted = true;
        //                    item.ModifiedDate = DateTime.Now;
        //                    typeIntervalList.Add(item);
        //                }
        //            }
        //            foreach (var item in typeIntervals.Where(i => i.HasChanges))
        //            {
        //                item.ModifiedDate = DateTime.Now;
        //                typeIntervalList.Add(item);
        //            }

        //            return SaveTypeIntervals(typeIntervalList);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public List<BudgetThreshold> SyncBudgetThresholds(List<BudgetThreshold> budgetThresholds)
        //{
        //    try
        //    {
        //        using (EntityContext context = new EntityContext())
        //        {
        //            var budgetThresholdList = new List<BudgetThreshold>();
        //            foreach (var item in GetAllBudgetThresholds())
        //            {
        //                if (budgetThresholds.Where(i => i.BudgetThresholdId == item.BudgetThresholdId).Count() == 0)
        //                {
        //                    item.IsDeleted = true;
        //                    item.ModifiedDate = DateTime.Now;
        //                    budgetThresholdList.Add(item);
        //                }
        //            }
        //            foreach (var item in budgetThresholds.Where(i => i.HasChanges))
        //            {
        //                item.ModifiedDate = DateTime.Now;
        //                budgetThresholdList.Add(item);
        //            }

        //            return SaveBudgetThresholds(budgetThresholdList);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public CategoryList SaveCategories(CategoryList categories)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in categories)
                    {
                        if (item.CategoryId > 0) //Update
                        {
                            var original = context.Category.Where(t =>
                                                                t.CategoryId == item.CategoryId &&
                                                                t.ModifiedDate < item.ModifiedDate &&
                                                                !t.IsDeleted).FirstOrDefault();

                            if (original != null)
                            {
                                context.Entry(original).Collection(x => x.TypeTransactionReasons).Load();

                                item.HasChanges = false;
                                
                                original.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                                original.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                                if (item.TypeTransactionReasons != null)
                                {
                                    item.TypeTransactionReasons.ForEach(x =>
                                    {
                                        var query = context.TransactionReason.FirstOrDefault(k => k.TypeTransactionReasonId == x.TypeTransactionReasonId);
                                        if (x.IsDeleted)
                                            original.TypeTransactionReasons.Remove(query);
                                        else
                                            original.TypeTransactionReasons.Add(query);
                                    });
                                }

                                context.Entry(original).CurrentValues.SetValues(item);
                                updateFound = true;
                            }
                        }
                        else //Insert
                        {
                            item.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                            item.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                            if (item.TypeTransactionReasons != null)
                            {
                                item.TypeTransactionReasons.ForEach(x =>
                                {
                                    x.CreatedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == x.CreatedUser.UserId);
                                    x.ModifiedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == x.ModifiedUser.UserId);

                                    x.Categories = null; 
                                    
                                    context.Entry(x).State = System.Data.EntityState.Unchanged;
                                });
                            }

                            context.Category.Add(item);
                            
                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }

                categories.PrepareForServiceSerialization();

                return categories;
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder s = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        s.Append(string.Format("{0} - {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(s.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TypeTransactionReasonList SaveTypeTransactionReasons(TypeTransactionReasonList typeTransactionReasons)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in typeTransactionReasons)
                    {
                        if (item.TypeTransactionReasonId > 0) //Update
                        {
                            item.HasChanges = false;
                            
                            var original = context.TransactionReason.Where(t => t.TypeTransactionReasonId == item.TypeTransactionReasonId &&
                                                                            t.ModifiedDate < item.ModifiedDate &&
                                                                            !t.IsDeleted).FirstOrDefault();

                            if (original != null)
                            {
                                context.Entry(original).Collection(x => x.Categories).Load();

                                if (item.Categories != null)
                                {
                                    for(var i=item.Categories.Count-1; i >= 0 ;i--)
                                    {
                                        var catId = item.Categories[i].CategoryId;
                                        var query = context.Category.FirstOrDefault(k => k.CategoryId == catId);
                                        if (item.Categories[i].IsDeleted)
                                        {
                                            original.Categories.Remove(query);
                                            item.Categories.RemoveAt(i);
                                        }
                                        else
                                        {
                                            original.Categories.Add(query);
                                        }
                                    }
                                }

                                context.Entry(original).CurrentValues.SetValues(item);
                                updateFound = true;
                            }
                        }
                        else //Insert
                        {
                            item.CreatedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                            item.ModifiedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                            if (item.Categories != null)
                            {
                                item.Categories.ForEach(x =>
                                        {
                                            x.CreatedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == x.CreatedUser.UserId);
                                            x.ModifiedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == x.ModifiedUser.UserId);

                                            x.TypeTransactionReasons = null;

                                            context.Entry(x).State = System.Data.EntityState.Unchanged;
                                        });
                            }

                            context.TransactionReason.Add(item);
                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }

                typeTransactionReasons.PrepareForServiceSerialization();

                return typeTransactionReasons;
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder s = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        s.Append(string.Format("{0} - {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(s.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotificationList SaveNotifications(NotificationList notifications)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in notifications)
                    {
                        if (item.NotificationId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.Notification.Where(t => t.NotificationId == item.NotificationId && !t.IsDeleted).FirstOrDefault();

                            if (original.ModifiedDate < item.ModifiedDate)
                            {
                                context.Entry(original).CurrentValues.SetValues(item);
                                updateFound = true;
                            }
                        }
                        else //Insert
                        {

                            item.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                            item.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);


                            context.Notification.Add(item);
                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }

                notifications.PrepareForServiceSerialization();

                return notifications;
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder s = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        s.Append(string.Format("{0} - {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(s.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TypeTransaction> SaveTypeTransactions(List<TypeTransaction> typeTransactions)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in typeTransactions)
                    {
                        if (item.TypeTransactionId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.TypeTransaction.Where(t => t.TypeTransactionId == item.TypeTransactionId && !t.IsDeleted).FirstOrDefault();

                            if (original.ModifiedDate < item.ModifiedDate)
                            {
                                context.Entry(original).CurrentValues.SetValues(item);
                                updateFound = true;
                            }
                        }
                        else //Insert
                        {
                            context.TypeTransaction.Add(item);
                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }
                return GetAllTypeTransactions(0);
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder s = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        s.Append(string.Format("{0} - {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(s.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TypeFrequency> SaveTypeFrequencies(List<TypeFrequency> typeFrequencies)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in typeFrequencies)
                    {
                        if (item.TypeFrequencyId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.TypeFrequency.Where(t => t.TypeFrequencyId == item.TypeFrequencyId && !t.IsDeleted).FirstOrDefault();

                            if (original.ModifiedDate < item.ModifiedDate)
                            {
                                context.Entry(original).CurrentValues.SetValues(item);
                                updateFound = true;
                            }
                        }
                        else //Insert
                        {
                            context.TypeFrequency.Add(item);
                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }
                return GetAllTypeFrequencies(0);
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder s = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        s.Append(string.Format("{0} - {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(s.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TypeIntervalList SaveTypeIntervals(TypeIntervalList typeIntervals)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in typeIntervals)
                    {

                        if (item.TypeIntervalId > 0) //Update
                        {
                            //## RulePart will remain null after this query, but it is not need
                            var original = context.TypeInterval
                                                    .Include(i => i.CreatedUser)
                                                    .Include(i => i.Category)
                                                    .Include(i => i.TransactionReasonType)
                                                    .Include(i => i.TransactionType)
                                                    .Include(i => i.RecurrenceRuleValue.RecurrenceRule)
                                                    .Include(i => i.RecurrenceRuleValue.RulePartValueList)
                                                    .Include(i => i.RecurrenceRangeRuleValue.RecurrenceRule)
                                                    .Include(i => i.RecurrenceRangeRuleValue.RulePartValueList)
                                                    .Where(t =>
                                                        t.TypeIntervalId == item.TypeIntervalId &&
                                                        t.ModifiedDate < item.ModifiedDate &&
                                                        !t.IsDeleted).FirstOrDefault();

                            

                            if (original == null)
                                throw new Exception("No TypeInterval found to update");

                            item.HasChanges = false;

                            if (original.ModifiedDate < item.ModifiedDate)
                            {
                                //## RECURRENCE RULE ##//
                                if (item.RecurrenceRuleValue != null && item.RecurrenceRuleValue.RulePartValueList != null)
                                {
                                    //## Remove previous rule
                                    if (original.RecurrenceRuleValue.RecurrenceRule != null)
                                    {
                                        context.Entry(original.RecurrenceRuleValue.RecurrenceRule).State = System.Data.EntityState.Unchanged;
                                        for (int x = original.RecurrenceRuleValue.RulePartValueList.Count - 1; x >= 0; x--)
                                        {
                                            var i = original.RecurrenceRuleValue.RulePartValueList[x].RulePartValueId;
                                            var temp = context.RulePartValue.FirstOrDefault(z => z.RulePartValueId == i);
                                            context.RulePartValue.Remove(temp);
                                        }
                                        context.Entry(original.RecurrenceRuleValue).State = System.Data.EntityState.Deleted;
                                        //## Previous rule is removed
                                    }

                                    original.RecurrenceRuleValue = new RecurrenceRulePart();
                                    original.RecurrenceRuleValue.RecurrenceRule = context.RecurrenceRule.FirstOrDefault(x=>x.RecurrenceRuleId == item.RecurrenceRuleValue.RecurrenceRule.RecurrenceRuleId);

                                    original.RecurrenceRuleValue.RulePartValueList = new List<RulePartValue>();
                                    original.RecurrenceRuleValue.RulePartValueList = item.RecurrenceRuleValue.RulePartValueList;
                                    original.RecurrenceRuleValue.RulePartValueList.ForEach(x =>
                                    {
                                        x.RulePart = context.RulePart.FirstOrDefault(z => z.RulePartId == x.RulePart.RulePartId);
                                    });

                                }

                                //## RECURRENCE RANGE RULE ##//
                                if (item.RecurrenceRangeRuleValue != null && item.RecurrenceRangeRuleValue.RulePartValueList != null)
                                {
                                  //## Remove previous rule
                                    if (original.RecurrenceRangeRuleValue.RecurrenceRule != null)
                                    {
                                        context.Entry(original.RecurrenceRangeRuleValue.RecurrenceRule).State = System.Data.EntityState.Unchanged;
                                        for (int x = original.RecurrenceRangeRuleValue.RulePartValueList.Count - 1; x >= 0; x--)
                                        {
                                            var i = original.RecurrenceRangeRuleValue.RulePartValueList[x].RulePartValueId;
                                            var temp = context.RulePartValue.FirstOrDefault(z => z.RulePartValueId == i);
                                            context.RulePartValue.Remove(temp);
                                        }
                                        context.Entry(original.RecurrenceRangeRuleValue).State = System.Data.EntityState.Deleted;
                                    }
                                    //## Previous rule is removed


                                    original.RecurrenceRangeRuleValue = new RecurrenceRulePart();
                                    original.RecurrenceRangeRuleValue.RecurrenceRule = context.RecurrenceRule.FirstOrDefault(x => x.RecurrenceRuleId == item.RecurrenceRangeRuleValue.RecurrenceRule.RecurrenceRuleId);

                                    original.RecurrenceRangeRuleValue.RulePartValueList = new List<RulePartValue>();
                                    original.RecurrenceRangeRuleValue.RulePartValueList = item.RecurrenceRangeRuleValue.RulePartValueList;
                                    original.RecurrenceRangeRuleValue.RulePartValueList.ForEach(x =>
                                    {
                                        x.RulePart = context.RulePart.FirstOrDefault(z => z.RulePartId == x.RulePart.RulePartId);
                                    });
                                }

                                original.Category = context.Category.Where(k => !k.IsDeleted).Single(p => p.CategoryId == item.Category.CategoryId);
                                original.TransactionReasonType = context.TransactionReason.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionReasonId == item.TransactionReasonType.TypeTransactionReasonId);
                                original.TransactionType = context.TypeTransaction.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionId == item.TransactionType.TypeTransactionId);

                                context.Entry(original).CurrentValues.SetValues(item);

                                updateFound = true;
                            }
                        }
                        else //Insert
                        {

                            item.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                            item.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                            item.Category = context.Category.Where(k => !k.IsDeleted).Single(p => p.CategoryId == item.Category.CategoryId);
                            item.TransactionReasonType = context.TransactionReason.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionReasonId == item.TransactionReasonType.TypeTransactionReasonId);
                            item.TransactionType = context.TypeTransaction.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionId == item.TransactionType.TypeTransactionId);
                            
                            item.Category.TypeTransactionReasons = null;

                            ////// \\\\\
                            item.RecurrenceRuleValue.SetPrivateRecurrenceRuleForSave(context.RecurrenceRule.FirstOrDefault(x => x.RecurrenceRuleId == item.RecurrenceRuleValue.RecurrenceRule.RecurrenceRuleId));
                            item.RecurrenceRangeRuleValue.SetPrivateRecurrenceRuleForSave(context.RecurrenceRule.FirstOrDefault(x => x.RecurrenceRuleId == item.RecurrenceRangeRuleValue.RecurrenceRule.RecurrenceRuleId));

                            item.RecurrenceRuleValue.RulePartValueList.ForEach(x =>
                            {
                                x.RulePart = context.RulePart.Include(i => i.FieldType).Include(i => i.RecurrenceRules).FirstOrDefault(z => z.RulePartId == x.RulePart.RulePartId);
                                x.RulePart.FieldType = context.FieldType.FirstOrDefault(z=>z.FieldTypeId == x.RulePart.FieldType.FieldTypeId);
                            });

                            item.RecurrenceRangeRuleValue.RulePartValueList.ForEach(x =>
                            {
                                x.RulePart = context.RulePart.Include(i => i.FieldType).Include(i => i.RecurrenceRules).FirstOrDefault(z => z.RulePartId == x.RulePart.RulePartId);
                                x.RulePart.FieldType = context.FieldType.FirstOrDefault(z => z.FieldTypeId == x.RulePart.FieldType.FieldTypeId);
                            });

                            context.TypeInterval.Add(item);

                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }

                typeIntervals.PrepareForServiceSerialization();

                return typeIntervals;
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder s = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        s.Append(string.Format("{0} - {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(s.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public TypeIntervalConfiguration SaveTypeIntervalConfig(TypeIntervalConfiguration typeIntervalConfig)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    if (typeIntervalConfig.TypeIntervalConfigurationId > 0) //Update
                    {
                        typeIntervalConfig.HasChanges = false;
                        var original = context.TypeIntervalConfiguration.Where(t => t.TypeIntervalConfigurationId == typeIntervalConfig.TypeIntervalConfigurationId && !t.IsDeleted).FirstOrDefault();

                        if (original.ModifiedDate < typeIntervalConfig.ModifiedDate)
                        {
                            context.Entry(original).CurrentValues.SetValues(typeIntervalConfig);
                            updateFound = true;
                        }
                    }
                    else //Insert
                    {
                        typeIntervalConfig.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == typeIntervalConfig.CreatedUser.UserId);
                        typeIntervalConfig.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == typeIntervalConfig.ModifiedUser.UserId);

                        context.TypeIntervalConfiguration.Add(typeIntervalConfig);
                        updateFound = true;
                    }

                    if (updateFound)
                        context.SaveChanges();
                }

                //typeIntervalConfig.PrepareForServiceSerialization();

                return typeIntervalConfig;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BudgetThreshold> SaveBudgetThresholds(List<BudgetThreshold> budgetThresholds)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in budgetThresholds)
                    {
                        if (item.BudgetThresholdId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.BudgetThreshold.Where(t => t.BudgetThresholdId == item.BudgetThresholdId && !t.IsDeleted).FirstOrDefault();

                            if (original.ModifiedDate < item.ModifiedDate)
                            {
                                context.Entry(original).CurrentValues.SetValues(item);
                                updateFound = true;
                            }
                        }
                        else //Insert
                        {
                            context.BudgetThreshold.Add(item);
                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }
                return GetAllBudgetThresholds(0);
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder s = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        s.Append(string.Format("{0} - {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(s.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public User ChangePassword(User user)
        {
            try
            {
                User result = UpdateRegisterUser(user);
                SendEmail(result, NoticeState.ResetPassword, error => { var a = ""; });

                return new User();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder s = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        s.Append(string.Format("{0} - {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(s.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public User UpdateUser(User user)
        {
            try
            {
                User result = UpdateRegisterUser(user);
                //SendEmail(result, NoticeState.ResetPassword);

                return new User();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public User RegisterUser(User user)
        {
            User result = UpdateRegisterUser(user);

            //Task threads = new Task[1];
            SendEmail(result, NoticeState.NewAccount, error => { var a = ""; });

            return result;

        }

        public User ForgotPassword(User user)
        {
            try
            {
                User result = new User();
                using (EntityContext context = new EntityContext())
                {
                    var query = context.User.Where(i => i.UserName == user.UserName).FirstOrDefault();

                    if (query == null)
                        throw new Exception("User doesn't exist");

                    result = query;
                }

                SendEmail(result, NoticeState.SendPassword, error => { var a = ""; });

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private User UpdateRegisterUser(User user)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    if (user.UserId > 0) //Update
                    {
                        user.HasChanges = false;
                        var original = context.User.Where(t => t.UserId == user.UserId && !t.IsDeleted).FirstOrDefault();

                        //original.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == user.CreatedUser.UserId);
                        //original.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == user.ModifiedUser.UserId);

                        context.Entry(original).CurrentValues.SetValues(user);
                    }
                    else //Insert
                    {
                        if (context.User.Where(i => i.UserName == user.UserName).ToList().Count > 0)
                            throw new Exception("Username exist");

                        if (context.User.Where(i => i.Email == user.Email).ToList().Count > 0)
                            throw new Exception("Email exist");

                        if(user.Birthdate == DateTime.MinValue)
                            user.Birthdate = new DateTime(1900, 1,1);

                        var query = context.User.Where(i => i.UserName == ADMIN_USERNAME).FirstOrDefault();
                        user.CreatedUser = query;
                        user.ModifiedUser = query;

                        context.User.Add(user);
                    }

                    context.SaveChanges();
                }

                return AuthenticateUser(user);
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder s = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        s.Append(string.Format("{0} - {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }

                throw new DbEntityValidationException(s.ToString());
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Email Helpers

        enum NoticeState
        {
            NewAccount,
            ResetPassword,
            SendPassword
        }

        private void SendEmail(User user, NoticeState state, Action<Exception> callback)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("yiannisezn@gmail.com", "ysdd@gmail")
                };

                MailMessage message = new MailMessage();
                message.To.Add(user.Email);
                message.IsBodyHtml = true;
                message.Subject = "Automated mail notification";
                message.From = new System.Net.Mail.MailAddress("yiannisezn@gmail.com");

                switch (state)
                {
                    case NoticeState.NewAccount:
                        message.Body = string.Format("Thanks for creating a new account. Your details are \n\rUsername: {0}\n\rPassword{1}", 
                                                    user.UserName, user.Password);
                        break;
                    case NoticeState.ResetPassword:
                        message.Body = string.Format("Password reset successfully. Your new password is {0}", user.Password);
                        break;
                    case NoticeState.SendPassword:
                        message.Body = string.Format("Password: {0}", user.Password);
                        break;
                    default:
                        message.Body = "This is the message body";
                        break;
                }

                client.Send(message);

                callback(null);
            }
            catch (Exception ex)
            {
                callback(ex);
            }
        }
        #endregion
    }
}
