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
using System.Reflection;

namespace BMA_WP.View
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();

            SetupAppBar();

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            var asm = Assembly.GetExecutingAssembly();
            var parts = asm.FullName.Split(',');
            //appVersion.Text = parts[1].Split('=')[1];
            string strVersion = (from manifest in System.Xml.Linq.XElement.Load("WMAppManifest.xml").Descendants("App") select manifest).SingleOrDefault().Attribute("Version").Value;
            appVersion.Text = strVersion;
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

        private void Website_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("www.softcarbongear.com/moneysaver", UriKind.Absolute));
        }

        private void Rate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("www.softcarbongear.com/moneysaver", UriKind.Absolute));
        }

        private void Donate_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("www.paypal.com", UriKind.Absolute));
        }

        private void Privacy_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //NavigationService.Navigate(new Uri("http://www.softcarbongear.com/privacystatement/wp8/MoneySaver_PrivacyPolicy.html", UriKind.Absolute));
        }
    }
}