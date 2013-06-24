using BMA.BusinessLogic;
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
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace BMA_WP.View.AdminView
{
    public partial class Interval : PhoneApplicationPage
    {
        ApplicationBarMenuItem mainMenu ;
        ApplicationBarIconButton add ;
        ApplicationBarIconButton delete;
        ApplicationBarIconButton saveContinute;
        ApplicationBarIconButton save;

        public IntervalViewModel vm
        {
            get { return (IntervalViewModel)DataContext; }
        }

        public Interval()
        {
            InitializeComponent();
        }

        #region Binding
        //workaround for the ListPicker issue when binding object becomes null
        private void SetBindings(bool isEnabled)
        {
            if (isEnabled)
            {
                Binding bindTransType = new Binding("TransactionType");
                bindTransType.Mode = BindingMode.TwoWay;
                bindTransType.Source = vm.CurrInterval;
                if (vm.CurrInterval.TransactionType != null &&
                    ((ObservableCollection<TypeTransaction>)cmbType.ItemsSource)
                                                .FirstOrDefault(x => x.TypeTransactionId == vm.CurrInterval.TransactionType.TypeTransactionId) != null)
                    cmbType.SetBinding(ListPicker.SelectedItemProperty, bindTransType);

                Binding bindCategory = new Binding("Category");
                bindCategory.Mode = BindingMode.TwoWay;
                bindCategory.Source = vm.CurrInterval;
                if (vm.CurrInterval.Category != null &&
                    ((ObservableCollection<BMA.BusinessLogic.Category>)cmbCategory.ItemsSource)
                        .FirstOrDefault(x => x.CategoryId == vm.CurrInterval.Category.CategoryId) != null)
                    cmbCategory.SetBinding(ListPicker.SelectedItemProperty, bindCategory);
            }
            else
            {
                if (cmbType.GetBindingExpression(ListPicker.SelectedIndexProperty) != null)
                    cmbType.ClearValue(ListPicker.SelectedItemProperty);
            }
        }
        #endregion

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string piName = (e.AddedItems[0] as PivotItem).Name;

            switch (piName)
            {
                case "piInterval":
                    ItemSelected();                    
                    SetupAppBar_Interval();
                    break;
                case "piIntervalList":
                    IntervalsMultiSelect.SelectedItem = null;
                    vm.CurrInterval = null;
                    SetupAppBar_IntervalList();
                    break;
            }
        }

        private void ItemSelected()
        {
            var item = (TypeInterval)IntervalsMultiSelect.SelectedItem;
            SetBindings(false);

            vm.CurrInterval = item;

            if (item != null)
                SetBindings(true);

            if (vm.CurrInterval == null || vm.CurrInterval.IsDeleted)
                vm.IsEnabled = false;
            else
                vm.IsEnabled = true;
        }

        private void SetupAppBar_IntervalList()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = false;
        }

        private void SetupAppBar_Interval()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/save.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSave;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);

            saveContinute = new ApplicationBarIconButton();
            saveContinute.IconUri = new Uri("/Assets/icons/Dark/refresh.png", UriKind.Relative);
            saveContinute.Text = AppResources.AppBarButtonContinue;
            ApplicationBar.Buttons.Add(saveContinute);
            saveContinute.Click += new EventHandler(Continue_Click);

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
            throw new NotImplementedException();
        }

        private async void Save_Click(object sender, EventArgs e)
        {
            var saveOC = vm.TypeIntervalList.Where(t => t.HasChanges).ToObservableCollection();

            //await App.Instance.StaticServiceData.SaveTypeInterval(saveOC);

            //pivotContainer.SelectedIndex = 1;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            var item = new BMA.BusinessLogic.TypeInterval(vm.CategoryList.ToList(), vm.TypeTransactionList.ToList(), App.Instance.User);

            vm.PivotIndex = 0;
            vm.TypeIntervalList.Add(item);
            IntervalsMultiSelect.SelectedItem = item;
            vm.CurrInterval = item;
            SetBindings(true);

            save.IsEnabled = true;
            vm.IsEnabled = true;

        }

        private void tglDaily_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UncheckAllRules();
            reccurenceDaily.Visibility = System.Windows.Visibility.Visible;
            tglDaily.IsChecked = true;
        }

        private void tglWeekly_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UncheckAllRules();
            reccurenceWeekly.Visibility = System.Windows.Visibility.Visible;
            tglWeekly.IsChecked = true;

        }

        private void tglMonthly_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UncheckAllRules();
            reccurenceMonthly.Visibility = System.Windows.Visibility.Visible;
            tglMonthly.IsChecked = true;

        }

        private void tglYearly_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            UncheckAllRules();
            reccurenceYearly.Visibility = System.Windows.Visibility.Visible;
            tglYearly.IsChecked = true;

        }
        void UncheckAllRules()
        {
            tglDaily.IsChecked = false;
            tglWeekly.IsChecked = false;
            tglMonthly.IsChecked = false;
            tglYearly.IsChecked = false;

            reccurenceDaily.Visibility = System.Windows.Visibility.Collapsed;
            reccurenceWeekly.Visibility = System.Windows.Visibility.Collapsed;
            reccurenceMonthly.Visibility = System.Windows.Visibility.Collapsed;
            reccurenceYearly.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void rbDailyRule1_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleDailyEveryDays = true;
                vm.CurrInterval.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == "RuleDailyEveryDays");
            }
        }

        private void rbWeeklyRule1_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleWeeklyEveryWeek = true;
                vm.CurrInterval.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == "RuleWeeklyEveryWeek");
            }
        }

        private void rbMonthlyRule1_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleMonthlyDayNum = true;
                vm.CurrInterval.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == "RuleMonthlyDayNum");
            }
        }

        private void rbMonthlyRule2_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleMonthlyPrecise = true;
                vm.CurrInterval.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == "RuleMonthlyPrecise");
            }
        }

        private void rbYearlyRule1_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleYearlyOnMonth = true;
                vm.IsEnabled_RuleYearlyOnTheWeekDay = true;
                vm.CurrInterval.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == "RuleYearlyOnMonth");
            }
        }

        private void rbYearlyRule2_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleYearlyOnMonth = true;
                vm.IsEnabled_RuleYearlyOnMonth2 = true;
                vm.CurrInterval.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == "RuleYearlyOnTheWeekDay");
            }
        }

        private void rbRangeRule1_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRangeRules();
                
                vm.IsEnabled_RuleRangeStartDate = true;
                
                vm.CurrInterval.RecurrenceRangeRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == "RuleRangeNoEndDate");
            }

        }
        private void rbRangeRule2_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRangeRules();

                vm.IsEnabled_RuleRangeStartDate = true;
                vm.IsEnabled_RuleRangeTotalOcurrences = true;
                
                vm.CurrInterval.RecurrenceRangeRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == "RuleRangeTotalOcurrences");
            }

        }
        private void rbRangeRule3_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRangeRules();

                vm.IsEnabled_RuleRangeStartDate = true; 
                vm.IsEnabled_RuleRangeEndBy = true;
                
                vm.CurrInterval.RecurrenceRangeRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == "RuleRangeEndBy");
            }

        }

        private void DisableAllRules()
        {
            vm.IsEnabled_RuleDailyEveryDays = false;
            vm.IsEnabled_RuleWeeklyEveryWeek = false;
            vm.IsEnabled_RuleMonthlyDayNum = false;
            vm.IsEnabled_RuleMonthlyPrecise = false;
            vm.IsEnabled_RuleYearlyOnTheWeekDay = false;
            vm.IsEnabled_RuleYearlyOnMonth = false;
            vm.IsEnabled_RuleYearlyOnMonth2 = false;
        }

        private void DisableAllRangeRules()
        {
            vm.IsEnabled_RuleRangeStartDate = false;
            vm.IsEnabled_RuleRangeTotalOcurrences = false;
            vm.IsEnabled_RuleRangeEndBy = false;
        }

    }
}