using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Storage;

namespace BMA_WP
{
    public partial class Splash : PhoneApplicationPage
    {
        public Splash()
        {
            Loaded += ExtendedSplashScreen_Loaded;
            InitializeComponent();
        }

        private void ExtendedSplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.Relative));
        }
    }
}