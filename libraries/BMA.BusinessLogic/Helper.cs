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
            return GetDayOcurrenceOfMonth(date, dayNum, count, -1);
        }

        public static DateTime GetDayOcurrenceOfMonth(DateTime date, int dayNum, int count, int monthNum)
        {
            DateTime result = new DateTime();

            if (monthNum > 0 && date.Month > monthNum)
                result = new DateTime(date.AddYears(1).Year, monthNum, 1);
            else
                result = new DateTime(date.Year, monthNum, 1);

            Dictionary<string, int> dayOfTheWeekNumber = new Dictionary<string, int>();
            dayOfTheWeekNumber.Add("Monday", 1);
            dayOfTheWeekNumber.Add("Tuesday", 2);
            dayOfTheWeekNumber.Add("Wednesday", 3);
            dayOfTheWeekNumber.Add("Thursday", 4);
            dayOfTheWeekNumber.Add("Friday", 5);
            dayOfTheWeekNumber.Add("Saturday", 6);
            dayOfTheWeekNumber.Add("Sunday", 7);

            var firstDayOfMonth = dayOfTheWeekNumber[result.AddDays(-result.Day + 1).DayOfWeek.ToString()];
            firstDayOfMonth = dayNum - firstDayOfMonth;

            if (firstDayOfMonth < 0)
                firstDayOfMonth += 7;

            var occurenceDay = firstDayOfMonth + (count-1) * 7;

            result = result.AddDays(occurenceDay);

            if (result < date)
            {
                result = GetDayOcurrenceOfMonth(new DateTime(result.Year, result.AddMonths(1).Month, 1), dayNum, monthNum, count);
            }

            return result;
        }

        public static DateTime AdjustYearStatDay(DateTime startDay, int monthOfYear, int dayPosOfYear)
        {
            if (startDay.Year % 4 != 0 && monthOfYear == 2 && dayPosOfYear == 29)
                dayPosOfYear--;

            var result = new DateTime(startDay.Year, monthOfYear, dayPosOfYear);

            if (result < DateTime.Now)
            {
                if (startDay.AddYears(1).Year % 4 != 0 && monthOfYear == 2 && dayPosOfYear == 29)
                    dayPosOfYear--;
                
                result = new DateTime(startDay.AddYears(1).Year, monthOfYear, dayPosOfYear);
            }

            return result;
        }

        public static int YearRange(DateTime dateFrom, DateTime dateTo)
        {
            var result = 0;

            if (dateTo > dateFrom)
            {
                int years = dateTo.Year - dateFrom.Year;

                if (years > 0 && dateTo.DayOfYear < dateFrom.DayOfYear)
                    years--;

                result = years;
            }

            return result;
        }
    }
}
