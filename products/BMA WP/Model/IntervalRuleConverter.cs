using BMA.BusinessLogic;
using BMA_WP.Model.RuleSupportItems;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BMA_WP.Model
{
    public class IntervalRuleConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                string param = parameter.ToString().ToLower();

                switch (param)
                {
                    case "ruledateint":
                        string dateInt = value as string;
                        DateTime startDate = DateTime.Now;
                        
                        if (dateInt != null && dateInt.Length > 0)
                            startDate = Helper.ConvertStringToDate(dateInt);
                        
                            return startDate;
                    case "weekdays":
                        BasicItem weekDay = null;
                        if (value == "")
                            weekDay = WeekDayList.GetWeekDays()[0];
                        else if (value != null)
                            weekDay = WeekDayList.GetWeekDays().FirstOrDefault(x => x.Index.ToString() == value.ToString());

                        return weekDay;
                    case "months":
                        BasicItem month = null;
                        if (value == "")
                            month = MonthList.GetMonths()[0];
                        else if (value != null)
                            month = MonthList.GetMonths().FirstOrDefault(x => x.Index.ToString() == value.ToString());

                        return month;
                    case "positionall":
                        BasicItem posAll = null;
                        if (value == "")
                            posAll = PositionList.GetPositions()[0];
                        else if (value != null)
                            posAll = PositionList.GetPositions().FirstOrDefault(x => x.Index.ToString() == value.ToString());

                        return posAll;
                    case "position4":
                        BasicItem pos4 = null;
                        if (value == "")
                            pos4 = Position4List.GetPositions()[0];
                        else if (value != null)
                            pos4 = Position4List.GetPositions().FirstOrDefault(x => x.Index.ToString() == value.ToString());

                        return pos4;
                    case "position12":
                        BasicItem pos12 = null;
                        if (value == "")
                            pos12 = Position12List.GetPositions()[0];
                        else if (value != null)
                            pos12 = Position12List.GetPositions().FirstOrDefault(x => x.Index.ToString() == value.ToString());

                        return pos12;

                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                string param = parameter.ToString().ToLower();

                switch (param)
                {
                    case "ruledateint":
                        DateTime? dateInt = value as DateTime?;
                        string startDate = "";
                        if (dateInt != null)
                        {
                            startDate = string.Format("{0:0000}{1:00}{2:00}", dateInt.Value.Year, dateInt.Value.Month, dateInt.Value.Day);
                        }
                        return startDate;
                    case "weekdays":
                        BasicItem weekDay = value as BasicItem;
                        string resultWeekDay = null;
                        if (weekDay != null)
                            resultWeekDay = weekDay.Index.ToString();

                        return resultWeekDay;
                    case "months":
                        BasicItem month = value as BasicItem;
                        string resultMonth = null;
                        if (month != null)
                            resultMonth = month.Index.ToString();

                        return resultMonth;
                    case "positionall":
                    case "position4":
                    case "position12":
                        BasicItem posAll = value as BasicItem;
                        string resultPosAll = null;
                        if (posAll != null)
                            resultPosAll = posAll.Index.ToString();

                        return resultPosAll;
                }
            }
            return null;
        }
    }
}
