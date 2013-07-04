using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class Const
    {
        public enum Rule
        {
            None,
            RuleDailyEveryDays,
            RuleWeeklyEveryWeek,
            RuleMonthlyDayNum,
            RuleMonthlyPrecise,
            RuleYearlyOnMonth,
            RuleYearlyOnTheWeekDay,
            //## RANGE ##//
            RuleRangeNoEndDate,
            RuleRangeTotalOcurrences,
            RuleRangeEndBy
        }

        public enum RuleField
        {
            None,
            RangeStartDate,
            RangeNoEndDate,
            RangeTotalOcurrences,
            RangeEndBy,
            DailyEveryDay,
            DailyOnlyWeekdays,
            WeeklyEveryWeek,
            WeeklyDayName,
            MonthlyDayNumber,
            MonthlyEveryMonth,
            MonthlyCountOfWeekDay,
            MonthlyDayName,
            MonthlyCountOfMonth,
            YearlyEveryYear,
            YearlyOnDayPos,
            YearlyMonthName,
            YearlyPositions,
            YearlyDayName,
            YearlyMonthNameSec
        }

        public enum FieldType
        {
            None,
            Label,
            Int,
            DayNum,
            DateInt,
            Bit,
            String,
            Position
        }
    }
}
