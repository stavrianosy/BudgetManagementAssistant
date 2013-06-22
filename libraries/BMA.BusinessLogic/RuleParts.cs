using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class RulePart
    {
        public int RulePartId { get; set; }

        public string FieldName { get; set; }

        public FieldType FieldType { get; set; }
    }
}
