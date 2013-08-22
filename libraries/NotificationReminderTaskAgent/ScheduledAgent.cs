#define DEBUG_AGENT

using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using System;
using BMA.BusinessLogic;
using Microsoft.Phone.Net.NetworkInformation;
using BMA.Proxy.BMAStaticDataService;
using BMA_WP.Common;

namespace NotificationReminderTaskAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        private const string STATIC_NOTIFICATION_FOLDER = "Static_Notification";

        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background
            LoadNotifications((notificationsList, error) =>
                {
                    if (error == null)
                    {
                        
                        //string toastMessage = "Periodic task running.";

                        //ShellToast toast = new ShellToast();
                        //toast.Title = string.Format("{0} Background Agent Sample", notificationsList.Count);
                        //toast.Content = toastMessage;
                        //toast.Show();

#if DEBUG_AGENT
                        ScheduledActionService.LaunchForTest(task.Name, TimeSpan.FromSeconds(20));
#endif
                    }
                    NotifyComplete();
                });
        }

        public void LoadNotifications(Action<NotificationList, Exception> callback)
        {
            GetServerStatus(error =>
                {
                    if (error == null)
                        LoadLiveNotifications((notificationsList, errorCall) => callback(notificationsList, errorCall));
                    else
                        LoadCachedNotifications((notificationsList, errorCall) => callback(notificationsList, errorCall));
                });
        }

        private void LoadLiveNotifications(Action<NotificationList, Exception> callback)
        {
            var client = new StaticClient();

            client.GetAllNotificationsAsync(-1);
            client.GetAllNotificationsCompleted += (o, e) =>
            {
                //if (e.Error == null)
                //    SetupNotificationData(e.Result, true);
                if (e.Error == null)
                {
                    var notificationList = new NotificationList();
                    foreach (var item in e.Result)
                        notificationList.Add(item);

                    callback(notificationList, null);
                }
                else
                    callback(null, e.Error);
            };
        }

        private async void LoadCachedNotifications(Action<NotificationList, Exception> callback)
        {
            var retVal = new NotificationList();
            try
            {
                foreach (var item in await StorageUtility.ListItems(STATIC_NOTIFICATION_FOLDER, ""))
                {

                    var staticType = await StorageUtility.RestoreItem<BMA.BusinessLogic.Notification>(STATIC_NOTIFICATION_FOLDER, item, "");
                    retVal.Add(staticType);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            callback(retVal, null);
        }

        public void GetServerStatus(Action<Exception> callback)
        {
            //var result = ServerStatus.Communicating;
            try
            {
                if (DeviceNetworkInformation.IsNetworkAvailable)
                {
                    var client = new StaticClient();
                    client.GetDBStatusAsync();
                    client.GetDBStatusCompleted += (sender, e) =>
                    {
                        callback(e.Error);
                    };
                }
                else
                {
                    callback(new Exception("Offline"));
                }
            }
            catch (Exception)
            {
                //throw;
                callback(new Exception("Error"));
            }
        }
    }
}