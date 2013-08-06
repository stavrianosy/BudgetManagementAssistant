using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA_WP.ViewModel;
using BMA.BusinessLogic;
using BMA_WP.Resources;

namespace BMA_WP.View
{
    public partial class Budgets : PhoneApplicationPage
    {
        #region Private Members
        ApplicationBarIconButton save;
        ApplicationBarIconButton delete;
        ApplicationBarIconButton add;
        ApplicationBarMenuItem mainMenu;
        ApplicationBarMenuItem transaction;
        #endregion

        #region Public Properties
        public BudgetViewModel vm
        {
            get { return (BudgetViewModel)DataContext; }
        }
        #endregion

        public Budgets()
        {
            InitializeComponent();
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string piName = (e.AddedItems[0] as PivotItem).Name;

            vm.CurrBudget = (Budget)BudgetMultiSelect.SelectedItem;

            if (vm.CurrBudget == null || vm.CurrBudget.IsDeleted)
                vm.IsEnabled = false;
            else
                vm.IsEnabled = true;

            switch (piName)
            {
                case "piBudget":
                    SetupAppBar_Budget();
                   svItem.ScrollToVerticalOffset(0d);
                    break;
                case "piBudgetList":
                    SetupAppBar_BudgetList();
                    break;
            }

            if (vm.CurrBudget != null)
            {
                vm.CurrBudget.PropertyChanged += (o, changedEventArgs) =>
                {
                    var items = vm.Budgets.Where(t => t.HasChanges).ToObservableCollection();
                    if (items.Count > 0)
                    {
                        save.IsEnabled = true;
                        delete.IsEnabled = true;
                    }
                };
                delete.IsEnabled = true;
            }

        }

        private void SetupAppBar_BudgetList()
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

            transaction = new ApplicationBarMenuItem();
            transaction.Text = AppResources.AppBarButtonTransaction;
            transaction.IsEnabled = true;
            ApplicationBar.MenuItems.Add(transaction);
            transaction.Click += new EventHandler(Transaction_Click);
        }

        private void SetupAppBar_Budget()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/save.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSave;
            save.IsEnabled = false;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);

            delete = new ApplicationBarIconButton();
            delete.IconUri = new Uri("/Assets/icons/Dark/delete.png", UriKind.Relative);
            delete.Text = AppResources.AppBarButtonDelete;
            delete.IsEnabled = false;
            ApplicationBar.Buttons.Add(delete);
            delete.Click += new EventHandler(Delete_Click);

            add = new ApplicationBarIconButton();
            add.IconUri = new Uri("/Assets/icons/Dark/add.png", UriKind.Relative);
            add.Text = AppResources.AppBarButtonAdd;
            add.IsEnabled = true;
            ApplicationBar.Buttons.Add(add);
            add.Click += new EventHandler(Add_Click);

            transaction = new ApplicationBarMenuItem();
            transaction.Text = AppResources.AppBarButtonTransaction;
            ApplicationBar.MenuItems.Add(transaction);
            transaction.Click += new EventHandler(Transaction_Click);

            mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);
        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            vm.CurrBudget.IsDeleted = true;

            SaveBudget();
        }

        private void Transaction_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Transactions.xaml", UriKind.Relative));
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveBudget();
        }

        private async void SaveBudget()
        {
            var saveOC = vm.Budgets.Where(t => t.HasChanges).ToObservableCollection();

            await App.Instance.ServiceData.SaveBudgets(saveOC, (error) => 
            {
                if(error == null)
                    vm.IsLoading = false;
            });

            pivotContainer.SelectedIndex = 1;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            Budget item = new Budget(App.Instance.User);

            vm.PivotIndex = 0;

            svItem.ScrollToVerticalOffset(0d);

            vm.Budgets.Add(item);
            BudgetMultiSelect.SelectedItem = item;
            vm.CurrBudget = item;

            save.IsEnabled = true;
            vm.IsEnabled = true;
        }
    }
}