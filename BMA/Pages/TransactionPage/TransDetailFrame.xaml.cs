using BMA.BusinessLogic;
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

        private void txtAmount_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //txtAmount.SelectionStart = 0;
            //txtAmount.SelectionLength = txtAmount.Text.Length-1;
        }
        #endregion

        private void txtAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            //(sender as TextBox).SelectAll();
        }

        #region Private Methods
        private void RepositionInsertionPoint(TextBox textBox)
        {
            if (textBox.Text.Length == 1)
                textBox.SelectionStart = 1;
        }
        #endregion

    }
}
