using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class RecurrenceRule:BaseItem
    {
        List<RulePart> ruleParts;
        public int RecurrenceRuleId { get; set; }

        public string Name { get; set; }

        public List<RulePart> RuleParts { get { return ruleParts; } set { ruleParts = value; OnPropertyChanged("RuleParts"); } }

        public RecurrenceRule()
            : base(null)
        {

        }
        public RecurrenceRule(User user)
            : base(user)
        {
            RuleParts = new List<RulePart>();
        }
    }
}
