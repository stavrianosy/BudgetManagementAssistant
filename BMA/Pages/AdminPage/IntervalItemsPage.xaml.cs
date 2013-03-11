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

using BMA.BusinessLogic;
using BMA.Common;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace BMA.Pages.AdminPage
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class IntervalItemsPage : LayoutAwarePage
    {
        public IntervalItemsPage()
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

            frmInterval.Navigate(typeof(IntervalDetailFrame));

            //DefaultViewModel["PeriodicInOuts"] = App.Instance.TransDataSource.PeriodicInOutList;

            //itemsViewSource.Source = App.Instance.TransDataSource.PeriodicInOutList;

            itemGridView.ItemsSource = itemsViewSource.Source;
        }

        private void DisplayData(TypeInterval typeInterval)
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            frmInterval.Navigate(typeof(IntervalDetailFrame), typeInterval);
        }

        private void EnableAppBarStatus(bool status)
        {
            AppBarDoneButton.IsEnabled = status;
            AppBarCancelButton.IsEnabled = status;
            AppBarAddButton.IsEnabled = status;
        }

        #region Events

        private async void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            //var tempTrans = (frmTransDetail.Content as TransDetailFrame);
            //tempTrans.UpdateLayout();

            //EnableAppBarStatus(false);

            //var saveOC = App.Instance.TransDataSource.BudgetList.Where(t => t.HasChanges).ToObservableCollection();
            //foreach (var trans in App.Instance.TransDataSource.TransactionList.Where(t => t.HasChanged))
            //    saveOC.Add(trans);

            //await App.Instance.TransDataSource.SaveTransaction(saveOC);

            //SyncLists();
        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            //currTransaction = NewTransaction();
            //((ObservableCollection<Transaction>)DataContext).Add(currTransaction);

            //DisplayData();

            //EnableAppBarStatus(true);
        }

        private void Cancel_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            //DisplayData();

            //EnableAppBarStatus(false);

            //RevertCurrentList();
        }

        private void Delete_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            //curr.IsDeleted = true;

            //((ObservableCollection<Category>)DataContext).Remove(currCategory);

            //DisplayData();

            //EnableAppBarStatus(true);
        }

        #endregion
    }
}
