using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
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
    public class TransactionPlaceViewModel : ViewModelBase
    {
        #region Private Members
        private ObservableCollection<KeyValuePair<string, double>> reportResult;
        private int pivotIndex;
        bool isSortByAmount;
        #endregion

        #region Public Properties
        public bool IsLoading { get { return App.Instance.IsSyncing; } set { App.Instance.IsSyncing = value; } }
        public bool IsSortByAmount { get { return isSortByAmount; } set { isSortByAmount = value; RaisePropertyChanged("IsSortByAmount"); } }
        public ObservableCollection<KeyValuePair<string, double>> ReportResult { get { return reportResult; } set { reportResult = value; RaisePropertyChanged("ReportResult"); } }
        public TypeTransactionList TransactionTypeList { get { return App.Instance.StaticServiceData.TypeTransactionList; } }
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }
        #endregion
        /// <summary>
        /// Initializes a new instance of the TransactionPlaceViewModel class.
        /// </summary>
        public TransactionPlaceViewModel()
        {
        }
    }
}