using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class StaticTypeList
    {
        public List<Category> Categories { get; set; }
        public List<TypeSavingsDencity> TypeSavingsDencities { get; set; }
        public List<TypeTransaction> TypeTransactions { get; set; }
        public List<TypeTransactionReason> TypeTransactionReasons { get; set; }
    }
}
