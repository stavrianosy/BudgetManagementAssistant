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

namespace BMA_WP.View.AdminView
{
    public partial class Reason : PhoneApplicationPage
    {
        #region Private Members
        ApplicationBarIconButton save;
        ApplicationBarIconButton saveContinue;
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
            // **** save when switching screens ****

            string piName = (e.AddedItems[0] as PivotItem).Name;


            switch (piName)
            {
                case "piReason":
                    vm.CurrTransactionReason = (BMA.BusinessLogic.TypeTransactionReason)ReasonsMultiSelect.SelectedItem;                    
                    ReasonItem();
                    SetupAppBar_Reason();
                    break;
                case "piReasonList":
                    
                    ReasonsMultiSelect.SelectedItem = null;
                    vm.CurrTransactionReason = (BMA.BusinessLogic.TypeTransactionReason)ReasonsMultiSelect.SelectedItem;

                    categoryList.SelectedItems.Clear();
                    SetupAppBar_ReasonList();
                    break;
            }

            if (vm.CurrTransactionReason != null)
            {
                vm.CurrTransactionReason.PropertyChanged += (o, changedEventArgs) =>
                {
                    var items = vm.TransactionReasonList.Where(t => t.HasChanges).ToList();
                    if (items.Count > 0)
                    {
                        save.IsEnabled = true;
                        delete.IsEnabled = true;
                    }
                };
                delete.IsEnabled = true;
            }
        }

        private void ReasonItem()
        {
            if (vm.CurrTransactionReason == null || vm.CurrTransactionReason.IsDeleted)
                vm.IsEnabled = false;
            else
            {
                vm.IsEnabled = true;
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
            }
        }

        private void SetupAppBar_ReasonList()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

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

        private void SetupAppBar_Reason()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/save.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSave;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);
            
            saveContinue = new ApplicationBarIconButton();
            saveContinue.IconUri = new Uri("/Assets/icons/Dark/refresh.png", UriKind.Relative);
            saveContinue.Text = AppResources.AppBarButtonContinue;
            ApplicationBar.Buttons.Add(saveContinue);
            saveContinue.Click += new EventHandler(Continue_Click);
            
            delete = new ApplicationBarIconButton();
            delete.IconUri = new Uri("/Assets/icons/Dark/delete.png", UriKind.Relative);
            delete.Text = AppResources.AppBarButtonDelete;
            ApplicationBar.Buttons.Add(delete);
            delete.Click += new EventHandler(Delete_Click);

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

        private void Continue_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

        private async void SaveTransactionReason()
        {
            var saveOC = vm.TransactionReasonList.Where(t => t.HasChanges).ToObservableCollection();

            await App.Instance.StaticServiceData.SaveTransactionReason(saveOC);

            pivotContainer.SelectedIndex = 1;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveTransactionReason();
        }

        private void Add_Click(object sender, EventArgs e)
        {

            var item = new BMA.BusinessLogic.TypeTransactionReason(App.Instance.User);

            vm.PivotIndex = 0;
            vm.TransactionReasonList.Add(item);
            ReasonsMultiSelect.SelectedItem = item;
            vm.CurrTransactionReason = item;

            save.IsEnabled = true;
            vm.IsEnabled = true;
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