using BMA.BusinessLogic;
using BMA.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMAServiceLib
{
    public class Main:IMain
    {
        public List<Transaction> GetAllTransactions()
        {
            try
            {
                using (EntityContext context = new EntityContext())
                {
                    var query = from i in context.Transaction
                                select i;

                    return query.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
