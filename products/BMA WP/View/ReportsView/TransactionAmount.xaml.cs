using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA.BusinessLogic;
using BMA_WP.ViewModel.ReportsView;
using System.Text;
using BMA_WP.Resources;
using System.Windows.Data;
using BMA_WP.Model;

namespace BMA_WP.View.ReportsView
{
    public partial class TransactionAmount : PhoneApplicationPage
    {
        #region Private Members

        #endregion

        #region Public Properties
        public TransactionAmountViewModel vm
        {
            get { return (TransactionAmountViewModel)DataContext; }
        }
        #endregion

        public TransactionAmount()
        {
            InitializeComponent();

            SetupAppBar();

            SetupLoadingBinding();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!App.Instance.IsAuthorized)
                NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.Relative));
        }

        #region Binding

        private void SetupLoadingBinding()
        {
            Binding bind = new Binding("IsSyncing");
            bind.Mode = BindingMode.TwoWay;
            bind.Source = App.Instance;

            bind.Converter = new StatusConverter();
            bind.ConverterParameter = "trueVisible";

            spLoading.SetBinding(StackPanel.VisibilityProperty, bind);
        }
        #endregion

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

            mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);

            transaction = new ApplicationBarMenuItem();
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
        private void btnViewReport_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ManualUpdate();

            if(!IsValid())
                return;

            spLoading.Visibility = System.Windows.Visibility.Visible;

            App.Instance.ServiceData.ReportTransactionAmount(vm.DateFrom, vm.DateTo, vm.TransactionType.TypeTransactionId, vm.AmountFrom, vm.AmountTo,
                (result, error) =>
                {
                    spLoading.Visibility = System.Windows.Visibility.Collapsed;

                    if (error == null)
                    {
                        vm.Total = result.Sum(x => x.Amount);

                        if (vm.IsSortByAmount)
                            vm.ReportResult = result.OrderByDescending(x => x.Amount).ToObservableCollection();
                        else
                            vm.ReportResult = result.OrderByDescending(x => x.TransactionDate).ToObservableCollection();

                        vm.PivotIndex = 1;
                    }
                });
        }

        private void ManualUpdate()
        {
            var amountFrom = 0d;
            var amountTo = 0d;

            double.TryParse(txtAmountFrom.Text.Trim(), out amountFrom);
            double.TryParse(txtAmountTo.Text.Trim(), out amountTo);

            vm.AmountFrom = amountFrom;
            vm.AmountTo = amountTo;
        }

        private bool IsValid()
        {
            var result = true;
            var errorMsg = new StringBuilder();

            if (vm.DateFrom > vm.DateTo)
                errorMsg.AppendLine(AppResources.DateFromBigger);

            if (vm.AmountFrom > vm.AmountTo)
                errorMsg.AppendLine(AppResources.AmountFromBigger);


            if (errorMsg.Length > 0)
            {
                result = false;
                MessageBox.Show(errorMsg.ToString());
            }
            else if (App.Instance.IsSyncing)
            {
                result = false;
                MessageBox.Show(AppResources.BusySynchronizing);
            }

            return result;
        }

    }
}