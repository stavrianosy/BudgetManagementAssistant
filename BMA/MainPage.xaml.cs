using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using BMA.BusinessLogic;

using BMA.Pages.TransactionPage;
using BMA.Pages.BudgetPage;
using BMA.Pages.AdminPage;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BMA
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : BMA.Common.LayoutAwarePage
    {
        public MainPage()
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
            DefaultViewModel["LoadCounts"] = App.Instance.TransDataSource.LoadCounts;
            //groupGridView.DataContext = groupedItemsViewSource.View.CollectionGroups;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void brdTransactions_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ProgressText.Text = ApplicationData.Current.LocalSettings.Values.ContainsKey("Initialized")
                   && (bool)ApplicationData.Current.LocalSettings.Values["Initialized"]
                                       ? "Loading transactions..."
                                       : "Initializing: this may take several minutes...";

            // Determine what group the Button instance represents
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;
            var group = frameworkElement.DataContext;

            await App.Instance.TransDataSource.LoadTransactions();

            Progress.IsActive = true;
            Progress.IsEnabled = true;

            foreach (var trans in App.Instance.TransDataSource.TransactionList)
            {
                //Progress.IsActive = true;
                //ProgressText.Text = "Loading transactions " + trans.TypeTransaction;
                //await App.Instance.DataSource.LoadAllItems(trans);
            }

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            Frame.Navigate(typeof(GroupedItemsPage));
        }

        private async void brdBudgets_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            ProgressText.Text = ApplicationData.Current.LocalSettings.Values.ContainsKey("Initialized")
                    && (bool)ApplicationData.Current.LocalSettings.Values["Initialized"]
                                        ? "Loading transactions..."
                                        : "Initializing: this may take several minutes...";

            // Determine what group the Button instance represents
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;
            var group = frameworkElement.DataContext;

            await App.Instance.TransDataSource.LoadAllBudgets();

            Progress.IsActive = true;
            Progress.IsEnabled = true;

            foreach (var trans in App.Instance.TransDataSource.BudgetList)
            {
                //Progress.IsActive = true;
                //ProgressText.Text = "Loading transactions " + trans.TypeTransaction;
                //await App.Instance.DataSource.LoadAllItems(trans);
            }

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            Frame.Navigate(typeof(ItemsPage));
        }

        private void brdTargets_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;
            var group = frameworkElement.DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            Frame.Navigate(typeof(GroupDetailPage), ((Transaction)group).TransactionId);
        }

        private void PeriodicInOut_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PeriodicInOutItemsPage));
        }

        private void Notifications_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(NotificationItemsPage));
        }

        private void Security_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SecurityItemsPage));

        }

        private void ExpenseReason_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExpenseItemsPage));
        }

        private void Categories_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CategoryItemsPage));
        }

    }
}
