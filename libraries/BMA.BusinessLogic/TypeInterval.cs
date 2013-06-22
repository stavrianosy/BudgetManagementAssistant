using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace BMA.BusinessLogic
{
    public class TypeInterval : BaseItem
    {
        #region Private Members
        string name;
        string purpose;
        bool isIncome;
        Category category;
        Double amount;
        string comments;
        RecurrenceRangeRule recRangeRule;
        #endregion

        #region Public Properties
        public int TypeIntervalId { get; set; }

        public bool IsIncome { get { return isIncome; } set { isIncome = value; OnPropertyChanged("IsIncome"); } }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        public string Purpose { get { return purpose; } set { purpose = value; OnPropertyChanged("Purpose"); } }

        public Category Category { get { return category; } set { category = value; OnPropertyChanged("Category"); } }

        public double Amount { get { return amount; } set { amount = value; OnPropertyChanged("Amount"); } }

        public string Comments { get { return comments; } set { comments = value; OnPropertyChanged("Comments"); } }

        public RecurrenceRangeRule RecurrenceRangeRule { get { return recRangeRule; } set { recRangeRule = value; OnPropertyChanged("RecurrenceRangeRule"); } }
        #endregion

        #region Constructors
        //parameterless ctor in order to be used in generic as T
        public TypeInterval()
            : base(null)
        {}
        public TypeInterval(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
        }
        #endregion

        #region Public Events
        #endregion

        #region Private Properties
        #endregion
    }
}
