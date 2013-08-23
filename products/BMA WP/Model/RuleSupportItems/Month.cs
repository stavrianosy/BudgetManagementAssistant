using BMA_WP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model.RuleSupportItems
{
    public static class MonthList
    {
        static List<BasicItem> months = new List<BasicItem> 
        { 
            new BasicItem(1, AppResources.January), 
            new BasicItem(2, AppResources.February), 
            new BasicItem(3, AppResources.March), 
            new BasicItem(4, AppResources.April), 
            new BasicItem(5, AppResources.May), 
            new BasicItem(6, AppResources.June), 
            new BasicItem(7, AppResources.July), 
            new BasicItem(8, AppResources.August), 
            new BasicItem(9, AppResources.Septeber), 
            new BasicItem(10, AppResources.October), 
            new BasicItem(11, AppResources.Noveber), 
            new BasicItem(12, AppResources.December)};

        public static List<BasicItem> GetMonths()
        {
            return months;
        }
    }

}
