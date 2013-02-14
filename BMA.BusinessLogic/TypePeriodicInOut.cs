using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class TypePeriodicInOut:BaseItem
    {
        public int TypePeriodicInOutId { get; set; }

        public bool isIncome { get; set; }

        public string Name { get; set; }
    }
}
