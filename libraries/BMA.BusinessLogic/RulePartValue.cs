using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class RulePartValue
    {
        public int RulePartValueId { get; set; }

        public RulePart RulePart { get; set; }

        public string Value { get; set; }
    }
}
