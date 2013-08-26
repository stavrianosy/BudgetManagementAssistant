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
                case "piBudget":
                    SetupAppBar_Budget();
                    ItemSelected();
                   svItem.ScrollToVerticalOffset(0d);
                    break;
                case "piBudgetList":
                    SetupAppBar_BudgetList();
                    BudgetMultiSelect.SelectedItem = null;
                    vm.CurrBudget = null;
                    break;
            }
        }

        private void ItemSelected()
        {
            var budget = (Budget)BudgetMultiSelect.SelectedItem;

            vm.CurrBudget = budget;

            if (vm.CurrBudget == null || vm.CurrBudget.IsDeleted)
            {
                vm.IsEnabled = false;
            }
            else
            {
                vm.CurrBudget.PropertyChanged += (o, changedEventArgs) => save.IsEnabled = vm.Budgets.HasItemsWithChanges();

                vm.IsEnabled = !vm.IsLoading;
                delete.IsEnabled = !vm.IsLoading;
            }
        }

        private void SetupAppBar_BudgetList()
        {
            SetupAppBar_Common(false);
        }

        private void SetupAppBar_Budget()
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
            save.IsEnabled = false;
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
            ManualUpdate();
            if (!ValidateBudget())
                return;

            SaveBudget();
        }

        private void SaveBudget()
        {
            vm.IsLoading = true;

            var saveOC = vm.Budgets.Where(t => t.HasChanges).ToObservableCollection();

             App.Instance.ServiceData.SaveBudgets(saveOC, (error) => 
            {
                if(error == null)
                    vm.IsLoading = false;
            });

            pivotContainer.SelectedIndex = 1;
            save.IsEnabled = vm.Budgets.HasItemsWithChanges() && vm.IsLoading == false;
        }

        private bool ValidateBudget()
        {
            return true;
        }

        private void ManualUpdate()
        {
            //manually update model. textbox dont work well with numeric bindings
            var amount = 0d;

            double.TryParse(txtAmount.Text, out amount);

            if (vm.CurrBudget != null)
            {
                vm.CurrBudget.Amount = amount;
            }

            //set the focus to a control without keyboard
            piBudget.Focus();

            //end of - manually update model
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (vm.IsLoading)
            {
                MessageBox.Show(AppResources.BusySynchronizing);
                return;
            }

            ManualUpdate();

            if (!ValidateBudget())
                return;

            Budget item = new Budget(App.Instance.User);

            vm.PivotIndex = 0;

            svItem.ScrollToVerticalOffset(0d);

            vm.Budgets.Add(item);
            BudgetMultiSelect.SelectedItem = item;
            vm.CurrBudget = item;

            save.IsEnabled = vm.Budgets.HasItemsWithChanges() && vm.IsLoading == false;
            delete.IsEnabled = !vm.IsLoading;
            vm.IsEnabled = !vm.IsLoading;
        }
    }
}