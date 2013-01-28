using BMA.BusinessLogic;
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

namespace BMA.Pages.AdminPage
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class SecurityItemsPage : BMA.Common.LayoutAwarePage
    {
        public SecurityItemsPage()
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

            SecurityDetailFrame.Navigate(typeof(SecurityDetailFrame));

            //DefaultViewModel["Securitys"] = App.Instance.TransDataSource.SecurityList;

            //itemsViewSource.Source = App.Instance.TransDataSource.SecurityList;

            itemGridView.ItemsSource = itemsViewSource.Source;
        }

        private void DisplayData(Security security)
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            SecurityDetailFrame.Navigate(typeof(SecurityDetailFrame), security);
        }

        private void EnableAppBarStatus(bool status)
        {
            AppBarDoneButton.IsEnabled = status;
            AppBarCancelButton.IsEnabled = status;
            AppBarAddButton.IsEnabled = status;
        }

        private void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_AppBarButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
