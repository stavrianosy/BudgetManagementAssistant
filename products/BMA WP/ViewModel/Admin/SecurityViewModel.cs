using GalaSoft.MvvmLight;
using System.ComponentModel;

namespace BMA_WP.ViewModel.Admin
{
    public class PasswordChange:INotifyPropertyChanged
    {
        string oldPass;
        string newPass;
        string confirmPass;
        string serverResponse;

        public string OldPass { get { return oldPass; } set { oldPass = value; OnPropertyChanged("OldPass"); } }
        public string NewPass { get { return oldPass; } set { oldPass = value; OnPropertyChanged("NewPass"); } }
        public string ConfirmPass { get { return oldPass; } set { oldPass = value; OnPropertyChanged("ConfirmPass"); } }
        public string ServerResponse { get { return serverResponse; } set { serverResponse = value; OnPropertyChanged("ServerResponse"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
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