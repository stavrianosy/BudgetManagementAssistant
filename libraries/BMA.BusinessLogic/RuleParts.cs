using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class RulePartList : ObservableCollection<RulePart>
    { }

    public class RulePart:INotifyPropertyChanged
    {
        public int RulePartId { get; set; }

        public string FieldName { get; set; }

        public FieldType FieldType { get; set; }

        [IgnoreDataMember]
        public List<RecurrenceRule> RecurrenceRules { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

    }
}
