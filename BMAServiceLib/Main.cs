using BMA.BusinessLogic;
using BMA.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace BMAServiceLib
{
    [Serializable]
    public class Main:IMain
    {
        public List<Transaction> GetAllTransactions()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.Transaction
                                .Include(i => i.Category)
                                .Include(i => i.CreatedUser)
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

        public List<TypeTransaction> GetAllTypeTransactions()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.TypeTransaction
                                .Include(i => i.CreatedUser)
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

        public Dictionary<Category, List<Transaction>> GetAllTransCategories()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var temp = (from c in context.Category
                                from a in context.Transaction where c.CategoryId == a.Category.CategoryId
                                select new {c,a}).ToList();

                    Dictionary<Category, List<Transaction>> query2 = new Dictionary<Category,List<Transaction>>(); 
                    foreach(var item in temp)
                    {
                        query2.Add(item.c, item.c.Transactions); 
                    }

                    var query = (from c in context.Category
                                 .Include(c => c.Transactions)
                                 .Include(c => c.CreatedUser)
                                 .Include(c => c.ModifiedUser)
                                 .Include("Transactions.TransactionType")
                                 .Include("Transactions.TransactionReasonType")
                                 select c).ToList();


                    List<Category> query1 = new List<Category>();
                    return query2;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public StaticTypeList GetAllStaticData()
        {
            try
            {
                StaticTypeList typeData = new StaticTypeList();
                using (EntityContext context = new EntityContext())
                {
                    var typeTrans = (from i in context.TypeTransaction
                                     .Include(i => i.CreatedUser)
                                     select i).ToList();

                    var cat = (from i in context.Category
                               select i).ToList();

                    var typeSD = (from i in context.TypeSavingsDencity
                                  //.Include(i => i.CreatedUser)
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
    }
}
