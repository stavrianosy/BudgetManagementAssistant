using BMA.BusinessLogic;
using BMA.Common;
using BMA.Pages.BudgetPage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace BMA.Pages.TransactionPage
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class TransactionItemsPage : LayoutAwarePage
    {
        #region Private Members
        bool isDirty;
        Transaction currTransaction;
        List<Transaction> originalTransactionList;        
        #endregion

        #region Constructor
        public TransactionItemsPage()
        {
            App.Instance.TransDataSource.TransactionList.CollectionChanged += TransactionList_CollectionChanged;
            this.InitializeComponent();
        }
        #endregion

        #region Private Methods
        private Transaction NewTransaction()
        {
            currTransaction = new Transaction
            {
                Category = App.Instance.StaticDataSource.CategoryList.Where(c => c.FromDate.Hour <= DateTime.Now.Hour && c.ToDate.Hour >= DateTime.Now.Hour).FirstOrDefault(),
                TransactionType = App.Instance.StaticDataSource.TypeTransactionList.Single(t => t.Name == "Expense")                
            };

            currTransaction.PropertyChanged += new PropertyChangedEventHandler(currTransaction_PropertyChanged);

            return currTransaction;
        }

        private void DisplayData()
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            frmTransDetail.Navigate(typeof(TransDetailFrame), currTransaction);
        }

        private void EnableAppBarStatus(bool status)
        {
            AppBarDoneButton.IsEnabled = status;
            AppBarCancelButton.IsEnabled = status;
            AppBarDeleteButton.IsEnabled = status;
            
            AppBarAddButton.IsEnabled = !status;
        }

        private void SyncLists()
        {
            originalTransactionList = App.Instance.TransDataSource.TransactionList.ToList();
        }

        private void RevertCurrentList()
        {
            App.Instance.TransDataSource.TransactionList.Clear();
            foreach (var item in originalTransactionList)
                App.Instance.TransDataSource.TransactionList.Add(item);
        }

        #endregion

        #region Events
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

            frmTransDetail.Navigate(typeof(TransDetailFrame));

            SyncLists();

            this.DataContext = App.Instance.TransDataSource.TransactionList;
            itemsViewSource.Source = App.Instance.TransDataSource.TransactionList;

            itemGridView.ItemsSource = itemsViewSource.Source;

            EnableAppBarStatus(false);

            if (navigationParameter is Budget)
                txtBudget.Text = string.Format("{0} {1}", "for budget", (navigationParameter as Budget).Name);
        }

        void currTransaction_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            isDirty = true;
            EnableAppBarStatus(true);
        }

        void TransactionList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            isDirty = true;
            //EnableAppBarStatus(true);
        }

        private async void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageDialog dialog = null;
            if (currTransaction != null && currTransaction.HasChanged)
            {
                dialog = new MessageDialog("The are changes. Please save the first.");
                //await dialog.ShowAsync();
                //return;
            }
            currTransaction = e.ClickedItem as Transaction;
            currTransaction.PropertyChanged += new PropertyChangedEventHandler(currTransaction_PropertyChanged);

            DisplayData();

            currTransaction.HasChanged = false;
            isDirty = false;
        }

        private async void Budget_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            await App.Instance.TransDataSource.LoadAllBudgets();
            Frame.Navigate(typeof(ItemsPage));
        }

        private async void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var tempTrans = (frmTransDetail.Content as TransDetailFrame);
            tempTrans.UpdateLayout();

            EnableAppBarStatus(false);

            var saveOC = new ObservableCollection<Transaction>();
            foreach (var trans in App.Instance.TransDataSource.TransactionList.Where(t => t.HasChanged))
                saveOC.Add(trans);

            await App.Instance.TransDataSource.SaveTransaction(saveOC);

            SyncLists();
        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            currTransaction = NewTransaction();
            ((ObservableCollection<Transaction>)DataContext).Add(currTransaction);

            DisplayData();

            EnableAppBarStatus(true);
        }

        private void Cancel_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            DisplayData();

            EnableAppBarStatus(false);

            RevertCurrentList();
        }

        private void Delete_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            ((ObservableCollection<Transaction>)DataContext).Remove(currTransaction);

            DisplayData();

            EnableAppBarStatus(true);
        }
        
        #endregion

    }
}
