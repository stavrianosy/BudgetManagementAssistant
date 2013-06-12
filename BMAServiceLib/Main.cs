﻿using BMA.BusinessLogic;
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
        #region Load

        public bool GetStatus()
        {
            return true;
        }

        public TransactionList GetAllTransactions()
        {
            try
            {
                TransactionList transList = new TransactionList();
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.Transaction
                                .Include(i => i.Category).Where(k => !k.IsDeleted)
                                .Include(i => i.CreatedUser)
                                //.Include(i => i.ModifiedUser)
                                .Include(i => i.TransactionReasonType).Where(k => !k.IsDeleted)
                                .Include(i => i.TransactionType).Where(k => !k.IsDeleted)
                                where !i.IsDeleted
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
                    var query = from i in context.Transaction.Take(latestRecs == 0 ? int.MaxValue : latestRecs)
                                .Include(i => i.TransactionType).Where(k => !k.IsDeleted)
                                .Include(i => i.TransactionReasonType).Where(k => !k.IsDeleted)
                                .Include(i => i.Category).Where(k => !k.IsDeleted)
                                .Include(i => i.CreatedUser)
                                //.Include(i => i.ModifiedUser)
                                where !i.IsDeleted
                                orderby i.CreatedDate descending
                                select i;

                    //investigate if there is a better way to convert the generic list to ObservableCollection
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

        public List<TypeTransaction> GetAllTypeTransactions()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.TypeTransaction
                                //.Include(i => i.CreatedUser)
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

        public BudgetList GetAllBudgets()
        {
            try
            {
                BudgetList budgetList = new BudgetList();
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.Budget
                                .Include(i => i.CreatedUser)
                                //.Include(i=>i.Transactions.Where(t=>t.CreatedDate >= i.FromDate && t.CreatedDate <=i.ToDate))
                                select i;

                    foreach (var item in query.ToList())
                    {
                        var inQuery = from t in context.Transaction
                                      .Include(t => t.TransactionType).Where(k => !k.IsDeleted)
                                      where t.CreatedDate >= item.FromDate && t.CreatedDate <= item.ToDate
                                      && !t.IsDeleted
                                      select t;

                        TransactionList tl = new TransactionList();

                        foreach (var inItem in inQuery.ToList())
                            tl.Add(inItem);

                        item.Transactions = tl;
                    }

                    //investigate if there is a better way to convert the generic list to ObservableCollection
                    foreach (var item in query)
                        budgetList.Add(item);

                    return budgetList;
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
                    startupInfo.BudgetCount = context.Budget.Where(k => !k.IsDeleted).Count();
                    startupInfo.TransactionCount = context.Transaction.Where(k => !k.IsDeleted).Count();
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
        public TransactionList SyncTransactions(TransactionList transactions)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var transList = new TransactionList();
                    foreach (var item in GetAllTransactions())
                    {
                        if (transactions.Where(i => i.TransactionId == item.TransactionId).Count() == 0)
                        {
                            item.IsDeleted = true;
                            item.ModifiedDate = DateTime.Now;
                            transList.Add(item);
                        }
                    }
                    foreach (var item in transactions.Where(i => i.HasChanges))
                    {
                        item.ModifiedDate = DateTime.Now;
                        transList.Add(item);
                    }

                    return SaveTransactions(transList);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

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
                            var original = context.Transaction.Where(t => t.TransactionId == item.TransactionId && 
                                                                    t.ModifiedDate < item.ModifiedDate &&
                                                                    !t.IsDeleted).FirstOrDefault();

                            if (original !=null)
                            {
                                item.HasChanges = false;

                                original.Category = context.Category.Where(k => !k.IsDeleted).Single(p => p.CategoryId == item.Category.CategoryId);
                                original.Category.CreatedUser = null;
                                original.Category.ModifiedUser = null;

                                original.TransactionType = context.TypeTransaction.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionId == item.TransactionType.TypeTransactionId);
                                original.TransactionReasonType = context.TransactionReason.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionReasonId == item.TransactionReasonType.TypeTransactionReasonId);

                                original.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                                original.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                                context.Entry(original).CurrentValues.SetValues(item);
                                updateFound = true;
                            }
                        }
                        else //Insert
                        {
                            item.Category = context.Category.Where(k => !k.IsDeleted).Single(p => p.CategoryId == item.Category.CategoryId);
                            item.TransactionType = context.TypeTransaction.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionId == item.TransactionType.TypeTransactionId);
                            item.TransactionReasonType = context.TransactionReason.Where(k => !k.IsDeleted).Single(p => p.TypeTransactionReasonId == item.TransactionReasonType.TypeTransactionReasonId);

                            item.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                            item.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                            context.Transaction.Add(item);
                            updateFound = true;
                        }
                    }

                    if (updateFound)
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

        public BudgetList SyncBudgets(BudgetList budgets)
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var budgetList = new BudgetList();
                    foreach (var item in GetAllBudgets())
                    {
                        if (budgets.Where(i => i.BudgetId == item.BudgetId).Count() == 0)
                        {
                            item.IsDeleted = true;
                            item.ModifiedDate = DateTime.Now;
                            budgetList.Add(item);
                        }
                    }
                    foreach (var item in budgetList.Where(i => i.HasChanges))
                    {
                        item.ModifiedDate = DateTime.Now;
                        budgetList.Add(item);
                    }

                    return SaveBudgets(budgetList);
                }
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

                                context.Entry(original).CurrentValues.SetValues(item);

                                updateFound = true;
                            }
                        }
                        else //Insert
                        {
                            item.CreatedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.CreatedUser.UserId);
                            item.ModifiedUser = context.User.Where(k => !k.IsDeleted).Single(p => p.UserId == item.ModifiedUser.UserId);

                            context.Budget.Add(item);

                            updateFound = true;
                        }
                    }

                    if (updateFound)
                        context.SaveChanges();
                }
                return GetAllBudgets();
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