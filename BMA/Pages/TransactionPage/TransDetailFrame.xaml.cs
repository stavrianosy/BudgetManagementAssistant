using BMA.BusinessLogic;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BMA.Pages.TransactionPage
{
    public sealed partial class TransDetailFrame : BMA.Common.LayoutAwarePage
    {
        #region Private Members
        Transaction currTransaction;
        #endregion

        #region Constructors
        public TransDetailFrame()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Events
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            App.Instance.Share = null;

            currTransaction = navigationParameter as Transaction;

            DefaultViewModel["TypeTransactions"] = App.Instance.StaticDataSource.TypeTransactionList;
            DefaultViewModel["Categories"] = App.Instance.StaticDataSource.CategoryList;
            DefaultViewModel["TypeTransactionReasons"] = App.Instance.StaticDataSource.TypeTransactionReasonList;
            DefaultViewModel["Transaction"] = currTransaction;

            this.IsEnabled = currTransaction != null;

            this.UpdateLayout();

            InitControls();
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            RepositionInsertionPoint((sender as TextBox));

            double result = 0d;
            double.TryParse(txtAmount.Text, out result);

            currTransaction.Amount = result;
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TypeTransaction transType = (TypeTransaction)(sender as ComboBox).SelectedItem;
            currTransaction.TransactionType = transType;
        }

        private void txtTip_TextChanged(object sender, TextChangedEventArgs e)
        {
            RepositionInsertionPoint((sender as TextBox));

            double result = 0d;
            double.TryParse(txtTip.Text, out result);

            currTransaction.TipAmount = result;
        }

        private void txtComments_TextChanged(object sender, TextChangedEventArgs e)
        {
            currTransaction.Comments = txtComments.Text;
        }

        private void txtNameOfPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            currTransaction.NameOfPlace = txtNameOfPlace.Text;
        }

        private void cmbReason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TypeTransactionReason transType = (TypeTransactionReason)(sender as ComboBox).SelectedItem;
            currTransaction.TransactionReasonType = transType;
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category transCat = (Category)(sender as ComboBox).SelectedItem;
            currTransaction.Category = transCat;
        }

        private void txtAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            //(sender as TextBox).SelectAll();
        }


        private void txtAmount_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //txtAmount.SelectionStart = 0;
            //txtAmount.SelectionLength = txtAmount.Text.Length-1;
        }


        private void cbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currTransaction.CreatedDate = GetDateControl(sender as ComboBox, currTransaction.CreatedDate, false);

            InitDayControl(cbDay, currTransaction.CreatedDate);
        }

        private void cbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currTransaction.CreatedDate = GetDateControl(sender as ComboBox, currTransaction.CreatedDate, true);

            InitDayControl(cbDay, currTransaction.CreatedDate);
        }

        private void cbDay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currTransaction == null || (sender as ComboBox).SelectedItem == null)
                return;

            int day = (int)((sender as ComboBox).SelectedItem as ComboBoxItem).Tag;

            currTransaction.CreatedDate = new DateTime(
                                            currTransaction.CreatedDate.Year, 
                                            currTransaction.CreatedDate.Month, 
                                            day, 
                                            currTransaction.CreatedDate.Hour, 
                                            currTransaction.CreatedDate.Minute, 
                                            currTransaction.CreatedDate.Second);
        }

        private void cbHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currTransaction == null)
                return;

            int tagH = (int)(cbHour.SelectedItem as ComboBoxItem).Tag;
            int tagM = currTransaction.CreatedDate.Minute;

            currTransaction.CreatedDate = new DateTime(currTransaction.CreatedDate.Year, 
                                                        currTransaction.CreatedDate.Month, 
                                                        currTransaction.CreatedDate.Day, 
                                                        tagH, tagM, 0);
        }

        private void cbMinute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currTransaction == null)
                return;

            int tagH = currTransaction.CreatedDate.Hour;
            int tagM = (int)(cbMinute.SelectedItem as ComboBoxItem).Tag;

            currTransaction.CreatedDate = new DateTime(currTransaction.CreatedDate.Year, 
                                                        currTransaction.CreatedDate.Month, 
                                                        currTransaction.CreatedDate.Day, 
                                                        tagH, tagM, 0);
        }
        #endregion

        #region Private Methods
        private void RepositionInsertionPoint(TextBox textBox)
        {
            if (textBox.Text.Length == 1)
                textBox.SelectionStart = 1;
        }

        private void InitControls()
        {
            if (currTransaction == null)
                return;

            DateTime dateForYear = DateTime.Now.AddYears(-1);
            List<string> monthList = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames.Take(12).ToList();

            DateTime date = currTransaction == null ? new DateTime() : currTransaction.CreatedDate;

            //Do day part first because month & year interracts with day cb
            InitDayControl(cbDay, currTransaction.CreatedDate);

            //SETUP YEAR COMBO
            for (int i = 0; i < 2; i++)
            {
                int itemYear = dateForYear.AddYears(i).Year;

                ComboBoxItem cbItem = new ComboBoxItem();

                cbItem.Content = itemYear;
                cbItem.Tag = itemYear;

                cbItem.IsSelected = itemYear == date.Year;

                cbYear.Items.Add(cbItem);
            }

            //SETUP MONTH COMBO
            for (int i = 0; i < monthList.Count(); i++)
            {
                ComboBoxItem cbItem = new ComboBoxItem();

                cbItem.Content = cbItem.Content = string.Format("{0:00} {1}", i + 1, monthList[i]);
                cbItem.Tag = i + 1;

                cbItem.IsSelected = date.Month == i + 1;

                cbMonth.Items.Add(cbItem);
            }

            //SETUP HOUR COMBO
            for (int i = 0; i < 24; i++)
            {
                int itemHour = i;

                ComboBoxItem cbItem = new ComboBoxItem();

                cbItem.Content = string.Format("{0:00}", itemHour);
                cbItem.Tag = itemHour;

                cbItem.IsSelected = itemHour == date.Hour;

                cbHour.Items.Add(cbItem);
            }

            //SETUP MINUTE COMBO
            for (int i = 0; i < 60; i++)
            {
                int itemMinute = i;

                ComboBoxItem cbItem = new ComboBoxItem();

                cbItem.Content = string.Format("{0:00}", itemMinute);
                cbItem.Tag = itemMinute;

                cbItem.IsSelected = date.Minute == itemMinute;

                cbMinute.Items.Add(cbItem);
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

        private DateTime GetDateControl(ComboBox control, DateTime date, bool controlIsYear)
        {
            DateTime result = new DateTime();
            if (currTransaction == null)
                return result;

            int tag = (int)(control.SelectedItem as ComboBoxItem).Tag;
            int day = controlIsYear ?
                        DateTime.DaysInMonth(tag, date.Month) < date.Day ? 1 : date.Day
                        :
                        DateTime.DaysInMonth(date.Year, tag) < date.Day ? 1 : date.Day;

            result = controlIsYear ? new DateTime(tag, date.Month, day, date.Hour, date.Minute,date.Second) :
                                        new DateTime(date.Year, tag, day, date.Hour, date.Minute, date.Second);

            return result;
        }
        #endregion
    }
}
