using BMA_WP.Model;
using BMA_WP.Resources;
using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace BMA_WP.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        List<string> titles = new List<string> { AppResources.Login, AppResources.Register, AppResources.ForgotPassword };
        StaticServiceData.ServerStatus status;
        
        public List<string> Titles
        {
            get
            {
                return titles;
            }
        }

        public bool IsLoading { get { return App.Instance.IsSyncing; } set { App.Instance.IsSyncing = value; } }

        public StaticServiceData.ServerStatus Status { get { return status; } set { status = value; RaisePropertyChanged("Status"); } }
        /// <summary>
        /// Initializes a new instance of the LoginViewModel class.
        /// </summary>
        public LoginViewModel()
        {
        }
    }
}