using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace BMA_WP.ViewModel.Admin
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class IntervalViewModel : ViewModelBase
    {
        public ObservableCollection<TypeInterval> TypeIntervalList { get { return App.Instance.StaticServiceData.IntervalList; } }
        public ObservableCollection<TypeTransaction> TypeTransactionList { get { return App.Instance.StaticServiceData.TypeTransactionList; } }
        /// <summary>
        /// Initializes a new instance of the IntervalViewModel class.
        /// </summary>
        public IntervalViewModel()
        {
        }
    }
}