using System;
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
    }
}