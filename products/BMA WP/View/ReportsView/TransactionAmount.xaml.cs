using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA.BusinessLogic;
using BMA_WP.ViewModel.ReportsView;

namespace BMA_WP.View.ReportsView
{
    public partial class TransactionAmount : PhoneApplicationPage
    {
        #region Private Members

        #endregion

        #region Public Properties
        public TransactionAmountViewModel vm
        {
            get { return (TransactionAmountViewModel)DataContext; }
        }
        #endregion

        public TransactionAmount()
        {
            InitializeComponent();
        }

        private void btnViewReport_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if(!IsValid())
                return;

            var dateFrom = dpDateFrom.Value.Value;
            var dateTo = dpDateTo.Value.Value;
            var transType = cmbType.SelectedItem as TypeTransaction;
            var amountFrom=double.Parse(txtAmountFrom.Text.Trim());
            var amountTo=double.Parse(txtAmountTo.Text.Trim());
            var sortByDate = tglDate.IsChecked == true; 

            App.Instance.ServiceData.ReportTransactionAmount(dateFrom, dateTo, transType.TypeTransactionId, amountFrom, amountTo,
                (result, error) =>
                {
                    if (error == null)
                    {
                        if(sortByDate)
                            vm.ReportResult = result.OrderByDescending(x=>x.TransactionDate).ToObservableCollection();
                        else
                            vm.ReportResult = result.OrderByDescending(x => x.Amount).ToObservableCollection();

                        vm.PivotIndex = 1;
                    }
                });
        }

        private bool IsValid()
        {
            var result = true;

            return result;
        }

    }
}