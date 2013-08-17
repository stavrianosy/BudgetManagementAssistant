using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public static class Helper
    {
        private static Dictionary<string, int> dayOfTheWeekNumber;
        static Helper()
        {
            dayOfTheWeekNumber = new Dictionary<string, int>();
            dayOfTheWeekNumber.Add("Monday", 1);
            dayOfTheWeekNumber.Add("Tuesday", 2);
            dayOfTheWeekNumber.Add("Wednesday", 3);
            dayOfTheWeekNumber.Add("Thursday", 4);
            dayOfTheWeekNumber.Add("Friday", 5);
            dayOfTheWeekNumber.Add("Saturday", 6);
            dayOfTheWeekNumber.Add("Sunday", 7);
        }

        public static DateTime ConvertStringToDate(string dateString)
        {
            var result = DateTime.Now;


            var year = int.Parse(dateString.Substring(0,4));
            var month = int.Parse(dateString.Substring(4,2));
            var day = int.Parse(dateString.Substring(6, 2));

            result = new DateTime(year, month, day);

            return result;
        }

        public static int DayRange(DateTime dateFrom, DateTime dateTo)
        {
            var result = 0;

            if (dateTo > dateFrom)
            {
                var daysSpan = dateTo.Subtract(dateFrom);
                result = daysSpan.Days;
            }

            return result;
        }

        public static int WeekRange(DateTime dateFrom, DateTime dateTo)
        {
            var result = 0;
            if (dateTo > dateFrom)
            {
                var daysSpan = dateTo.Subtract(dateFrom);
                result = daysSpan.Days / 7;
            }

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

        public static DateTime AdjustDayOcurrenceDaily(DateTime date, bool onlyWeekDays)
        {
            var result = date;

            if (onlyWeekDays)
                if (date.DayOfWeek == DayOfWeek.Saturday)
                    result = result.AddDays(2);
                else if(date.DayOfWeek == DayOfWeek.Sunday)
                    result = result.AddDays(1);

            return result;
        }

        public static DateTime AdjustDayOcurrenceWeekly(DateTime date, int dayOfTheWeek)
        {
            var result = date;
            
            while (dayOfTheWeekNumber[result.DayOfWeek.ToString()] != dayOfTheWeek)
                result = result.AddDays(1);

            return result;
        }

        public static DateTime AdjustDayOcurrenceMonthly(DateTime date, int dayOfMonth)
        {
            var result = date;
            var totalDaysOfMonth = 0;

            if (result.Day > dayOfMonth)
                result = result.AddMonths(1);

            totalDaysOfMonth = new DateTime(result.Year, result.AddMonths(1).Month, 1).AddDays(-1).Day;
            
            if (totalDaysOfMonth >= dayOfMonth)
                result = new DateTime(result.Year, result.Month, dayOfMonth);
            
            return result;
        }

        public static DateTime GetDayOcurrenceOfMonth(DateTime date, int dayNum, int count)
        {
            return GetDayOcurrenceOfMonth(date, dayNum, count, -1);
        }

        public static DateTime GetDayOcurrenceOfMonth(DateTime date, int dayNum, int count, int monthNum)
        {
            var tempMonthNum = monthNum > 0 ? monthNum : date.Month;
            var result = new DateTime(date.Year, tempMonthNum, 1);

            result = AdjustToDayOfTheWeek(result, dayNum, count);

            if (result < date)
            {
                var tempDate = monthNum > 0 ? result.AddYears(1) : result.AddMonths(1);
                result = GetDayOcurrenceOfMonth(new DateTime(tempDate.Year, tempDate.Month, 1), dayNum, count, monthNum);
            }

            return result;
        }

        private static DateTime AdjustToDayOfTheWeek(DateTime date, int dayNum, int count)
        {
            var result = date;

            var firstDayOfMonth = dayOfTheWeekNumber[result.AddDays(-result.Day + 1).DayOfWeek.ToString()];
            firstDayOfMonth = dayNum - firstDayOfMonth;

            if (firstDayOfMonth < 0)
                firstDayOfMonth += 7;

            var occurenceDay = firstDayOfMonth + (count - 1) * 7;

            result = result.AddDays(occurenceDay);

            return result;
        }

        public static DateTime AdjustYearStatDay(DateTime startDay, int monthOfYear, int dayPosOfYear)
        {
            var result = startDay;
            var totalDaysOfMonth = 0;

            if (startDay.Month > monthOfYear || (startDay.Month == monthOfYear && startDay.Day > dayPosOfYear))
                result = new DateTime(startDay.AddYears(1).Year, monthOfYear, 1);

            totalDaysOfMonth = new DateTime(result.Year, result.AddMonths(1).Month, 1).AddDays(-1).Day;

            if (dayPosOfYear > totalDaysOfMonth)
                result = new DateTime(result.Year, result.Month, totalDaysOfMonth);
            else
                result = new DateTime(result.Year, result.Month, dayPosOfYear);

            return result;
        }

        public static int CalculateTotalOcurrences(int occurencesSinceBeginning, int totalOccurences, int ruleTotalOccurences)
        {
            var result = totalOccurences;

            int calcPastOccurences = 0;
            int newTotalOccurences = 0;

            calcPastOccurences = occurencesSinceBeginning - totalOccurences;
            newTotalOccurences = ruleTotalOccurences - calcPastOccurences;

            if (ruleTotalOccurences > 0 && newTotalOccurences < totalOccurences)
                result = newTotalOccurences;

            return result;
        }

    }
}
