using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class TypeExpense
    {
        public int TypeExpenseid { get; set; }
        
        public string Name { get; set; }

        public DateTime FromDate { get; set; }
        
        public DateTime ToDate { get; set; }
    }
}
