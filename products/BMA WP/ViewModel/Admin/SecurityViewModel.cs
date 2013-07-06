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
        string oldPass;
        string newPass;
        string confirmPass;
        string serverResponse;
        User user;

        public string OldPass { get { return oldPass; } set { oldPass = value; RaisePropertyChanged("OldPass"); } }
        public string NewPass { get { return oldPass; } set { oldPass = value; RaisePropertyChanged("NewPass"); } }
        public string ConfirmPass { get { return oldPass; } set { oldPass = value; RaisePropertyChanged("ConfirmPass"); } }
        public string ServerResponse { get { return serverResponse; } set { serverResponse = value; RaisePropertyChanged("ServerResponse"); } }

        public User User { get { return user; } set { user = value; RaisePropertyChanged("User"); } }

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
        public SecurityViewModel()
        {
        }
    }
}