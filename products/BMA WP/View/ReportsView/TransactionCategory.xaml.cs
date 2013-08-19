﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA_WP.ViewModel.ReportsView;
using BMA.BusinessLogic;
using System.Collections.ObjectModel;

namespace BMA_WP.View.ReportsView
{
    public partial class TransactionCategory : PhoneApplicationPage
    {
        #region Private Members

        #endregion

        #region Public Properties
        public TransactionCategoryViewModel vm
        {
            get { return (TransactionCategoryViewModel)DataContext; }
        }
        #endregion

        public TransactionCategory()
        {
            InitializeComponent();
        }


        private void btnViewReport_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!IsValid())
                return;

            var dateFrom = dpDateFrom.Value.Value;
            var dateTo = dpDateTo.Value.Value;
            var transType = cmbType.SelectedItem as TypeTransaction;
            var sortByAmount = tglAmount.IsChecked == true;

            App.Instance.ServiceData.ReportTransactionCategory(dateFrom, dateTo, transType.TypeTransactionId,
                (result, error) =>
                {
                    if (error == null)
                    {
                        if (sortByAmount)
                        {
                            vm.ReportResult = new ObservableCollection<KeyValuePair<Category, double>>();
                            foreach (var item in result.ToList().OrderByDescending(x=>x.Value))
                                vm.ReportResult.Add(new KeyValuePair<Category,double>(item.Key, item.Value));
                        }
                        else
                        {
                            vm.ReportResult = new ObservableCollection<KeyValuePair<Category, double>>();
                            foreach (var item in result.ToList().OrderBy(x=>x.Key.Name))
                                vm.ReportResult.Add(new KeyValuePair<Category,double>(item.Key, item.Value));
                        }

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