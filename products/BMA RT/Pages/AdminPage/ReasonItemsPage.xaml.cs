using BMA.BusinessLogic;
using BMA.Common;
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
    public sealed partial class ReasonItemsPage : BMA.Common.LayoutAwarePage
    {
        #region Private Members
        TypeTransactionReason currTypeTransReason;
        List<TypeTransactionReason> originalTypeTransReasonList;
        bool isDirty;
        #endregion

        #region Constructor
        public ReasonItemsPage()
        {
            App.Instance.StaticDataSource.TypeTransactionReasonList.CollectionChanged += TypeTransactionReasonList_CollectionChanged;

            this.InitializeComponent();
        }
        #endregion

        #region Private Methods
        private TypeTransactionReason NewTypeTransactionReason()
        {
            currTypeTransReason = new TypeTransactionReason();

            currTypeTransReason.PropertyChanged += new PropertyChangedEventHandler(currTypeTransReason_PropertyChanged);

            return currTypeTransReason;
        }

        private void DisplayData()
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            frmReason.Navigate(typeof(ReasonDetailFrame), currTypeTransReason);
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
            originalTypeTransReasonList = App.Instance.StaticDataSource.TypeTransactionReasonList.ToList();
        }

        private void RevertCurrentList()
        {
            App.Instance.StaticDataSource.TypeTransactionReasonList.Clear();
            foreach (var item in originalTypeTransReasonList)
                App.Instance.StaticDataSource.TypeTransactionReasonList.Add(item);
        }

        private void DisplayData(TypeTransactionReason expense)
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            frmReason.Navigate(typeof(ReasonDetailFrame), expense);
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

            frmReason.Navigate(typeof(ReasonDetailFrame));

            SyncLists();

            this.DataContext = App.Instance.StaticDataSource.TypeTransactionReasonList;
            itemsViewSource.Source = this.DataContext;

            itemGridView.ItemsSource = itemsViewSource.Source;

            EnableAppBarStatus(false);
        }

        private async void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageDialog dialog = null;
            if (currTypeTransReason != null && currTypeTransReason.HasChanges)
            {
                dialog = new MessageDialog("The are changes. Please save the first.");
                //await dialog.ShowAsync();
                //return;
            }
            currTypeTransReason = e.ClickedItem as TypeTransactionReason;
            currTypeTransReason.PropertyChanged += new PropertyChangedEventHandler(currTypeTransReason_PropertyChanged);

            DisplayData();
        }

        void TypeTransactionReasonList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            isDirty = true;
        }

        void currTypeTransReason_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            isDirty = true;
        }

        private async void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var tempReason = (frmReason.Content as ReasonDetailFrame);
            tempReason.UpdateLayout();

            EnableAppBarStatus(false);

            var saveOC = App.Instance.StaticDataSource.TypeTransactionReasonList.Where(t => t.HasChanges).ToObservableCollection();

           await App.Instance.StaticDataSource.SaveTypeTransactionReason(saveOC);

            SyncLists();
        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            currTypeTransReason = NewTypeTransactionReason();
            ((ObservableCollection<TypeTransactionReason>)DataContext).Add(currTypeTransReason);

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
            currTypeTransReason.IsDeleted = true;

            ((ObservableCollection<TypeTransactionReason>)DataContext).Remove(currTypeTransReason);

            DisplayData();

            EnableAppBarStatus(true);
        }

        #endregion
    }
}
