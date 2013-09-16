using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


namespace BMA.BusinessLogic
{
    public abstract class BaseList<T> : ObservableCollection<T> where T : BaseItem, new()
    {
        //public BaseList<T> GetChanges<T>()
        //    where T : BaseList<T>, new()
        //{
        //    T obj = new T();
        //    var query = this.Where(t => t.HasChanges).ToList();

        //    //foreach (var item in query)
        //    //    obj.Add(item);

        //    return obj;
        //}

        public bool HasItemsWithChanges()
        {
            bool result = false;

            result = this.FirstOrDefault(x => x.HasChanges) != null;

            return result;
        }

        public void RemoveDeleted()
        {
            var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).OrderByDescending(x => x.index).ToList();

            foreach (var item in deletedIDs)
                this.RemoveAt(item.index);
        }

        public void AcceptChanges()
        {
            foreach (var item in Items)
                item.HasChanges = false;
        }
    }
}
