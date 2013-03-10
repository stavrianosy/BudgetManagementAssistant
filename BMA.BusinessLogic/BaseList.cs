using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public abstract class BaseList:ObservableCollection<BaseItem>
    {
        public BaseList GetChanges<T>() 
            where T: BaseList, new()
        {
            T obj = new T();
            var query = this.Where(t => t.HasChanges).ToList();
            foreach (var item in query)
                obj.Add(item);

            return obj;
        }
    }
}
