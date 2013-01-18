using BMA.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BMA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SplashPage : Page
    {
        private readonly SplashScreen _splash;
        private readonly LaunchActivatedEventArgs _activationArgs;
        private readonly SearchActivatedEventArgs _searchArgs;

        public SplashPage(SplashScreen splash, object args)
        {
            _splash = splash;

            if (args is LaunchActivatedEventArgs)
            {
                _activationArgs = args as LaunchActivatedEventArgs;
            }
            else
            {
                _searchArgs = args as SearchActivatedEventArgs;
            }

            Loaded += ExtendedSplashScreen_Loaded;
            Window.Current.SizeChanged += Current_SizeChanged;

            InitializeComponent();
            PositionElements();
            App.Instance.RegisterForShare();
        }


        private void PositionElements()
        {
            var x = _splash == null ? 2 : _splash.ImageLocation.X;
            var y = _splash == null ? 2 : _splash.ImageLocation.Y;
            var height = _splash == null ? 480 : _splash.ImageLocation.Height;
            var width = _splash == null ? 640 : _splash.ImageLocation.Width;

            SplashImage.SetValue(Canvas.LeftProperty, x);
            SplashImage.SetValue(Canvas.TopProperty, y);
            SplashImage.Height = height;
            SplashImage.Width = width;

            var topWithBuffer = y + height - 50;
            var textTop = topWithBuffer + 30;
            var left = x + 40;

            Progress.SetValue(Canvas.TopProperty, topWithBuffer);
            Progress.SetValue(Canvas.LeftProperty, left);
            Progress.IsActive = true;

            ProgressText.SetValue(Canvas.TopProperty, textTop - 15);
            ProgressText.SetValue(Canvas.LeftProperty, left + 90);

            ProgressText.Text = "Initializing...";
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (null != _splash)
            {
                // Re-position the extended splash screen image due to window resize event.
                PositionElements();
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        internal void DismissedEventHandler(SplashScreen sender, object e)
        {
        }

        async void ExtendedSplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            
            ProgressText.Text = ApplicationData.Current.LocalSettings.Values.ContainsKey("Initialized")
                   && (bool)ApplicationData.Current.LocalSettings.Values["Initialized"]
                                       ? "Loading blogs..."
                                       : "Initializing for first use: this may take several minutes...";

            await App.Instance.DataSource.LoadGroups();

            //foreach (var group in App.Instance.DataSource.GroupList)
            //{
            //    Progress.IsActive = true;
            //    ProgressText.Text = "Loading " + group.Title;
            //    await App.Instance.DataSource.LoadAllItems(group);
            //}

            ApplicationData.Current.LocalSettings.Values["Initialized"] = true;

            // Create a Frame to act as the navigation context and associate it with
            // a SuspensionManager key
            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");
        }
    }
}
