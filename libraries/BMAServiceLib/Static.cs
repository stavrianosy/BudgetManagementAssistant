using BMA.BusinessLogic;
using BMA.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
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
        #region Load
        public StaticTypeList GetAllStaticData()
        {
            try
            {
                StaticTypeList typeData = new StaticTypeList();
                using (EntityContext context = new EntityContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;
                    var typeTrans = (from i in context.TypeTransaction
                                     .Include(i => i.CreatedUser)
                                     where !i.IsDeleted
                                     select i).ToList();

                    var typeSD = (from i in context.TypeSavingsDencity
                                  .Include(i => i.CreatedUser)
                                  where !i.IsDeleted
                                  select i).ToList();

                    
                    var notice = (from i in context.Notification
                                  .Include(i => i.CreatedUser)
                                  where !i.IsDeleted
                                  select i).ToList();

                    var typeF = (from i in context.TypeFrequency
                                 .Include(i => i.CreatedUser)
                                 where !i.IsDeleted
                                 select i).ToList();

                    var inter = (from i in context.TypeInterval
                                 .Include(i => i.CreatedUser)
                                 where !i.IsDeleted
                                 select i).ToList();

                    var budgetTH = (from i in context.BudgetThreshold
                                    .Include(i => i.CreatedUser)
                                    where !i.IsDeleted
                                    select i).ToList();

                    typeData.TypeTransactions = typeTrans;
                    typeData.TypeSavingsDencities = typeSD;
                    typeData.Notifications = notice;
                    typeData.TypeFrequencies = typeF;
                    typeData.TypeIntervals = inter;
                    typeData.BudgetThresholds = budgetTH;

                    return typeData;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Category> GetAllCategories()
        {
        try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = (from i in context.Category
                                .Include(x=>x.TypeTransactionReasons)
                                .Include(x=>x.CreatedUser)
                                orderby i.Name ascending
                                where !i.IsDeleted
                                select i).ToList();

                    query.ForEach(x => x.TypeTransactionReasons.ForEach(z => z.Categories = null));

                    return query;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TypeTransactionReason> GetAllTypeTransactionReasons()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = (from i in context.TransactionReason
                                .Include(x => x.CreatedUser)
                                .Include(x => x.Categories)
                                 orderby i.Name ascending
                                where !i.IsDeleted
                                select i).ToList();

                    query.ForEach(x=>x.Categories.ForEach(z=>z.TypeTransactionReasons=null));

                    return query;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Notification> GetAllNotifications()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.Notification
                                where !i.IsDeleted
                                select i;

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TypeTransaction> GetAllTypeTransactions()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.TypeTransaction
                                where !i.IsDeleted
                                select i;

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TypeFrequency> GetAllTypeFrequencies()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.TypeFrequency
                                where !i.IsDeleted
                                select i;

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TypeInterval> GetAllTypeIntervals()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.TypeInterval
                                where !i.IsDeleted
                                select i;

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BudgetThreshold> GetAllBudgetThresholds()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.BudgetThreshold
                                where !i.IsDeleted
                                select i;

                    return query.ToList();
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
                    var query = (from i in context.User
                                     //.Include(i => i.CreatedUser)
                                     //.Include(i => i.ModifiedUser)
                                where i.UserName == user.UserName && !i.IsDeleted
                                select i).ToList();

                    if (query.Count() < 1)
                        throw new Exception("Username cannot be found");
                    else if (query.Count() == 1 && query[0].Password != user.Password)
                        throw new Exception("Username and password don't match");
                    else
                        result = query[0];
                }
            }
            catch (Exception)
            {                
                throw;
            }
            return result;
        }

        public List<Notification> GetUpcomingNotifications(DateTime clientTime)
        {
            try
            {
                using (EntityContext context= new EntityContext())
                {
                    List<Notification> result = new List<Notification>();
                    var query = context.Notification.Where(i => !i.IsDeleted);

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
        #endregion

        #region Save / Add
        public StaticTypeList SyncStaticData(StaticTypeList staticData, User user)
        {
            var result = new StaticTypeList();

            result.Categories = SyncCategories(staticData.Categories, user);
            result.BudgetThresholds = SyncBudgetThresholds(staticData.BudgetThresholds);
            result.Notifications = SyncNotifications(staticData.Notifications);
            result.TypeFrequencies = SyncTypeFrequencies(staticData.TypeFrequencies);
            result.TypeIntervals = SyncTypeIntervals(staticData.TypeIntervals);
            result.TypeTransactionReasons = SyncTypeTransactionReasons(staticData.TypeTransactionReasons);
            result.TypeTransactions = SyncTypeTransactions(staticData.TypeTransactions);

            return result;
        }

        public List<Category> SyncCategories(List<Category> categories, User user)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var categoryList = new List<Category>();
                    foreach (var item in GetAllCategories())
                    {
                        if (categories.Where(i => i.CategoryId == item.CategoryId).Count() == 0)
                        {
                            item.IsDeleted = true;
                            item.ModifiedDate = DateTime.Now;
                            categoryList.Add(item);
                        }
                    }
                    foreach (var item in categories.Where(i => i.HasChanges))
                    {
                        item.ModifiedDate = DateTime.Now;
                        categoryList.Add(item);
                    }

                    return SaveCategories(categoryList, user);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TypeTransactionReason> SyncTypeTransactionReasons(List<TypeTransactionReason> typeTransactionReasons)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var typeTransactionReasonList = new List<TypeTransactionReason>();
                    foreach (var item in GetAllTypeTransactionReasons())
                    {
                        if (typeTransactionReasons.Where(i => i.TypeTransactionReasonId == item.TypeTransactionReasonId).Count() == 0)
                        {
                            item.IsDeleted = true;
                            item.ModifiedDate = DateTime.Now;
                            typeTransactionReasonList.Add(item);
                        }
                    }
                    foreach (var item in typeTransactionReasons.Where(i => i.HasChanges))
                    {
                        item.ModifiedDate = DateTime.Now;
                        typeTransactionReasonList.Add(item);
                    }

                    return SaveTypeTransactionReasons(typeTransactionReasonList);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TypeTransaction> SyncTypeTransactions(List<TypeTransaction> typeTransactions)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var typeTransactionList = new List<TypeTransaction>();
                    foreach (var item in GetAllTypeTransactions())
                    {
                        if (typeTransactions.Where(i => i.TypeTransactionId == item.TypeTransactionId).Count() == 0)
                        {
                            item.IsDeleted = true;
                            item.ModifiedDate = DateTime.Now;
                            typeTransactionList.Add(item);
                        }
                    }
                    foreach (var item in typeTransactions.Where(i => i.HasChanges))
                    {
                        item.ModifiedDate = DateTime.Now;
                        typeTransactionList.Add(item);
                    }

                    return SaveTypeTransactions(typeTransactionList);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Notification> SyncNotifications(List<Notification> notifications)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var notificationList = new List<Notification>();
                    foreach (var item in GetAllNotifications())
                    {
                        if (notifications.Where(i => i.NotificationId == item.NotificationId).Count() == 0)
                        {
                            item.IsDeleted = true;
                            item.ModifiedDate = DateTime.Now;
                            notificationList.Add(item);
                        }
                    }
                    foreach (var item in notifications.Where(i => i.HasChanges))
                    {
                        item.ModifiedDate = DateTime.Now;
                        notificationList.Add(item);
                    }

                    return SaveNotifications(notificationList);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TypeFrequency> SyncTypeFrequencies(List<TypeFrequency> typeFrequencies)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var typeFrequencyList = new List<TypeFrequency>();
                    foreach (var item in GetAllTypeFrequencies())
                    {
                        if (typeFrequencies.Where(i => i.TypeFrequencyId == item.TypeFrequencyId).Count() == 0)
                        {
                            item.IsDeleted = true;
                            item.ModifiedDate = DateTime.Now;
                            typeFrequencyList.Add(item);
                        }
                    }
                    foreach (var item in typeFrequencies.Where(i => i.HasChanges))
                    {
                        item.ModifiedDate = DateTime.Now;
                        typeFrequencyList.Add(item);
                    }

                    return SaveTypeFrequencies(typeFrequencyList);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TypeInterval> SyncTypeIntervals(List<TypeInterval> typeIntervals)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var typeIntervalList = new List<TypeInterval>();
                    foreach (var item in GetAllTypeIntervals())
                    {
                        if (typeIntervals.Where(i => i.TypeIntervalId == item.TypeIntervalId).Count() == 0)
                        {
                            item.IsDeleted = true;
                            item.ModifiedDate = DateTime.Now;
                            typeIntervalList.Add(item);
                        }
                    }
                    foreach (var item in typeIntervals.Where(i => i.HasChanges))
                    {
                        item.ModifiedDate = DateTime.Now;
                        typeIntervalList.Add(item);
                    }

                    return SaveTypeIntervals(typeIntervalList);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BudgetThreshold> SyncBudgetThresholds(List<BudgetThreshold> budgetThresholds)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var budgetThresholdList = new List<BudgetThreshold>();
                    foreach (var item in GetAllBudgetThresholds())
                    {
                        if (budgetThresholds.Where(i => i.BudgetThresholdId == item.BudgetThresholdId).Count() == 0)
                        {
                            item.IsDeleted = true;
                            item.ModifiedDate = DateTime.Now;
                            budgetThresholdList.Add(item);
                        }
                    }
                    foreach (var item in budgetThresholds.Where(i => i.HasChanges))
                    {
                        item.ModifiedDate = DateTime.Now;
                        budgetThresholdList.Add(item);
                    }

                    return SaveBudgetThresholds(budgetThresholdList);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<Category> SaveCategories(List<Category> categories, User user)
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
                                
                                //original.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                                original.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == user.UserId);

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
                return GetAllCategories();
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

        public List<TypeTransactionReason> SaveTypeTransactionReasons(List<TypeTransactionReason> typeTransactionReason)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in typeTransactionReason)
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
                                    item.Categories.ForEach(x =>
                                    {
                                        var query = context.Category.FirstOrDefault(k => k.CategoryId == x.CategoryId);
                                        if (x.IsDeleted)
                                            original.Categories.Remove(query);
                                        else
                                            original.Categories.Add(query);
                                    });
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
                return GetAllTypeTransactionReasons();
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

        public List<Notification> SaveNotifications(List<Notification> notifications)
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
                            context.Notification.Add(item);
                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }
                return GetAllNotifications();
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
                return GetAllTypeTransactions();
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
                return GetAllTypeFrequencies();
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

        public List<TypeInterval> SaveTypeIntervals(List<TypeInterval> typeIntervals)
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
                            item.HasChanges = false;
                            var original = context.TypeInterval.Where(t => t.TypeIntervalId == item.TypeIntervalId && !t.IsDeleted).FirstOrDefault();

                            if (original.ModifiedDate < item.ModifiedDate)
                            {
                                context.Entry(original).CurrentValues.SetValues(item);
                                updateFound = true;
                            }
                        }
                        else //Insert
                        {
                            context.TypeInterval.Add(item);
                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }
                return GetAllTypeIntervals();
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
                return GetAllBudgetThresholds();
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
                SendEmail(result, NoticeState.ResetPassword);

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

        public User RegisterUser(User user)
        {
            User result = UpdateRegisterUser(user);
            SendEmail(result, NoticeState.NewAccount);

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

                SendEmail(result, NoticeState.SendPassword);

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

                        var query = context.User.Where(i => i.UserName == "admin").FirstOrDefault();
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

        private void SendEmail(User user, NoticeState state)
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
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
