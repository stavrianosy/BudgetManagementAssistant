using BMA.BusinessLogic;
using BMA_WP.Resources;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
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
        List<int> months = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        List<int> days = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
        List<string> posDayOfMonth = new List<string> { AppResources.First, AppResources.Second, AppResources.Third, AppResources.Forth, AppResources.Last };
        List<string> weekDays = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        List<string> monthNames = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "Septeber", "October", "Nivember", "December" };

        public ObservableCollection<TypeInterval> TypeIntervalList { get { return App.Instance.StaticServiceData.IntervalList; } }
        public ObservableCollection<TypeTransaction> TypeTransactionList { get { return App.Instance.StaticServiceData.TypeTransactionList; } }

        public List<int> Months { get { return months; } }
        public List<int> Days { get { return months; } }
        public List<string> PosDayOfMonth { get { return posDayOfMonth; } }
        public List<string> WeekDays { get { return weekDays; } }
        public List<string> MonthNames { get { return monthNames; } }

        /// <summary>
        /// Initializes a new instance of the IntervalViewModel class.
        /// </summary>
        public IntervalViewModel()
        {
        }
    }
}