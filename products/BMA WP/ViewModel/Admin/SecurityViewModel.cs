using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace BMA_WP.ViewModel.Admin
{
    public class PasswordChange : ViewModelBase
    {

    }
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SecurityViewModel : ViewModelBase
    {
        //string oldPass 
        /// <summary>
        /// Initializes a new instance of the SecurityViewModel class.
        /// </summary>
        string oldPass;
        string newPass;
        string confirmPass;
        string serverResponse;
        User user;
        int pivotIndex;
        private bool isLoading;

        public bool IsLoading { get { return App.Instance.IsBusyComm; } set { App.Instance.IsLoading = value; } }

        public string OldPass { get { return oldPass; } set { oldPass = value; RaisePropertyChanged("OldPass"); } }
        public string NewPass { get { return newPass; } set { newPass = value; User.Password = value; RaisePropertyChanged("NewPass"); } }
        public string ConfirmPass { get { return confirmPass; } set { confirmPass = value; RaisePropertyChanged("ConfirmPass"); } }
        public string ServerResponse { get { return serverResponse; } set { serverResponse = value; RaisePropertyChanged("ServerResponse"); } }

        public User User { get { return user; } set { user = value; RaisePropertyChanged("User"); } }
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }

        public SecurityViewModel()
        {
            PivotIndex = 0;
            User = new BMA.BusinessLogic.User();
            User.Update(App.Instance.User);
        }
    }
}