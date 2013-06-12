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
    public sealed partial class BudgetThresholdItemsPage : BMA.Common.LayoutAwarePage
    {
        #region Private Members
        bool isDirty;
        BudgetThreshold currBudgetThreshold;
        List<BudgetThreshold> originalBudgetThresholdList;
        #endregion

        #region Constructor
        public BudgetThresholdItemsPage()
        {
            App.Instance.StaticDataSource.BudgetThresholdList.CollectionChanged += BudgetThresholdList_CollectionChanged;
            this.InitializeComponent();
        }
        #endregion

        #region Private Methods
        private void DisplayData()
        {
            EnableAppBarStatus(true);
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            frmBudgetThresholdDetail.Navigate(typeof(BudgetThresholdDetailFrame), currBudgetThreshold);
        }

        private void EnableAppBarStatus(bool status)
        {
            AppBarDoneButton.IsEnabled = status;
            AppBarCancelButton.IsEnabled = status;
            AppBarDeleteButton.IsEnabled = status;

            AppBarAddButton.IsEnabled = !status;
        }

        private BudgetThreshold NewBudgetThreshold()
        {
            currBudgetThreshold = new BudgetThreshold();

            currBudgetThreshold.PropertyChanged += new PropertyChangedEventHandler(currBudgetThreshold_PropertyChanged);

            return currBudgetThreshold;
        }

        private void SyncLists()
        {
            originalBudgetThresholdList = App.Instance.StaticDataSource.BudgetThresholdList.ToList();
        }

        private void RevertCurrentList()
        {
            App.Instance.StaticDataSource.CategoryList.Clear();

            if (originalBudgetThresholdList == null)
                return;

            foreach (var item in originalBudgetThresholdList)
                App.Instance.StaticDataSource.BudgetThresholdList.Add(item);
        }
        #endregion

        #region Events

        void BudgetThresholdList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            isDirty = true;
            EnableAppBarStatus(true);
        }

        void currBudgetThreshold_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            isDirty = true;
            EnableAppBarStatus(true);
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

            frmBudgetThresholdDetail.Navigate(typeof(BudgetThresholdDetailFrame));

            SyncLists();

            this.DataContext = App.Instance.StaticDataSource.BudgetThresholdList;

            itemsViewSource.Source = App.Instance.StaticDataSource.BudgetThresholdList;

            itemGridView.ItemsSource = itemsViewSource.Source;

            EnableAppBarStatus(false);
        }

        private async void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var temp = (frmBudgetThresholdDetail.Content as BudgetThresholdDetailFrame);
            temp.UpdateLayout();

            EnableAppBarStatus(false);

            var saveOC = App.Instance.StaticDataSource.BudgetThresholdList.Where(t => t.HasChanges).ToObservableCollection();

            await App.Instance.StaticDataSource.SaveBudgetThreshold(saveOC);

            SyncLists();
        }

        private void Cancel_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            DisplayData();

            EnableAppBarStatus(false);

            RevertCurrentList();
        }

        private void Delete_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            currBudgetThreshold.IsDeleted = true;

            ((ObservableCollection<BudgetThreshold>)DataContext).Remove(currBudgetThreshold);

            DisplayData();

            EnableAppBarStatus(true);
        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            currBudgetThreshold = NewBudgetThreshold();
            ((ObservableCollection<BudgetThreshold>)DataContext).Add(currBudgetThreshold);

            DisplayData();

            EnableAppBarStatus(true);
        }

        private async void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageDialog dialog = null;
            if (currBudgetThreshold != null && currBudgetThreshold.HasChanges)
            {
                dialog = new MessageDialog("The are changes. Please save the first.");
                //await dialog.ShowAsync();
                //return;
            }
            currBudgetThreshold = e.ClickedItem as BudgetThreshold;
            currBudgetThreshold.PropertyChanged += new PropertyChangedEventHandler(currBudgetThreshold_PropertyChanged);

            DisplayData();

            currBudgetThreshold.HasChanges = false;
            isDirty = false;
        }

        #endregion

    }
}
