using BMA.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace BMA
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Extended = false;
            InitializeComponent();
            Suspending += OnSuspending;
            RegisterChannel();
            Instance.DataSource = new TransDataSource();
        }

        public PushNotificationChannel Channel { get; private set; }
        public string ChannelError { get; private set; }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            RegisterSettings();
            ExtendedSplash(args.SplashScreen, args);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        void RegisterSettings()
        {
            var pane = SettingsPane.GetForCurrentView();
            pane.CommandsRequested += Pane_CommandsRequested;
        }

        void Pane_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            var aboutCommand = new SettingsCommand("About", "About", SettingsHandler);
            args.Request.ApplicationCommands.Add(aboutCommand);
        }
        
        void SettingsHandler(IUICommand command)
        {
            //_flyout = new SettingsFlyout
            //{
            //    HeaderText = "About",
            //    Content = new WintellogSettings(),
            //    IsOpen = true
            //};
            //_flyout.Closed += (o, e) => _flyout = null;
        }
        private static void ExtendedSplash(SplashScreen splashScreen, object args)
        {
            var splash = new SplashPage(splashScreen, args);
            splashScreen.Dismissed += splash.DismissedEventHandler;
            Window.Current.Content = splash;
            Window.Current.Activate();
        }

        private async void RegisterChannel()
        {
            try
            {
                Channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            }
            catch (Exception ex)
            {
                ChannelError = ex.Message;
            }
        }

        public static App Instance
        {
            get { return ((App)Current); }
        }
        public bool Extended { get; set; }

        public TransDataSource DataSource { get; private set; }

        public void RegisterForShare()
        {
            var dataManager = DataTransferManager.GetForCurrentView();
            dataManager.DataRequested += DataManager_DataRequested;
        }

        public Action<DataTransferManager, DataRequestedEventArgs> Share { get; set; }

        void DataManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (Share != null)
            {
                Share(sender, args);
            }
            else
            {
                args.Request
                    .FailWithDisplayText("Please choose a blog or item to enable sharing.");
            }
        }
    }
}
