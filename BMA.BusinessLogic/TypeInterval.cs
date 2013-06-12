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
        bool isIncome;
        #endregion

        #region Public Properties
        public int TypeIntervalId { get; set; }

        public bool IsIncome { get { return isIncome; } set { isIncome = value; OnPropertyChanged("IsIncome"); } }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }
        #endregion

        #region Constructors
        public TypeInterval():this(null)
        { 
        }
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
