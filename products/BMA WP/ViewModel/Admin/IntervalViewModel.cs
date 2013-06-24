using BMA.BusinessLogic;
using System.Linq;
using BMA_WP.Resources;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BMA_WP.Model.RuleSupportItems;

namespace BMA_WP.ViewModel.Admin
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class IntervalViewModel : ViewModelBase
    {
        TypeInterval currInterval;
        bool isEnabled;
        DateTime rangeRuleEnddate;

        RecurrenceRule ruleDailyEveryDays;
        int pivotIndex;

        bool isEnabled_RuleDailyEveryDays;
        bool isEnabled_RuleWeeklyEveryWeek;
        bool isEnabled_RuleMonthlyDayNum;
        bool isEnabled_RuleMonthlyPrecise;
        bool isEnabled_RuleYearlyOnMonth;
        bool isEnabled_RuleYearlyOnTheWeekDay;
        bool isEnabled_RuleYearlyOnMonth2;
        bool isEnabled_RuleRangeStartDate;
        bool isEnabled_RuleRangeTotalOcurrences;
        bool isEnabled_RuleRangeEndBy;

        List<int> days = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        List<string> monthNames = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "Septeber", "October", "Nivember", "December" };

        public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; RaisePropertyChanged("IsEnabled"); } }
        public TypeInterval CurrInterval { get { return currInterval; } set { currInterval = value; RaisePropertyChanged("CurrInterval"); } }
        public DateTime RangeRuleEndDate { get { return rangeRuleEnddate; } set { rangeRuleEnddate = value; RaisePropertyChanged("RangeRuleEnddate"); } }
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }

        public ObservableCollection<TypeInterval> TypeIntervalList { get { return App.Instance.StaticServiceData.IntervalList; } }
        public ObservableCollection<RecurrenceRule> RecurrenceRuleList { get { return App.Instance.StaticServiceData.RecurrenceRuleList; } }
        public ObservableCollection<TypeTransaction> TypeTransactionList { get { return App.Instance.StaticServiceData.TypeTransactionList; } }
        public ObservableCollection<Category> CategoryList { get { return App.Instance.StaticServiceData.CategoryList; } }


        public List<BasicItem> Months {get {return MonthList.GetMonths();}}
        public List<BasicItem> WeekDays { get { return WeekDayList.GetWeekDays(); } }
        public List<BasicItem> PosDayOfMonth { get { return PositionList.GetPositions(); } }
        public List<BasicItem> Position4 { get { return Position4List.GetPositions(); } }

        public bool IsEnabled_RuleDailyEveryDays { get { return isEnabled_RuleDailyEveryDays; } set { isEnabled_RuleDailyEveryDays = value; RaisePropertyChanged("IsEnabled_RuleDailyEveryDays"); } }
        public bool IsEnabled_RuleWeeklyEveryWeek { get { return isEnabled_RuleWeeklyEveryWeek; } set { isEnabled_RuleWeeklyEveryWeek = value; RaisePropertyChanged("IsEnabled_RuleWeeklyEveryWeek"); } }
        public bool IsEnabled_RuleMonthlyDayNum { get { return isEnabled_RuleMonthlyDayNum; } set { isEnabled_RuleMonthlyDayNum = value; RaisePropertyChanged("IsEnabled_RuleMonthlyDayNum"); } }
        public bool IsEnabled_RuleMonthlyPrecise { get { return isEnabled_RuleMonthlyPrecise; } set { isEnabled_RuleMonthlyPrecise = value; RaisePropertyChanged("IsEnabled_RuleMonthlyPrecise"); } }
        public bool IsEnabled_RuleYearlyOnMonth { get { return isEnabled_RuleYearlyOnMonth; } set { isEnabled_RuleYearlyOnMonth = value; RaisePropertyChanged("IsEnabled_RuleYearlyOnMonth"); } }
        public bool IsEnabled_RuleYearlyOnTheWeekDay { get { return isEnabled_RuleYearlyOnTheWeekDay; } set { isEnabled_RuleYearlyOnTheWeekDay = value; RaisePropertyChanged("IsEnabled_RuleYearlyOnTheWeekDay"); } }
        public bool IsEnabled_RuleYearlyOnMonth2 { get { return isEnabled_RuleYearlyOnMonth2; } set { isEnabled_RuleYearlyOnMonth2 = value; RaisePropertyChanged("IsEnabled_RuleYearlyOnMonth2"); } }
        public bool IsEnabled_RuleRangeTotalOcurrences { get { return isEnabled_RuleRangeTotalOcurrences; } set { isEnabled_RuleRangeTotalOcurrences = value; RaisePropertyChanged("IsEnabled_RuleRangeTotalOcurrences"); } }
        public bool IsEnabled_RuleRangeEndBy { get { return isEnabled_RuleRangeEndBy; } set { isEnabled_RuleRangeEndBy = value; RaisePropertyChanged("IsEnabled_RuleRangeEndBy"); } }
        public bool IsEnabled_RuleRangeStartDate { get { return isEnabled_RuleRangeStartDate; } set { isEnabled_RuleRangeStartDate = value; RaisePropertyChanged("IsEnabled_RuleRangeStartDate"); } }
        

        public string DailyEveryDay
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "DailyEveryDay").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "DailyEveryDay").FieldValue = value; RaisePropertyChanged("DailyEveryDay"); }
        }

        public string DailyOnlyWeekdays
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "DailyOnlyWeekdays").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "DailyOnlyWeekdays").FieldValue = value; RaisePropertyChanged("DailyOnlyWeekdays"); }
        }

        public string WeeklyEveryWeek
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "WeeklyEveryWeek").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "WeeklyEveryWeek").FieldValue = value; RaisePropertyChanged("WeeklyEveryWeek"); }
        }

        public string WeeklyDayName
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "WeeklyDayName").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "WeeklyDayName").FieldValue = value; RaisePropertyChanged("WeeklyDayName"); }
        }

        public string MonthlyDayNumber
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyDayNumber").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyDayNumber").FieldValue = value; RaisePropertyChanged("MonthlyDayNumber"); }
        }

        public string MonthlyEveryMonth
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyEveryMonth").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyEveryMonth").FieldValue = value; RaisePropertyChanged("MonthlyEveryMonth"); }
        }

        public string MonthlyCountOfWeekDay
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyCountOfWeekDay").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyCountOfWeekDay").FieldValue = value; RaisePropertyChanged("MonthlyCountOfWeekDay"); }
        }

        public string MonthlyDayName
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyDayName").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyDayName").FieldValue = value; RaisePropertyChanged("MonthlyDayName"); }
        }

        public string MonthlyCountOfMonth
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyCountOfMonth").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyCountOfMonth").FieldValue = value; RaisePropertyChanged("MonthlyCountOfMonth"); }
        }

        public string MonthlyEveryMonth2
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyEveryMonth").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "MonthlyEveryMonth").FieldValue = value; RaisePropertyChanged("MonthlyEveryMonth"); }
        }

        public string YearlyEveryYear
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyEveryYear").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyEveryYear").FieldValue = value; RaisePropertyChanged("YearlyEveryYear"); }
        }

        public string YearlyOnDayPos
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyOnDayPos").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyOnDayPos").FieldValue = value; RaisePropertyChanged("YearlyOnDayPos"); }
        }

        public string YearlyMonthName
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyMonthName").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyMonthName").FieldValue = value; RaisePropertyChanged("YearlyMonthName"); }
        }

        public string YearlyPositions
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyPositions").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyPositions").FieldValue = value; RaisePropertyChanged("YearlyPositions"); }
        }

        public string YearlyDayName
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyDayName").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyDayName").FieldValue = value; RaisePropertyChanged("YearlyDayName"); }
        }

        public string YearlyMonthNameSec
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyMonthNameSec").FieldValue : null; }
            set { CurrInterval.RecurrenceRule.RuleParts.FirstOrDefault(x => x.FieldName == "YearlyMonthNameSec").FieldValue = value; RaisePropertyChanged("YearlyMonthNameSec"); }
        }

        public string RangeTotalOcurrences
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRangeRule.RuleParts.FirstOrDefault(x => x.FieldName == "RangeTotalOcurrences").FieldValue : null; }
            set { CurrInterval.RecurrenceRangeRule.RuleParts.FirstOrDefault(x => x.FieldName == "RangeTotalOcurrences").FieldValue = value; RaisePropertyChanged("RangeTotalOcurrences"); }
        }

        public string RangeEndBy
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRangeRule.RuleParts.FirstOrDefault(x => x.FieldName == "RangeEndBy").FieldValue : null; }
            set { CurrInterval.RecurrenceRangeRule.RuleParts.FirstOrDefault(x => x.FieldName == "RangeEndBy").FieldValue = value; RaisePropertyChanged("RangeEndBy"); }
        }

        public string RangeStartDate
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRangeRule.RuleParts.FirstOrDefault(x => x.FieldName == "RangeStartDate").FieldValue : null; }
            set { CurrInterval.RecurrenceRangeRule.RuleParts.FirstOrDefault(x => x.FieldName == "RangeStartDate").FieldValue = value; RaisePropertyChanged("RangeStartDate"); }
        }

        /// <summary>
        /// Initializes a new instance of the IntervalViewModel class.
        /// </summary>
        public IntervalViewModel()
        {
            isEnabled = true;
        }
    }
}