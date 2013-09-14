using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Text;
using System.Globalization;
using System.Windows.Markup;
using BMA_WP.Resources;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using BMA_WP.ViewModel;
using BMA_WP.Model;
using System.Windows.Data;
using BMA.BusinessLogic;
using Microsoft.Phone.Scheduler;

namespace BMA_WP.View
{
    public partial class Login : PhoneApplicationPage
    {
        #region Public Properties
        public LoginViewModel vm
        {
            get { return (LoginViewModel)DataContext; }
        }
        #endregion

        public Login()
        {
            //Explicitely set the language and use localized resources
            //SetUILanguage("el-GR");

            InitializeComponent();

            SetupAppBar_Signin();

            if (!App.Instance.IsInitialized)
            {
                //App.Instance.StaticServiceData.SaveCachedCategories(InitialData.InitializeCategories(new BMA.BusinessLogic.User { UserId=4}), error =>
                //    { var a = true; });
                
                //App.Instance.StaticServiceData.SaveCategory();
            }
            //var screenWidth = System.Windows.Application.Current.Host.Content.ActualWidth;
            //var screenHeight = System.Windows.Application.Current.Host.Content.ActualHeight;
            
            //Progress.Width = screenWidth - (cnvProgress.Margin.Left + (cnvProgress.Margin.Right * 2));
            //Progress.Margin = new Thickness(0, -screenHeight/2, 0, 0);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //clear history
            while (NavigationService.CanGoBack) NavigationService.RemoveBackEntry();

            SetupLoadingBinding();

            CheckOnlineStatus(error => { });
        }

        private void SetupLoadingBinding()
        {
            Binding bind = new Binding("IsSyncing");
            bind.Mode = BindingMode.TwoWay;
            bind.Source = App.Instance;

            bind.Converter = new StatusConverter();
            bind.ConverterParameter = "trueVisible";

            spLoading.SetBinding(StackPanel.VisibilityProperty, bind);
        }

        private void SetUILanguage(string locale)
        {
            // Set this thread's current culture to the culture associated with the selected locale.
            CultureInfo newCulture = new CultureInfo(locale);
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;

            // Set the FlowDirection of the RootFrame to match the new culture.
            FlowDirection flow = (FlowDirection)Enum.Parse(typeof(FlowDirection),
                AppResources.ResourceFlowDirection);
            App.RootFrame.FlowDirection = flow;

            // Set the Language of the RootFrame to match the new culture.
            App.RootFrame.Language = XmlLanguage.GetLanguage(AppResources.ResourceLanguage);

        }

        private void Login_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string piName = (e.AddedItems[0] as PivotItem).Name;

            switch (piName)
            {
                case "piLogin":
                    SetupAppBar_Signin();
                    break;
                case "piRegister":
                    SetupAppBar_Register();
                    break;
                case "piForgotPass":
                    SetupAppBar_ForgotPass();
                    break;
            }
        }

        void SetupAppBar_Signin()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton signIn = new ApplicationBarIconButton();
            signIn.IconUri = new Uri("/Assets/icons/Dark/check.png", UriKind.Relative);
            signIn.Text = AppResources.AppBarButtonSignIn;
            ApplicationBar.Buttons.Add(signIn);
            signIn.Click += new EventHandler(SignIn_Click);

            SetupAppBar_Cancel();
            SetupAppBar_Common();
        }

        void SetupAppBar_Register()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton register = new ApplicationBarIconButton();
            register.IconUri = new Uri("/Assets/icons/Dark/check.png", UriKind.Relative);
            register.Text = AppResources.AppBarButtonRegister;
            ApplicationBar.Buttons.Add(register);
            register.Click += new EventHandler(Register_Click);

            SetupAppBar_Cancel();
            SetupAppBar_Common();
        }

        void SetupAppBar_ForgotPass()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton forgotPass = new ApplicationBarIconButton();
            forgotPass.IconUri = new Uri("/Assets/icons/Dark/check.png", UriKind.Relative);
            forgotPass.Text = AppResources.AppBarButtonSend;
            ApplicationBar.Buttons.Add(forgotPass);
            forgotPass.Click += new EventHandler(ForgotPass_Click);

            SetupAppBar_Cancel();
            SetupAppBar_Common();
        }

        void SetupAppBar_Cancel()
        {
            ApplicationBarIconButton cancel = new ApplicationBarIconButton();
            cancel.IconUri = new Uri("/Assets/icons/Dark/cancel.png", UriKind.Relative);
            cancel.Text = AppResources.AppBarButtonCancel;
            ApplicationBar.Buttons.Add(cancel);
            cancel.Click += new EventHandler(Cancel_Click);
        }

        void SetupAppBar_Common()
        {
            ApplicationBarMenuItem help = new ApplicationBarMenuItem();
            ApplicationBarMenuItem about = new ApplicationBarMenuItem();

            help.Text = AppResources.AppBarButtonHelp;
            ApplicationBar.MenuItems.Add(help);
            help.Click += new EventHandler(Help_Click);

            about.Text = AppResources.AppBarButtonAbout;
            ApplicationBar.MenuItems.Add(about);
            about.Click += new EventHandler(About_Click);
        }

        private void About_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/About.xaml", UriKind.Relative));
        }

        private void Help_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Help.xaml", UriKind.Relative));
        }

        private async void SignIn_Click(object sender, EventArgs e)
        {
            //close keyboard
            piLoginPage.Focus();

            App.Instance.User.UserName = txtUsername.Text.Trim();
            App.Instance.User.Password = txtPassword.Password.Trim();

            var errMsg = App.Instance.User.SelfValidation(false);
            StringBuilder sb = new StringBuilder();
            string delim = "";
            foreach (var item in errMsg)
            {
                sb.AppendFormat("{0}{1}", delim, item);
                delim = "\r";
            }

            if (sb.Length > 0)
            {
                MessageBox.Show(string.Format("{0}{1}:\n\r{2}",
                                            AppResources.LoginFailMessage,
                                            errMsg.Count > 1 ? "s" : "",
                                            sb.ToString()),
                                            AppResources.LoginFail, MessageBoxButton.OK);

                txtPassword.Password = "";
                txtUsername.Focus();
                txtUsername.Select(0, txtUsername.Text.Length);

                return;
            }

            try
            {
                ProgressShow(true);
                txtMessage.Text = AppResources.LoggingIn;

                //# waiting for a reply in stackoverflow !!
                //await App.Instance.StaticServiceData.LoadUser(App.Instance.User);

                await App.Instance.StaticServiceData.LoadUser(App.Instance.User, (error) =>
                    {
                        if (error == null)
                        {
                            //everything is ok
                            LoginSuccess();

                            //Progress.IsActive = false;
                            txtMessage.Text = AppResources.LoginSuccess;
                        }
                        else
                        {
                            //handle errors
                            txtMessage.Text = string.Format("{0}\n{1}", AppResources.LoginFail, error.Message);

                            //## ** ONLY FOR DEMO ** ##//
                            ////LoginSuccess();
                            //## ################## ##//
                        }
                        ProgressShow(false);
                    });
            }
            catch (Exception ex)
            {
                ProgressShow(false);

                MessageBox.Show(string.Format("{0}", ex.Message, AppResources.LoginFail));

                txtMessage.Text = AppResources.LastLoginFail;
            }
        }

        private void Register_Click(object sender, EventArgs e)
        {
            //close keyboard
            piLoginPage.Focus();

            App.Instance.User.UserName = txtRegUsername.Text.Trim();
            App.Instance.User.Email = txtRegEmail.Text.Trim();
            App.Instance.User.Password = txtRegPassword.Password.Trim();

            StringBuilder sb = new StringBuilder();
            string delim = "";
            foreach (var item in App.Instance.User.SelfValidation(true))
            {
                sb.AppendFormat("{0}{1}", delim, item);
                delim = "\r";
            }

            if (sb.Length > 0)
            {
                MessageBox.Show(string.Format("Please review the following validation error{0}:\n\r{1}",
                                                            App.Instance.User.SelfValidation(true).Count > 1 ? "s" : "",
                                                            sb.ToString()),
                                                            "Registration Failed", MessageBoxButton.OK);
                
                txtRegPassword.Password = "";
                txtRegUsername.Select(0, txtRegUsername.Text.Length);

                return;
            }

            try
            {
                ProgressShow(true);
                txtMessageRegister.Text = AppResources.Registering;

                 App.Instance.StaticServiceData.RegisterUser(App.Instance.User, (result, error) =>
                    {
                        if (result != null && error == null)
                        {
                            LoginSuccess();

                            txtMessageRegister.Text = AppResources.RegisterSuccess;
                        }
                        else
                        {
                            MessageBox.Show(string.Format("{0}", error.Message, AppResources.RegistrationFailed));
                            txtMessageRegister.Text = AppResources.RegisterFail;
                        }
                        ProgressShow(false);
                    });
            }
            catch (Exception ex)
            {
                ProgressShow(false);

                MessageBox.Show(string.Format("{0}", ex.Message, AppResources.RegistrationFailed));

                txtMessageRegister.Text = AppResources.RegisterFail;
            }
        }

        private void ForgotPass_Click(object sender, EventArgs e)
        {
            //close keyboard
            piLoginPage.Focus();

            var userItem = txtForgotInfo.Text.Trim();
            App.Instance.User.UserName = userItem.Contains("@") ? "" : userItem;
            App.Instance.User.Email = userItem.Contains("@") ? userItem : "";

            try
            {
                ProgressShow(true);
                txtMessageForgot.Text = AppResources.PasswordSending;

                 App.Instance.StaticServiceData.ForgotPassword(App.Instance.User, (result, error) =>
                {
                    if (result != null && error == null)
                        txtMessageForgot.Text = AppResources.PasswordSentSuccess;
                    else
                        txtMessageForgot.Text = error.Message;

                    ProgressShow(false);
                });
            }
            catch (Exception ex)
            {
                ProgressShow(false);

                MessageBox.Show(string.Format("{0}", ex.Message, AppResources.ForgotPassword));

                txtMessageForgot.Text = AppResources.PasswordSentFailed;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            ClearAllTextboxes();
        }

        private void ClearAllTextboxes()
        {
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtRegUsername.Text = "";
            txtRegPassword.Password = "";
            txtRegEmail.Text = "";
            txtForgotInfo.Text = "";
        }

        private void btnCancel_Tap(object sender, EventArgs e)
        {

        }

        private void btnForgot_Tap(object sender, EventArgs e)
        {

        }

        void LoginSuccess()
        {
            CheckOnlineStatus(error => {
                if (App.Instance.IsLoading)
                    return;

                App.Instance.IsLoading = true;

                LoadAllStaticData(() =>
                {
                    LoadAllData(() =>
                    {
                        App.Instance.IsLoading = false;
                    });
                });
            });
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
        }

        private void LoadAllData(Action callback)
        {
            var transaction = false;
            var budget = false;

            App.Instance.ServiceData.LoadTransactions(error =>
            {
                transaction = true;
                if (AllDataLoaded(transaction, budget))
                    callback();
            });


            App.Instance.ServiceData.LoadBudgets(error =>
            {
                budget = true;
                if (AllDataLoaded(transaction, budget))
                    callback();
            });

        }

        private void LoadAllStaticData(Action callback)
        {
            var staticData = false;
            var typeInterval = false;
            var notifications = false;
            
            App.Instance.StaticServiceData.LoadStaticData(error =>
            {
                staticData = true;
                if (AllStaticDataLoaded(staticData, typeInterval, notifications))
                {
                    callback();
                }
            });

            App.Instance.StaticServiceData.LoadTypeIntervalConfiguration(true, errorCall =>
                {
                    if (errorCall != null)
                    {
                        typeInterval = true;

                        if (AllStaticDataLoaded(staticData, typeInterval, notifications))
                            callback();
                    }

                    App.Instance.StaticServiceData.LoadTypeIntervals(true, error =>
                    {
                        typeInterval = true;

                        App.Instance.ServiceData.IntervalTransactionList = new TransactionList(App.Instance.StaticServiceData.IntervalList, App.Instance.StaticServiceData.IntervalConfiguration, App.Instance.User);

                        if (App.Instance.ServiceData.IntervalTransactionList != null && App.Instance.ServiceData.IntervalTransactionList.Count > 0)
                            NavigationService.Navigate(new Uri("/View/TransactionsInterval.xaml", UriKind.Relative));

                        if (AllStaticDataLoaded(staticData, typeInterval, notifications))
                        {
                            callback();
                        }
                    });
                });

            App.Instance.StaticServiceData.LoadNotifications(true, error =>
                {
                    if (error != null)
                        callback();

                    SetupNotificationReminders();

                    notifications = true;

                    if (AllStaticDataLoaded(staticData, typeInterval, notifications))
                    {
                        callback();
                    }
                });

        }

        private void SetupNotificationReminders()
        {
            try
            {
                Model.Reminders.SetupReminders();
            }
            catch(Exception)
            {
                MessageBox.Show(AppResources.ReminderError);
            }
        }

        private bool AllDataLoaded(bool transaction, bool budget)
        {
            return transaction && budget;
        }

        private bool AllStaticDataLoaded(bool staticData, bool recurrenceRule, bool notifications)
        {
            return staticData && recurrenceRule && notifications;
        }

        void ProgressShow(bool visible)
        {
            Visibility visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            Progress.Visibility = visibility;
            ProgressRegister.Visibility = visibility;
            ProgressForgot.Visibility = visibility;
            
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void btnForgotPass_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            piLoginPage.SelectedIndex = 2;
        }

        private void btnRegister_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            piLoginPage.SelectedIndex = 1;
        }

        private void txtTryAgain_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CheckOnlineStatus(error => { });
        }


        private void CheckOnlineStatus(Action<Exception> callback)
        {
            App.Instance.StaticDataOnlineStatus = App.Instance.StaticServiceData.SetServerStatus(status =>
            {
                App.Instance.StaticDataOnlineStatus = status;
                vm.Status = status;
                if (status == Model.StaticServiceData.ServerStatus.Ok && !App.Instance.IsSync)
                {
                    if (!App.Instance.IsSyncing )
                    {
                        App.Instance.IsSyncing = true;
                        App.Instance.Sync(() =>
                            {
                                App.Instance.IsSyncing = false;
                                if (callback != null)
                                    callback(null);
                            });
                    }
                }
                else
                {
                    callback(null);
                }
                
                
            });
            vm.Status = App.Instance.StaticDataOnlineStatus;
        }
        
    }
}