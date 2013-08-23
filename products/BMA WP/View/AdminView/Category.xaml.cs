using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA_WP.ViewModel.Admin;
using BMA_WP.Resources;
using BMA.BusinessLogic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace BMA_WP.View.AdminView
{
    public partial class Category : PhoneApplicationPage
    {
        #region Private Members
        ApplicationBarIconButton save;
        ApplicationBarIconButton delete;
        ApplicationBarIconButton add;
        ApplicationBarMenuItem transactions;
        ApplicationBarMenuItem mainMenu;
        #endregion

        public CategoryViewModel vm
        {
            get { return (CategoryViewModel)DataContext; }
        }

        public Category()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!App.Instance.IsAuthorized)
                NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.Relative));
        }


        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string piName = (e.AddedItems[0] as PivotItem).Name;

            switch (piName)
            {
                case "piCategory":
                    SetupAppBar_Category();
                    ItemSelected();
                    svItem.ScrollToVerticalOffset(0d);
                    break;
                case "piCategoryList":
                    SetupAppBar_CategoryList();
                    CategoriesMultiSelect.SelectedItem = null;                    
                    vm.CurrCategory = null;
                    if(transactionReasonList.SelectedItems.Count > 0)
                        transactionReasonList.SelectedItems.Clear();
                    
                    break;
            }
        }

        private void ItemSelected()
        {
            vm.CurrCategory = (BMA.BusinessLogic.Category)CategoriesMultiSelect.SelectedItem;

            if (vm.CurrCategory == null || vm.CurrCategory.IsDeleted)
                vm.IsEnabled = false;
            else
            {
                var items = (ObservableCollection<TypeTransactionReason>)transactionReasonList.ItemsSource;
                foreach (var item in items)
                {
                    var container = transactionReasonList.ContainerFromItem(item) as LongListMultiSelectorItem;
                    if (vm.CurrCategory.TypeTransactionReasons != null &&
                        vm.CurrCategory.TypeTransactionReasons.Where(x => x.TypeTransactionReasonId == item.TypeTransactionReasonId && !x.IsDeleted).Count() == 0)
                        container.IsSelected = false;
                    else
                        container.IsSelected = true;

                    if (item.Name.Trim().Equals("other", StringComparison.InvariantCultureIgnoreCase))
                        container.IsEnabled = false;
                }

                vm.CurrCategory.PropertyChanged += (o, changedEventArgs) => save.IsEnabled = vm.CategoryList.HasItemsWithChanges();

                vm.IsEnabled = !vm.IsLoading;
                delete.IsEnabled = !vm.IsLoading;
            }
        }

        private void SetupAppBar_CategoryList()
        {
            SetupAppBar_Common(false);
        }

        private void SetupAppBar_Category()
        {
            SetupAppBar_Common(true);
        }

        void SetupAppBar_Common(bool includeDelete)
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/save.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSave;
            save.IsEnabled = vm.CategoryList.HasItemsWithChanges() && vm.IsLoading == false;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);

            if (includeDelete)
            {
                delete = new ApplicationBarIconButton();
                delete.IconUri = new Uri("/Assets/icons/Dark/delete.png", UriKind.Relative);
                delete.Text = AppResources.AppBarButtonDelete;
                delete.IsEnabled = false;
                ApplicationBar.Buttons.Add(delete);
                delete.Click += new EventHandler(Delete_Click);
            }

            add = new ApplicationBarIconButton();
            add.IconUri = new Uri("/Assets/icons/Dark/add.png", UriKind.Relative);
            add.Text = AppResources.AppBarButtonAdd;
            ApplicationBar.Buttons.Add(add);
            add.Click += new EventHandler(Add_Click);

            mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);

            transactions = new ApplicationBarMenuItem();
            transactions.Text = AppResources.AppBarButtonTransaction;
            ApplicationBar.MenuItems.Add(transactions);
            transactions.Click += new EventHandler(Transactions_Click);
        }

        private void Transactions_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Transactions.xaml", UriKind.Relative));
        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(AppResources.DeleteMessage, AppResources.ConfirmDeletion, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                vm.CurrCategory.IsDeleted = true;
                SaveCategory();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveCategory();
        }

        private  void SaveCategory()
        {
            if (!ValidateCaregory())
                return;

            vm.IsLoading = true;

            var saveOC = vm.CategoryList.Where(t => t.HasChanges).ToObservableCollection();

            App.Instance.StaticServiceData.SaveCategory(saveOC,(error) => 
            {
                if(error == null)
                    vm.IsLoading = false;
            });

            pivotContainer.SelectedIndex = 1;
            save.IsEnabled = vm.CategoryList.HasItemsWithChanges() && vm.IsLoading == false;
        }

        private bool ValidateCaregory()
        {
            var result = true;

            result = ValidateSingleTransaction() && ValidateAllTransactions();

            return result;   
        }

        private bool ValidateSingleTransaction()
        {
            var result = true;
            if (vm.CurrCategory == null)
                return result;

            SolidColorBrush okColor = new SolidColorBrush(new Color() { A = 255, B = 255, G = 255, R = 255 });
            SolidColorBrush errColor = new SolidColorBrush(new Color() { A = 255, B = 75, G = 75, R = 240 });

            txtName.Background = okColor;

            if (vm.CurrCategory.Name == null || vm.CurrCategory.Name.Length == 0)
            {
                result = false;
                txtName.Background = errColor;
            }

            if (vm.CurrCategory.Name != null)
            {
                var nameExists = vm.CategoryList.FirstOrDefault(x => x.Name.Trim().Equals(vm.CurrCategory.Name.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                                                                x.CategoryId != vm.CurrCategory.CategoryId && !x.IsDeleted);
                if (nameExists != null)
                {
                    result = false;
                    MessageBox.Show(AppResources.NameAlreadyExist);
                }
            }

            if (!result)
                svItem.ScrollToVerticalOffset(0);

            return result;
        }

        public bool ValidateAllTransactions()
        {
            var result = true;

            var tempCategoryCount = vm.CategoryList.Where(x => !x.IsDeleted && (x.Name == null || x.Name.Trim().Length == 0)).Count();

            var tempCategoryNameExists = (from i in vm.CategoryList 
                                          where i.Name != null && !i.IsDeleted 
                                          group i by i.Name.Trim().ToLower() into g
                                             select new { item = g.Key, count = g.Count() }).Where(x => x.count > 1).Count();

            if (tempCategoryNameExists > 0)
            {
                result = false;
                MessageBox.Show(string.Format(AppResources.FaildValidationNameExists, AppResources.Categories));
            }
            else if (tempCategoryCount > 0)
            {
                result = false;
                //for more specific message
                if (tempCategoryCount == 1)
                    MessageBox.Show(string.Format(AppResources.FaildValidationSingle, AppResources.Category));
                else
                    MessageBox.Show(string.Format(AppResources.FaildValidation, tempCategoryCount, AppResources.Categories));
            }

            return result;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (vm.IsLoading)
            {
                MessageBox.Show(AppResources.BusySynchronizing);
                return;
            }

            if (!ValidateCaregory())
                return;


            var item = new BMA.BusinessLogic.Category(App.Instance.User, App.Instance.StaticServiceData.TypeTransactionReasonList);

            if (vm.CategoryList.Count + 1 >= CategoryList.DEVICE_MAX_COUNT)
            {
                MessageBox.Show(string.Format(AppResources.MaxItemsCount, CategoryList.DEVICE_MAX_COUNT));
                return;
            }

            vm.PivotIndex = 0;

            svItem.ScrollToVerticalOffset(0d);

            vm.CategoryList.Add(item);
            CategoriesMultiSelect.SelectedItem = item;
            vm.CurrCategory = item;

            save.IsEnabled = vm.CategoryList.HasItemsWithChanges() && vm.IsLoading == false;
            delete.IsEnabled = !vm.IsLoading;
            vm.IsEnabled = !vm.IsLoading;
        }

        private void transactionReasonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isAdded = e.AddedItems.Count > 0;
            var item = isAdded ? (TypeTransactionReason)e.AddedItems[0] : (TypeTransactionReason)e.RemovedItems[0];

            if (vm.CurrCategory == null)
                return;

            if (vm.CurrCategory.TypeTransactionReasons == null && isAdded)
                vm.CurrCategory.TypeTransactionReasons = new List<TypeTransactionReason>();

            if (vm.CurrCategory.TypeTransactionReasons.FirstOrDefault(x => x.TypeTransactionReasonId == item.TypeTransactionReasonId) == null && isAdded)
            {
                vm.CurrCategory.TypeTransactionReasons.Add(item);
                vm.CurrCategory.ModifiedDate = DateTime.Now;
                vm.CurrCategory.HasChanges = true;
            }
            else if (vm.CurrCategory.TypeTransactionReasons.FirstOrDefault(x => x.TypeTransactionReasonId == item.TypeTransactionReasonId) != null && !isAdded)
            {
                //vm.CurrCategory.TypeTransactionReasons.Remove(item);
                vm.CurrCategory.TypeTransactionReasons.Find(x => x.TypeTransactionReasonId == item.TypeTransactionReasonId).IsDeleted = true;
                vm.CurrCategory.ModifiedDate = DateTime.Now;
                vm.CurrCategory.HasChanges = true;
            }
        }
    }
}