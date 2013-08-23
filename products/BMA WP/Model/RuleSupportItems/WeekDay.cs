using BMA_WP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model.RuleSupportItems
{
    public static class WeekDayList
    {
        static List<BasicItem> weekDays = new List<BasicItem> 
        { 
            new BasicItem(1, AppResources.Monday), 
            new BasicItem(2, AppResources.Tuesday), 
            new BasicItem(3, AppResources.Wednesday), 
            new BasicItem(4, AppResources.Thursday), 
            new BasicItem(5, AppResources.Friday), 
            new BasicItem(6, AppResources.Saturday), 
            new BasicItem(7, AppResources.Sunday)};

        public static List<BasicItem> GetWeekDays()
        {
            return weekDays;
        }
    }

}
