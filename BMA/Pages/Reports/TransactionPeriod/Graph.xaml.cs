using BMA.BusinessLogic;
using BMA.Common;
using ModernUI.Toolkit.Data.Charting.Charts.Series;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BMA.Pages.Reports.TransactionPeriod
{
    public class NameValueItem
    {
        private string _groupDate;
        private int _count;
        private double _sumAmount;

        public string GroupDate
        {
            get;
            set;
        }

        public int Count
        {
            get;
            set;
        }

        public double SumAmount
        {
            get;
            set;
        }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Graph : LayoutAwarePage
    {

        List<NameValueItem> groupedExp = new List<NameValueItem>();
        List<NameValueItem> groupedInc = new List<NameValueItem>();

        public Graph()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            var item = navigationParameter as Tuple<string, DateTime, DateTime>;

            if (item == null)
                return;

            var queryExp = App.Instance.TransDataSource.TransactionList.Where(i => i.TransactionType.Name == "Expense" &&
                                                                                i.CreatedDate >= item.Item2 &&
                                                                                i.CreatedDate <= item.Item3);

            var queryInc = App.Instance.TransDataSource.TransactionList.Where(i => i.TransactionType.Name == "Income" &&
                                                                                i.CreatedDate >= item.Item2 &&
                                                                                i.CreatedDate <= item.Item3);

            switch (item.Item1)
            {
                case "Year":
                    groupedExp = GroupedResultYearly(queryExp.ToList());
                    groupedInc = GroupedResultYearly(queryInc.ToList());
                    break;
                case "Month":
                    groupedExp = GroupedResultMonthly(queryExp.ToList());
                    groupedInc = GroupedResultMonthly(queryInc.ToList());
                    break;
                case "Week":
                    groupedExp = GroupedResultWeekly(queryExp.ToList());
                    groupedInc = GroupedResultWeekly(queryInc.ToList());
                    break;
                default:
                    break;
            }

            ((ColumnSeries)Chart.Series[0]).ItemsSource = groupedInc;
            ((ColumnSeries)Chart.Series[1]).ItemsSource = groupedExp;
        }

        private List<NameValueItem> GroupedResultWeekly(List<Transaction> transactions)
        {
            List<NameValueItem> result = null;
            int weekOfYear = 0;
            int daysInYear = 0;

            daysInYear = DateTime.IsLeapYear(DateTime.Now.Year) ? 365 : 364;
            weekOfYear = int.Parse(Math.Round((double)(DateTime.Now.DayOfYear * (daysInYear / 7) / daysInYear), 0).ToString());

            var query = from p in transactions
                        group p by
                        new
                        {
                            week = int.Parse(Math.Round((double)(p.CreatedDate.DayOfYear * (daysInYear / 7) / (DateTime.IsLeapYear(DateTime.Now.Year) ? 365 : 364)), 0).ToString()),
                            year = p.CreatedDate.Year
                        } into d
                        select new NameValueItem() { GroupDate = string.Format("Week {0} of Year {1}", d.Key.week, d.Key.year), Count = d.Count(), SumAmount = d.Sum(p => p.Amount) };

            result = query.ToList();

            return result;
        }

        private List<NameValueItem> GroupedResultMonthly(List<Transaction> transactions)
        {
            List<NameValueItem> result = null;
            List<string> monthList = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12).ToList();

            var query = from p in transactions
                        group p by new { month = p.CreatedDate.Month, year = p.CreatedDate.Year } into d
                        select new NameValueItem() { GroupDate = string.Format("{0} {1}", monthList[d.Key.month - 1], d.Key.year), Count = d.Count(), SumAmount = d.Sum(p => p.Amount) };

            result = query.ToList();

            return result;
        }

        private List<NameValueItem> GroupedResultYearly(List<Transaction> transactions)
        {
            List<NameValueItem> result = null;

            var query = from p in transactions
                        group p by new { year = p.CreatedDate.Year } into d
                        select new NameValueItem() { GroupDate = string.Format("Year {0}", d.Key.year), Count = d.Count(), SumAmount = d.Sum(p => p.Amount) };

            result = query.ToList();

            return result;
        }
    }
}
