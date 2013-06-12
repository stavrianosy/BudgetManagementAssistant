using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace BMA.BusinessLogic
{
    public class TypeExpense:BaseItem
    {
        #region Private Members
        string name;
        DateTime fromDate;
        DateTime toDate;
        #endregion

        #region Public Properties
        
        public int TypeExpenseid { get; set; }

        //public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        //public DateTime FromDate { get { return fromDate; } set { fromDate = value; OnPropertyChanged("FromDate"); } }

        //public DateTime ToDate { get { return toDate; } set { toDate = value; OnPropertyChanged("ToDate"); } }
        #endregion

        #region Constructors
        public TypeExpense():this(null)
        { 
        }
        public TypeExpense(User user)
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
