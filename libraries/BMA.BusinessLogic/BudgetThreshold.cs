using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace BMA.BusinessLogic
{
    public class BudgetThreshold : BaseItem
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
                OnPropertyChanged("Amount");
            }
        }
        #endregion

        #region Constructor
        //parameterless ctor in order to be used in generic as T
        public BudgetThreshold()
            : base(null)
        {}
        public BudgetThreshold(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
            BudgetThresholdId = -1;
            Amount = 0d;
        }
        #endregion

        #region Public Events
        #endregion

        #region Private Properties
        #endregion
    }
}
