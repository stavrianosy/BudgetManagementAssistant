using BMA.BusinessLogic;
using System.Linq;
using BMA_WP.Resources;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BMA_WP.Model.RuleSupportItems;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;

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
        bool isLoading;
        DateTime rangeRuleEnddate;

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

        bool isChecked_RuleDailyEveryDays;
        bool isChecked_RuleWeeklyEveryWeek;
        bool isChecked_RuleMonthlyDayNum;
        bool isChecked_RuleMonthlyPrecise;
        bool isChecked_RuleYearlyOnMonth;
        bool isChecked_RuleYearlyOnTheWeekDay;
        bool isChecked_RuleYearlyOnMonth2;
        bool isChecked_RuleRangeNoEndDate;
        bool isChecked_RuleRangeTotalOcurrences;
        bool isChecked_RuleRangeEndBy;

        bool isToggled_tglDaily;
        bool isToggled_tglWeekly;
        bool isToggled_tglMonthly;
        bool isToggled_tglYearly;

        public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; RaisePropertyChanged("IsEnabled"); } }
        public bool IsLoading { get { return App.Instance.IsBusyComm; } set { App.Instance.IsLoading = value; } }
        public TypeInterval CurrInterval { get { return currInterval; } set { currInterval = value; RaisePropertyChanged("CurrInterval"); } }
        public DateTime RangeRuleEndDate { get { return rangeRuleEnddate; } set { rangeRuleEnddate = value; RaisePropertyChanged("RangeRuleEnddate"); } }
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }

        public TypeIntervalList TypeIntervalList { get { return App.Instance.StaticServiceData.IntervalList; } }
        public RecurrenceRuleList RecurrenceRuleList { get { return App.Instance.StaticServiceData.RecurrenceRuleList; } }
        public TypeTransactionList TypeTransactionList { get { return App.Instance.StaticServiceData.TypeTransactionList; } }
        public CategoryList CategoryList { get { return App.Instance.StaticServiceData.CategoryList; } }
        public TypeTransactionReasonList TransactionReasonTypeList { get { return App.Instance.StaticServiceData.TypeTransactionReasonList; } }


        public List<BasicItem> Months {get {return MonthList.GetMonths();}}
        public List<BasicItem> WeekDays { get { return WeekDayList.GetWeekDays(); } }
        public List<BasicItem> PosDayOfMonth { get { return PositionList.GetPositions(); } }
        public List<BasicItem> Position4 { get { return Position4List.GetPositions(); } }
        public List<BasicItem> PosMonthOfYear { get { return Position12List.GetPositions(); } }

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

        public bool IsChecked_RuleDailyEveryDays { get { return isChecked_RuleDailyEveryDays; } set { isChecked_RuleDailyEveryDays = value; RaisePropertyChanged("IsChecked_RuleDailyEveryDays"); } }
        public bool IsChecked_RuleWeeklyEveryWeek { get { return isChecked_RuleWeeklyEveryWeek; } set { isChecked_RuleWeeklyEveryWeek = value; RaisePropertyChanged("IsChecked_RuleWeeklyEveryWeek"); } }
        public bool IsChecked_RuleMonthlyDayNum { get { return isChecked_RuleMonthlyDayNum; } set { isChecked_RuleMonthlyDayNum = value; RaisePropertyChanged("IsChecked_RuleMonthlyDayNum"); } }
        public bool IsChecked_RuleMonthlyPrecise { get { return isChecked_RuleMonthlyPrecise; } set { isChecked_RuleMonthlyPrecise = value; RaisePropertyChanged("IsChecked_RuleMonthlyPrecise"); } }
        public bool IsChecked_RuleYearlyOnMonth { get { return isChecked_RuleYearlyOnMonth; } set { isChecked_RuleYearlyOnMonth = value; RaisePropertyChanged("IsChecked_RuleYearlyOnMonth"); } }
        public bool IsChecked_RuleYearlyOnTheWeekDay { get { return isChecked_RuleYearlyOnTheWeekDay; } set { isChecked_RuleYearlyOnTheWeekDay = value; RaisePropertyChanged("IsChecked_RuleYearlyOnTheWeekDay"); } }
        public bool IsChecked_RuleYearlyOnMonth2 { get { return isChecked_RuleYearlyOnMonth2; } set { isChecked_RuleYearlyOnMonth2 = value; RaisePropertyChanged("IsChecked_RuleYearlyOnMonth2"); } }
        public bool IsChecked_RuleRangeTotalOcurrences { get { return isChecked_RuleRangeTotalOcurrences; } set { isChecked_RuleRangeTotalOcurrences = value; RaisePropertyChanged("IsChecked_RuleRangeTotalOcurrences"); } }
        public bool IsChecked_RuleRangeEndBy { get { return isChecked_RuleRangeEndBy; } set { isChecked_RuleRangeEndBy = value; RaisePropertyChanged("IsChecked_RuleRangeEndBy"); } }
        public bool IsChecked_RuleRangeNoEndDate { get { return isChecked_RuleRangeNoEndDate; } set { isChecked_RuleRangeNoEndDate = value; RaisePropertyChanged("IsChecked_RuleRangeNoEndDate"); } }

        public bool IsToggled_tglDaily { get { return isToggled_tglDaily; } set { isToggled_tglDaily = value; RaisePropertyChanged("IsToggled_tglDaily"); } }
        public bool IsToggled_tglWeekly { get { return isToggled_tglWeekly; } set { isToggled_tglWeekly = value; RaisePropertyChanged("IsToggled_tglWeekly"); } }
        public bool IsToggled_tglMonthly { get { return isToggled_tglMonthly; } set { isToggled_tglMonthly = value; RaisePropertyChanged("IsToggled_tglMonthly"); } }
        public bool IsToggled_tglYearly { get { return isToggled_tglYearly; } set { isToggled_tglYearly = value; RaisePropertyChanged("IsToggled_tglYearly"); } }

        
        public string DailyEveryDay
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "DailyEveryDay").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "DailyEveryDay").Value = value; RaisePropertyChanged("DailyEveryDay"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now; }
        }

        public string DailyOnlyWeekdays
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "DailyOnlyWeekdays").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "DailyOnlyWeekdays").Value = value; RaisePropertyChanged("DailyOnlyWeekdays"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string WeeklyEveryWeek
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "WeeklyEveryWeek").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "WeeklyEveryWeek").Value = value; RaisePropertyChanged("WeeklyEveryWeek"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string WeeklyDayName
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "WeeklyDayName").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "WeeklyDayName").Value = value; RaisePropertyChanged("WeeklyDayName"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string MonthlyDayNumber
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyDayNumber").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyDayNumber").Value = value; RaisePropertyChanged("MonthlyDayNumber"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string MonthlyEveryMonth
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyEveryMonth").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyEveryMonth").Value = value; RaisePropertyChanged("MonthlyEveryMonth"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string MonthlyCountOfWeekDay
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyCountOfWeekDay").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyCountOfWeekDay").Value = value; RaisePropertyChanged("MonthlyCountOfWeekDay"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string MonthlyDayName
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyDayName").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyDayName").Value = value; RaisePropertyChanged("MonthlyDayName"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string MonthlyCountOfMonth
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyCountOfMonth").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyCountOfMonth").Value = value; RaisePropertyChanged("MonthlyCountOfMonth"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string MonthlyEveryMonth2
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyEveryMonth").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "MonthlyEveryMonth").Value = value; RaisePropertyChanged("MonthlyEveryMonth"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string YearlyEveryYear
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyEveryYear").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyEveryYear").Value = value; RaisePropertyChanged("YearlyEveryYear"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string YearlyOnDayPos
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyOnDayPos").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyOnDayPos").Value = value; RaisePropertyChanged("YearlyOnDayPos"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string YearlyMonthName
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyMonthName").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyMonthName").Value = value; RaisePropertyChanged("YearlyMonthName"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string YearlyPositions
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyPositions").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyPositions").Value = value; RaisePropertyChanged("YearlyPositions"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string YearlyDayName
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyDayName").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyDayName").Value = value; RaisePropertyChanged("YearlyDayName"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string YearlyMonthNameSec
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyMonthNameSec").Value : null; }
            set { CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "YearlyMonthNameSec").Value = value; RaisePropertyChanged("YearlyMonthNameSec"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string RangeTotalOcurrences
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "RangeTotalOcurrences").Value : null; }
            set { CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "RangeTotalOcurrences").Value = value; RaisePropertyChanged("RangeTotalOcurrences"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string RangeEndBy
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "RangeEndBy").Value : null; }
            set { CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "RangeEndBy").Value = value; RaisePropertyChanged("RangeEndBy"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }

        public string RangeStartDate
        {
            get { return CurrInterval != null ? CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "RangeStartDate").Value : null; }
            set { CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == "RangeStartDate").Value = value; RaisePropertyChanged("RangeStartDate"); CurrInterval.HasChanges = true; CurrInterval.ModifiedDate = DateTime.Now;}
        }


        #region Event To Commands
        public ICommand Intervals_SelectionChanged
        {
            get
            {
                return new RelayCommand<object>((param) =>
                {
                    var selectedItem = (SelectionChangedEventArgs)param;
                    if (selectedItem.AddedItems[0] != null)
                    {
                        var item = (TypeInterval)selectedItem.AddedItems[0];
                        CurrInterval = item;

                        Const.Rule recurrenceRule = Const.Rule.None;
                        if (item.RecurrenceRuleValue.RecurrenceRule != null)
                            recurrenceRule = (Const.Rule)Enum.Parse(typeof(Const.Rule), item.RecurrenceRuleValue.RecurrenceRule.Name);
                        switch (recurrenceRule)
                        {
                            case Const.Rule.RuleDailyEveryDays:
                                DailyEveryDay = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.DailyEveryDay.ToString()).Value;
                                DailyOnlyWeekdays = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.DailyOnlyWeekdays.ToString()).Value;
                                IsChecked_RuleDailyEveryDays = true;
                                
                                IsToggled_tglMonthly = false;
                                IsToggled_tglWeekly = false;
                                IsToggled_tglYearly = false;

                                IsToggled_tglDaily = true;
                                break;
                            case Const.Rule.RuleWeeklyEveryWeek:
                                WeeklyEveryWeek = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.WeeklyEveryWeek.ToString()).Value;
                                WeeklyDayName = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.WeeklyDayName.ToString()).Value;
                                IsChecked_RuleWeeklyEveryWeek = true;
                                
                                IsToggled_tglDaily = false;
                                IsToggled_tglMonthly = false;
                                IsToggled_tglYearly = false;

                                IsToggled_tglWeekly = true;
                                break;
                            case Const.Rule.RuleMonthlyDayNum:
                                MonthlyDayNumber = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyDayNumber.ToString()).Value;
                                MonthlyEveryMonth = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyEveryMonth.ToString()).Value;
                                IsChecked_RuleMonthlyDayNum = true;

                                IsToggled_tglDaily = false;
                                IsToggled_tglWeekly = false;
                                IsToggled_tglYearly = false;

                                IsToggled_tglMonthly = true;
                                break;
                            case Const.Rule.RuleMonthlyPrecise:
                                MonthlyCountOfWeekDay = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyCountOfWeekDay.ToString()).Value;
                                MonthlyDayName = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyDayName.ToString()).Value;
                                MonthlyCountOfMonth = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyCountOfMonth.ToString()).Value;
                                IsChecked_RuleMonthlyPrecise = true;
                                
                                IsToggled_tglDaily = false;
                                IsToggled_tglWeekly = false;
                                IsToggled_tglYearly = false;
                                
                                IsToggled_tglMonthly = true;
                                break;
                            case Const.Rule.RuleYearlyOnMonth:
                                YearlyEveryYear = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyEveryYear.ToString()).Value;
                                YearlyOnDayPos = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyOnDayPos.ToString()).Value;
                                YearlyMonthName = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyMonthName.ToString()).Value;
                                IsChecked_RuleYearlyOnMonth = true;
                                
                                IsToggled_tglDaily = false;
                                IsToggled_tglWeekly = false;
                                IsToggled_tglMonthly = false;
                                
                                IsToggled_tglYearly = true;
                                break;
                            case Const.Rule.RuleYearlyOnTheWeekDay:
                                YearlyEveryYear = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyEveryYear.ToString()).Value;
                                YearlyPositions = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyPositions.ToString()).Value;
                                YearlyDayName = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyDayName.ToString()).Value;
                                YearlyMonthNameSec = item.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyMonthNameSec.ToString()).Value;
                                IsChecked_RuleYearlyOnTheWeekDay = true;
                                
                                IsToggled_tglDaily = false;
                                IsToggled_tglWeekly = false;
                                IsToggled_tglMonthly = false;
                                
                                IsToggled_tglYearly = true;
                                break;
                            default:
                                break;
                        }

                        Const.Rule recurrenceRangeRule = Const.Rule.None;
                        if (item.RecurrenceRangeRuleValue.RecurrenceRule !=null)
                            recurrenceRangeRule = (Const.Rule)Enum.Parse(typeof(Const.Rule), item.RecurrenceRangeRuleValue.RecurrenceRule.Name);

                        switch (recurrenceRangeRule)
                        {
                            case Const.Rule.RuleRangeNoEndDate:
                                RangeStartDate = item.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeStartDate.ToString()).Value;
                                IsChecked_RuleRangeNoEndDate = true;
                                break;
                            case Const.Rule.RuleRangeTotalOcurrences:
                                RangeStartDate = item.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeStartDate.ToString()).Value;
                                RangeTotalOcurrences = item.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeTotalOcurrences.ToString()).Value;
                                IsChecked_RuleRangeTotalOcurrences = true;
                                break;
                            case Const.Rule.RuleRangeEndBy:
                                RangeStartDate = item.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeStartDate.ToString()).Value;
                                RangeEndBy = item.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeEndBy.ToString()).Value;
                                IsChecked_RuleRangeEndBy = true;
                                break;
                            default:
                                break;
                        }
                        PivotIndex = 0;
                    }
                });
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the IntervalViewModel class.
        /// </summary>
        public IntervalViewModel()
        {
            PivotIndex = 1;
            isEnabled = true;
        }
    }
}