using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace BMA_WP.ViewModel.Admin
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ReasonViewModel : ViewModelBase
    {
        #region Private Members
        private bool isEnabled;
        private bool isLoading;
        private int pivotIndex;
        private TypeTransactionReason currTransactionReason;
        #endregion

        #region Public Properties
        public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; RaisePropertyChanged("IsEnabled"); } }
        public bool IsLoading { get { return App.Instance.IsSyncing; } set { App.Instance.IsSyncing = value; } }
        public TypeTransactionReason CurrTransactionReason { get { return currTransactionReason; } set { currTransactionReason = value; RaisePropertyChanged("CurrTransactionReason"); } }
        public TypeTransactionReasonList TransactionReasonList { get { return App.Instance.StaticServiceData.TypeTransactionReasonList; } }
        public ObservableCollection<Category> CategoryList { get { return App.Instance.StaticServiceData.CategoryList; } }
        
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }

        #endregion


        #region Event To Commands
        public ICommand Reasons_SelectionChanged
        {
            get
            {
                return new RelayCommand<object>((param) => 
                {
                    var selectedItem = (SelectionChangedEventArgs)param;
                    if (selectedItem.AddedItems[0] != null)
                        PivotIndex = 0;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the ReasonViewModel class.
        /// </summary>
        public ReasonViewModel()
        {
            IsEnabled = false;
            PivotIndex = 1;
        }
    }
}