using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class Target:BaseItem
    {
        public int TargetId { get; set; }

        public double Amount { get; set; }

        public DateTime EndDate { get; set; }

        public TypeSavingsDencity SavingsDencityType { get; set; }

        public string Comments { get; set; }

    }
}
