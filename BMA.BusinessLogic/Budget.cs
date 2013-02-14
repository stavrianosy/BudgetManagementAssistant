using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BMA.BusinessLogic
{
    public class Budget:BaseItem, INotifyPropertyChanged
    {
        #region Private Members
        private string name;
        private double amount;
        private DateTime fromDate;
        private DateTime toDate;
        private string comments;
        private bool includeInstallemnts;
        #endregion

        #region Public Properties
        
        public int BudgetId { get; set; }

        [Required]
        public string Name { get { return name; } set { name = value; NotifyPropertyChanged("Name"); } }

        public double Amount { get { return amount; } set { amount = value; NotifyPropertyChanged("Amount"); } }

        public DateTime FromDate { get { return fromDate; } set { fromDate = value; NotifyPropertyChanged("FromDate"); NotifyPropertyChanged("DurrationDays"); } }

        public DateTime ToDate { get { return toDate; } set { toDate = value; NotifyPropertyChanged("ToDate"); NotifyPropertyChanged("DurrationDays"); } }

        public string Comments { get { return comments; } set { comments = value; NotifyPropertyChanged("Comments"); } }

        /// <summary>
        /// Subtract the installment amount when its date come.
        /// </summary>
        public bool IncludeInstallments { get { return includeInstallemnts; } set { includeInstallemnts = value; NotifyPropertyChanged("IncludeInstallemnts"); } }

        public bool HasChanged { get; set; }

        /// <summary>
        /// Display the duration of the budget in days.
        /// </summary>
        public double DurrationDays
        {
            get
            {
                TimeSpan timeSpan = ToDate.Subtract(FromDate);
                return timeSpan.Duration().TotalDays;
            }
        }
        #endregion

        #region Constructor
        public Budget()
        {
            BudgetId = -1;
            Name = "";
            Amount = 0d;
            FromDate = DateTime.Now;
            ToDate = DateTime.Now.AddDays(1);
            Comments = "";
            IncludeInstallments = false;
        }
        #endregion

        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Private Properties
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                HasChanged = true;
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion
    }
}
