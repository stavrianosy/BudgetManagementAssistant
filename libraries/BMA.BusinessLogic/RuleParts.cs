using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class RulePart:INotifyPropertyChanged
    {
        string fieldValue;

        public int RulePartId { get; set; }

        public string FieldName { get; set; }

        public string FieldValue { get { return fieldValue; } set { fieldValue = value; OnPropertyChanged("FieldValue"); } }

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
