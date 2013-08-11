using System;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA_WP.Resources;
using BMA_WP.Model;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Microsoft.Phone.Net.NetworkInformation;
using System.IO.IsolatedStorage;
using BMA.BusinessLogic;
using BMA_WP.Common;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BMA_WP
{
    public partial class App : Application, INotifyPropertyChanged
    {

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        private const string SETTINGS_FOLDER = "settings";

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions.
            UnhandledException += Application_UnhandledException;

            // Standard XAML initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Language display initialization
            InitializeLanguage();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            Instance.ServiceData = new ServiceData();
            Instance.StaticServiceData = new StaticServiceData();

            Instance.User = new User();

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            //// Ensure that application state is restored appropriately
            //if (!App.ViewModel.IsDataLoaded)
            //{
            //    App.ViewModel.LoadData();
            //}
            
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new TransitionFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Handle reset requests for clearing the backstack
            RootFrame.Navigated += CheckForResetNavigation;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // If the app has received a 'reset' navigation, then we need to check
            // on the next navigation to see if the page stack should be reset
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // Unregister the event so it doesn't get called again
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // Only clear the stack for 'new' (forward) and 'refresh' navigations
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // For UI consistency, clear the entire page stack
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // do nothing
            }
        }

        #endregion

        // Initialize the app's font and flow direction as defined in its localized resource strings.
        //
        // To ensure that the font of your application is aligned with its supported languages and that the
        // FlowDirection for each of those languages follows its traditional direction, ResourceLanguage
        // and ResourceFlowDirection should be initialized in each resx file to match these values with that
        // file's culture. For example:
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage's value should be "es-ES"
        //    ResourceFlowDirection's value should be "LeftToRight"
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage's value should be "ar-SA"
        //     ResourceFlowDirection's value should be "RightToLeft"
        //
        // For more info on localizing Windows Phone apps see http://go.microsoft.com/fwlink/?LinkId=262072.
        //
        private void InitializeLanguage()
        {
            try
            {
                // Set the font to match the display language defined by the
                // ResourceLanguage resource string for each supported language.
                //
                // Fall back to the font of the neutral language if the Display
                // language of the phone is not supported.
                //
                // If a compiler error is hit then ResourceLanguage is missing from
                // the resource file.
                RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

                // Set the FlowDirection of all elements under the root frame based
                // on the ResourceFlowDirection resource string for each
                // supported language.
                //
                // If a compiler error is hit then ResourceFlowDirection is missing from
                // the resource file.
                FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection), AppResources.ResourceFlowDirection);
                RootFrame.FlowDirection = flow;
            }
            catch
            {
                // If an exception is caught here it is most likely due to either
                // ResourceLangauge not being correctly set to a supported language
                // code or ResourceFlowDirection is set to a value other than LeftToRight
                // or RightToLeft.

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }

        public static App Instance
        {
            get { return ((App)Current); }
        }

        public ServiceData ServiceData { get; private set; }
        public StaticServiceData StaticServiceData { get; private set; }
        public User User { get; private set; }

        public StaticServiceData.ServerStatus StaticDataOnlineStatus { get; set; }
        public ServiceData.ServerStatus OnlineStatus { get; set; }

        public bool IsSyncing { get { return _isSyncing; } set { _isSyncing = value; OnPropertyChanged("IsSyncing"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public bool IsOnline
        {
            get
            {
                var isOnline = false;

                isOnline = DeviceNetworkInformation.IsNetworkAvailable;

                //custom way to check. MS suggests to use the DeviceNetworkInformation.IsNetworkAvailable
                //isOnline = App.Instance.StaticDataOnlineStatus != StaticServiceData.ServerStatus.Ok;

                return isOnline;
            }
        }

        bool _isUserAuthenticated;
        private bool _isSyncing;

        public bool IsInitialized
        {
            get
            {
                bool result = false;

                if (IsolatedStorageSettings.ApplicationSettings.Contains("IsInitialized"))
                    IsolatedStorageSettings.ApplicationSettings.TryGetValue("IsInitialized", out result);

                return result;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings["IsInitialized"] = value;
            }
        }

        public bool IsUserAuthenticated
        {
            get{return _isUserAuthenticated;}
            set { _isUserAuthenticated = value; }
        }

        public bool IsSync
        {
            get
            {
                bool result = false;

                if (IsolatedStorageSettings.ApplicationSettings.Contains("IsSync"))
                    IsolatedStorageSettings.ApplicationSettings.TryGetValue("IsSync", out result);

                return result;
            }
            set 
            {
                IsolatedStorageSettings.ApplicationSettings["IsSync"] = value;
                if (value)
                    LastSyncDate = DateTime.Now;
                    
            }
        }

        public DateTime LastSyncDate
        {
            get {

                DateTime result = DateTime.Now;

                if (IsolatedStorageSettings.ApplicationSettings.Contains("LastSyncDate"))
                    IsolatedStorageSettings.ApplicationSettings.TryGetValue("LastSyncDate", out result);

                return result;
            }
            private set { IsolatedStorageSettings.ApplicationSettings["LastSyncDate"] = value; }
        }

        public void Sync(Action callback)
        {
            try
            {
                var transRespont = false;
                var transSuccess = false;

                var budgetRespont = false;
                var budgetSuccess = false;

                var staticRespont = false;
                var staticSuccess = false;

                App.Instance.IsSyncing = true;

                SyncTransactions((isSuccess) => 
                { 
                    transRespont = true;
                    transSuccess = true;
                    if (ReadyToCallback(transRespont, budgetRespont, staticRespont))
                    {
                        UpdateSyncStatus(transSuccess, budgetSuccess, staticSuccess);
                        callback();
                    }
                });

                SyncBudgets((isSuccess) =>
                {
                    budgetRespont = true;
                    budgetSuccess = true;
                    if (ReadyToCallback(transRespont, budgetRespont, staticRespont))
                    {
                        UpdateSyncStatus(transSuccess, budgetSuccess, staticSuccess);
                        callback();
                    }
                });

                SyncStaticData((isSuccess) =>
                {
                    staticRespont = true;
                    staticSuccess = true;
                    if (ReadyToCallback(transRespont, budgetRespont, staticRespont))
                    {
                        UpdateSyncStatus(transSuccess, budgetSuccess, staticSuccess);
                        callback();
                    }
                });

                //callback(true);
            }
            catch
            {
                //callback(false);
            }

        }

        private void UpdateSyncStatus(bool transSuccess, bool budgetSuccess, bool staticSuccess)
        {
            var result = transSuccess && budgetSuccess && staticSuccess;
            if (result)
            {
                App.Instance.IsSync = true;
            }
        }

        /// <summary>
        /// Return true only when all async calls return replied
        /// </summary>
        /// <param name="transRespont"></param>
        /// <param name="catRespont"></param>
        /// <param name="staticRespont"></param>
        /// <returns></returns>
        private bool ReadyToCallback(bool transRespont, bool catRespont, bool staticRespont)
        {
            var result = transRespont && catRespont && staticRespont;

            return result;
        }

        public void SyncTransactions(Action<bool> callback)
        {
            App.Instance.ServiceData.SyncTransactions((transError) => callback(transError == null));
        }

        public void SyncBudgets(Action<bool> callback)
        {
            callback(true);
            //App.Instance.ServiceData.SyncBudgets((transError) => callback(transError == null));
        }

        public void SyncStaticData(Action<bool> callback)
        {
            callback(true);
        }
    }
}