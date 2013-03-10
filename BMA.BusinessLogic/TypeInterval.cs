using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class TypeInterval : BaseItem, INotifyPropertyChanged
    {
        #region Private Members
        string name;
        bool isIncome;
        #endregion

        #region Public Properties
        public int TypeIntervalId { get; set; }

        public bool IsIncome { get { return isIncome; } set { isIncome = value; NotifyPropertyChanged("IsIncome"); } }

        public string Name { get { return name; } set { name = value; NotifyPropertyChanged("Name"); } }
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
