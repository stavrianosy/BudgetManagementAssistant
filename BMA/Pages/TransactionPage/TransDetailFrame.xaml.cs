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
        bool isDirty = false;

        public TransDetailFrame()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            App.Instance.Share = null;
            
            DefaultViewModel["TypeTransactions"] = App.Instance.StaticDataSource.TypeTransactionList;
            DefaultViewModel["Categories"] = App.Instance.StaticDataSource.CategoryList;
            DefaultViewModel["TypeTransactionReasons"] = App.Instance.StaticDataSource.TypeTransactionReasonList;

            this.UpdateLayout();

            this.IsEnabled = false;

            if (navigationParameter is Transaction)
            {
                this.IsEnabled = true;

                var trans = navigationParameter as Transaction;

                cmbType.SelectedValue = trans.TransactionType==null ? 0 : trans.TransactionType.TypeTransactionId ;
                txtAmount.Text = trans.Amount.ToString();
                txtTip.Text = trans.TipAmount.ToString();
                cmbCategory.SelectedValue = trans.Category == null ? 0 : trans.Category.CategoryId;
                cmbReason.SelectedValue = trans.TransactionReasonType == null ? 0 : trans.TransactionReasonType.TypeTransactionReasonId;
                txtNameOfPlace.Text = trans.NameOfPlace ?? String.Empty;
                txtComments.Text = trans.Comments ?? String.Empty;

                isDirty = false;
            }
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            isDirty = true;
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isDirty = true;
        }

        private void txtTip_TextChanged(object sender, TextChangedEventArgs e)
        {
            isDirty = true;
        }

        private void txtComments_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtNameOfPlace_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cmbReason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
