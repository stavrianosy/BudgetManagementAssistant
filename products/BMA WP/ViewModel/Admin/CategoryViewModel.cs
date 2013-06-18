using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BMA_WP.ViewModel.Admin
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class CategoryViewModel : ViewModelBase
    {
        #region Private Members
        private bool isEnabled;
        private Category currCategory;
        private int pivotIndex;
        #endregion

        #region Public Properties
        public bool IsEnabled { get { return isEnabled; } set { isEnabled = value; RaisePropertyChanged("IsEnabled"); } }
        public Category CurrCategory { get { return currCategory; } set { currCategory = value; RaisePropertyChanged("CurrCategory"); } } 
        public ObservableCollection<Category> CategoryList { get { return App.Instance.StaticServiceData.CategoryList; } }
        public ObservableCollection<TypeTransactionReason> TransactionReasonList { get { return App.Instance.StaticServiceData.TypeTransactionReasonList; } }
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }
        #endregion

        #region Event To Commands
        public ICommand Categories_SelectionChanged
        {
            get
            {
                return new RelayCommand<object>((param) =>
                {
                    PivotIndex = 0;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the CategoryViewModel class.
        /// </summary>
        public CategoryViewModel()
        {
            IsEnabled = false;
            PivotIndex = 1;
        }
    }
}