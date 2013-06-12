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

namespace BMA_WP.View.AdminView
{
    public partial class Category : PhoneApplicationPage
    {
        #region Private Members
        ApplicationBarIconButton save;
        ApplicationBarIconButton saveContinue;
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

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string piName = (e.AddedItems[0] as PivotItem).Name;

            vm.CurrCategory = (BMA.BusinessLogic.Category)CategoriesMultiSelect.SelectedItem;

            if (vm.CurrCategory == null || vm.CurrCategory.IsDeleted)
                vm.IsEnabled = false;
            else
                vm.IsEnabled = true;

            switch (piName)
            {
                case "piCategory":
                    SetupAppBar_Category();
                    break;
                case "piCategoryList":
                    SetupAppBar_CategoryList();
                    break;
            }

            if (vm.CurrCategory != null)
            {
                vm.CurrCategory.PropertyChanged += (o, changedEventArgs) =>
                {
                    var items = vm.CategoryList.Where(t => t.HasChanges).ToList();
                    if (items.Count > 0)
                    {
                        save.IsEnabled = true;
                        delete.IsEnabled = true;
                    }
                };
                delete.IsEnabled = true;
            }
        }

        private void SetupAppBar_CategoryList()
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

        private void SetupAppBar_Category()
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
            //throw new NotImplementedException();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            vm.CurrCategory.IsDeleted = true;
            SaveCategory();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveCategory();
        }

        private async void SaveCategory()
        {
            var saveOC = vm.CategoryList.Where(t => t.HasChanges).ToObservableCollection();

            await App.Instance.StaticServiceData.SaveCategory(saveOC);

            pivotContainer.SelectedIndex = 1;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            var item = new BMA.BusinessLogic.Category(App.Instance.User);

            vm.PivotIndex = 0;
            vm.CategoryList.Add(item);
            CategoriesMultiSelect.SelectedItem = item;
            vm.CurrCategory = item;

            save.IsEnabled = true;
            vm.IsEnabled = true;
        }
    }
}