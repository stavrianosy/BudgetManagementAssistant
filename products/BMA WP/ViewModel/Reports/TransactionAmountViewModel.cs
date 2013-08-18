﻿using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BMA_WP.ViewModel.ReportsView
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TransactionAmountViewModel : ViewModelBase
    {
        #region Private Members
        private ObservableCollection<Transaction> reportResult;
        private int pivotIndex;
        bool isSortByDate;
        #endregion

        #region Public Properties
        public bool IsLoading { get { return App.Instance.IsSyncing; } set { App.Instance.IsSyncing = value; } }
        public bool IsSortByDate { get { return isSortByDate; } set { isSortByDate = value; RaisePropertyChanged("IsSortByDate"); } }
        public ObservableCollection<Transaction> ReportResult { get { return reportResult; } set { reportResult = value; RaisePropertyChanged("ReportResult"); } }
        public TypeTransactionList TransactionTypeList { get { return App.Instance.StaticServiceData.TypeTransactionList; } }
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }
        #endregion

        /// <summary>
        /// Initializes a new instance of the TransactionAmountViewModel class.
        /// </summary>
        public TransactionAmountViewModel()
        {
        }
    }
}