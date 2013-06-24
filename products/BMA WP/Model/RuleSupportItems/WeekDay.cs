using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model.RuleSupportItems
{
    public static class WeekDayList
    {
        static List<BasicItem> weekDays = new List<BasicItem> { new BasicItem(1, "Monday"), new BasicItem(2, "Tuesday"), new BasicItem(3, "Wednesday"), 
                                new BasicItem(4, "Thursday"), new BasicItem(5, "Friday"), new BasicItem(6, "Saturday"), new BasicItem(7, "Sunday")};

        public static List<BasicItem> GetWeekDays()
        {
            return weekDays;
        }
    }

}
