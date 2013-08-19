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
            if (!IsValid())
                return;

            App.Instance.ServiceData.ReportTransactionCategory(vm.DateFrom, vm.DateTo, vm.TransactionType.TypeTransactionId,
                (result, error) =>
                {
                    if (error == null)
                    {
                        if (vm.IsSortByAmount)
                        {
                            vm.ReportResult = new ObservableCollection<KeyValuePair<Category, double>>();
                            foreach (var item in result.ToList().OrderByDescending(x=>x.Value))
                                vm.ReportResult.Add(new KeyValuePair<Category,double>(item.Key, item.Value));
                        }
                        else
                        {
                            vm.ReportResult = new ObservableCollection<KeyValuePair<Category, double>>();
                            foreach (var item in result.ToList().OrderBy(x=>x.Key.Name))
                                vm.ReportResult.Add(new KeyValuePair<Category,double>(item.Key, item.Value));
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
            else if (App.Instance.IsSyncing)
            {
                result = false;
                MessageBox.Show(AppResources.BusySynchronizing);
            }

            return result;
        }

    }
}