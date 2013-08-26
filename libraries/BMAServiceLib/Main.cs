using BMA.BusinessLogic;
using BMA.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Data;

namespace BMAServiceLib
{
    [Serializable]
    public class Main:IMain
    {
        private const int SYSTEM_USER_ID = 2;
        #region Load

        public bool GetDBStatus()
        {
            var result = false;
            using (var context = new EntityContext())
            {
                var user = context.User.Take(1);
                result = user != null;
            }
            return result;
        }

        public bool SyncTransactions(TransactionList transactions)
        {
            try
            {
                var transList = new TransactionList();
                var transIDs = transactions.Select(x => x.TransactionId).ToList();
                using (EntityContext context = new EntityContext())
                {
                    var query = (from i in context.Transaction
                                 where transIDs.Contains(i.TransactionId)
                                 select i).ToList();

                    //investigate if there is a better way to convert the generic list to ObservableCollection
                    foreach (var item in query)
                    {
                        var tempItem = transactions.FirstOrDefault(x => x.TransactionId == item.TransactionId);
                        if (tempItem.ModifiedDate < item.ModifiedDate)
                            transactions.Remove(tempItem);
                    }
                }

                SaveTransactions(transactions);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }

        public bool SyncBudgets(BudgetList budgets)
        {
            try
            {
                var budgetsList = new BudgetList();
                var budgetIDs = budgets.Select(x => x.BudgetId).ToList();
                using (EntityContext context = new EntityContext())
                {
                    var query = (from i in context.Budget
                                 where budgetIDs.Contains(i.BudgetId)
                                 select i).ToList();

                    //investigate if there is a better way to convert the generic list to ObservableCollection
                    foreach (var item in query)
                    {
                        var tempItem = budgets.FirstOrDefault(x => x.BudgetId == item.BudgetId);
                        if (tempItem.ModifiedDate < item.ModifiedDate)
                            budgets.Remove(tempItem);
                    }
                }

                SaveBudgets(budgets);
            }
            catch (Exception)
            {
                throw;
            }

            return true;
        }


        public DateTime GetLatestTransactionDate()
        {
            try
            {
                var date = DateTime.Now;
                
                using (EntityContext context = new EntityContext())
                    date = context.Transaction.OrderByDescending(x => x.ModifiedDate).FirstOrDefault().ModifiedDate;


                return date;
            }
            catch
            {
                throw;
            }
        }

        public double GetLatestTransactionDateDouble(int userId)
        {
            try
            {
                return double.Parse(DateToString(GetLatestTransactionDate()));
            }
            catch
            {
                throw new Exception("aaaaa ttttt");
                throw;
            }
        }

        private string DateToString(DateTime date)
        {
            var result = string.Format("{0:00}{1:00}{2:00}{3:00}{4:00}{5:00}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            return result;
        }

        public TransactionList GetAllTransactions(int userId)
        {
            try
            {
                TransactionList transList = new TransactionList();
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.Transaction
                                .Include(i => i.Category)
                                .Include(i => i.CreatedUser)
                                .Include(i => i.ModifiedUser)
                                .Include(i => i.TransactionReasonType)
                                .Include(i => i.TransactionType)
                                where !i.IsDeleted && i.ModifiedUser.UserId == userId
                                select i;

                    foreach (var item in query.ToList())
                        transList.Add(item);

                    transList.AcceptChanges();

                    return transList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TransactionList GetLatestTransactions(int userId)
        {
            return GetLatestTransactionsLimit(100, userId);
        }

        public TransactionList GetLatestTransactionsOnDate(int userId)
        {
            return GetLatestTransactionsLimit(50, userId);
        }

        public TransactionList GetLatestTransactionsLimit(int latestRecs, int userId)
        {
            try
            {
                TransactionList transList = new TransactionList();
                using (EntityContext context = new EntityContext())
                {
                    context.Configuration.LazyLoadingEnabled = true;

                    var query = (from i in context.Transaction
                                .Include(i => i.TransactionType)
                                .Include(i => i.TransactionReasonType)
                                .Include(i => i.Category)
                                .Include(i => i.Category.TypeTransactionReasons)
                                .Include(i => i.ModifiedUser)
                                .Include(i => i.CreatedUser)
                                 where !i.IsDeleted && i.ModifiedUser.UserId == userId
                                 orderby i.TransactionDate descending
                                 select i).Take(latestRecs == 0 ? int.MaxValue : latestRecs);

                    //investigate if there is a better way to convert the generic list to ObservableCollection
                    foreach (var item in query)
                        transList.Add(item);

                    transList.PrepareForServiceSerialization();

                    return transList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TransactionList GetTransactionsForBudget(int budgetId)
        {
            try
            {
                TransactionList transactionList = new TransactionList();
                using (EntityContext context = new EntityContext())
                {
                    // DateTime d = DateTime.Now.AddDays(100);
                    var query = from i in context.Transaction
                                .Include(i => i.TransactionType).Where(k => !k.IsDeleted)
                                .Include(i => i.TransactionReasonType).Where(k => !k.IsDeleted)
                                .Include(i => i.Category).Where(k => !k.IsDeleted)
                                //.Include(i => i.CreatedUser)
                                //.Include(i => i.ModifiedUser)
                                where i.CreatedDate >= context.Budget.Where(b => b.BudgetId == budgetId).Select(b => b.FromDate).FirstOrDefault()
                                && i.CreatedDate <= context.Budget.Where(b => b.BudgetId == budgetId).Select(b => b.ToDate).FirstOrDefault()
                                && !i.IsDeleted
                                orderby i.CreatedDate descending
                                select i;

                    foreach (var item in query)
                        transactionList.Add(item);

                    transactionList.AcceptChanges();

                    return transactionList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TransactionImageList GetImagesForTransaction(int transactionId)
        {
            TransactionImageList result = new TransactionImageList();
            using(EntityContext context = new EntityContext())
            {
                var query = context.TransactionImage
                                    .Include(x => x.Transaction)
                                    .Include(x => x.CreatedUser)
                                    .Include(x => x.ModifiedUser)
                                    .Where(x =>x.Transaction.TransactionId == transactionId && !x.IsDeleted);

                query.ToList().ForEach(x=> 
                {
                    x.Transaction.TransactionImages = null;
                    result.Add(x);
                });
                result.AcceptChanges();


            }
            return result;
        }

        public BudgetList GetAllBudgets(int userId)
        {
            try
            {
                BudgetList budgetList = new BudgetList();
                using (EntityContext context = new EntityContext())
                {
                    var query = (from i in context.Budget
                                .Include(i => i.CreatedUser)
                                .Include(i => i.ModifiedUser)
                                where !i.IsDeleted && i.ModifiedUser.UserId == userId
                                select i).ToList();

                    //investigate if there is a better way to convert the generic list to ObservableCollection
                    foreach (var item in query)
                        budgetList.Add(item);

                    GetTransactionsForBudgets(budgetList);

                    budgetList.PrepareForServiceSerialization();

                    return budgetList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetTransactionsForBudgets(BudgetList budgetList)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in budgetList)
                    {
                        var inQuery = from t in context.Transaction
                                      .Include(t => t.TransactionType).Where(k => !k.IsDeleted)
                                      where t.TransactionDate >= item.FromDate && t.TransactionDate <= item.ToDate
                                      && !t.IsDeleted && t.ModifiedUser.UserId == item.ModifiedUser.UserId
                                      select t;

                        TransactionList tl = new TransactionList();

                        foreach (var inItem in inQuery.ToList())
                            tl.Add(inItem);

                        item.Transactions = tl;

                        ////Apply the rule to create a new budget if the expired one is set to Repeat
                        //if (item.Repeat && item.ToDate < DateTime.Now)
                        //{
                        //    var dataSpanRange = item.ToDate - item.FromDate;
                        //    var dataSpanToday = DateTime.Now - item.ToDate;
                        //    var dateDif = dataSpanRange.Days;
                        //    var diff = dataSpanToday.Days % dataSpanRange.Days;
                        //    var newFromDate = DateTime.Now.AddDays(-diff + 1);
                        //    var newToDate = newFromDate.AddDays(dataSpanRange.Days);

                        //    var test = newToDate - newFromDate;

                        //    var newBudget = item.Clone();
                        //    newBudget.FromDate = newFromDate;
                        //    newBudget.ToDate = newToDate;

                        //    item.Repeat = false;
                        //    context.Budget.Add(newBudget);

                        //    //context.SaveChanges();
                        //}
                    }
                    //return budgetList;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StartupInfo LoadItemCounts(int userId)
        {
            try
            {
                StartupInfo startupInfo = new StartupInfo();
                using (EntityContext context = new EntityContext())
                {
                    startupInfo.BudgetCount = context.Budget.Where(k => !k.IsDeleted && k.ModifiedUser.UserId == userId).Count();
                    startupInfo.TransactionCount = context.Transaction.Where(k => !k.IsDeleted && k.ModifiedUser.UserId == userId).Count();
                    //startupInfo.TargetCount = context.Target.Where(k => !k.IsDeleted).Count();
                }
                return startupInfo;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Save
        public TransactionList SaveTransactions(TransactionList transactions)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in transactions)
                    {
                        if (item.TransactionId > 0) //Update
                        {
                            //update database only if the transaction in DB is older
                            var original = context.Transaction
                                                            .Include(x => x.Category)
                                                            .Include(x => x.TransactionType)
                                                            .Include(x => x.TransactionImages)
                                                            .Include(x => x.TransactionReasonType)
                                                            .Where(t => t.TransactionId == item.TransactionId && 
                                                            t.ModifiedDate < item.ModifiedDate &&
                                                            !t.IsDeleted).FirstOrDefault();

                            if (original !=null)
                            {
                                item.HasChanges = false;

                                original.Category = context.Category.Where(k => !k.IsDeleted).Single(p => p.CategoryId == item.Category.CategoryId);
                                //original.Category.CreatedUser = null;
                                //original.Category.ModifiedUser = null;

                                //context.Entry(original).Collection(x => x.TransactionImages).Load();
                                
                                original.TransactionType = context.TypeTransaction.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionId == item.TransactionType.TypeTransactionId);
                                original.TransactionReasonType = context.TransactionReason.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionReasonId == item.TransactionReasonType.TypeTransactionReasonId);

                                original.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                                original.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                                if (item.TransactionImages != null)
                                {
                                    foreach (var x in item.TransactionImages)
                                    {
                                        if (x.TransactionImageId > 0)
                                            original.TransactionImages.Where(k => k.TransactionImageId == x.TransactionImageId).Select(k =>
                                            {
                                                k.Name = x.Name; k.Path = x.Path; k.ModifiedDate = x.ModifiedDate; k.IsDeleted = x.IsDeleted;
                                                k.ModifiedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == x.ModifiedUser.UserId);
                                                return k;
                                            }).ToList();
                                        else
                                        {
                                            x.CreatedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == x.CreatedUser.UserId);
                                            x.ModifiedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == x.ModifiedUser.UserId);

                                            original.TransactionImages.Add(x);
                                        }
                                    }
                                }

                                context.Entry(original).CurrentValues.SetValues(item);
                                updateFound = true;
                                           
                            }
                        }
                        else //Insert
                        {
                            item.Category = context.Category.Where(k => !k.IsDeleted).Single(p => p.CategoryId == item.Category.CategoryId);
                            item.TransactionType = context.TypeTransaction.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionId == item.TransactionType.TypeTransactionId);

                            if (item.TransactionReasonType != null)
                                item.TransactionReasonType = context.TransactionReason.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionReasonId == item.TransactionReasonType.TypeTransactionReasonId);

                            item.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                            item.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                            if (item.TransactionImages != null)
                            {
                                foreach (var x in item.TransactionImages)
                                {
                                    x.CreatedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == x.CreatedUser.UserId);
                                    x.ModifiedUser = context.User.Where(p => !p.IsDeleted).Single(p => p.UserId == x.ModifiedUser.UserId);

                                    //item.TransactionImages.Add(x);
                                }
                            }

                            context.Transaction.Add(item);
                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }

                transactions.PrepareForServiceSerialization();

                //return GetLatestTransactions();
                return transactions;
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

        public BudgetList SaveBudgets(BudgetList budgets)
        {
            try
            {
                bool updateFound = false;

                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in budgets)
                    {
                        if (item.BudgetId > 0) //Update
                        {
                            var original = context.Budget.Where(t => 
                                                                t.BudgetId == item.BudgetId &&
                                                                t.ModifiedDate < item.ModifiedDate &&
                                                                !t.IsDeleted).FirstOrDefault();

                            //update database only if the transaction in DB is older
                            if (original != null)
                            {
                                item.HasChanges = false;

                                original.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                                original.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                                item.FromDate = new DateTime(item.FromDate.Year, item.FromDate.Month, item.FromDate.Day, 0, 0, 0);
                                item.ToDate = new DateTime(item.ToDate.Year, item.ToDate.Month, item.ToDate.Day, 0, 0, 0);

                                context.Entry(original).CurrentValues.SetValues(item);

                                updateFound = true;
                            }
                        }
                        else //Insert
                        {
                            item.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                            item.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                            item.FromDate = new DateTime(item.FromDate.Year, item.FromDate.Month, item.FromDate.Day, 0, 0, 0);
                            item.ToDate = new DateTime(item.ToDate.Year, item.ToDate.Month, item.ToDate.Day, 0, 0, 0);

                            context.Budget.Add(item);

                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }

                GetTransactionsForBudgets(budgets);
                budgets.PrepareForServiceSerialization();

                return budgets;
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

        public bool SaveTransactionImages(TransactionImageList transactionImages)
        {
            try
            {
                bool updateFound = false;
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in transactionImages)
                    {
                        if (item.TransactionImageId > 0) //Update
                        {
                            var original = context.TransactionImage.Where(t =>
                                                                t.TransactionImageId == item.TransactionImageId &&
                                                                t.ModifiedDate < item.ModifiedDate &&
                                                                !t.IsDeleted).FirstOrDefault();

                            if (original != null)
                            {
                                item.HasChanges = false;

                                original.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                                original.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                                context.Entry(original).CurrentValues.SetValues(item);
                                updateFound = true;
                            }
                        }
                        else //Insert
                        {
                            item.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                            item.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                            item.Transaction.Category = null;
                            item.Transaction.TransactionImages = null;
                            item.Transaction.TransactionReasonType = null;
                            item.Transaction.TransactionType = null;
                            item.Transaction.ModifiedUser = null;
                            item.Transaction.CreatedUser = null;

                            context.Entry(item.Transaction).State = EntityState.Unchanged;

                            context.TransactionImage.Add(item);

                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }
                return true;
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

        #region Reports
        private List<Transaction> ReportTransaction(DateTime dateFrom, DateTime dateTo, int transTypeId, double amountFrom, double amountTo, int userId)
        {
            try
            {
                var result = new List<Transaction>();
                using (EntityContext context = new EntityContext())
                {
                    var query = (from i in context.Transaction
                                 .Include(i => i.Category)
                                 .Include(i => i.TransactionReasonType)
                                 where i.TransactionDate >= dateFrom && i.TransactionDate <= dateTo &&
                                 i.TransactionType.TypeTransactionId == transTypeId &&
                                 (amountFrom <= 0 || i.Amount >= amountFrom) && (amountTo <= 0 || i.Amount <= amountTo) &&
                                 !i.IsDeleted && i.ModifiedUser.UserId == userId
                                 orderby i.Amount descending
                                 select i).ToList();

                    query.ForEach(x => result.Add(x));

                    return result;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<Transaction> ReportTransactionAmount(DateTime dateFrom, DateTime dateTo, int transTypeId, double amountFrom, double amountTo, int userId)
        {
            return ReportTransaction(dateFrom, dateTo, transTypeId, amountFrom, amountTo, userId).Take(200).ToList();
        }

        public Dictionary<Category, double> ReportTransactionCategory(DateTime dateFrom, DateTime dateTo, int transTypeId, int userId)
        {
            var result = new Dictionary<Category, double>();

            var query = (from i in ReportTransaction(dateFrom, dateTo, transTypeId, -1, -1, userId).GroupBy(x => x.Category)
                         select new
                         {
                             item = i.FirstOrDefault().Category,
                             sum = i.Sum(x => x.Amount)
                         }).OrderByDescending(x => x.sum).ToList();

            query.ForEach(x =>
            {
                result.Add(x.item, x.sum);
            });

            return result;
        }

        public Dictionary<TypeTransactionReason, double> ReportTransactionReason(DateTime dateFrom, DateTime dateTo, int transTypeId, int userId)
        {
            var result = new Dictionary<TypeTransactionReason, double>();

            var query = (from i in ReportTransaction(dateFrom, dateTo, transTypeId, -1, -1, userId).GroupBy(x => x.TransactionReasonType)
                         select new
                         {
                             item = i.FirstOrDefault().TransactionReasonType,
                             sum = i.Sum(x => x.Amount)
                         }).OrderByDescending(x => x.sum).ToList();

            query.ForEach(x =>
            {
                result.Add(x.item, x.sum);
            });

            return result;
        }

        public List<Budget> ReportTransactionBudget(DateTime dateFrom, DateTime dateTo, int transTypeId, int userId)
        {
            var result = new List<Budget>();

            //get all budgets

            return result;
        }

        public Dictionary<string, double> ReportTransactionNameOfPlace(DateTime dateFrom, DateTime dateTo, int transTypeId, int userId)
        {
            var result = new Dictionary<string, double>();

            var query = (from i in ReportTransaction(dateFrom, dateTo, transTypeId, -1, -1, userId).GroupBy(x => x.NameOfPlace.Trim())
                         select new
                         {
                             item = i.FirstOrDefault().NameOfPlace,
                             sum = i.Sum(x => x.Amount)
                         }).OrderByDescending(x => x.sum).ToList();

            query.ForEach(x =>
            {
                result.Add(x.item, x.sum);
            });

            return result;
        }

        public Dictionary<int, double> ReportTransactionByPeriod(DateTime dateFrom, DateTime dateTo, int transTypeId, Const.ReportPeriod period, int userId)
        {
            var result = new Dictionary<int, double>();

            string periodStr = "yyyyMM";
            //var periodEnum = (Const.ReportPeriod)Enum.Parse(typeof(Const.ReportPeriod), period);

            switch (period)
            {
                case Const.ReportPeriod.Monthly:
                    periodStr = "yyyyMM";
                    break;
                case Const.ReportPeriod.Yearly:
                    periodStr = "yyyy";
                    break;
                default:
                    periodStr = "yyyyMMdd";
                    break;
            }
            

            var query = (from i in ReportTransaction(dateFrom, dateTo, transTypeId, -1, -1, userId)
                             .GroupBy(x => x.TransactionDate.ToString(periodStr))
                         select new
                         {
                             item = i.Key,
                             sum = i.Sum(x => x.Amount)
                         }).OrderByDescending(x => x.sum).ToList();

            query.ForEach(x =>
            {
                result.Add(int.Parse(x.item), x.sum);
            });

            return result;
        }
        #endregion
    }
}
