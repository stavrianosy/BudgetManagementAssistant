using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class Notification : BaseItem
    {
        #region Private Members
        string name;
        DateTime time;
        string description;
        #endregion

        #region Public Properties

        public int NotificationId { get; set; }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        public DateTime Time { get { return time; } set { time = value; OnPropertyChanged("Time"); } }

        public string Description { get { return description; } set { description = value; OnPropertyChanged("Description"); } }

        #endregion

        #region Constructors
        public Notification():this(null)
        { 
        }
        public Notification(User user)
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
