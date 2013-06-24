using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model.RuleSupportItems
{
    public static class MonthList
    {
        static List<BasicItem> months = new List<BasicItem> { new BasicItem(1, "January"), new BasicItem(2, "February"), new BasicItem(3, "March"), new BasicItem(4, "April"), 
                                new BasicItem(5, "May"), new BasicItem(6, "June"), new BasicItem(7, "July"), new BasicItem(8, "August"), 
                                new BasicItem(9, "Septeber"), new BasicItem(10, "October"), new BasicItem(11, "Noveber"), new BasicItem(12, "December")};

        public static List<BasicItem> GetMonths()
        {
            return months;
        }
    }

}
