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
using System.Windows.Data;
using BMA_WP.Model;
using System.Windows.Media;

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
            var result = MessageBox.Show(AppResources.DeleteMessage, AppResources.ConfirmDeletion, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                vm.CurrBudget.IsDeleted = true;
                vm.PivotIndex = 1;
            }
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
            var result = true;

            result = ValidateSingleBudget() && ValidateAllBudgets();

            if (vm.IsLoading)
            {
                MessageBox.Show(AppResources.BusySynchronizing);
                result = false;
            }

            return result;   
        }

        private bool ValidateSingleBudget()
        {
            var result = true;
            if (vm.CurrBudget == null)
                return result;

            SolidColorBrush okColor = new SolidColorBrush(new Color() { A = 255, B = 255, G = 255, R = 255 });
            SolidColorBrush errColor = new SolidColorBrush(new Color() { A = 255, B = 75, G = 75, R = 240 });

            txtName.Background = okColor;
            txtAmount.Background = okColor;

            if (vm.CurrBudget.Amount <= 0)
            {
                result = false;
                txtAmount.Background = errColor;
            }

            if (vm.CurrBudget.Name == null || vm.CurrBudget.Name.Length == 0)
            {
                result = false;
                txtName.Background = errColor;
            }

            if (vm.CurrBudget.Name != null)
            {
                var nameExists = vm.Budgets.FirstOrDefault(x => x.Name.Trim().Equals(vm.CurrBudget.Name.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                                                                x.BudgetId != vm.CurrBudget.BudgetId && !x.IsDeleted);
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

        private bool ValidateAllBudgets()
        {
            var result = true;

            var tempBudgetCount = vm.Budgets.Where(x => !x.IsDeleted && 
                                                    (x.Amount <= 0  ||  
                                                    (x.Name == null || x.Name.Trim().Length == 0))).Count();

            var tempBudgetNameExists = (from i in vm.Budgets
                                          where i.Name != null && !i.IsDeleted
                                          group i by i.Name.Trim().ToLower() into g
                                          select new { item = g.Key, count = g.Count() }).Where(x => x.count > 1).Count();

            if (tempBudgetNameExists > 0)
            {
                result = false;
                MessageBox.Show(string.Format(AppResources.FaildValidationNameExists, AppResources.Budgets));
            }
            else if (tempBudgetCount > 0)
            {
                result = false;
                //for more specific message
                if (tempBudgetCount == 1)
                    MessageBox.Show(string.Format(AppResources.FaildValidationSingle, AppResources.Budget));
                else
                    MessageBox.Show(string.Format(AppResources.FaildValidation, tempBudgetCount, AppResources.Budgets));
            }

            return result;
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