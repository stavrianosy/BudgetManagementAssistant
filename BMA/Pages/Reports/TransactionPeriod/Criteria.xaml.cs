using BMA.BusinessLogic;
using BMA.Common;
using ModernUI.Toolkit.Data.Charting.Charts.Series;
using System;
using System.Collections.Generic;
using System.Globalization;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BMA.Pages.Reports.TransactionPeriod
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class Criteria : LayoutAwarePage
    {
        DateTime dateFrom = new DateTime();
        DateTime dateTo = new DateTime();

        public Criteria()
        {
            this.InitializeComponent();
        }

        #region Events
        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            InitControls();
        }

        private void cbPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void brdViewReport_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Tuple<string, DateTime, DateTime> item = new Tuple<string, DateTime, DateTime>(
                                                        (cbPeriod.SelectedValue as ComboBoxItem).Content.ToString(),
                                                        dateFrom,
                                                        dateTo);

            frmGraph.Navigate(typeof(Graph), item);

        }

        private void brdViewReportPie_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Tuple<string, DateTime, DateTime> item = new Tuple<string, DateTime, DateTime>(
                                                        (cbPeriod.SelectedValue as ComboBoxItem).Content.ToString(),
                                                        dateFrom,
                                                        dateTo);

            frmGraph.Navigate(typeof(Pie), item);
        }

        private void cbFromMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dateFrom = GetDateFromControl(sender as ComboBox, dateFrom, false);

            InitDayControl(cbFromDay, dateFrom);
        }

        private void cbFromYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dateFrom = GetDateFromControl(sender as ComboBox, dateFrom, true);

            InitDayControl(cbFromDay, dateFrom);
        }

        private void cbFromDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem == null)
                return;

            int day = (int)((sender as ComboBox).SelectedItem as ComboBoxItem).Tag;

            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, day);
        }

        private void cbToMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dateTo = GetDateFromControl(sender as ComboBox, dateTo, false);

            InitDayControl(cbToDay, dateTo);
        }

        private void cbToYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dateTo = GetDateFromControl(sender as ComboBox, dateTo, true);

            InitDayControl(cbToDay, dateTo);
        }

        private void cbToDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem == null)
                return;

            int day = (int)((sender as ComboBox).SelectedItem as ComboBoxItem).Tag;

            dateTo = new DateTime(dateTo.Year, dateTo.Month, day);
        }
        #endregion

        #region Private Methods
        private void InitControls()
        {
            DateTime dateForYear = DateTime.Now.AddYears(-1);
            List<string> monthList = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12).ToList();

            DateTime dateNowFrom = DateTime.Now.AddDays(-1);
            DateTime dateNowTo = DateTime.Now;

            //Do day part first because month & year interracts with day cb
            InitDayControl(cbFromDay, dateNowFrom);
            InitDayControl(cbToDay, dateNowTo);

            for (int i = 0; i < 2; i++)
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

        private DateTime GetDateFromControl(ComboBox control, DateTime date, bool controlIsYear)
        {
            DateTime result = new DateTime();

            int tag = (int)(control.SelectedItem as ComboBoxItem).Tag;
            int day = controlIsYear ?
                        DateTime.DaysInMonth(tag, date.Month) < date.Day ? 1 : date.Day
                        :
                        DateTime.DaysInMonth(date.Year, tag) < date.Day ? 1 : date.Day;

            result = controlIsYear ? new DateTime(tag, date.Month, day) : new DateTime(date.Year, tag, day);

            return result;
        }
        #endregion
    }
}
