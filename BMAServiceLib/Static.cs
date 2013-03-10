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
                    var typeTrans = (from i in context.TypeTransaction
                                     //.Include(i => i.CreatedUser)
                                     where !i.IsDeleted
                                     select i).ToList();

                    var cat = (from i in context.Category
                               where !i.IsDeleted
                               select i).ToList();

                    var typeSD = (from i in context.TypeSavingsDencity
                                  ////.Include(i => i.CreatedUser)
                                  where !i.IsDeleted
                                  select i).ToList();

                    var typeTR = (from i in context.TransactionReason
                                  ////.Include(i => i.CreatedUser)
                                  where !i.IsDeleted
                                  select i).ToList();

                    var notice = (from i in context.Notification
                                  ////.Include(i => i.CreatedUser)
                                  where !i.IsDeleted
                                  select i).ToList();

                    var typeF = (from i in context.TypeFrequency
                                  ////.Include(i => i.CreatedUser)
                                 where !i.IsDeleted
                                 select i).ToList();

                    var inter = (from i in context.TypeInterval
                                  ////.Include(i => i.CreatedUser)
                                 where !i.IsDeleted
                                 select i).ToList();

                    var budgetTH = (from i in context.BudgetThreshold
                                  ////.Include(i => i.CreatedUser)
                                    where !i.IsDeleted
                                    select i).ToList();

                    typeData.Categories = cat;
                    typeData.TypeTransactions = typeTrans;
                    typeData.TypeSavingsDencities = typeSD;
                    typeData.TypeTransactionReasons = typeTR;
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
                    var query = from i in context.Category
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

        public List<TypeTransactionReason> GetAllTypeTransactionReasons()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.TransactionReason
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
        public List<Category> SaveCategories(Category[] categories)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in categories)
                    {
                        if (item.CategoryId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.Category.Where(t => t.CategoryId == item.CategoryId && !t.IsDeleted).FirstOrDefault();

                            context.Entry(original).CurrentValues.SetValues(item);
                        }
                        else //Insert
                        {
                            context.Category.Add(item);
                        }
                    }
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

        public List<TypeTransactionReason> SaveTypeTransactionReasons(TypeTransactionReason[] typeTransactionReason)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in typeTransactionReason)
                    {
                        if (item.TypeTransactionReasonId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.TransactionReason.Where(t => t.TypeTransactionReasonId == item.TypeTransactionReasonId && !t.IsDeleted).FirstOrDefault();

                            context.Entry(original).CurrentValues.SetValues(item);
                        }
                        else //Insert
                        {
                            context.TransactionReason.Add(item);
                        }
                    }
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

        public List<Notification> SaveNotifications(Notification[] notifications)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in notifications)
                    {
                        if (item.NotificationId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.Notification.Where(t => t.NotificationId == item.NotificationId && !t.IsDeleted).FirstOrDefault();

                            context.Entry(original).CurrentValues.SetValues(item);
                        }
                        else //Insert
                        {
                            context.Notification.Add(item);
                        }
                    }
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

        public List<TypeTransaction> SaveTypeTransactions(TypeTransaction[] typeTransactions)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in typeTransactions)
                    {
                        if (item.TypeTransactionId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.TypeTransaction.Where(t => t.TypeTransactionId == item.TypeTransactionId && !t.IsDeleted).FirstOrDefault();

                            context.Entry(original).CurrentValues.SetValues(item);
                        }
                        else //Insert
                        {
                            context.TypeTransaction.Add(item);
                        }
                    }
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

        public List<TypeFrequency> SaveTypeFrequencies(TypeFrequency[] typeFrequencies)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in typeFrequencies)
                    {
                        if (item.TypeFrequencyId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.TypeFrequency.Where(t => t.TypeFrequencyId == item.TypeFrequencyId && !t.IsDeleted).FirstOrDefault();

                            context.Entry(original).CurrentValues.SetValues(item);
                        }
                        else //Insert
                        {
                            context.TypeFrequency.Add(item);
                        }
                    }
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

        public List<TypeInterval> SaveTypeIntervals(TypeInterval[] typeIntervals)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in typeIntervals)
                    {
                        if (item.TypeIntervalId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.TypeInterval.Where(t => t.TypeIntervalId == item.TypeIntervalId && !t.IsDeleted).FirstOrDefault();

                            context.Entry(original).CurrentValues.SetValues(item);
                        }
                        else //Insert
                        {
                            context.TypeInterval.Add(item);
                        }
                    }
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

        public List<BudgetThreshold> SaveBudgetThresholds(BudgetThreshold[] budgetThresholds)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in budgetThresholds)
                    {
                        if (item.BudgetThresholdId > 0) //Update
                        {
                            item.HasChanges = false;
                            var original = context.BudgetThreshold.Where(t => t.BudgetThresholdId == item.BudgetThresholdId && !t.IsDeleted).FirstOrDefault();

                            context.Entry(original).CurrentValues.SetValues(item);
                        }
                        else //Insert
                        {
                            context.BudgetThreshold.Add(item);
                        }
                    }
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
