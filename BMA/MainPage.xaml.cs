using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using BMA.BusinessLogic;

using BMA.Pages.TransactionPage;
using BMA.Pages.BudgetPage;
using BMA.Pages.AdminPage;
using BMA.Pages.Reports;
using Windows.UI.Notifications;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Popups;
using NotificationsExtensions.ToastContent;
using Windows.Networking.Connectivity;
using System.Threading.Tasks;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BMA
{
    public class Item
    {
        public DateTimeOffset DeliveryTime { get; set; }
    } 

    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : BMA.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private const string TASK_NAME = "TileUpdater";
        private const string TASK_ENTRY = "BackTask.TileUpdater";

        private async Task UpdateTile()
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();
            if (result == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                result == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == TASK_NAME)
                        task.Value.Unregister(true);

                    //task.Value.Progress += new BackgroundTaskProgressEventHandler(OnProgress);
                    //task.Value.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
                }

                BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                var condition = new SystemCondition(SystemConditionType.InternetAvailable);

                builder.Name = TASK_NAME;
                builder.AddCondition(condition);
                builder.TaskEntryPoint = TASK_ENTRY;
                builder.SetTrigger(new TimeTrigger(15, false));
                var registration = builder.Register();

                registration.Progress += new BackgroundTaskProgressEventHandler(task_Progress);
                registration.Completed += new BackgroundTaskCompletedEventHandler(task_Completed);

            }
        }

        private void task_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void task_Progress(BackgroundTaskRegistration sender, BackgroundTaskProgressEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private XmlDocument UpdateBadge()
        {

            int itemCount = App.Instance.TransDataSource.TransactionList.Count;

            BadgeTemplateType templateType = itemCount > 10
                ? BadgeTemplateType.BadgeGlyph : BadgeTemplateType.BadgeNumber;

            XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(templateType);
            ((XmlElement)badgeXml.GetElementsByTagName("badge")[0]).SetAttribute("value",
                (itemCount > 10) ? "alert" : DateTime.Now.Second.ToString());

            for (int i = 0; i < 5; i++)
            {
                BadgeUpdateManager.CreateBadgeUpdaterForApplication()
                    .Update(new BadgeNotification(badgeXml));
            }
            return badgeXml;

        }

        XmlDocument GetTile(string Message)
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideText05);
            string t = tileXml.ToString();

            XmlNodeList tileTextAttributes = tileXml.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = Message;

            XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText04);

            XmlNodeList squareTileTextAttributes = squareTileXml.GetElementsByTagName("text");
            squareTileTextAttributes[0].AppendChild(squareTileXml.CreateTextNode(Message));

            IXmlNode node = tileXml.ImportNode(squareTileXml.GetElementsByTagName("binding").Item(0), true);
            tileXml.GetElementsByTagName("visual").Item(0).AppendChild(node);

            return tileXml;
        }

        private void SetupTiles()
        {
            try
            {
                DateTimeOffset time = DateTime.Now.AddSeconds(30);

                for (int i = 0; i < 2; i++)
                {
                    Windows.UI.Notifications.ScheduledTileNotification stf = new ScheduledTileNotification(GetTile(i.ToString()), time);
                    time.Add(new TimeSpan(0, 0, 0, 30, 0));
                    TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(stf);
                }
            }
            catch (Exception ex)
            {
                new MessageDialog(ex.Message).ShowAsync();
            }
        }

        private void UpdateList()
        {
            List<Item> items = new List<Item>();
            foreach (var n in ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications())
            {
                items.Add(new Item { DeliveryTime = n.DeliveryTime });
            }
        }

        ToastNotification scenario6Toast = null;

        XmlDocument DisplayLongToast()
        {
            bool loopAudio = false;
            IToastText02 toastContent = ToastContentFactory.CreateToastText02();

            // Toasts can optionally be set to long duration
            toastContent.Duration = ToastDuration.Long;

            toastContent.TextHeading.Text = "Long Duration Toast";

            if (loopAudio)
            {
                toastContent.Audio.Loop = true;
                toastContent.Audio.Content = ToastAudioContent.LoopingAlarm;
                toastContent.TextBodyWrap.Text = "Looping audio";
            }
            else
            {
                toastContent.Audio.Content = ToastAudioContent.IM;
            }

            XmlDocument x = new XmlDocument();
            x.LoadXml(toastContent.GetContent());
            return x;

            //scenario6Toast = toastContent.CreateNotification();
            //ToastNotificationManager.CreateToastNotifier().Show(scenario6Toast);

            //rootPage.NotifyUser(toastContent.GetContent(), NotifyType.StatusMessage);
        }

        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            await SyncData();                

            App.Instance.Share = null;
            DefaultViewModel["CountBudgets"] = App.Instance.TransDataSource.BudgetList.Count;
            DefaultViewModel["CountTransactions"] = App.Instance.TransDataSource.TransactionList.Count;

            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            await UpdateTile();

            //UpdateBadge();
            //SetupTiles();
            //groupGridView.DataContext = groupedItemsViewSource.View.CollectionGroups;
        }

        async Task SyncData()
        {
            if (!App.Instance.IsOnline)
            {
                txtMessage.Text = string.Format("You are not online. Data are displayed from local cache.");
            }
            else if (App.Instance.PendingSync)
            {
                await App.Instance.TransDataSource.SyncTransactions();
                await App.Instance.TransDataSource.SyncBudgets();
                //await App.Instance.StaticDataSource.LoadStaticData();

                txtMessage.Text = string.Format("Last update: {0:dd/MM/yyyy hh:mm:ss}", DateTime.Now);
            }

        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void brdTransactions_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;
            var group = frameworkElement.DataContext;

            foreach (var trans in App.Instance.TransDataSource.TransactionList)
            {
                //Progress.IsActive = true;
                //ProgressText.Text = "Loading transactions " + trans.TypeTransaction;
                //await App.Instance.DataSource.LoadAllItems(trans);
            }

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            Frame.Navigate(typeof(TransactionItemsPage));
        }

        private async void brdBudgets_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;
            var group = frameworkElement.DataContext;

            foreach (var trans in App.Instance.TransDataSource.BudgetList)
            {
                //Progress.IsActive = true;
                //ProgressText.Text = "Loading transactions " + trans.TypeTransaction;
                //await App.Instance.DataSource.LoadAllItems(trans);
            }

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            Frame.Navigate(typeof(ItemsPage));
        }

        private void brdTargets_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null) return;
            var group = frameworkElement.DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
           // Frame.Navigate(typeof(GroupDetailPage), ((Transaction)group).TransactionId);
        }

        private void brdIntervals_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(IntervalItemsPage));
        }

        private void brdNotifications_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(NotificationItemsPage));
        }

        private void brdSecurity_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SecurityItemsPage));

        }

        private void brdExpenseReason_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ReasonItemsPage));
        }

        private void brdCategories_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CategoryItemsPage));
        }

        private void brdTransactionTypes_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TypeTransactionItemsPage));
        }

        private void brdFrequencies_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TypeFrequencyItemsPage));
        }

        private void brdBudgetThreshold_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(BudgetThresholdItemsPage));
        }

        private void brdIntervalType_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TypeIntervalItemsPage));
        }

        private void brdReports_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(ReportList));
        }

    }
}
