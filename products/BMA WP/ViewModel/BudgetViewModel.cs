using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace BMA_WP.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BudgetViewModel : ViewModelBase
    {
        #region Private Members
        private Budget _currBudget;
        private bool _isEnabled;
        private int pivotIndex;
        #endregion

        #region Public Properties
        public Budget CurrBudget { get { return _currBudget; } set { _currBudget = value; RaisePropertyChanged("CurrBudget"); } }
        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; RaisePropertyChanged("IsEnabled"); } }
        public BudgetList Budgets { get { return App.Instance.ServiceData.BudgetList; } }
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }
        #endregion

        #region Event To Commands
        public ICommand Budgets_SelectionChanged
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
        /// Initializes a new instance of the BudgetViewModel class.
        /// </summary>
        public BudgetViewModel()
        {
            App.Instance.ServiceData.LoadBudgets();

            //Display the list of budgets first
            PivotIndex = 1;
        }
    }
}