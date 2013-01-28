using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BMA.BusinessLogic
{
    public class Budget:BaseItem
    {
        public int BudgetId { get; set; }

        [Required]
        public string Name { get; set; }

        public double Amount { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Comments { get; set; }

        /// <summary>
        /// Subtract the installment amount when its date come.
        /// </summary>
        public bool IncludeInstallments { get; set; }

        /// <summary>
        /// Display the duration of the budget in days.
        /// </summary>
        public double DurrationDays()
        {
            TimeSpan timeSpan = ToDate.Subtract(FromDate);
            return timeSpan.Duration().TotalDays;
        }
    }
}
