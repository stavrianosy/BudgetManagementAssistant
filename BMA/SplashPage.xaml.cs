using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using BMA.Common;

using BMA.Pages.TransactionPage;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

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

            flyLogin.LoginSuccess += flyLogin_LoginSuccess;

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

            var topWithBuffer = y + height - 25;
            var textTop = topWithBuffer + 30;
            var left = x + 40;

            Progress.SetValue(Canvas.TopProperty, topWithBuffer);
            Progress.SetValue(Canvas.LeftProperty, left);
            Progress.IsActive = true;

            ProgressText.SetValue(Canvas.TopProperty, textTop-10);
            ProgressText.SetValue(Canvas.LeftProperty, left + 80);

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

        void ExtendedSplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            
            ProgressText.Text = ApplicationData.Current.LocalSettings.Values.ContainsKey("Initialized")
                   && (bool)ApplicationData.Current.LocalSettings.Values["Initialized"]
                                       ? "Loading ..."
                                       : "Initializing for first use: this may take several minutes...";

            flyLogin.Visibility = Windows.UI.Xaml.Visibility.Visible;
            flyLogin.Width = canvasSplash.ActualWidth;
            //flyLogin.Height = canvasSplash.ActualHeight;           

            var verCenter = canvasSplash.ActualHeight / 2 - flyLogin.ActualHeight / 2;
            //var vertBottom = canvasSplash.ActualHeight - verCenter + ;
            
            flyLogin.Margin = new Thickness(0, verCenter+30, 0, 0);
            
        }

        void flyLogin_LoginSuccess()
        {
            ContinueLoading();
            flyLogin.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        async void ContinueLoading()
        {
            //await App.Instance.TransDataSource.LoadAllGroups();


            if (App.Instance.IsOnline && App.Instance.PendingSync)
            {
                ProgressText.Text = string.Format("Synchronizing Transactions...");
                await App.Instance.TransDataSource.SyncTransactions();

                ProgressText.Text = string.Format("Synchronizing Budgets...");
                await App.Instance.TransDataSource.SyncBudgets();

                //## YS Create Synchronization methods and replace load for Budget and static data.
                ProgressText.Text = string.Format("Synchronizing Settings...");
                await App.Instance.StaticDataSource.LoadStaticData();
            }
            else
            {
                ProgressText.Text = string.Format("Loading Transactions...");
                await App.Instance.TransDataSource.LoadTransactions();

                ProgressText.Text = string.Format("Loading Budgets...");
                await App.Instance.TransDataSource.LoadBudgets();

                ProgressText.Text = string.Format("Loading Settings...");
                await App.Instance.StaticDataSource.LoadStaticData();
            }

            ApplicationData.Current.LocalSettings.Values["Initialized"] = true;

            // Create a Frame to act as the navigation context and associate it with
            // a SuspensionManager key
            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

            var list = App.Instance.TransDataSource.BudgetList;

            var totalNew = list.Sum(g => g.BudgetId);

            if (totalNew > 99)
            {
                totalNew = 99;
            }

            if (totalNew > 0)
            {
                //## Budge update ##
                //var badgeContent = new BadgeNumericNotificationContent((uint)totalNew);
                //BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(badgeContent.CreateNotification());
            }
            else
            {
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
            }

            // load most recent 5 items then order from oldest to newest

            var query = from i in list
                        //(from g in list
                        // from i in g.Items
                        // orderby i.PostDate descending
                        // select i).Take(5)
                        orderby i.Name
                        select i;

            var x = 0;

            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);


            foreach (var item in query)
            {
                //var squareTile = new TileSquarePeekImageAndText04();
                //squareTile.TextBodyWrap.Text = item.Title;
                //squareTile.Image.Alt = item.Title;
                //squareTile.Image.Src = item.DefaultImageUri.ToString();

                //var wideTile = new TileWideSmallImageAndText03
                //{
                //    SquareContent = squareTile
                //};
                //wideTile.Image.Alt = item.Title;
                //wideTile.Image.Src = item.DefaultImageUri.ToString();
                //wideTile.TextBodyWrap.Text = item.Title;

                //var notification = wideTile.CreateNotification();
                //notification.Tag = string.Format("Item {0}", x++);
                //TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
            }

            Windows.ApplicationModel.Search.SearchPane.GetForCurrentView().SuggestionsRequested += SplashPage_SuggestionsRequested;

            App.Instance.Extended = true;

            if (_searchArgs != null)
            {
                //SearchResultsPage.Activate(_searchArgs);
                return;
            }

            if (_activationArgs != null)
            {
                if (_activationArgs.Arguments.StartsWith("Group"))
                {
                    var group = _activationArgs.Arguments.Split('=');
                    //rootFrame.Navigate(typeof(GroupDetailPage), group[1]);
                }
                else if (_activationArgs.Arguments.StartsWith("Item"))
                {
                    var item = _activationArgs.Arguments.Split('=');
                    //rootFrame.Navigate(typeof(ItemDetailPage), item[1]);
                }
                else if (_activationArgs.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    await SuspensionManager.RestoreAsync();
                }
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        void SplashPage_SuggestionsRequested(Windows.ApplicationModel.Search.SearchPane sender,
            Windows.ApplicationModel.Search.SearchPaneSuggestionsRequestedEventArgs args)
        {
            var query = args.QueryText.ToLower();

            if (query.Length < 3) return;

        }
    }
}
