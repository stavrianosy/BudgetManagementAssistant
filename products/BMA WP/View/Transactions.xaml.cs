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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BMA_WP.ViewModel;
using BMA_WP.Resources;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;
using Microsoft.Phone.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;

namespace BMA_WP.View
{
    public partial class Transactions : PhoneApplicationPage
    {
        #region Private Members
        ApplicationBarIconButton save;
        ApplicationBarIconButton delete;
        ApplicationBarIconButton add;
        ApplicationBarMenuItem mainMenu;
        ApplicationBarMenuItem budget;
        #endregion

        #region Public Properties

        public TransactionViewModel vm
        {
            get { return (TransactionViewModel)DataContext; }
        }

        #endregion

        #region Constructors
        public Transactions()
        {
            InitializeComponent();

            

                //Binding bindTransactionType = new Binding("TransactionType");
                //bindTransactionType.Mode = BindingMode.TwoWay;
                //bindTransactionType.Source = vm.CurrTransaction;
                //cmdType.SetBinding(ListPicker.SelectedItemProperty, bindTransactionType);

                //Binding bindTransactionReasonType = new Binding("TransactionReasonType");
                //bindTransactionReasonType.Source = vm.CurrTransaction;
                //bindTransactionReasonType.Mode = BindingMode.TwoWay;
                //cmdType.SetBinding(ListPicker.SelectedItemProperty, bindTransactionReasonType);

                //Binding bindCategory = new Binding("Category");
                //bindCategory.Source = vm.CurrTransaction;
                //bindCategory.Mode = BindingMode.TwoWay;
                //cmdType.SetBinding(ListPicker.SelectedItemProperty, bindCategory);

        }
        #endregion

        #region Events
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);            
        }
        #endregion

        private void Transactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            scrollItem.ScrollToVerticalOffset(0d);

            string piName = (e.AddedItems[0] as PivotItem).Name;

            vm.CurrTransaction = (Transaction)TransactionMultiSelect.SelectedItem;

            if (vm.CurrTransaction == null || vm.CurrTransaction.IsDeleted)
                vm.IsEnabled = false;
            else
                vm.IsEnabled = true;

            switch (piName)
            {
                case "piTransaction":
                    SetupAppBar_Transaction();
                    break;
                case "piTransactionList":
                    SetupAppBar_TransactionList();
                    break;
            }

            if (vm.CurrTransaction != null)
            {
                vm.CurrTransaction.PropertyChanged += (o, changedEventArgs) =>
                {
                    var items = vm.Transactions.Where(t => t.HasChanges).ToObservableCollection();
                    if (items.Count > 0)
                    {
                        save.IsEnabled = true;
                        delete.IsEnabled = true;
                    }
                };
                delete.IsEnabled = true;
            }

        }

        private void SetupAppBar_TransactionList()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            add = new ApplicationBarIconButton();
            add.IconUri = new Uri("/Assets/icons/Dark/add.png", UriKind.Relative);
            add.Text = AppResources.AppBarButtonAdd;
            add.IsEnabled = true;
            ApplicationBar.Buttons.Add(add);
            add.Click += new EventHandler(Add_Click);

            mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            mainMenu.IsEnabled = true;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);

            budget = new ApplicationBarMenuItem();
            budget.Text = AppResources.AppBarButtonBudget;
            budget.IsEnabled = true;
            ApplicationBar.MenuItems.Add(budget);
            budget.Click += new EventHandler(Budget_Click);
        }

        void SetupAppBar_Transaction()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/save.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSave;
            save.IsEnabled = false;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);

            delete = new ApplicationBarIconButton();
            delete.IconUri = new Uri("/Assets/icons/Dark/delete.png", UriKind.Relative);
            delete.Text = AppResources.AppBarButtonDelete;
            delete.IsEnabled = false;
            ApplicationBar.Buttons.Add(delete);
            delete.Click += new EventHandler(Delete_Click);

            add = new ApplicationBarIconButton();
            add.IconUri = new Uri("/Assets/icons/Dark/add.png", UriKind.Relative);
            add.Text = AppResources.AppBarButtonAdd;
            add.IsEnabled = true;
            ApplicationBar.Buttons.Add(add);
            add.Click += new EventHandler(Add_Click);

            mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            mainMenu.IsEnabled = true;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);

            budget = new ApplicationBarMenuItem();
            budget.Text = AppResources.AppBarButtonBudget;
            budget.IsEnabled = true;
            ApplicationBar.MenuItems.Add(budget);
            budget.Click += new EventHandler(Budget_Click);
        }

        void MainMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            vm.CurrTransaction.IsDeleted = true;

            SaveTransaction();
        }

        private void Budget_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Budgets.xaml", UriKind.Relative));
        }

        void Save_Click(object sender, EventArgs e)
        {
            SaveTransaction();
        }

        private async void SaveTransaction()
        {
            var saveOC = vm.Transactions.Where(t => t.HasChanges).ToObservableCollection();

            await App.Instance.ServiceData.SaveTransaction(saveOC);

            pivotContainer.SelectedIndex = 1;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            Transaction item = new Transaction(App.Instance.StaticServiceData.CategoryList.ToList(),
                                                App.Instance.StaticServiceData.TypeTransactionList.ToList(),
                                                App.Instance.StaticServiceData.TypeTransactionReasonList.ToList(),
                                                App.Instance.User);

            vm.PivotIndex = 0;

            vm.Transactions.Add(item);
            TransactionMultiSelect.SelectedItem = item;
            vm.CurrTransaction = item;

            save.IsEnabled = true;
            vm.IsEnabled = true;
        }

        private void btnTest_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CameraCaptureTask cameraTask = new CameraCaptureTask();
            
            cameraTask.Completed += cameraTask_Completed;

            cameraTask.Show();
        }

        private void cameraTask_Completed(object sender, PhotoResult e)
        {
            BitmapImage imgSource = new BitmapImage(new Uri(e.OriginalFileName, UriKind.Absolute));
            imgReceipt.Source = imgSource;
        }

      
        

    }
}