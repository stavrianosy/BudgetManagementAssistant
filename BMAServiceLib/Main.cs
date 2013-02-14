using BMA.BusinessLogic;
using BMA.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Data;

namespace BMAServiceLib
{
    [Serializable]
    public class Main:IMain
    {
        #region Load
        public List<Transaction> GetAllTransactions()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.Transaction
                                .Include(i => i.Category)
                                ////.Include(i => i.CreatedUser)
                                .Include(i => i.TransactionReasonType)
                                .Include(i => i.TransactionType)
                                select i;

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TransactionList GetLatestTransactions()
        {
            return GetLatestTransactionsLimit(30);
        }

        public TransactionList GetLatestTransactionsLimit(int latestRecs)
        {
            try
            {
                TransactionList transList = new TransactionList();
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.Transaction.Take(30)                               
                                .Include(i => i.TransactionType)
                                .Include(i => i.TransactionReasonType)
                                .Include(i => i.Category)
                                ////.Include(i => i.CreatedUser)
                                //.Include(i => i.ModifiedUser)
                                orderby i.CreatedDate descending
                                select i;
                
                    //investigate if there is a better way to convert the generic list to ObservableCollection
                    foreach (var item in query.ToList())
                        transList.Add(item);

                    return transList;
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
                                //.Include(i => i.CreatedUser)
                                select i;

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Transaction> GetTransactionsForBudget(int budgetId)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    DateTime d = DateTime.Now.AddDays(100);
                    var query = from i in context.Transaction
                                .Include(i => i.TransactionType)
                                .Include(i => i.TransactionReasonType)
                                .Include(i => i.Category)
                                //.Include(i => i.CreatedUser)
                                //.Include(i => i.ModifiedUser)
                                where i.CreatedDate >= context.Budget.Where(b => b.BudgetId == budgetId).Select(b => b.FromDate).FirstOrDefault()
                                && i.CreatedDate <= context.Budget.Where(b => b.BudgetId == budgetId).Select(b => b.ToDate).FirstOrDefault()
                                orderby i.CreatedDate descending
                                select i;

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Budget> GetAllBudgets()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.Budget
                                select i;

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public StartupInfo LoadItemCounts()
        {
            try
            {
                StartupInfo startupInfo = new StartupInfo();
                using (EntityContext context = new EntityContext())
                {
                    startupInfo.BudgetCount = context.Budget.Count();
                    startupInfo.TransactionCount = context.Transaction.Count();
                    startupInfo.TargetCount = context.Target.Count();
                }
                return startupInfo;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        //public Dictionary<Category, List<Transaction>> GetAllTransCategories()
        //{
        //    try
        //    {
        //        using (EntityContext context = new EntityContext())
        //        {
        //            var temp = (from c in context.Category
        //                        from a in context.Transaction where c.CategoryId == a.Category.CategoryId
        //                        select new {c,a}).ToList();

        //            Dictionary<Category, List<Transaction>> query2 = new Dictionary<Category,List<Transaction>>(); 
        //            foreach(var item in temp)
        //            {
        //                query2.Add(item.c, item.c.Transactions); 
        //            }

        //            var query = (from c in context.Category
        //                         .Include(c => c.Transactions)
        //                         .Include(c => c.CreatedUser)
        //                         .Include(c => c.ModifiedUser)
        //                         .Include("Transactions.TransactionType")
        //                         .Include("Transactions.TransactionReasonType")
        //                         select c).ToList();


        //            List<Category> query1 = new List<Category>();
        //            return query2;

        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public StaticTypeList GetAllStaticData()
        {
            try
            {
                StaticTypeList typeData = new StaticTypeList();
                using (EntityContext context = new EntityContext())
                {
                    var typeTrans = (from i in context.TypeTransaction
                                     //.Include(i => i.CreatedUser)
                                     select i).ToList();

                    var cat = (from i in context.Category
                               select i).ToList();

                    var typeSD = (from i in context.TypeSavingsDencity
                                  ////.Include(i => i.CreatedUser)
                                  select i).ToList();

                    var typeTR = (from i in context.TransactionReason
                                  select i).ToList();


                    typeData.Categories = cat;
                    typeData.TypeTransactions = typeTrans;
                    typeData.TypeSavingsDencities = typeSD;
                    typeData.TypeTransactionReasons = typeTR;

                    return typeData;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Save
        public TransactionList SaveTransactions(Transaction[] transactions)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    foreach (var item in transactions)
                    {
                        if (item.TransactionId > 0) //Update
                        {
                            var original = context.Transaction.Where(t => t.TransactionId == item.TransactionId).FirstOrDefault();

                            original.Category = context.Category.Single(p => p.CategoryId == item.Category.CategoryId);
                            original.TransactionType = context.TypeTransaction.Single(p => p.TypeTransactionId == item.TransactionType.TypeTransactionId);
                            original.TransactionReasonType = context.TransactionReason.Single(p => p.TypeTransactionReasonId == item.TransactionReasonType.TypeTransactionReasonId);

                            context.Entry(original).CurrentValues.SetValues(item);
                        }
                        else //Insert
                        {
                            item.Category = context.Category.Single(p => p.CategoryId == item.Category.CategoryId);
                            item.TransactionType = context.TypeTransaction.Single(p => p.TypeTransactionId == item.TransactionType.TypeTransactionId);
                            item.TransactionReasonType = context.TransactionReason.Single(p => p.TypeTransactionReasonId == item.TransactionReasonType.TypeTransactionReasonId);

                            context.Transaction.Add(item);
                        }
                    }
                    context.SaveChanges();
                }
                return GetLatestTransactions();
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

        public TransactionList SaveTransaction(Transaction transaction)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var u = context.User.Where(k=>k.UserId==2).FirstOrDefault();
                    var c = context.Category.FirstOrDefault();
                    var trt = context.TransactionReason.FirstOrDefault();
                    var tt = context.TypeTransaction.FirstOrDefault();

                    var trans = new Transaction();
                    //trans.CreatedUser = u;
                    //trans.ModifiedUser = u;
                    trans.Category = c;
                    trans.TransactionReasonType = trt;
                    trans.TransactionType = tt;
 
////                    context.Transaction.AddOrUpdate(t => transaction.TransactionId, transaction);
                    //var query = (from i in context.Transaction
                    //            .Include(i => i.Category)
                    //            //.Include(i => i.CreatedUser)
                    //            .Include(i => i.TransactionReasonType)
                    //            .Include(i => i.TransactionType)
                    //                       select i).ToList();

//                    Transaction[] arr = new Transaction[3]{null,null,null};
//                        arr.SetValue(query.GetValue(0), 0);
//                        arr.SetValue(query.GetValue(1), 1);
//                        arr.SetValue(trans, 2);

                    context.Transaction.AddOrUpdate(t => t.TransactionId, transaction);
                    context.SaveChanges();
                }
                return GetLatestTransactions();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                throw;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Category> SaveCategories(Category[] categories)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    
                    context.Category.AddOrUpdate(categories);
                    context.SaveChanges();
                }
                return null;
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
    }
}
