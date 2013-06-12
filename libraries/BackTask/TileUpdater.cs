using BMA.BusinessLogic;
using NotificationsExtensions.ToastContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Popups;

namespace BackTask
{
    public sealed class TileUpdater : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {

            var defferal = taskInstance.GetDeferral();

            await GetUpcomingNotifications();

            defferal.Complete();
        }

        /// <summary>
        /// Get the notifications for the nex 15 minutes
        /// </summary>
        async Task GetUpcomingNotifications()
        {
            try
            {
                var client = new BMAStaticDataService.StaticClient();
                var result = await client.GetUpcomingNotificationsAsync(DateTime.Now);

                if (result.Count == 0)
                    return;

                List<int> durationList = new List<int>();
                foreach (var item in result)
                {
                    TimeSpan ts1 = new TimeSpan(item.Time.Hour, item.Time.Minute, 0);
                    TimeSpan ts2 = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                    
                    int duration = Convert.ToInt32(Math.Round(ts1.Subtract(ts2).Duration().TotalMinutes, MidpointRounding.AwayFromZero));

                    durationList.Add(duration);
                }


                if (durationList.Count() > 0)
                {
                    ShowNotification(durationList);
                    SetupTiles(durationList);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        void ShowNotification(List<int> durationList)
        {
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            

            foreach (var item in durationList)
            {
                DateTime d = DateTime.Now.AddMinutes(item);
                ToastNotificationManager.CreateToastNotifier().AddToSchedule(new ScheduledToastNotification(DisplayLongToast(), new DateTimeOffset(d)));
            } 
        }

        XmlDocument DisplayLongToast()
        {
            bool loopAudio = false;
            IToastText02 toastContent = ToastContentFactory.CreateToastText02();

            // Toasts can optionally be set to long duration
            toastContent.Duration = ToastDuration.Long;

            toastContent.TextHeading.Text = "BMA Notification Reminder: Capture all transactions of the day";

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

        }

        private XmlDocument UpdateBadge(int msgCount)
        {
            BadgeTemplateType templateType = BadgeTemplateType.BadgeGlyph;

            XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(templateType);
            ((XmlElement)badgeXml.GetElementsByTagName("badge")[0]).SetAttribute("value", "away");

            BadgeUpdateManager.CreateBadgeUpdaterForApplication().Update(new BadgeNotification(badgeXml));

            return badgeXml;
        }

        void SetupTiles(List<int> durationList)
        {
            try
            {
                if (durationList.Count > 0)
                {
                    DateTimeOffset time = DateTime.Now.AddMinutes(durationList[0]);

                    //Windows.UI.Notifications.ScheduledToastNotification stf = new ScheduledToastNotification(GetTile(durationList.Count.ToString()), time);
                    //time.Add(new TimeSpan(0, 0, 0, durationList[0], 0));
                    //TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(stf);

                    Windows.UI.Notifications.ScheduledTileNotification stf = new ScheduledTileNotification(GetTile(durationList.Count.ToString()), time);
                    //time.Add(new TimeSpan(0, 0, 0, durationList[0], 0));
                    TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(stf);
                }
            }
            catch (Exception ex)
            {
                //new MessageDialog(ex.Message).ShowAsync();
            }
        }

        XmlDocument GetTile(string Message)
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideSmallImageAndText01);
            string t = tileXml.ToString();

            XmlNodeList tileTextAttributes = tileXml.GetElementsByTagName("text");
            tileTextAttributes[0].InnerText = "Alert:" + Message;

            //XmlNodeList tileBranding = tileXml.GetElementsByTagName("visual");
            //tileBranding[0].InnerText = "logo";

            XmlDocument squareTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText04);

            XmlNodeList squareTileTextAttributes = squareTileXml.GetElementsByTagName("text");
            squareTileTextAttributes[0].AppendChild(squareTileXml.CreateTextNode("Alert: " + Message));

            IXmlNode node = tileXml.ImportNode(squareTileXml.GetElementsByTagName("binding").Item(0), true);
            tileXml.GetElementsByTagName("visual").Item(0).AppendChild(node);

            return tileXml;
        }
    }
}
