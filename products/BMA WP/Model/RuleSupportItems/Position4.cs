using BMA_WP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model.RuleSupportItems
{
    public static class Position4List
    {
        static List<BasicItem> position4 = new List<BasicItem> { new BasicItem(1, AppResources.First), new BasicItem(2, "2nd - Second"), 
                            new BasicItem(3, "3rd - Third"), new BasicItem(4, "4th - Forth"), new BasicItem(5, "Last")};

        public static List<BasicItem> GetPositions()
        {
            return position4;
        }
    }
}
