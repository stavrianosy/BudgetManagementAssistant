using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class NotificationViewModel : ViewModelBase
    {
        #region Private Members
        private bool _isEnabled;
        private Notification _currNotification;
        private int pivotIndex;
        #endregion

        #region Public Properties
        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; RaisePropertyChanged("IsEnabled"); } }
        public bool IsLoading { get { return App.Instance.IsSyncing; } set { App.Instance.IsSyncing = value; } }
        public Notification CurrNotification { get { return _currNotification; } set { _currNotification = value; RaisePropertyChanged("CurrNotification"); } }

        public NotificationList Notifications { get { return App.Instance.StaticServiceData.NotificationList; } }

        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }
        
        #endregion

        #region Event To Commands
        public ICommand Notifications_SelectionChanged
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
        /// Initializes a new instance of the NotificationViewModel class.
        /// </summary>
        public NotificationViewModel()
        {
            IsEnabled = false;
            PivotIndex = 1;
        }
    }
}