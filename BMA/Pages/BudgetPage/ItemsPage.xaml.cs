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
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using System.ComponentModel;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace BMA.Pages.BudgetPage
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class ItemsPage : LayoutAwarePage
    {
        Budget currBudget;

        public ItemsPage()
        {
            App.Instance.TransDataSource.BudgetList.CollectionChanged += BudgetList_CollectionChanged;
            this.InitializeComponent();
        }

        void BudgetList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void currBudget_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
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

            frmBudgetDetail.Navigate(typeof(BudgetDetailFrame));

            this.DataContext = App.Instance.TransDataSource.BudgetList;
            itemsViewSource.Source = App.Instance.TransDataSource.BudgetList;

            itemGridView.ItemsSource = itemsViewSource.Source;            

            AppBarAddButton.IsEnabled = true;
            AppBarDoneButton.IsEnabled = false;
            AppBarCancelButton.IsEnabled = false;
        }

        private void DisplayData()
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            frmBudgetDetail.Navigate(typeof(BudgetDetailFrame), currBudget);
        }

        private async void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageDialog dialog = null;
            if (currBudget != null && currBudget.HasChanged)
            {
                dialog = new MessageDialog("The are changes. Please save the first.");
                //await dialog.ShowAsync();
                //return;
            }
            currBudget = e.ClickedItem as Budget;
            currBudget.PropertyChanged += new PropertyChangedEventHandler(currBudget_PropertyChanged);

            DisplayData();
        }

        private void EnableAppBarStatus(bool status)
        {
            AppBarDoneButton.IsEnabled = status;
            AppBarCancelButton.IsEnabled = status;
        }

        private async void Transaction_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var budget = (frmBudgetDetail.Content as BudgetDetailFrame).DataContext as Budget;

            int budgetId = budget != null ? budget.BudgetId : 0;  
            await App.Instance.TransDataSource.LoadTransactionsForBudget(budgetId);

            Frame.Navigate(typeof(TransactionItemsPage), budget);
        }

        private void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var tempBudget = (frmBudgetDetail.Content as BudgetDetailFrame);
            tempBudget.UpdateLayout();

            AppBarAddButton.IsEnabled = true;
            //AppBarDoneButton.IsEnabled = false;
            AppBarCancelButton.IsEnabled = false;
        }

        private void Cancel_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            DisplayData();

            AppBarAddButton.IsEnabled = true;
            AppBarDoneButton.IsEnabled = false;
            AppBarCancelButton.IsEnabled = false;
        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            currBudget = NewBudget();
            ((ObservableCollection<Budget>)DataContext).Add(currBudget);

            DisplayData();

            AppBarAddButton.IsEnabled = false;
            AppBarDoneButton.IsEnabled = true;
            AppBarCancelButton.IsEnabled = true;
        }

        private void Home_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            //MainPage.Navigate(typeof(MainPage));
        }

        private Budget NewBudget()
        {
            currBudget = new Budget();

            currBudget.PropertyChanged += new PropertyChangedEventHandler(currBudget_PropertyChanged);

            return currBudget;
        }

    }
}
