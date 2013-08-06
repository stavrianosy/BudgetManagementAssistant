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
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.View.AdminView
{
    public partial class Interval : PhoneApplicationPage
    {
        ApplicationBarMenuItem mainMenu ;
        ApplicationBarIconButton add ;
        ApplicationBarIconButton delete;
        ApplicationBarIconButton saveContinute;
        ApplicationBarIconButton save;
        ApplicationBarMenuItem transaction;

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
                    svItem.ScrollToVerticalOffset(0d);
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

        private void Transaction_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Transactions.xaml", UriKind.Relative));
        }

        private async void Delete_Click(object sender, EventArgs e)
        {
            vm.CurrInterval.IsDeleted = true;

            await SaveTypeInterval();
        }


        private async Task SaveTypeInterval()
        {
            //2. update record, then choose another one = error
            
            var saveOC = vm.TypeIntervalList.Where(t => t.HasChanges).ToObservableCollection();
            //var hasError = false;
            var errorMessage = new StringBuilder();

            saveOC.ToList().ForEach(x =>
            {
                errorMessage.Append(x.SelfValidation());
            });

            if (errorMessage.ToString().Length > 0)
            {
                MessageBox.Show(errorMessage.ToString());
                return;
            }

            await App.Instance.StaticServiceData.SaveTypeInterval(saveOC, (error) =>
            {
                if (error != null)
                    MessageBox.Show(AppResources.SaveFailed);

                vm.IsLoading = false;

            });

            pivotContainer.SelectedIndex = 1;
        }

        private async void Save_Click(object sender, EventArgs e)
        {
            await SaveTypeInterval();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            svItem.ScrollToVerticalOffset(0);

            var item = new BMA.BusinessLogic.TypeInterval(vm.CategoryList.ToList(), vm.TypeTransactionList.ToList(), App.Instance.User);

            ResetRules();

            vm.PivotIndex = 0;
            vm.TypeIntervalList.Add(item);
            IntervalsMultiSelect.SelectedItem = item;
            vm.CurrInterval = item;
            SetBindings(true);

            save.IsEnabled = true;
            vm.IsEnabled = true;
            vm.IsToggled_tglDaily = true;

        }

        private void tglDaily_Checked(object sender, RoutedEventArgs e)
        {
            HideAllRules();
            reccurenceDaily.Visibility = System.Windows.Visibility.Visible;

            tglWeekly.IsChecked = false;
            tglMonthly.IsChecked = false;
            tglYearly.IsChecked = false;
        }

        private void tglWeekly_Checked(object sender, RoutedEventArgs e)
        {
            HideAllRules();
            reccurenceWeekly.Visibility = System.Windows.Visibility.Visible;

            tglDaily.IsChecked = false;
            tglMonthly.IsChecked = false;
            tglYearly.IsChecked = false;

        }

        private void tglMonthly_Checked(object sender, RoutedEventArgs e)
        {
            HideAllRules();
            reccurenceMonthly.Visibility = System.Windows.Visibility.Visible;

            tglDaily.IsChecked = false;
            tglWeekly.IsChecked = false;
            tglYearly.IsChecked = false;

        }

        private void tglYearly_Checked(object sender, RoutedEventArgs e)
        {
            HideAllRules();
            reccurenceYearly.Visibility = System.Windows.Visibility.Visible;

            tglDaily.IsChecked = false;
            tglWeekly.IsChecked = false;
            tglMonthly.IsChecked = false;

        }

        private void rbDailyRule1_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleDailyEveryDays = true;

                vm.CurrInterval.RecurrenceRuleValue.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == Const.Rule.RuleDailyEveryDays.ToString());

                vm.DailyEveryDay = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.DailyEveryDay.ToString()).Value;
                vm.DailyOnlyWeekdays = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.DailyOnlyWeekdays.ToString()).Value;
            }
        }

        private void rbWeeklyRule1_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleWeeklyEveryWeek = true;
                
                vm.CurrInterval.RecurrenceRuleValue.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == Const.Rule.RuleWeeklyEveryWeek.ToString());

                vm.WeeklyEveryWeek = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.WeeklyEveryWeek.ToString()).Value;
                vm.WeeklyDayName = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.WeeklyDayName.ToString()).Value;
            }
        }

        private void rbMonthlyRule1_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleMonthlyDayNum = true;

                vm.CurrInterval.RecurrenceRuleValue.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == Const.Rule.RuleMonthlyDayNum.ToString());

                vm.MonthlyDayNumber = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyDayNumber.ToString()).Value;
                vm.MonthlyEveryMonth = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyEveryMonth.ToString()).Value;
            }
        }

        private void rbMonthlyRule2_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleMonthlyPrecise = true;
                vm.CurrInterval.RecurrenceRuleValue.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == Const.Rule.RuleMonthlyPrecise.ToString());

                vm.MonthlyCountOfWeekDay = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyCountOfWeekDay.ToString()).Value;
                vm.MonthlyDayName = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyDayName.ToString()).Value;
                vm.MonthlyCountOfMonth = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyCountOfMonth.ToString()).Value;
            }
        }

        private void rbYearlyRule1_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleYearlyOnMonth = true;
                vm.IsEnabled_RuleYearlyOnTheWeekDay = true;
                vm.CurrInterval.RecurrenceRuleValue.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == Const.Rule.RuleYearlyOnMonth.ToString());

                vm.YearlyEveryYear = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyEveryYear.ToString()).Value;
                vm.YearlyOnDayPos = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyOnDayPos.ToString()).Value;
                vm.YearlyMonthName = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyMonthName.ToString()).Value;
            }
        }

        private void rbYearlyRule2_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRules();
                vm.IsEnabled_RuleYearlyOnMonth = true;
                vm.IsEnabled_RuleYearlyOnMonth2 = true;
                vm.CurrInterval.RecurrenceRuleValue.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == Const.Rule.RuleYearlyOnTheWeekDay.ToString());

                vm.YearlyEveryYear = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyEveryYear.ToString()).Value;
                vm.YearlyPositions = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyPositions.ToString()).Value;
                vm.YearlyDayName = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyDayName.ToString()).Value;
                vm.YearlyMonthNameSec = vm.CurrInterval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyMonthNameSec.ToString()).Value;
            }
        }

        private void rbRangeRule1_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRangeRules();
                
                vm.IsEnabled_RuleRangeStartDate = true;

                vm.CurrInterval.RecurrenceRangeRuleValue.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == Const.Rule.RuleRangeNoEndDate.ToString());

                vm.RangeStartDate = vm.CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeStartDate.ToString()).Value;

            }

        }
        
        private void rbRangeRule2_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRangeRules();

                vm.IsEnabled_RuleRangeStartDate = true;
                vm.IsEnabled_RuleRangeTotalOcurrences = true;

                vm.CurrInterval.RecurrenceRangeRuleValue.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == Const.Rule.RuleRangeTotalOcurrences.ToString());

                vm.RangeStartDate = vm.CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeStartDate.ToString()).Value;
                vm.RangeTotalOcurrences = vm.CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeTotalOcurrences.ToString()).Value;

            }

        }
        
        private void rbRangeRule3_Checked(object sender, RoutedEventArgs e)
        {
            if (vm.CurrInterval != null)
            {
                DisableAllRangeRules();

                vm.IsEnabled_RuleRangeStartDate = true; 
                vm.IsEnabled_RuleRangeEndBy = true;

                vm.CurrInterval.RecurrenceRangeRuleValue.RecurrenceRule = vm.RecurrenceRuleList.FirstOrDefault(x => x.Name == Const.Rule.RuleRangeEndBy.ToString());

                vm.RangeStartDate = vm.CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeStartDate.ToString()).Value;
                vm.RangeEndBy = vm.CurrInterval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeEndBy.ToString()).Value;

            }

        }

        void HideAllRules()
        {
            reccurenceDaily.Visibility = System.Windows.Visibility.Collapsed;
            reccurenceWeekly.Visibility = System.Windows.Visibility.Collapsed;
            reccurenceMonthly.Visibility = System.Windows.Visibility.Collapsed;
            reccurenceYearly.Visibility = System.Windows.Visibility.Collapsed;
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

        void UnCheckAllRules()
        {
            vm.IsChecked_RuleDailyEveryDays = false;
            vm.IsChecked_RuleWeeklyEveryWeek = false;
            vm.IsChecked_RuleMonthlyDayNum = false;
            vm.IsChecked_RuleMonthlyPrecise = false;
            vm.IsChecked_RuleYearlyOnMonth = false;
            vm.IsChecked_RuleYearlyOnTheWeekDay = false;
            vm.IsChecked_RuleYearlyOnMonth2 = false;
            vm.IsChecked_RuleRangeNoEndDate = false;
            vm.IsChecked_RuleRangeTotalOcurrences = false;
            vm.IsChecked_RuleRangeEndBy = false;
        }

        private void DisableAllRangeRules()
        {
            vm.IsEnabled_RuleRangeStartDate = false;
            vm.IsEnabled_RuleRangeTotalOcurrences = false;
            vm.IsEnabled_RuleRangeEndBy = false;
        }

        void UnCheckAllToggles()
        {
            vm.IsToggled_tglDaily = false;
            vm.IsToggled_tglWeekly = false;
            vm.IsToggled_tglMonthly = false;
            vm.IsToggled_tglYearly = false;
        }

        private void ResetRules()
        {
            DisableAllRules();
            DisableAllRangeRules();
            UnCheckAllToggles();
            UnCheckAllRules();

            vm.IsToggled_tglDaily = true;
            reccurenceDaily.Visibility = System.Windows.Visibility.Visible;
        }
    }
}