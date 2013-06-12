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

namespace BMA.Pages.Reports.TransactionBudget
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
            var item = navigationParameter as Tuple<List<int>>;

            if (item == null)
                return;

            var budgets = from i in App.Instance.TransDataSource.BudgetList
                          where item.Item1.Contains(i.BudgetId)
                          select i;

            foreach (var budget in budgets)
            {
                var queryExp = (from i in budget.Transactions
                                where i.TransactionType.Name == "Expense"
                                group i by new { budget.Name } into d
                                select new NameValueItem() { GroupDate = budget.Name, Count = d.Count(), SumAmount = d.Sum(k => k.Amount) }).ToList();

                var queryInc = (from i in budget.Transactions
                                where i.TransactionType.Name == "Income"
                                group i by new { budget.Name } into d
                                select new NameValueItem() { GroupDate = budget.Name, Count = d.Count(), SumAmount = d.Sum(k => k.Amount) }).ToList();

                groupedExp.AddRange(queryExp);
                groupedInc.AddRange(queryInc);
            }


            ((ColumnSeries)Chart.Series[0]).ItemsSource = groupedInc;
            ((ColumnSeries)Chart.Series[1]).ItemsSource = groupedExp;
        }
    }
}
