using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class TypeFrequency:BaseItem, INotifyPropertyChanged
    {
        #region Private Members
        string name;
        int count;
        #endregion

        #region Public Properties
        public int TypeFrequencyId { get; set; }

        public string Name { get { return name; } set { name = value; NotifyPropertyChanged("Name"); } }

        public int Count { get { return count; } set { count = value; NotifyPropertyChanged("Count"); } }
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
