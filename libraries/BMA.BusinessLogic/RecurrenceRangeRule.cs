using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class RecurrenceRangeRule
    {
        public int RecurrenceRangeRuleId { get; set; }

        public string Name { get; set; }

        public List<RulePart> RuleParts { get; set; }

        public RecurrenceRangeRule()
        {
        }

    }
}
