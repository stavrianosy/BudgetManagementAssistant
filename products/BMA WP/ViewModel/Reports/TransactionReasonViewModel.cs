using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
using System;
using System.Linq;
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
    public class TransactionReasonViewModel : ViewModelBase
    {
        #region Private Members
        private ObservableCollection<KeyValuePair<TypeTransactionReason, double>> reportResult;
        private int pivotIndex;
        bool isSortByAmount;

        DateTime dateFrom;
        DateTime dateTo;
        TypeTransaction transType;
        double amountFrom;
        double amountTo;
        double total;
        #endregion

        #region Public Properties
        public bool IsLoading { get { return App.Instance.IsBusyComm; } set { App.Instance.IsLoading = value; } }

        public bool IsSortByAmount { get { return isSortByAmount; } set { isSortByAmount = value; RaisePropertyChanged("IsSortByAmount"); } }
        public DateTime DateFrom { get { return dateFrom; } set { dateFrom = value; RaisePropertyChanged("DateFrom"); } }
        public DateTime DateTo { get { return dateTo; } set { dateTo = value; RaisePropertyChanged("DateTo"); } }
        public TypeTransaction TransactionType { get { return transType; } set { transType = value; RaisePropertyChanged("TransactionType"); } }
        public double AmountFrom { get { return amountFrom; } set { amountFrom = value; RaisePropertyChanged("AmountFrom"); } }
        public double AmountTo { get { return amountTo; } set { amountTo = value; RaisePropertyChanged("AmountTo"); } }

        public double Total { get { return total; } set { total = value; RaisePropertyChanged("Total"); } }

        public ObservableCollection<KeyValuePair<TypeTransactionReason, double>> ReportResult { get { return reportResult; } set { reportResult = value; RaisePropertyChanged("ReportResult"); } }
        public TypeTransactionList TransactionTypeList { get { return App.Instance.StaticServiceData.TypeTransactionList; } }
        public TypeTransactionReasonList TransactionReasonList { get { return App.Instance.StaticServiceData.TypeTransactionReasonList; } }
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }
        #endregion

        /// <summary>
        /// Initializes a new instance of the TransactionPeriodViewModel class.
        /// </summary>
        public TransactionReasonViewModel()
        {
            var now = DateTime.Now;

            var from = now.AddMonths(-1);
            var to = now.AddDays(1);

            DateFrom = new DateTime(from.Year, from.Month, from.Day, 0, 0, 0);
            DateTo = new DateTime(to.Year, to.Month, to.Day, 0, 0, 0);

            if (TransactionTypeList != null && TransactionTypeList.Count > 1)
            {
                var selected = TransactionTypeList.FirstOrDefault(x => x.Name.Equals("Expense", StringComparison.InvariantCultureIgnoreCase));
                TransactionType = selected;
            }
        }
    }
}