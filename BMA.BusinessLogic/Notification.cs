using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BMA.BusinessLogic
{
    public class Notification : BaseItem, INotifyPropertyChanged
    {
        #region Private Members
        string name;
        DateTime time;
        string description;
        #endregion

        #region Public Properties

        public int NotificationId { get; set; }

        public string Name { get { return name; } set { name = value; NotifyPropertyChanged("Name"); } }

        public DateTime Time { get { return time; } set { time = value; NotifyPropertyChanged("Time"); } }

        public string Description { get { return description; } set { description = value; NotifyPropertyChanged("Description"); } }

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
