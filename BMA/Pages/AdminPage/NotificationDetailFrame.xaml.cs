using BMA.BusinessLogic;
using BMA.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BMA.Pages.AdminPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotificationDetailFrame : LayoutAwarePage
    {
        Notification currNotification;

        public NotificationDetailFrame()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            App.Instance.Share = null;

            currNotification = navigationParameter as Notification;

            DefaultViewModel["Notification"] = currNotification;

            this.IsEnabled = currNotification != null;

            InitControls();

            this.UpdateLayout();
        }

        private void InitControls()
        {
            if (currNotification == null)
                return;

            DateTime dateNow = currNotification == null ? new DateTime() : currNotification.Time;

            for (int i = 0; i < 24; i++)
            {
                int itemHour = i;

                ComboBoxItem cbItem = new ComboBoxItem();

                cbItem.Content = string.Format("{0:00}", itemHour);
                cbItem.Tag = itemHour;

                cbItem.IsSelected = itemHour == dateNow.Hour;

                cbHour.Items.Add(cbItem);
            }

            for (int i = 0; i < 60; i++)
            {
                int itemMinute = i;

                ComboBoxItem cbItem = new ComboBoxItem();

                cbItem.Content = string.Format("{0:00}", itemMinute);
                cbItem.Tag = itemMinute;

                cbItem.IsSelected = dateNow.Minute == itemMinute;

                cbMinute.Items.Add(cbItem);
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            currNotification.Name = txtName.Text;
        }

        private void txtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            currNotification.Description = txtDescription.Text;
        }

        private void cbHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currNotification == null)
                return;

            int tagH = (int)(cbHour.SelectedItem as ComboBoxItem).Tag;
            int tagM = currNotification.Time.Minute;

            currNotification.Time = new DateTime(2000, 1, 1, tagH, tagM, 0);
        }

        private void cbMinute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currNotification == null)
                return;

            int tagH = currNotification.Time.Hour;
            int tagM = (int)(cbMinute.SelectedItem as ComboBoxItem).Tag;

            currNotification.Time = new DateTime(2000, 1, 1, tagH, tagM, 0);
        }
    }
}
