using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using System.Windows.Input;

namespace BMA_WP.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TransactionsIntervalViewModel : ViewModelBase
    {
        #region Private Members
        bool _isEnabled;
        int pivotIndex;
        TransactionList transactionsIntervalSelectedList;
        #endregion

        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; RaisePropertyChanged("IsEnabled"); } }
        public bool IsLoading { get { return App.Instance.IsSyncing; } set { App.Instance.IsSyncing = value; } }

        public TransactionList TransactionsInterval { get { return App.Instance.ServiceData.IntervalTransactionList; } }
        public TransactionList TransactionsIntervalSelectedList { get { return transactionsIntervalSelectedList; } set { transactionsIntervalSelectedList = value; } }

        #region Event To Commands
        #endregion


        /// <summary>
        /// Initializes a new instance of the TransactionsIntervalViewModel class.
        /// </summary>
        public TransactionsIntervalViewModel()
        {
        }
    }
}