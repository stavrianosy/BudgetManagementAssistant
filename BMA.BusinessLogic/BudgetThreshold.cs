using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class BudgetThreshold : BaseItem
    {
        public int BudgetThresholdId { get; set; }
        /// <summary>
        /// Setup the minimum amount of the budget.
        /// </summary>
        public double Amount { get; set; }
    }
}
