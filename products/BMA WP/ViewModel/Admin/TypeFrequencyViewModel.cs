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
    public class TypeFrequencyViewModel : ViewModelBase
    {
        public TypeFrequencyList TypeIntervalList { get { return App.Instance.StaticServiceData.TypeFrequencyList; } }
        public TypeTransactionList TypeTransactionList { get { return App.Instance.StaticServiceData.TypeTransactionList; } }

        /// <summary>
        /// Initializes a new instance of the TypeFrequencyViewModel class.
        /// </summary>
        public TypeFrequencyViewModel()
        {
        }
    }
}