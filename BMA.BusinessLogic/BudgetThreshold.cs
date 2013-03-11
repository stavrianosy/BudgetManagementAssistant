using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class BudgetThreshold : BaseItem, INotifyPropertyChanged
    {
        #region Private Members
        private double amount;
        #endregion

        #region Public Properties

        public int BudgetThresholdId { get; set; }
        /// <summary>
        /// Setup the minimum amount of the budget.
        /// </summary>
        public double Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                NotifyPropertyChanged("Amount");
            }
        }
        #endregion

        #region Constructor
        public BudgetThreshold()
        {
            BudgetThresholdId = -1;
            Amount = 0d;
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
                HasChanges = true;
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion
    }
}
