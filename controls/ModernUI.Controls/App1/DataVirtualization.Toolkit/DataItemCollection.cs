using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVirtualization.Toolkit
{
    public class DataItemCollection : ObservableCollection<DataItem>
    {
        public static DataItemCollection DefaultList()
        {
            DataItemCollection DataItemSource = new DataItemCollection();
            DataItemSource.Add(new DataItem()
            {
                ID=1,
                Name = "HR",
                Value = 45
            });
            DataItemSource.Add(new DataItem()
            {
                ID = 2,
                Name = "Finance",
                Value = 100
            });
            DataItemSource.Add(new DataItem()
            {
                ID = 3,
                Name = "IT",
                Value = 178
            }
            );
            DataItemSource.Add(new DataItem()
            {
                ID = 4,
                Name = "Operations",
                Value = 250
            });
            DataItemSource.Add(new DataItem()
            {
                ID = 5,
                Name = "Administration",
                Value = 30
            });
            DataItemSource.Add(new DataItem()
            {
                ID = 6,
                Name = "Administration",
                Value = 35
            });
            DataItemSource.Add(new DataItem()
            {
                ID = 7,
                Name = "Administration",
                Value = 25
            });
            DataItemSource.Add(new DataItem()
            {
                ID = 8,
                Name = "Administration",
                Value = 40
            });
            return DataItemSource;
        }
    }
}
