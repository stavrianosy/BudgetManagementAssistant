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

using BMA.Common;
using BMA.BusinessLogic;
using BMA.Pages.TransactionPage;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace BMA.Pages.BudgetPage
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class ItemsPage : LayoutAwarePage
    {
        public ItemsPage()
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
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]
            App.Instance.Share = null;
            EnableAppBarStatus(false);

            //DefaultViewModel["Budgets"] = App.Instance.TransDataSource.BudgetList;

            BudgetDetailFrame.Navigate(typeof(BudgetDetailFrame));

            DefaultViewModel["Budgets"] = App.Instance.TransDataSource.BudgetList;
            //this.DataContext = App.Instance.TransDataSource.BudgetList;
            itemsViewSource.Source = App.Instance.TransDataSource.BudgetList;

            itemGridView.ItemsSource = itemsViewSource.Source;
        }

        private void DisplayData(Budget budget)
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            BudgetDetailFrame.Navigate(typeof(BudgetDetailFrame), budget);
        }

        private void EnableAppBarStatus(bool status)
        {
            AppBarDoneButton.IsEnabled = status;
            AppBarCancelButton.IsEnabled = status;
            AppBarAddButton.IsEnabled = status;
        }

        private void Transaction_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupedItemsPage));
        }

        private void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_AppBarButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DisplayData(e.ClickedItem as Budget);
        }

        private void Home_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            //MainPage.Navigate(typeof(MainPage));
        }

    }
}
