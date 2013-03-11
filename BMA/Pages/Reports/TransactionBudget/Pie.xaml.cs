using BMA.Common;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Pie : LayoutAwarePage
    {
        public Pie()
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

            List<NameValueItem> groupedExp = new List<NameValueItem>();
            List<NameValueItem> groupedInc = new List<NameValueItem>();
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

            DataVirtualization.Toolkit.DataItemCollection collection = new DataVirtualization.Toolkit.DataItemCollection();


            collection.Add(new DataVirtualization.Toolkit.DataItem()
            {
                DataSymbol = "Euro",
                ID = 1,
                Name = "Expense",
                Value = groupedExp.Sum(i => i.SumAmount)
            });
            
            collection.Add(new DataVirtualization.Toolkit.DataItem()
            {
                DataSymbol = "Euro",
                ID = 2,
                Name = "Income",
                Value = groupedInc.Sum(i => i.SumAmount)
            });

            chart.ItemsSource = collection;

            chart.ItemSelected += chart_ItemSelected;
            chart.ItemDeSelected += chart_ItemDeSelected;
        }

        void chart_ItemDeSelected(object sender, DataVirtualization.Toolkit.DataItem e)
        {
            //txtPageHeader.Text = string.Format("{0} Has been Selected with Value {1}", e.Name, e.Value.ToString());

        }

        void chart_ItemSelected(object sender, DataVirtualization.Toolkit.DataItem e)
        {
            //txtPageHeader.Text = string.Format("{0} Has been Selected with Value {1}", e.Name, e.Value.ToString());
        }
    }
}
