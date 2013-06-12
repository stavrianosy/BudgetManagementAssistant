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

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace BMA.Pages.Reports
{
    class ReportName
    {
        public string Name { get; set; }        
    }
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class ReportList : BMA.Common.LayoutAwarePage
    {
        public ReportList()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            List<ReportName> reports = new List<ReportName>();
            reports.Add(new ReportName() { Name = "Transaction Periods" });
            reports.Add(new ReportName() { Name = "Transaction Categories" });
            reports.Add(new ReportName() { Name = "Transaction Budgets" });
            reports.Add(new ReportName() { Name = "Transaction Amounts" });
            reports.Add(new ReportName() { Name = "Transaction Places" });

            DefaultViewModel["RepostList"] = reports;
            //this.DataContext = str;
        }
        private void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            

        }

        private void ItemGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListView).SelectedValue as ReportName;

            switch (item.Name)
            {
                case "Transaction Periods":
                    frmReport.Navigate(typeof(TransactionPeriod.Criteria));
                    break;
                case "Transaction Categories":
                    frmReport.Navigate(typeof(TransactionCategory.Criteria));
                    break;
                case "Transaction Budgets":
                    frmReport.Navigate(typeof(TransactionBudget.Criteria));
                    break;
                case "Transaction Amounts":
                    frmReport.Navigate(typeof(TransactionAmount.Criteria));
                    break;
                case "Transaction Places":
                    frmReport.Navigate(typeof(TransactionPlace.Criteria));
                    break;
            }
        }
    }
}
