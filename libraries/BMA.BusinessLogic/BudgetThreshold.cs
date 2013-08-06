using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace BMA.BusinessLogic
{
    public class BudgetThresholdList : ObservableCollection<BudgetThreshold>, IDataList
    {
        public void AcceptChanges()
        {
            foreach (var item in Items)
                item.HasChanges = false;
        }

        public void PrepareForServiceSerialization()
        {
            var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).ToList();

            foreach (var item in deletedIDs)
                this.RemoveAt(item.index);

            this.AcceptChanges();
        }

        public bool HasItemsWithChanges()
        {
            bool result = false;

            result = this.FirstOrDefault(x => x.HasChanges) != null;

            return result;
        }
    }

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
