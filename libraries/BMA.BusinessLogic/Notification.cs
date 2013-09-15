using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class NotificationList : ObservableCollection<Notification>, IDataList
    {
        public const int DEVICE_MAX_COUNT = 10;
        public void AcceptChanges()
        {
            foreach (var item in Items)
                item.HasChanges = false;
        }

        public void PrepareForServiceSerialization()
        {
            var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).OrderByDescending(x => x.index).ToList();

            foreach (var item in deletedIDs)
                this.RemoveAt(item.index);

            this.AcceptChanges();
        }

        public bool HasItemsWithChanges()
        {
            bool result = false;

            result = this.FirstOrDefault(x => x.HasChanges) != null;

            return result;
        }
    }


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
        //parameterless ctor in order to be used in generic as T
        public Notification()
            : base(null)
        {}
        public Notification(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
            Time = new DateTime(2000, 1, 1, 22, 0, 0);
        }

        #endregion

        #region Public Events
        #endregion

        #region Private Properties
        #endregion
    }
}
