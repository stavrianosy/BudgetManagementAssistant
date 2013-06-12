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
    }
}