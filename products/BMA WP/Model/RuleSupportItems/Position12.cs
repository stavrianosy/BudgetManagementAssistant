using BMA_WP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model.RuleSupportItems
{
    public static class Position12List
    {
        static List<BasicItem> positions = new List<BasicItem> { new BasicItem(1, AppResources.First), new BasicItem(2, "2nd Second"), 
                            new BasicItem(3, "3rd Third"), new BasicItem(4, "4th Forth"), new BasicItem(5, "5th Fifth"), new BasicItem(6, "6th Sisth"), 
                            new BasicItem(7, "7th Seventh"), new BasicItem(8, "8th Eightth"), new BasicItem(9, "9th Nineth"), 
                            new BasicItem(10, "10th Tenth"), new BasicItem(11, "11th Eleventh"), new BasicItem(12, "12th Twelveth") 
        };

        public static List<BasicItem> GetPositions()
        {
            return positions;
        }
    }
}
