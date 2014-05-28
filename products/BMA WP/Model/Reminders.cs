using BMA.BusinessLogic;
using BMA_WP.Resources;
using Microsoft.Phone.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model
{
    public class Reminders
    {
        public static void SetupReminders()
        {
            try
            {

                DeleteReminder(App.Instance.StaticServiceData.NotificationList);
                foreach (var item in App.Instance.StaticServiceData.NotificationList)
                {
                    string reminderName = item.Name;

                    var beginTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 
                                                    item.Time.Hour, item.Time.Minute, item.Time.Second);

                    var reminder = ScheduledActionService.Find(reminderName);

                    reminder = new Reminder(reminderName)
                    {
                        //    // NOTE: setting the Title property is supported for reminders 
                        //    // in contrast to alarms where setting the Title property is not supported
                        Title = string.Format("{0} {1}\n{2}", AppResources.ReminderForUser, item.ModifiedUser.UserName, item.Name),
                        Content = item.Description ?? "",
                        //    //double.TryParse(this.txtSeconds.Text, out seconds);

                        //    //NOTE: the value of BeginTime must be after the current time
                        //    //set the BeginTime time property in order to specify when the reminder should be shown

                        BeginTime = beginTime,
                        //BeginTime = DateTime.Now.AddMinutes(1),

                        //    // NOTE: ExpirationTime must be after BeginTime
                        //    // the value of the ExpirationTime property specifies when the schedule of the reminder expires
                        //    // very useful for recurring reminders, ex:
                        //    // show reminder every day at 5PM but stop after 10 days from now
                        ExpirationTime = beginTime.AddDays(1).AddDays(7),
                        RecurrenceType = RecurrenceInterval.Daily
                    };

                    // NOTE: another difference from alerts
                    // you can set a navigation uri that is passed to the application when it is launched from the reminder
                    //reminder.NavigationUri = navigationUri;
                    ScheduledActionService.Add(reminder);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void DeleteReminder(NotificationList notificationList)
        {
            foreach (var item in notificationList)
            {
                var reminder = ScheduledActionService.Find(item.Name);
                if (reminder != null)
                    ScheduledActionService.Remove(item.Name);
            }
        }
    }
}
