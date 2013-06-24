using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model.RuleSupportItems
{
    public class BasicItem
    {
            public int Index { get; set; }
            public string Name { get; set; }

            public BasicItem(int index, string name)
            {
                Index = index;
                Name = name;
            }
        
    }
}
