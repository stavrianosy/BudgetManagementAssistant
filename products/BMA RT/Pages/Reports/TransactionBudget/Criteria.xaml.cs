using BMA.BusinessLogic;
using BMA.Common;
using ModernUI.Toolkit.Data.Charting.Charts.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class BudgetMulti
    {
        public int BudgetId { get; set; }
        public string BudgetName { get; set; }
        public bool IsSelected { get; set; }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class Criteria : LayoutAwarePage
    {
        DateTime dateFrom = new DateTime();
        DateTime dateTo = new DateTime();



        public Criteria()
        {
            this.InitializeComponent();
        }

        #region Events
        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            List<BudgetMulti> budgets = new List<BudgetMulti>();
            foreach (var b in App.Instance.TransDataSource.BudgetList)
                budgets.Add(new BudgetMulti() {BudgetId=b.BudgetId, BudgetName=b.Name, IsSelected = false });

            DefaultViewModel["Budgets"] = budgets;

            this.UpdateLayout();

            if (budgets.Count() > 0)
                cbBudget.SelectedIndex = 0;
        }

        private void cbBudgets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbBudget_Unchecked(object sender, EventArgs e)
        {
        }

        private void cbBudget_Checked(object sender, EventArgs e)
        {
        }

        private void brdViewReport_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var budgets = (cbBudget.ItemsSource as List<BudgetMulti>).Where(i=>i.IsSelected);

            List<int> ids = new List<int>();

            foreach (var d in budgets)
                ids.Add(d.BudgetId);

            Tuple<List<int>> item = new Tuple<List<int>>(ids);

            frmGraph.Navigate(typeof(Graph), item);

        }

        private void brdViewReportPie_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var budgets = (cbBudget.ItemsSource as List<BudgetMulti>).Where(i => i.IsSelected);

            List<int> ids = new List<int>();

            foreach (var d in budgets)
                ids.Add(d.BudgetId);

            Tuple<List<int>> item = new Tuple<List<int>>(ids);

            frmGraph.Navigate(typeof(Pie), item);
        }
        #endregion

        #region Private Methods
        #endregion
    }
}
