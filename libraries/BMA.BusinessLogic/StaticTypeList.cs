using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace BMA.BusinessLogic
{
    public class StaticTypeList
    {
        public List<Category> Categories { get; set; }
        public List<TypeSavingsDencity> TypeSavingsDencities { get; set; }
        public List<TypeTransaction> TypeTransactions { get; set; }
        public List<TypeTransactionReason> TypeTransactionReasons { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<TypeInterval> TypeIntervals { get; set; }
        public List<TypeFrequency> TypeFrequencies { get; set; }
        public List<BudgetThreshold> BudgetThresholds { get; set; }
        public List<RecurrenceRule> RecurrenceRules { get; set; }
        
    }
}
