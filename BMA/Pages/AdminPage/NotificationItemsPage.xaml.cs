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
    public sealed partial class NotificationItemsPage : BMA.Common.LayoutAwarePage
    {
        #region Private Members
        Notification currNotification;
        List<Notification> originalNotificationList;
        bool isDirty;
        #endregion

        #region Constructor
        public NotificationItemsPage()
        {
            App.Instance.StaticDataSource.NotificationList.CollectionChanged += NotificationList_CollectionChanged;

            this.InitializeComponent();
        }
        #endregion

        #region Private Methods
        private Notification NewNotification()
        {
            currNotification = new Notification();

            currNotification.PropertyChanged += new PropertyChangedEventHandler(currNotification_PropertyChanged);

            return currNotification;
        }

        private void DisplayData()
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            frmNotification.Navigate(typeof(NotificationDetailFrame), currNotification);
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
            originalNotificationList = App.Instance.StaticDataSource.NotificationList.ToList();
        }

        private void RevertCurrentList()
        {
            App.Instance.StaticDataSource.NotificationList.Clear();
            foreach (var item in originalNotificationList)
                App.Instance.StaticDataSource.NotificationList.Add(item);
        }

        private void DisplayData(Notification notification)
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            frmNotification.Navigate(typeof(NotificationDetailFrame), notification);
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

            frmNotification.Navigate(typeof(NotificationDetailFrame));

            SyncLists();

            this.DataContext = App.Instance.StaticDataSource.NotificationList;
            itemsViewSource.Source = this.DataContext;

            itemGridView.ItemsSource = itemsViewSource.Source;

            EnableAppBarStatus(false);
        }

        private async void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageDialog dialog = null;
            if (currNotification != null && currNotification.HasChanges)
            {
                dialog = new MessageDialog("The are changes. Please save the first.");
                //await dialog.ShowAsync();
                //return;
            }
            currNotification = e.ClickedItem as Notification;
            currNotification.PropertyChanged += new PropertyChangedEventHandler(currNotification_PropertyChanged);

            DisplayData();
        }

        void NotificationList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            isDirty = true;
        }

        void currNotification_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            isDirty = true;
        }

        private async void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var tempNotification = (frmNotification.Content as NotificationDetailFrame);
            tempNotification.UpdateLayout();

            EnableAppBarStatus(false);

            var saveOC = App.Instance.StaticDataSource.NotificationList.Where(t => t.HasChanges).ToObservableCollection();

            await App.Instance.StaticDataSource.SaveNotification(saveOC);

            SyncLists();
        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            currNotification = NewNotification();
            ((ObservableCollection<Notification>)DataContext).Add(currNotification);

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
            currNotification.IsDeleted = true;

            ((ObservableCollection<Notification>)DataContext).Remove(currNotification);

            DisplayData();

            EnableAppBarStatus(true);
        }

        #endregion

    }
}
