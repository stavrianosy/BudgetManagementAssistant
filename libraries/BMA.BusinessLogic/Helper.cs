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
    }
}
