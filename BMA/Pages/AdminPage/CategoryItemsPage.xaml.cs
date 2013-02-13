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
    public sealed partial class CategoryItemsPage : BMA.Common.LayoutAwarePage
    {
        Category currCategory;

        public CategoryItemsPage()
        {
            App.Instance.StaticDataSource.CategoryList.CollectionChanged += CategoryList_CollectionChanged;
            this.InitializeComponent();
        }

        void CategoryList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        void currCategory_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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

            frmCategoryDetail.Navigate(typeof(CategoryDetailFrame));

            this.DataContext = App.Instance.StaticDataSource.CategoryList;
            itemsViewSource.Source = App.Instance.StaticDataSource.CategoryList;

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
            frmCategoryDetail.Navigate(typeof(CategoryDetailFrame), currCategory);
        }

        private async void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageDialog dialog = null;
            if (currCategory != null && currCategory.HasChanged)
            {
                dialog = new MessageDialog("The are changes. Please save the first.");
                //await dialog.ShowAsync();
                //return;
            }
            currCategory = e.ClickedItem as Category;
            currCategory.PropertyChanged += new PropertyChangedEventHandler(currCategory_PropertyChanged);

            DisplayData();
        }

        private void EnableAppBarStatus(bool status)
        {
            AppBarDoneButton.IsEnabled = status;
            AppBarCancelButton.IsEnabled = status;
        }

        private async void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var tempCategory = (frmCategoryDetail.Content as CategoryDetailFrame);
            tempCategory.UpdateLayout();

            AppBarAddButton.IsEnabled = true;
            //AppBarDoneButton.IsEnabled = false;
            AppBarCancelButton.IsEnabled = false;

             var saveOC = new ObservableCollection<Category>();
            foreach (var cat in App.Instance.StaticDataSource.CategoryList.Where(t => t.HasChanged))
                saveOC.Add(cat);

            await App.Instance.StaticDataSource.SaveCategory(saveOC);
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
            currCategory = NewCategory();
            ((ObservableCollection<Category>)DataContext).Add(currCategory);

            DisplayData();

            AppBarAddButton.IsEnabled = false;
            AppBarDoneButton.IsEnabled = true;
            AppBarCancelButton.IsEnabled = true;
        }

        private Category NewCategory()
        {
            currCategory = new Category();

            currCategory.PropertyChanged += new PropertyChangedEventHandler(currCategory_PropertyChanged);

            return currCategory;
        }
    }
}
