using BMA.BusinessLogic;
using BMA.DataModel;
using BMA.Pages.BudgetPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace BMA.Pages.TransactionPage
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class GroupedItemsPage : BMA.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
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
            App.Instance.Share = null;
            EnableAppBarStatus(false);

            TransDetailFrame.Navigate(typeof(TransDetailFrame));

            this.DataContext = App.Instance.TransDataSource.TransactionList;
            groupedItemsViewSource.Source = App.Instance.TransDataSource.TransactionList;
            
            groupGridView.ItemsSource = groupedItemsViewSource.View.CollectionGroups;
        }

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        private void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;
            var data = frameworkElement.DataContext;
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DisplayData(e.ClickedItem as Transaction);
        }

        private void DisplayData(Transaction transaction)
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            TransDetailFrame.Navigate(typeof(TransDetailFrame), transaction);
        }
        private void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            //App.Instance.TransDataSource.TransactionList.Add(new Category());
            //DefaultViewModel["Transactions"] = App.Instance.TransDataSource.TransactionList;

            ((ObservableCollection<Category>)DataContext).Add(
                ((ObservableCollection<Category>)DataContext)[0]);


            EnableAppBarStatus(false);

        }

        private void Cancel_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            DisplayData(null);
            EnableAppBarStatus(false);
        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            EnableAppBarStatus(false);
        }
        private void EnableAppBarStatus(bool status)
        {
            AppBarDoneButton.IsEnabled = status;
            AppBarCancelButton.IsEnabled = status;
            AppBarAddButton.IsEnabled = status;
        }

        private void Budget_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ItemsPage));
        }

        private void Home_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            TransDetailFrame.Navigate(typeof(MainPage));
        }
    }
}
