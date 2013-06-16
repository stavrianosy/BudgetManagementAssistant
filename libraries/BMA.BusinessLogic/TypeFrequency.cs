using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace BMA.BusinessLogic
{
    public class TypeFrequency:BaseItem
    {
        #region Private Members
        string name;
        int count;
        #endregion

        #region Public Properties
        public int TypeFrequencyId { get; set; }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        public int Count { get { return count; } set { count = value; OnPropertyChanged("Count"); } }
        #endregion

        #region Constructors
        //parameterless ctor in order to be used in generic as T
        public TypeFrequency()
            : base(null)
        {}
        public TypeFrequency(User user)
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
