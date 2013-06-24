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
        TypeTransaction transactionType;
        Category category;
        Double amount;
        string comments;
        RecurrenceRule recurrenceRule;
        RecurrenceRule recurrenceRangeRule;
        #endregion

        #region Public Properties
        public int TypeIntervalId { get; set; }

        public TypeTransaction TransactionType { get { return transactionType; } set { transactionType = value; OnPropertyChanged("TransactionType"); } }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        public string Purpose { get { return purpose; } set { purpose = value; OnPropertyChanged("Purpose"); } }

        public Category Category { get { return category; } set { category = value; OnPropertyChanged("Category"); } }

        public double Amount { get { return amount; } set { amount = value; OnPropertyChanged("Amount"); } }

        public string Comments { get { return comments; } set { comments = value; OnPropertyChanged("Comments"); } }

        public RecurrenceRule RecurrenceRule { get { return recurrenceRule; } set { recurrenceRule = value; OnPropertyChanged("RecurrenceRule"); } }

        public RecurrenceRule RecurrenceRangeRule { get { return recurrenceRangeRule; } set { recurrenceRangeRule = value; OnPropertyChanged("RecurrenceRangeRule"); } }
        #endregion

        #region Constructors
        //parameterless ctor in order to be used in generic as T
        public TypeInterval()
            : this(null,null,null)
        {}
        public TypeInterval(User user)
            : this(null, null, user)
        { }
        public TypeInterval(List<Category> categoryList, List<TypeTransaction> typeTransactionList, User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
            if (categoryList == null)
                Category = new Category(user);
            else
                Category = categoryList.Single(t => t.Name == "Other");

            if (typeTransactionList == null)
                TransactionType = new TypeTransaction(user);
            else
                TransactionType = typeTransactionList.Single(t => t.Name == "Expense");

            RecurrenceRule = new RecurrenceRule();
            
            RecurrenceRangeRule = new RecurrenceRule();
        }
        #endregion

        #region Public Events
        #endregion

        #region Private Properties
        #endregion
    }
}
