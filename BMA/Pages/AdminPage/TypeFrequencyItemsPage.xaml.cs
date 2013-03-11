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
    public sealed partial class TypeFrequencyItemsPage : BMA.Common.LayoutAwarePage
    {
        TypeFrequency currTypeFrequency;
        List<TypeFrequency> originalTypeFrequencyList;

        public TypeFrequencyItemsPage()
        {
            App.Instance.StaticDataSource.TypeFrequencyList.CollectionChanged += TypeFrequencyList_CollectionChanged;
            this.InitializeComponent();
        }

        void TypeFrequencyList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
        }

        void currTypeFrequency_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            App.Instance.Share = null;

            frmTypeFrequency.Navigate(typeof(TypeFrequencyDetailFrame));

            this.DataContext = App.Instance.StaticDataSource.TypeFrequencyList;
            itemsViewSource.Source = App.Instance.StaticDataSource.TypeFrequencyList;

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
            frmTypeFrequency.Navigate(typeof(CategoryDetailFrame), currTypeFrequency);
        }

        private async void ItemGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageDialog dialog = null;
            if (currTypeFrequency != null && currTypeFrequency.HasChanges)
            {
                dialog = new MessageDialog("The are changes. Please save the first.");
                //await dialog.ShowAsync();
                //return;
            }
            currTypeFrequency = e.ClickedItem as TypeFrequency;
            currTypeFrequency.PropertyChanged += new PropertyChangedEventHandler(currTypeFrequency_PropertyChanged);

            DisplayData();
        }

        private void EnableAppBarStatus(bool status)
        {
            AppBarDoneButton.IsEnabled = status;
            AppBarCancelButton.IsEnabled = status;
        }

        #region Events

        private async void Done_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            var tempTypeFrequency = (frmTypeFrequency.Content as CategoryDetailFrame);
            tempTypeFrequency.UpdateLayout();

            AppBarAddButton.IsEnabled = true;
            //AppBarDoneButton.IsEnabled = false;
            AppBarCancelButton.IsEnabled = false;

            var saveOC = App.Instance.StaticDataSource.TypeFrequencyList.Where(t => t.HasChanges).ToObservableCollection();

           await App.Instance.StaticDataSource.SaveTypeFrequency(saveOC);
        }

        private void Cancel_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            DisplayData();

            AppBarAddButton.IsEnabled = true;
            AppBarDoneButton.IsEnabled = false;
            AppBarCancelButton.IsEnabled = false;
        }

        private void Delete_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            //((ObservableCollection<Transaction>)DataContext).Remove(currTransaction);

            //DisplayData();

            //EnableAppBarStatus(true);
        }

        private void Add_AppBarButtonClick(object sender, RoutedEventArgs e)
        {
            currTypeFrequency = NewTypeFrequency();
            ((ObservableCollection<TypeFrequency>)DataContext).Add(currTypeFrequency);

            DisplayData();

            AppBarAddButton.IsEnabled = false;
            AppBarDoneButton.IsEnabled = true;
            AppBarCancelButton.IsEnabled = true;
        }

        #endregion

        private TypeFrequency NewTypeFrequency()
        {
            currTypeFrequency= new TypeFrequency();

            currTypeFrequency.PropertyChanged += new PropertyChangedEventHandler(currTypeFrequency_PropertyChanged);

            return currTypeFrequency;
        }
    }
}
