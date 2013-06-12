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

namespace BMA.Pages.Reports.TransactionAmount
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
                    groupedExp = GroupedResultGreaterThan(queryExp.ToList(), item.Item2);
                    groupedInc = GroupedResultGreaterThan(queryInc.ToList(), item.Item2);
                    break;
                case "=":
                    groupedExp = GroupedResultEquals(queryExp.ToList(), item.Item2);
                    groupedInc = GroupedResultEquals(queryInc.ToList(), item.Item2);
                    break;
                case "<":
                    groupedExp = GroupedResultLessThan(queryExp.ToList(), item.Item2);
                    groupedInc = GroupedResultLessThan(queryInc.ToList(), item.Item2);
                    break;
                default:
                    break;
            }

            ((ColumnSeries)Chart.Series[0]).ItemsSource = groupedInc;
            ((ColumnSeries)Chart.Series[1]).ItemsSource = groupedExp;
        }

        private List<NameValueItem> GroupedResultLessThan(List<Transaction> transactions, double amount)
        {
            List<NameValueItem> result = null;
            
            var query = from p in transactions
                        where p.Amount < amount
                        select new NameValueItem() { GroupDate = string.Format("{0:dd/MM/yyyy}({1})", p.CreatedDate, p.Amount), Count = 1, SumAmount = p.Amount };

            result = query.ToList();

            return result;
        }

        private List<NameValueItem> GroupedResultEquals(List<Transaction> transactions, double amount)
        {
            List<NameValueItem> result = null;

            var query = from p in transactions
                        where p.Amount == amount
                        select new NameValueItem() { GroupDate = p.CreatedDate.ToString("dd/MM/yy"), Count = 1, SumAmount = p.Amount };

            result = query.ToList();

            return result;
        }

        private List<NameValueItem> GroupedResultGreaterThan(List<Transaction> transactions, double amount)
        {
            List<NameValueItem> result = null;

            var query = from p in transactions
                        where p.Amount > amount
                        select new NameValueItem() { GroupDate = string.Format("{0:dd/MM/yyyy}({1})", p.CreatedDate, p.Amount), Count = 1, SumAmount = p.Amount };

            result = query.ToList();

            return result;
        }
    }
}
