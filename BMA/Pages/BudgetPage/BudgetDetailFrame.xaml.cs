using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using BMA.Common;
using BMA.BusinessLogic;
using System.Globalization;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BMA.Pages.BudgetPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BudgetDetailFrame : LayoutAwarePage
    {
        Budget currBudget;

        public BudgetDetailFrame()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            App.Instance.Share = null;

            currBudget = navigationParameter as Budget;

            DefaultViewModel["Budget"] = currBudget;

            this.IsEnabled = currBudget != null;

            this.UpdateLayout();

            InitControls();
        }

        private void InitControls()
        {
            if (currBudget == null)
                return;

            DateTime dateForYear = DateTime.Now.AddYears(-1);
            List<string> monthList = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12).ToList();

            DateTime dateNowFrom = currBudget == null ? new DateTime() : currBudget.FromDate;
            DateTime dateNowTo = currBudget == null ? new DateTime() : currBudget.ToDate;

            //Do day part first because month & year interracts with day cb
            InitDayControl(cbFromDay, currBudget.FromDate);

            for (int i = 0; i < 4; i++)
            {
                int itemYear = dateForYear.AddYears(i).Year;

                ComboBoxItem cbItemFrom = new ComboBoxItem();
                ComboBoxItem cbItemTo = new ComboBoxItem();

                cbItemFrom.Content = cbItemTo.Content = itemYear;
                cbItemFrom.Tag = cbItemTo.Tag = itemYear;

                cbItemFrom.IsSelected = itemYear == dateNowFrom.Year;
                cbItemTo.IsSelected = itemYear == dateNowTo.Year;

                cbFromYear.Items.Add(cbItemFrom);
                cbToYear.Items.Add(cbItemTo);
            }

            for (int i = 0; i < monthList.Count(); i++)
            {
                ComboBoxItem cbItemFrom = new ComboBoxItem();
                ComboBoxItem cbItemTo = new ComboBoxItem();
                cbItemFrom.Content = cbItemTo.Content = string.Format("{0:00} {1}", i + 1, monthList[i]);
                cbItemFrom.Tag = cbItemTo.Tag = i + 1;

                cbItemFrom.IsSelected = dateNowFrom.Month == i + 1;
                cbItemTo.IsSelected = dateNowTo.Month == i + 1;

                cbFromMonth.Items.Add(cbItemFrom);
                cbToMonth.Items.Add(cbItemTo);
            }
        }

        private void InitDayControl(ComboBox control, DateTime date)
        {
            control.Items.Clear();

            for (int i = 0; i < DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                ComboBoxItem cbItem = new ComboBoxItem();
                cbItem.Content = string.Format("{0:00}", i + 1);
                cbItem.Tag = i + 1;
                cbItem.IsSelected = date.Day == i + 1;

                control.Items.Add(cbItem);
            }
        }

        private void RepositionInsertionPoint(TextBox textBox)
        {
            if (textBox.Text.Length == 1)
                textBox.SelectionStart = 1;
        }

        private DateTime GetDateFromControl(ComboBox control, DateTime date, bool controlIsYear)
        {
            DateTime result = new DateTime();
            if (currBudget == null)
                return result;

            int tag = (int)(control.SelectedItem as ComboBoxItem).Tag;
            int day = controlIsYear ?
                        DateTime.DaysInMonth(tag, date.Month) < date.Day ? 1 : date.Day
                        :
                        DateTime.DaysInMonth(date.Year, tag) < date.Day ? 1 : date.Day;

            result = controlIsYear ? new DateTime(tag, date.Month, day) : new DateTime(date.Year, tag, day);

            return result;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            currBudget.Name = txtName.Text;
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            RepositionInsertionPoint((sender as TextBox));

            double result = 0d;
            double.TryParse(txtAmount.Text, out result);

            currBudget.Amount = result;
        }

        private void cbInstallment_CheckedChange(object sender, RoutedEventArgs e)
        {
            bool? result = (sender as CheckBox).IsChecked;
            currBudget.IncludeInstallments = result.Value;
        }

        private void txtComments_TextChanged(object sender, TextChangedEventArgs e)
        {
            currBudget.Comments = txtComments.Text;
        }

        private void cbFromMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currBudget.FromDate = GetDateFromControl(sender as ComboBox, currBudget.FromDate, false);
         
            InitDayControl(cbFromDay, currBudget.FromDate);
        }

        private void cbFromYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currBudget.FromDate = GetDateFromControl(sender as ComboBox, currBudget.FromDate, true);

            InitDayControl(cbFromDay, currBudget.FromDate);
        }

        private void cbFromDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currBudget == null || (sender as ComboBox).SelectedItem == null)
                return;

            int day = (int)((sender as ComboBox).SelectedItem as ComboBoxItem).Tag;

            currBudget.FromDate = new DateTime(currBudget.FromDate.Year, currBudget.FromDate.Month, day);
        }

        private void cbToMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currBudget.ToDate = GetDateFromControl(sender as ComboBox, currBudget.ToDate, false);

            InitDayControl(cbToDay, currBudget.ToDate);
        }

        private void cbToYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currBudget.ToDate = GetDateFromControl(sender as ComboBox, currBudget.ToDate, true);

            InitDayControl(cbToDay, currBudget.ToDate);
        }

        private void cbToDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currBudget == null || (sender as ComboBox).SelectedItem == null)
                return;

            int day = (int)((sender as ComboBox).SelectedItem as ComboBoxItem).Tag;

            currBudget.ToDate = new DateTime(currBudget.ToDate.Year, currBudget.ToDate.Month, day);
        }
    }
}
