using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class TypeExpense:BaseItem, INotifyPropertyChanged
    {
        #region Private Members
        string name;
        DateTime fromDate;
        DateTime toDate;
        #endregion

        #region Public Properties
        public int TypeExpenseid { get; set; }

        public string Name { get { return name; } set { name = value; NotifyPropertyChanged("Name"); } }

        public DateTime FromDate { get { return fromDate; } set { fromDate = value; NotifyPropertyChanged("FromDate"); } }

        public DateTime ToDate { get { return toDate; } set { toDate = value; NotifyPropertyChanged("ToDate"); } }
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
