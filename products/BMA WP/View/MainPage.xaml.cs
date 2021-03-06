﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA_WP.ViewModel;
using System.Globalization;
using System.Threading;
using BMA_WP.Resources;
using System.Windows.Markup;
using BMA_WP.View;
using BMA_WP.Model;
using System.Threading.Tasks;
using System.Windows.Data;
using Microsoft.Advertising.Mobile.UI;

namespace BMA_WP.View
{
    public partial class MainPage : PhoneApplicationPage
    {
        private static MainPage MainPageObject;

        public MainPageViewModel vm
        {
            get { return (MainPageViewModel)this.DataContext; }
        }

        public MainPage()
        {
            InitializeComponent();
            
            MainPageObject = this;

            SetupAppBar();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null)
            {
                var login = NavigationService.BackStack.FirstOrDefault(x=>x.Source.OriginalString == "/View/Login.xaml");
                if(login != null)
                    NavigationService.RemoveBackEntry();

            }

            SetupLoadingBinding();

            CheckOnlineStatus(error => { });
        }

        private void CheckOnlineStatus(Action<Exception> callback)
        {
            App.Instance.StaticDataOnlineStatus = App.Instance.StaticServiceData.SetServerStatus(status =>
            {
                App.Instance.StaticDataOnlineStatus = status;
                vm.Status = status;
                if (status == Model.StaticServiceData.ServerStatus.Ok && !App.Instance.IsSync)
                {
                    if (!App.Instance.IsSyncing)
                    {
                        App.Instance.IsSyncing = true;
                        App.Instance.Sync(() =>
                        {
                            App.Instance.IsSyncing = false;
                            if (callback != null)
                                callback(null);
                        });
                    }
                }
                else
                {
                    callback(null);
                }
                
            });
            vm.Status = App.Instance.StaticDataOnlineStatus;
        }

        private void SetupLoadingBinding()
        {
            Binding bind = new Binding("IsBusyComm");
            bind.Mode = BindingMode.TwoWay;
            bind.Source = App.Instance;

            bind.Converter = new StatusConverter();
            bind.ConverterParameter = "trueVisible";

            spLoading.SetBinding(StackPanel.VisibilityProperty, bind);
        }

        private void SetupAppBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            SetupAppBar_Common();
        }

        void SetupAppBar_Common()
        {
            ApplicationBarMenuItem help = new ApplicationBarMenuItem();
            ApplicationBarMenuItem about = new ApplicationBarMenuItem();

            help.Text = AppResources.AppBarButtonHelp;
            ApplicationBar.MenuItems.Add(help);
            help.Click += new EventHandler(Help_Click);

            about.Text = AppResources.AppBarButtonAbout;
            ApplicationBar.MenuItems.Add(about);
            about.Click += new EventHandler(About_Click);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(!App.Instance.IsAuthorized)
                NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.Relative));
        }

        public static bool NavigateTo(string uriString, System.UriKind uriKind)
        {
            if (Uri.IsWellFormedUriString(uriString, uriKind))
            {
                return MainPageObject.NavigationService.Navigate(new System.Uri(uriString, uriKind));
            }
            else return false;
        }

        private void hTileTransactions_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Transactions.xaml", UriKind.Relative));
        }

        private void hTileBudgets_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Budgets.xaml", UriKind.Relative));
        }

        private void AdminSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/About.xaml", UriKind.Relative));
        }

        private void Help_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Help.xaml", UriKind.Relative));
        }

        private void txtTryAgain_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CheckOnlineStatus(error => { });
        }

        private void MenuMultiSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var menuList = sender as LongListSelector;
            if (menuList.SelectedItem != null)
            {
                PageNavigationService.NavigateTo(typeof(MainPage), ((BMA_WP.ViewModel.MenuItem)e.AddedItems[0]).NavigateTo);
                menuList.SelectedItem = null;
            }
        }
    }
}