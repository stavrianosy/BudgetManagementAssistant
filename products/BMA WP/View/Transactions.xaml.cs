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
using BMA_WP.Model;

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
        }
        #endregion

        #region Binding
        //workaround for the ListPicker issue when binding object becomes null
        private void SetBindings(bool isEnabled)
        {
            if (isEnabled)
            {
                Binding bindTransType = new Binding("TransactionType");
                bindTransType.Mode = BindingMode.TwoWay;
                bindTransType.Source = vm.CurrTransaction;
                cmbType.SetBinding(ListPicker.SelectedItemProperty, bindTransType);

                Binding bindCategory = new Binding("Category");
                bindCategory.Mode = BindingMode.TwoWay;
                bindCategory.Source = vm.CurrTransaction;
                cmbCategory.SetBinding(ListPicker.SelectedItemProperty, bindCategory);

                Binding bindTransReasonType = new Binding("TransactionReasonType");
                bindTransReasonType.Mode = BindingMode.TwoWay;
                bindTransReasonType.Source = vm.CurrTransaction;
                cmbReason.SetBinding(ListPicker.SelectedItemProperty, bindTransReasonType);
            }
            else
            {
                if(cmbType.GetBindingExpression(ListPicker.SelectedIndexProperty) != null)
                    cmbType.ClearValue(ListPicker.SelectedItemProperty);
            }
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

            switch (piName)
            {
                case "piTransaction":
                    ItemSelected();
                    SetupAppBar_Transaction();
                    break;
                case "piTransactionList":
                    TransactionMultiSelect.SelectedItem = null;
                    vm.CurrTransaction = null;
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

        private void ItemSelected()
        {
            var trans = (Transaction)TransactionMultiSelect.SelectedItem;
            SetBindings(false);

            vm.CurrTransaction = trans;

            if (trans != null)
                SetBindings(true);

            if (vm.CurrTransaction == null || vm.CurrTransaction.IsDeleted)
                vm.IsEnabled = false;
            else
                vm.IsEnabled = true;

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
            var result = MessageBox.Show(AppResources.DeleteMessage, AppResources.ConfirmDeletion, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                vm.CurrTransaction.IsDeleted = true;
                SaveTransaction();
            }
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
            //manually update model. textbox dont work well with numeric bindings
            var amount =0d;
            var tipAmount =0d;

            double.TryParse(txtAmount.Text, out amount);
            double.TryParse(txtTip.Text, out tipAmount);

            vm.CurrTransaction.Amount = amount;
            vm.CurrTransaction.TipAmount = tipAmount;
            //end of - manually update model

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
            SetBindings(true);

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
            if (e.TaskResult == TaskResult.OK)
            {
                var bitmap = new BitmapImage();
                bitmap.SetSource(e.ChosenPhoto);
                //imgReceipt.Source = new BitmapImage(new Uri(e.OriginalFileName));
                //vm.CurrTransaction.TransactionImages.Add(new TransactionImage(App.Instance.User) { Path = e.OriginalFileName });
                vm.CurrTransaction.TransactionImages.Add(new TransactionImage(App.Instance.User){Path= "/Assets/login_white.png",Name="ys1"}) ;
                imgReceipt.Source = bitmap;
            }
        }

        private void deletePhoto_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var transImage = (TransactionImage)((Microsoft.Phone.Controls.MenuItem)sender).DataContext;
            transImage.IsDeleted = true;
            vm.CurrTransaction.HasChanges = true;

            save.IsEnabled = true;

            var a = vm.CurrTransaction;
        }
    }
}