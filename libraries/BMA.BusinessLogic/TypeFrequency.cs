using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace BMA.BusinessLogic
{
    public class TypeFrequencyList : ObservableCollection<TypeFrequency>, IDataList
    {
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
