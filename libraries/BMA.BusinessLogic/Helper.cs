using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public static class Helper
    {
        public static DateTime ConvertStringToDate(string dateString)
        {
            var result = DateTime.Now;


            var year = int.Parse(dateString.Substring(0,4));
            var month = int.Parse(dateString.Substring(4,2));
            var day = int.Parse(dateString.Substring(6, 2));

            result = new DateTime(year, month, day);

            return result;
        }

        public static int MonthRange(DateTime dateFrom, DateTime dateTo)
        {
            var result = 0;
            
            if (dateTo > dateFrom)
            {
                int months = dateTo.Month - dateFrom.Month;

                if (months > 0 && dateTo.Day < dateFrom.Day)
                    months--;

                int years = dateTo.Year - dateFrom.Year;
                months += years * 12;

                result = months;
            }

            return result;
        }

        public static DateTime GetDayOcurrenceOfMonth(DateTime date, int dayNum, int count)
        {
            DateTime result = new DateTime(date.Year, date.Month, 1);

            Dictionary<string, int> dayOfTheWeekNumber = new Dictionary<string, int>();
            dayOfTheWeekNumber.Add("Monday", 1);
            dayOfTheWeekNumber.Add("Tuesday", 2);
            dayOfTheWeekNumber.Add("Wednesday", 3);
            dayOfTheWeekNumber.Add("Thursday", 4);
            dayOfTheWeekNumber.Add("Friday", 5);
            dayOfTheWeekNumber.Add("Saturday", 6);
            dayOfTheWeekNumber.Add("Sunday", 7);

            var firstDayOfMonth = dayOfTheWeekNumber[date.AddDays(-date.Day + 1).DayOfWeek.ToString()];
            firstDayOfMonth = dayNum - firstDayOfMonth;

            if (firstDayOfMonth < 0)
                firstDayOfMonth += 7;

            var occurenceDay = firstDayOfMonth + (count-1) * 7;

            result = result.AddDays(occurenceDay);

            if (result < date)
            {
                result = GetDayOcurrenceOfMonth(new DateTime(result.Year, result.AddMonths(1).Month, 1), dayNum, count);
            }

            return result;
        }
    }
}
