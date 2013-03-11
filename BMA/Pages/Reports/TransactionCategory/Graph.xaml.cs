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

namespace BMA.Pages.Reports.TransactionCategory
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
            var item = navigationParameter as Tuple<int, DateTime, DateTime>;

            if (item == null)
                return;

            var queryExp = App.Instance.TransDataSource.TransactionList.Where(i => i.TransactionType.Name == "Expense" &&
                                                                                i.Category.CategoryId == item.Item1 &&
                                                                                i.CreatedDate >= item.Item2 &&
                                                                                i.CreatedDate <= item.Item3);

            var queryInc = App.Instance.TransDataSource.TransactionList.Where(i => i.TransactionType.Name == "Income" &&
                                                                                i.Category.CategoryId == item.Item1 &&
                                                                                i.CreatedDate >= item.Item2 &&
                                                                                i.CreatedDate <= item.Item3);

            groupedExp = GroupedResult(queryExp.ToList());
            groupedInc = GroupedResult(queryInc.ToList());

            ((ColumnSeries)Chart.Series[0]).ItemsSource = groupedInc;
            ((ColumnSeries)Chart.Series[1]).ItemsSource = groupedExp;
        }

        private List<NameValueItem> GroupedResult(List<Transaction> transactions)
        {
            List<NameValueItem> result = null;
            
            var query = from p in transactions
                        select new NameValueItem() { GroupDate = string.Format("{0:dd/MM/yyyy}", p.CreatedDate ), Count = 1, SumAmount = p.Amount };

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
