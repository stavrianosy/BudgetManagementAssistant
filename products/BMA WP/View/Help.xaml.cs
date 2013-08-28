using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA_WP.Resources;

namespace BMA_WP.View
{
    public partial class Help : PhoneApplicationPage
    {
        public Help()
        {
            InitializeComponent();

            SetupAppBar();
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private void SetupAppBar()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            SetupAppBar_Common();
        }

        void SetupAppBar_Common()
        {
            ApplicationBarMenuItem mainMenu = new ApplicationBarMenuItem();
            ApplicationBarMenuItem transaction = new ApplicationBarMenuItem();

            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);

            transaction.Text = AppResources.AppBarButtonTransaction;
            ApplicationBar.MenuItems.Add(transaction);
            transaction.Click += new EventHandler(Transaction_Click);
        }

        private void Transaction_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Transactions.xaml", UriKind.Relative));
        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
        }

    }
}