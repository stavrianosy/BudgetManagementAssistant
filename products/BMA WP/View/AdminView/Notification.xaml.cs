using System;
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
using System.Windows.Data;
using BMA_WP.Model;

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

            SetupLoadingBinding();
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!App.Instance.IsAuthorized)
                NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.Relative));
        }

        #region Binding
        private void SetupLoadingBinding()
        {
            Binding bind = new Binding("IsBusyComm");
            bind.Mode = BindingMode.TwoWay;
            bind.Source = App.Instance;

            bind.Converter = new StatusConverter();
            bind.ConverterParameter = "trueVisible";

            spLoading.SetBinding(StackPanel.VisibilityProperty, bind);
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
            NotificationList deletedList = new NotificationList();
            vm.Notifications.Where(x => x.IsDeleted).ToList().ForEach(x=>
            {
                deletedList.Add(x);
            });

             App.Instance.StaticServiceData.SaveNotification(saveOC, (error) =>
            {
                if (error != null)
                    MessageBox.Show(AppResources.SaveFailed);
                else
                {
                    Model.Reminders.DeleteReminder(deletedList);
                    Model.Reminders.SetupReminders();
                }

                vm.IsLoading = false;

            });

            pivotContainer.SelectedIndex = 1;
            save.IsEnabled = vm.Notifications.HasItemsWithChanges() && vm.IsLoading == false;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (!ValidateNotification())
                return;

            BMA.BusinessLogic.Notification item = new BMA.BusinessLogic.Notification(App.Instance.User);

            if (vm.Notifications.Where(x => !x.IsDeleted).Count() >= NotificationList.DEVICE_MAX_COUNT)
            {
                MessageBox.Show(string.Format(AppResources.MaxItemsCount, NotificationList.DEVICE_MAX_COUNT));
                return;
            }

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

            result = ValidateSingleNotification() && ValidateAllNotification();

            if (vm.IsLoading)
            {
                MessageBox.Show(AppResources.BusySynchronizing);
                result = false;
            }

            return result;
        }

        private bool ValidateSingleNotification()
        {
            var result = true;
            if (vm.CurrNotification == null)
                return result;

            SolidColorBrush okColor = new SolidColorBrush(new Color() { A = 255, B = 255, G = 255, R = 255 });
            SolidColorBrush errColor = new SolidColorBrush(new Color() { A = 255, B = 75, G = 75, R = 240 });

            txtName.Background = okColor;

            if (vm.CurrNotification.Name != null && vm.CurrNotification.Name.Trim().Length == 0)
            {
                result = false;
                txtName.Background = errColor;
            }

            if (vm.CurrNotification.Name != null)
            {
                var nameExists = vm.Notifications.FirstOrDefault(x => x.Name.Trim().Equals(vm.CurrNotification.Name.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                                                                            x.NotificationId != vm.CurrNotification.NotificationId && !x.IsDeleted);
                if (nameExists != null)
                {
                    result = false;
                    MessageBox.Show(AppResources.NameAlreadyExist);
                }
            }

            if (!result)
                svItem.ScrollToVerticalOffset(0);

            return result;
        }

        public bool ValidateAllNotification()
        {
            var result = true;

            var tempNotificationCount = vm.Notifications.Where(x => !x.IsDeleted && (x.Name == null || x.Name.Trim().Length == 0)).Count();

            var tempNotificationNameExists = (from i in vm.Notifications
                                             where i.Name != null && !i.IsDeleted
                                             group i by i.Name.Trim().ToLower() into g
                                             select new { item = g.Key, count = g.Count() }).Where(x => x.count > 1).Count();

            if (tempNotificationNameExists > 0)
            {
                result = false;
                MessageBox.Show(string.Format(AppResources.FaildValidationNameExists, AppResources.Notifications));
            }
            else if (tempNotificationCount > 0)
            {
                result = false;
                //for more specific message
                if (tempNotificationCount == 1)
                    MessageBox.Show(string.Format(AppResources.FaildValidationSingle, AppResources.Notification));
                else
                    MessageBox.Show(string.Format(AppResources.FaildValidation, tempNotificationCount, AppResources.Notifications));
            }

            return result;
        }

    }
}