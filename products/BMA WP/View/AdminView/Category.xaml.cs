﻿using System;
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
using System.Windows.Data;
using BMA_WP.Model;

namespace BMA_WP.View.AdminView
{
    public partial class Category : PhoneApplicationPage
    {
        #region Private Members
        ApplicationBarIconButton save;
        ApplicationBarIconButton delete;
        ApplicationBarIconButton add;
        ApplicationBarMenuItem reasons;
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

            SetupLoadingBinding();
        }

        #region Binding

        private void SetupLoadingBinding()
        {
            Binding bind = new Binding("IsBusyComm");
            bind.Mode = BindingMode.TwoWay;
            bind.Source = App.Instance;

            bind.Converter = new StatusConverter();
            bind.ConverterParameter = "trueVisible";

            spLoading.SetBinding(StackPanel.VisibilityProperty, bind);
        }

        #endregion

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

                    if ((vm.CurrCategory.Name != null && vm.CurrCategory.Name.Trim().Equals("other", StringComparison.InvariantCultureIgnoreCase)) || 
                        item.Name.Trim().Equals("other", StringComparison.InvariantCultureIgnoreCase))
                        container.IsEnabled = false;
                    else
                        container.IsEnabled = true;
                }

                vm.CurrCategory.PropertyChanged += (o, changedEventArgs) => save.IsEnabled = vm.CategoryList.HasItemsWithChanges();

                if (!string.IsNullOrEmpty(vm.CurrCategory.Name) && vm.CurrCategory.Name.Equals("other", StringComparison.CurrentCultureIgnoreCase))
                {
                    vm.IsEnabled = false;
                    delete.IsEnabled = false;
                }
                else
                {
                    vm.IsEnabled = !vm.IsLoading;
                    delete.IsEnabled = !vm.IsLoading;
                }
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

            reasons = new ApplicationBarMenuItem();
            reasons.Text = AppResources.AppBarButtonReason;
            ApplicationBar.MenuItems.Add(reasons);
            reasons.Click += new EventHandler(Reasons_Click);

            transactions = new ApplicationBarMenuItem();
            transactions.Text = AppResources.AppBarButtonTransaction;
            ApplicationBar.MenuItems.Add(transactions);
            transactions.Click += new EventHandler(Transactions_Click);
        }

        private void Reasons_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/AdminView/Reason.xaml", UriKind.Relative));
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
            if (!string.IsNullOrEmpty(vm.CurrCategory.Name) && vm.CurrCategory.Name.Equals("other", StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show(AppResources.ItemOtherNoDelete);
                return;
            }

            var result = MessageBox.Show(AppResources.DeleteMessage, AppResources.ConfirmDeletion, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                vm.CurrCategory.IsDeleted = true;
                vm.PivotIndex = 1;
                //SaveCategory();
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
                if (error != null)
                    MessageBox.Show(error.Message);
                //if(error == null)
                    vm.IsLoading = false;
            });

            pivotContainer.SelectedIndex = 1;
            save.IsEnabled = vm.CategoryList.HasItemsWithChanges() && vm.IsLoading == false;
        }

        private bool ValidateCaregory()
        {
            var result = true;

            result = ValidateSingleTransaction() && ValidateAllTransactions();

            if (vm.IsLoading)
            {
                MessageBox.Show(AppResources.BusySynchronizing);
                result = false;
            }

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
            if (!ValidateCaregory())
                return;

            var item = new BMA.BusinessLogic.Category(App.Instance.User, App.Instance.StaticServiceData.TypeTransactionReasonList);

            if (vm.CategoryList.Where(x=>!x.IsDeleted).Count() >= CategoryList.DEVICE_MAX_COUNT)
            {
                MessageBox.Show(string.Format(AppResources.MaxItemsCount, CategoryList.DEVICE_MAX_COUNT));
                return;
            }

            vm.PivotIndex = 0;

            svItem.ScrollToVerticalOffset(0d);

            vm.CategoryList.Add(item);
            CategoriesMultiSelect.SelectedItem = item;
            vm.CurrCategory = item;
            ItemSelected();

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

            if (isAdded)
            {
                if (vm.CurrCategory.TypeTransactionReasons.FirstOrDefault(x => x.TypeTransactionReasonId == item.TypeTransactionReasonId) == null)
                {
                    vm.CurrCategory.TypeTransactionReasons.Add(item);
                    CategoryChangeStatus();
                }
                else
                {
                    vm.CurrCategory.TypeTransactionReasons.Find(x => x.TypeTransactionReasonId == item.TypeTransactionReasonId).IsDeleted = false;
                    CategoryChangeStatus();
                }
            }
            else
            {
                if (vm.CurrCategory.TypeTransactionReasons.FirstOrDefault(x => x.TypeTransactionReasonId == item.TypeTransactionReasonId) != null)
                {
                    vm.CurrCategory.TypeTransactionReasons.Find(x => x.TypeTransactionReasonId == item.TypeTransactionReasonId).IsDeleted = true;
                    CategoryChangeStatus();
                }
            }
        }

        private void CategoryChangeStatus()
        {
            vm.CurrCategory.ModifiedDate = DateTime.Now;
            vm.CurrCategory.HasChanges = true;
        }
    }
}