using BMA.BusinessLogic;
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

namespace BMA.Pages.AdminPage
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class TypeTransactionItemsPage : BMA.Common.LayoutAwarePage
    {
        #region Private Members
        TypeTransaction currTypeTransaction;
        List<TypeTransaction> originalTypeTransactionList;
        bool isDirty;
        #endregion

        #region Constructor
        public TypeTransactionItemsPage()
        {
            App.Instance.StaticDataSource.TypeTransactionList.CollectionChanged += TypeTransactionList_CollectionChanged;
            this.InitializeComponent();
        }
        #endregion

        #region Private Methods
        private TypeTransaction NewTypeTransaction()
        {
            currTypeTransaction = new TypeTransaction();

            currTypeTransaction.PropertyChanged += new PropertyChangedEventHandler(currTypeTransaction_PropertyChanged);

            return currTypeTransaction;
        }

        private void DisplayData()
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            frmTypeTransactionDetail.Navigate(typeof(TypeTransactionDetailFrame), currTypeTransaction);
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
            originalTypeTransactionList = App.Instance.StaticDataSource.TypeTransactionList.ToList();
        }

        private void RevertCurrentList()
        {
            App.Instance.StaticDataSource.TypeTransactionList.Clear();
            foreach (var item in originalTypeTransactionList)
                App.Instance.StaticDataSource.TypeTransactionList.Add(item);
        }

        private void DisplayData(TypeTransaction typeTransaction)
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            frmTypeTransactionDetail.Navigate(typeof(TypeTransactionDetailFrame), typeTransaction);
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

            frmTypeTransactionDetail.Navigate(typeof(TypeTransactionDetailFrame));

            SyncLists();

            this.DataContext = App.Instance.StaticDataSource.TypeTransactionList;
            itemsViewSource.Source = this.DataContext;

            itemGridView.ItemsSource = itemsViewSource.Source;

            EnableAppBarStatus(false);
        }

        private async void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageDialog dialog = null;
            if (currTypeTransaction != null && currTypeTransaction.HasChanges)
            {
                dialog = new MessageDialog("The are changes. Please save the first.");
                //await dialog.ShowAsync();
                //return;
            }
            currTypeTransaction = e.ClickedItem as TypeTransaction;
            currTypeTransaction.PropertyChanged += new PropertyChangedEventHandler(currTypeTransaction_PropertyChanged);

            DisplayData();
        }

        void TypeTransactionList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            isDirty = true;
        }

        void currTypeTransaction_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            isDirty = true;
        }

        private async void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var tempTypeTransaction = (frmTypeTransactionDetail.Content as NotificationDetailFrame);
            tempTypeTransaction.UpdateLayout();

            EnableAppBarStatus(false);

            var saveOC = App.Instance.StaticDataSource.TypeTransactionList.Where(t => t.HasChanges).ToObservableCollection();

           await App.Instance.StaticDataSource.SaveTypeTransaction(saveOC);

            SyncLists();
        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            currTypeTransaction = NewTypeTransaction();
            ((ObservableCollection<TypeTransaction>)DataContext).Add(currTypeTransaction);

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
            currTypeTransaction.IsDeleted = true;

            ((ObservableCollection<TypeTransaction>)DataContext).Remove(currTypeTransaction);

            DisplayData();

            EnableAppBarStatus(true);
        }

        #endregion
    }
}
