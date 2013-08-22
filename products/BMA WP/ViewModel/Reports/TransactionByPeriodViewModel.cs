using BMA.BusinessLogic;
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
    public class TransactionByPeriodViewModel : ViewModelBase
    {
        
        #region Private Members
        private ObservableCollection<KeyValuePair<int, double>> reportResult;
        private int pivotIndex;
        bool isSortByAmount;

        DateTime dateFrom;
        DateTime dateTo;
        TypeTransaction transType;
        double amountFrom;
        double amountTo;
        Const.ReportPeriod period;
        ObservableCollection<Const.ReportPeriod> periodList;
        #endregion

        #region Public Properties
        public bool IsLoading { get { return App.Instance.IsSyncing; } set { App.Instance.IsSyncing = value; } }

        public bool IsSortByAmount { get { return isSortByAmount; } set { isSortByAmount = value; RaisePropertyChanged("IsSortByAmount"); } }
        public DateTime DateFrom { get { return dateFrom; } set { dateFrom = value; RaisePropertyChanged("DateFrom"); } }
        public DateTime DateTo { get { return dateTo; } set { dateTo = value; RaisePropertyChanged("DateTo"); } }
        public TypeTransaction TransactionType { get { return transType; } set { transType = value; RaisePropertyChanged("TransactionType"); } }
        public double AmountFrom { get { return amountFrom; } set { amountFrom = value; RaisePropertyChanged("AmountFrom"); } }
        public double AmountTo { get { return amountTo; } set { amountTo = value; RaisePropertyChanged("AmountTo"); } }
        public Const.ReportPeriod Period { get { return period; } set { period = value; RaisePropertyChanged("Period"); } }

        public ObservableCollection<KeyValuePair<int, double>> ReportResult { get { return reportResult; } set { reportResult = value; RaisePropertyChanged("ReportResult"); } }
        public ObservableCollection<Const.ReportPeriod> PeriodList { get { return periodList; } set { periodList = value; RaisePropertyChanged("PeriodList"); } }
        public TypeTransactionList TransactionTypeList { get { return App.Instance.StaticServiceData.TypeTransactionList; } }
        //public TypeTransactionReasonList TransactionReasonList { get { return App.Instance.StaticServiceData.TypeTransactionReasonList; } }
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }
        #endregion

        /// <summary>
        /// Initializes a new instance of the TransactionPeriodViewModel class.
        /// </summary>
        public TransactionByPeriodViewModel()
        {
            DateFrom = DateTime.Now.AddMonths(-1);
            DateTo = DateTime.Now;
            TransactionType = TransactionTypeList[0];

            PeriodList = new ObservableCollection<Const.ReportPeriod> { Const.ReportPeriod.Daily, Const.ReportPeriod.Monthly, Const.ReportPeriod.Yearly };
            Period = PeriodList[0];
        }
    }
}