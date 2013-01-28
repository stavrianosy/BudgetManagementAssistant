using BMA.BusinessLogic;
using BMA.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMAServiceLib
{
    [Serializable]
    public class Static:IStatic
    {
        public StaticTypeList GetAllStaticData()
        {
            try
            {
                StaticTypeList typeData = new StaticTypeList();
                using (EntityContext context = new EntityContext())
                {
                    var typeTrans = (from i in context.TypeTransaction
                                     select i).ToList();

                    var cat = (from i in context.Category
                               select i).ToList();

                    var typeSD = (from i in context.TypeSavingsDencity
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
