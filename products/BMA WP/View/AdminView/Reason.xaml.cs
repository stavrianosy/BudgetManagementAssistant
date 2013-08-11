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

namespace BMA_WP.View.AdminView
{
    public partial class Reason : PhoneApplicationPage
    {
        #region Private Members
        ApplicationBarIconButton save;
        ApplicationBarIconButton delete;
        ApplicationBarIconButton add;
        ApplicationBarMenuItem transactions;
        ApplicationBarMenuItem mainMenu;
        #endregion

        public ReasonViewModel vm
        {
            get { return (ReasonViewModel)DataContext; }
        }

        public Reason()
        {
            InitializeComponent();

        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string piName = (e.AddedItems[0] as PivotItem).Name;

            switch (piName)
            {
                case "piReason":
                    SetupAppBar_Reason();
                    vm.CurrTransactionReason = (BMA.BusinessLogic.TypeTransactionReason)ReasonsMultiSelect.SelectedItem;
                    ItemSelected();
                    svItem.ScrollToVerticalOffset(0d);
                    break;
                case "piReasonList":

                    SetupAppBar_ReasonList();
                    ReasonsMultiSelect.SelectedItem = null;
                    
                    vm.CurrTransactionReason = null;
                    categoryList.SelectedItems.Clear();

                    break;
            }
        }

        private void ItemSelected()
        {
            vm.CurrTransactionReason = (BMA.BusinessLogic.TypeTransactionReason)ReasonsMultiSelect.SelectedItem;

            if (vm.CurrTransactionReason == null || vm.CurrTransactionReason.IsDeleted)
                vm.IsEnabled = false;
            else
            {
                var items = (ObservableCollection<BMA.BusinessLogic.Category>)categoryList.ItemsSource;
                foreach (var item in items)
                {
                    var container = categoryList.ContainerFromItem(item) as LongListMultiSelectorItem;
                    if (vm.CurrTransactionReason.Categories != null &&
                        vm.CurrTransactionReason.Categories.Where(x => x.CategoryId == item.CategoryId && !x.IsDeleted).Count() == 0)
                    {
                        container.IsSelected = false;
                        continue;
                    }

                    if (container != null)
                        container.IsSelected = true;
                }
                vm.CurrTransactionReason.PropertyChanged += (o, changedEventArgs) => save.IsEnabled = vm.TransactionReasonList.HasItemsWithChanges();

                vm.IsEnabled = !vm.IsLoading;
                delete.IsEnabled = !vm.IsLoading;
            }
        }

        private void SetupAppBar_ReasonList()
        {
            SetupAppBar_Common(false);
        }

        private void SetupAppBar_Reason()
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
            save.IsEnabled = vm.TransactionReasonList.HasItemsWithChanges() && vm.IsLoading == false;
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
            add.IsEnabled = true;
            ApplicationBar.Buttons.Add(add);
            add.Click += new EventHandler(Add_Click);

            mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            mainMenu.IsEnabled = true;
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
                vm.CurrTransactionReason.IsDeleted = true;
                SaveTransactionReason();
            }
        }

        private void SaveTransactionReason()
        {
            if (!ValidateTransactionReason())
                return;

            vm.IsLoading = true;

            var saveOC = vm.TransactionReasonList.Where(t => t.HasChanges).ToObservableCollection();

            App.Instance.StaticServiceData.SaveTransactionReason(saveOC, (error) =>
            {
                if (error == null)
                    vm.IsLoading = false;
            });

            pivotContainer.SelectedIndex = 1;
            save.IsEnabled = vm.TransactionReasonList.HasItemsWithChanges() && vm.IsLoading == false;
        }

        private bool ValidateTransactionReason()
        {
            var result = true;
            if (vm.TransactionReasonList == null)
                return result;

            SolidColorBrush okColor = new SolidColorBrush(new Color() { A = 255, B = 255, G = 255, R = 255 });
            SolidColorBrush errColor = new SolidColorBrush(new Color() { A = 255, B = 75, G = 75, R = 240 });

            txtName.Background = okColor;

            if (vm.CurrTransactionReason.Name == null || vm.CurrTransactionReason.Name.Length == 0)
            {
                result = false;
                txtName.Background = errColor;
            }

            if (!result)
                svItem.ScrollToVerticalOffset(0);
            else
            {
                var tempTransReason = vm.TransactionReasonList.Where(x => !x.IsDeleted && (x.Name == null || x.Name.Length == 0)).ToList();
                if (tempTransReason.Count > 0)
                {
                    result = false;
                    //for more specific message
                    if (tempTransReason.Count == 1)
                        MessageBox.Show(string.Format(AppResources.FaildValidationSingle, "transaction reason"));
                    else
                        MessageBox.Show(string.Format(AppResources.FaildValidation, tempTransReason.Count, "transaction reasons"));
                }
            }

            return result;

        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveTransactionReason();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (vm.IsLoading)
            {
                MessageBox.Show(AppResources.BusySynchronizing);
                return;
            }

            if (!ValidateTransactionReason())
                return;


            var item = new BMA.BusinessLogic.TypeTransactionReason(App.Instance.User, App.Instance.StaticServiceData.CategoryList);

            if (vm.TransactionReasonList.Count + 1 >= TypeTransactionReasonList.DEVICE_MAX_COUNT)
            {
                MessageBox.Show(string.Format(AppResources.MaxItemsCount, TypeTransactionReasonList.DEVICE_MAX_COUNT));
                return;
            }

            vm.PivotIndex = 0;

            svItem.ScrollToVerticalOffset(0d);

            vm.TransactionReasonList.Add(item);
            ReasonsMultiSelect.SelectedItem = item;
            vm.CurrTransactionReason = item;

            save.IsEnabled = vm.TransactionReasonList.HasItemsWithChanges() && vm.IsLoading == false;
            delete.IsEnabled = !vm.IsLoading;
            vm.IsEnabled = !vm.IsLoading;
        }

        private void categoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var isAdded = e.AddedItems.Count > 0;
            var item = isAdded ? (BMA.BusinessLogic.Category)e.AddedItems[0] : (BMA.BusinessLogic.Category)e.RemovedItems[0];

            if (vm.CurrTransactionReason == null)
                return;

            if (vm.CurrTransactionReason.Categories == null && isAdded)
                vm.CurrTransactionReason.Categories = new List<BMA.BusinessLogic.Category>();

            if (vm.CurrTransactionReason.Categories.FirstOrDefault(x => x.CategoryId == item.CategoryId) == null && isAdded)
            {
                vm.CurrTransactionReason.Categories.Add(item);
                vm.CurrTransactionReason.ModifiedDate = DateTime.Now;
                vm.CurrTransactionReason.HasChanges = true;
            }
            else if (vm.CurrTransactionReason.Categories.FirstOrDefault(x => x.CategoryId == item.CategoryId) != null && !isAdded)
            {
                vm.CurrTransactionReason.Categories.Find(x => x.CategoryId == item.CategoryId).IsDeleted = true;
                vm.CurrTransactionReason.ModifiedDate = DateTime.Now;
                vm.CurrTransactionReason.HasChanges = true;
            }

            
        }

    }
}