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

namespace BMA.Pages.Reports.TransactionAmount
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
            var item = navigationParameter as Tuple<string, double, DateTime, DateTime>;

            if (item == null)
                return;

            var queryExp = App.Instance.TransDataSource.TransactionList.Where(i => i.TransactionType.Name == "Expense" &&
                                                                                i.CreatedDate >= item.Item3 &&
                                                                                i.CreatedDate <= item.Item4);

            var queryInc = App.Instance.TransDataSource.TransactionList.Where(i => i.TransactionType.Name == "Income" &&
                                                                                i.CreatedDate >= item.Item3 &&
                                                                                i.CreatedDate <= item.Item4);

            switch (item.Item1)
            {
                case ">":
                    queryExp = from i in queryExp where i.Amount > item.Item2 select i;
                    queryInc = from i in queryInc where i.Amount > item.Item2 select i;
                    break;
                case "=":
                    queryExp = from i in queryExp where i.Amount == item.Item2 select i;
                    queryInc = from i in queryInc where i.Amount == item.Item2 select i;
                    break;
                case "<":
                    queryExp = from i in queryExp where i.Amount < item.Item2 select i;
                    queryInc = from i in queryInc where i.Amount < item.Item2 select i;
                    break;
                default:
                    break;
            }

            DataVirtualization.Toolkit.DataItemCollection collection = new DataVirtualization.Toolkit.DataItemCollection();

            collection.Add(new DataVirtualization.Toolkit.DataItem()
            {
                DataSymbol = "Euro",
                ID = 1,
                Name = "Expense",
                Value = queryExp.Sum(i => i.Amount)
            });
            
            collection.Add(new DataVirtualization.Toolkit.DataItem()
            {
                DataSymbol = "Euro",
                ID = 2,
                Name = "Income",
                Value = queryInc.Sum(i=>i.Amount)
            });

            chart.ItemsSource = collection;

            chart.ItemSelected += chart_ItemSelected;
            chart.ItemDeSelected += chart_ItemDeSelected;

            chart.legend.Foreground = new SolidColorBrush(new Windows.UI.Color(){A=255, R=0, G=0, B=0});
            this.UpdateLayout();
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
