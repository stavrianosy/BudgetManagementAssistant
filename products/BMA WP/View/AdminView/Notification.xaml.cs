﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA_WP.ViewModel.Admin;
using BMA_WP.Resources;
using BMA.BusinessLogic;
using System.Windows.Media;

namespace BMA_WP.View.AdminView
{
    public partial class Notification : PhoneApplicationPage
    {
        #region Private Members
        ApplicationBarIconButton save;
        ApplicationBarIconButton delete;
        ApplicationBarIconButton add;
        ApplicationBarMenuItem mainMenu;
        ApplicationBarMenuItem transaction;
        #endregion

        #region Public Properties
        public NotificationViewModel vm
        {
            get { return (NotificationViewModel)DataContext; }
        }
        #endregion

        #region Constructor
        public Notification()
        {
            InitializeComponent();
        }
        #endregion

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string piName = (e.AddedItems[0] as PivotItem).Name;

            switch (piName)
            {
                case "piNotification":
                    SetupAppBar_Notification();
                    ItemSelected();
                    svItem.ScrollToVerticalOffset(0d);
                    break;
                case "piNotificationList":
                    SetupAppBar_NotificationList();
                    NotificationsMultiSelect.SelectedItem = null;
                    vm.CurrNotification = null;
                    break;
            }
        }

        private void ItemSelected()
        {
            var notification = (BMA.BusinessLogic.Notification)NotificationsMultiSelect.SelectedItem;

            vm.CurrNotification = notification;

            if (vm.CurrNotification == null || vm.CurrNotification.IsDeleted)
            {
                vm.IsEnabled = false;
            }
            else
            {
                vm.CurrNotification.PropertyChanged += (o, changedEventArgs) => save.IsEnabled = vm.Notifications.HasItemsWithChanges();

                vm.IsEnabled = !vm.IsLoading;
                delete.IsEnabled = !vm.IsLoading;
            }
        }

        private void SetupAppBar_NotificationList()
        {
            SetupAppBar_Common(false);
        }

        private void SetupAppBar_Notification()
        {
            SetupAppBar_Common(true);
        }

        void SetupAppBar_Common(bool includeDelete)
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/save.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSave;
            save.IsEnabled = vm.Notifications.HasItemsWithChanges() && vm.IsLoading == false; ;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);

            if (includeDelete)
            {
                delete = new ApplicationBarIconButton();
                delete.IconUri = new Uri("/Assets/icons/Dark/delete.png", UriKind.Relative);
                delete.Text = AppResources.AppBarButtonDelete;
                delete.IsEnabled = false;
                ApplicationBar.Buttons.Add(delete);
                delete.Click += new EventHandler(Delete_Click);
            }

            add = new ApplicationBarIconButton();
            add.IconUri = new Uri("/Assets/icons/Dark/add.png", UriKind.Relative);
            add.Text = AppResources.AppBarButtonAdd;
            add.IsEnabled = true;
            ApplicationBar.Buttons.Add(add);
            add.Click += new EventHandler(Add_Click);

            mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            mainMenu.IsEnabled = true;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);

            transaction = new ApplicationBarMenuItem();
            transaction.Text = AppResources.AppBarButtonTransaction;
            transaction.IsEnabled = true;
            ApplicationBar.MenuItems.Add(transaction);
            transaction.Click += new EventHandler(Transaction_Click);
        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
        }

        private void Transaction_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Transactions.xaml", UriKind.Relative));
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(AppResources.DeleteMessage, AppResources.ConfirmDeletion, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                vm.CurrNotification.IsDeleted = true;
                vm.PivotIndex = 1;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (!ValidateNotification())
                return;

            SaveNotification();
        }

        private void SaveNotification()
        {
            vm.IsLoading = true;

            var saveOC = vm.Notifications.Where(t => t.HasChanges).ToObservableCollection();

             App.Instance.StaticServiceData.SaveNotification(saveOC, (error) =>
            {
                if (error != null)
                    MessageBox.Show(AppResources.SaveFailed);

                vm.IsLoading = false;

            });

            pivotContainer.SelectedIndex = 1;
            save.IsEnabled = vm.Notifications.HasItemsWithChanges() && vm.IsLoading == false;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (vm.IsLoading)
            {
                MessageBox.Show(AppResources.BusySynchronizing);
                return;
            }

            if (!ValidateNotification())
                return;

            BMA.BusinessLogic.Notification item = new BMA.BusinessLogic.Notification(App.Instance.User);

            vm.PivotIndex = 0;

            svItem.ScrollToVerticalOffset(0d);

            vm.Notifications.Add(item);
            NotificationsMultiSelect.SelectedItem = item;
            vm.CurrNotification = item;

            save.IsEnabled = vm.Notifications.HasItemsWithChanges() && vm.IsLoading == false;
            delete.IsEnabled = !vm.IsLoading;
            vm.IsEnabled = !vm.IsLoading;
        }

        private bool ValidateNotification()
        {
            var result = true;
            if (vm.CurrNotification == null)
                return result;

            SolidColorBrush okColor = new SolidColorBrush(new Color() { A = 255, B = 255, G = 255, R = 255 });
            SolidColorBrush errColor = new SolidColorBrush(new Color() { A = 255, B = 75, G = 75, R = 240 });

            txtName.Background = okColor;

            if (vm.CurrNotification.Name.Trim().Length == 0)
            {
                result = false;
                txtName.Background = errColor;
            }

            if (!result)
                svItem.ScrollToVerticalOffset(0);
            else
            {
                var tempNotification = vm.Notifications.Where(x => !x.IsDeleted && vm.CurrNotification.Name.Trim().Length == 0).ToList();
                if (tempNotification.Count > 0)
                {
                    result = false;
                    //for more specific message
                    if (tempNotification.Count == 1)
                        MessageBox.Show(string.Format("There is another notification that failed validation.\nUpdate it from the list and save again."));
                    else
                        MessageBox.Show(string.Format("There are another {0} notifications that failed validation.\nUpdate them from the list and save again.", tempNotification.Count));
                }
            }

            return result;
        }
    }
}