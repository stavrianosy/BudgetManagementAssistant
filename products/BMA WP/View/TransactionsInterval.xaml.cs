using BMA_WP.Model;
using BMA_WP.Resources;
using BMA_WP.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Linq;
using BMA.BusinessLogic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace BMA_WP.View
{
    public partial class TransactionsInterval : PhoneApplicationPage
    {
        #region Private Members
        ApplicationBarIconButton save;
        ApplicationBarMenuItem mainMenu;
        ApplicationBarMenuItem transaction;
        #endregion

        #region Public Properties

        public TransactionsIntervalViewModel vm
        {
            get { return (TransactionsIntervalViewModel)DataContext; }
        }

        #endregion

        #region Constructors
        public TransactionsInterval()
        {
            InitializeComponent();
            SetupAppBar_TransactionList();
        }
        #endregion

        void SetupAppBar_TransactionList()
        {
            SetupAppBar_Common(false);
        }


        void SetupAppBar_Common(bool includeDelete)
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/save.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSave;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);

            mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);

            transaction = new ApplicationBarMenuItem();
            transaction.Text = AppResources.AppBarButtonBudget;
            ApplicationBar.MenuItems.Add(transaction);
            transaction.Click += new EventHandler(Transaction_Click);
        }


        void MainMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
        }

        private void Transaction_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Transactions.xaml", UriKind.Relative));
        }

        void Save_Click(object sender, EventArgs e)
        {
            SaveTransaction();
        }

        private void SaveTransaction()
        {
            vm.IsLoading = true;

            var saveOC = vm.TransactionsIntervalSelectedList.ToObservableCollection();
            var all = vm.TransactionsInterval;

            if (saveOC.Count == 0)
            {
                var result = MessageBox.Show(AppResources.IntervalTransactionsMsgCommon,
                    "", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.Cancel)
                {
                    vm.IsLoading = false;
                    return;
                }
            }

            if (saveOC.Count == all.Count)
            {
                var result = MessageBox.Show(AppResources.IntervalTransactionsMsgCommon,
                    "", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.Cancel)
                {
                    vm.IsLoading = false;
                    return;
                }
            }

            if (saveOC.Count < all.Count)
            {
                var result = MessageBox.Show(AppResources.IntervalTransactionsMsgCommon,
                    "", MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.Cancel)
                {
                    vm.IsLoading = false;
                    return;
                }
            }

            App.Instance.ServiceData.SaveTransaction(saveOC, (error) =>
            {
                if (error != null)
                    MessageBox.Show(AppResources.SaveFailed);
                else
                {
                    if(App.Instance.StaticServiceData.IntervalConfiguration == null)
                        App.Instance.StaticServiceData.IntervalConfiguration = new TypeIntervalConfiguration(App.Instance.User);

                    App.Instance.StaticServiceData.IntervalConfiguration.GeneratedDate = DateTime.Now;
                    App.Instance.StaticServiceData.IntervalConfiguration.ModifiedDate = DateTime.Now;
                    App.Instance.StaticServiceData.IntervalConfiguration.ModifiedUser = App.Instance.User;

                    App.Instance.StaticServiceData.SaveTypeIntervalConfiguration(App.Instance.StaticServiceData.IntervalConfiguration, errorConfig =>
                        {
                            if (error != null)
                                MessageBox.Show(AppResources.SaveFailed);
                            else
                                NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
                        });
                }

                vm.IsLoading = false;

            });

            save.IsEnabled = vm.IsLoading == false;

        }

        private void TransactionMultiSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (vm.TransactionsIntervalSelectedList == null)
                    vm.TransactionsIntervalSelectedList = new TransactionList();

                vm.TransactionsIntervalSelectedList.Add((Transaction)e.AddedItems[0]);
            }
            else if (e.RemovedItems.Count > 0)
                vm.TransactionsIntervalSelectedList.Remove((Transaction)e.RemovedItems[0]);
                
        }
    }
}