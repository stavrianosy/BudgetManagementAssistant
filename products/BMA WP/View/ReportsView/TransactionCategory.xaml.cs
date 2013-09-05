using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA_WP.ViewModel.ReportsView;
using BMA.BusinessLogic;
using System.Collections.ObjectModel;
using System.Text;
using BMA_WP.Resources;
using BMA_WP.Model;
using System.Windows.Data;

namespace BMA_WP.View.ReportsView
{
    public partial class TransactionCategory : PhoneApplicationPage
    {
        #region Private Members

        #endregion

        #region Public Properties
        public TransactionCategoryViewModel vm
        {
            get { return (TransactionCategoryViewModel)DataContext; }
        }
        #endregion

        public TransactionCategory()
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
            Binding bind = new Binding("IsBusyComm");
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
            if (!IsValid())
                return;

            spLoading.Visibility = System.Windows.Visibility.Visible;

            App.Instance.ServiceData.ReportTransactionCategory(vm.DateFrom, vm.DateTo, vm.TransactionType.TypeTransactionId,
                (result, error) =>
                {

                    spLoading.Visibility = System.Windows.Visibility.Collapsed;
                    
                    if (error == null)
                    {
                        vm.Total = result.Sum(x => x.Value);
                        if (vm.IsSortByAmount)
                        {
                            vm.ReportResult = new ObservableCollection<KeyValuePair<Category, double>>();
                            foreach (var item in result.ToList().OrderByDescending(x => x.Value))
                            {
                                item.Key.TypeTransactionReasons = vm.CategoryList.FirstOrDefault(x => x.CategoryId == item.Key.CategoryId).TypeTransactionReasons;
                                vm.ReportResult.Add(new KeyValuePair<Category, double>(item.Key, item.Value));
                            }
                        }
                        else
                        {
                            vm.ReportResult = new ObservableCollection<KeyValuePair<Category, double>>();
                            foreach (var item in result.ToList().OrderBy(x => x.Key.Name))
                            {
                                item.Key.TypeTransactionReasons = vm.CategoryList.FirstOrDefault(x => x.CategoryId == item.Key.CategoryId).TypeTransactionReasons;
                                vm.ReportResult.Add(new KeyValuePair<Category, double>(item.Key, item.Value));
                            }
                        }

                        vm.PivotIndex = 1;
                    }
                });
        }

        private bool IsValid()
        {
            var result = true;
            var errorMsg = new StringBuilder();

            if (vm.DateFrom > vm.DateTo)
                errorMsg.AppendLine(AppResources.DateFromBigger);

            if (errorMsg.Length > 0)
            {
                result = false;
                MessageBox.Show(errorMsg.ToString());
            }
            else if (App.Instance.IsBusyComm)
            {
                result = false;
                MessageBox.Show(AppResources.BusySynchronizing);
            }

            return result;
        }

    }
}