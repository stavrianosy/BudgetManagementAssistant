using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class FieldTypeList : ObservableCollection<FieldType>
    {
    }

    public class FieldType
    {
        public int FieldTypeId { get; set; }

        public string Name{get;set;}

        public string DefaultValue { get; set; }

        public string Type { get; set; }

    }
}
