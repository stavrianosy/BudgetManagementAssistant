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


            //var screenWidth = System.Windows.Application.Current.Host.Content.ActualWidth;
            //var screenHeight = System.Windows.Application.Current.Host.Content.ActualHeight;
            
            //Progress.Width = screenWidth - (cnvProgress.Margin.Left + (cnvProgress.Margin.Right * 2));
            //Progress.Margin = new Thickness(0, -screenHeight/2, 0, 0);
        }

        private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.Instance.StaticDataOnlineStatus = await App.Instance.StaticServiceData.SetServerStatus(async status =>
            {
                App.Instance.StaticDataOnlineStatus = status;
                vm.Status = status;
                if (status == Model.StaticServiceData.ServerStatus.Ok && !App.Instance.IsSync)
                {
                    grdSync.Visibility = System.Windows.Visibility.Visible;

                    await App.Instance.Sync(() => grdSync.Visibility = System.Windows.Visibility.Collapsed);

                }
            });
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

                await App.Instance.StaticServiceData.LoadUser(App.Instance.User, async (result, error) =>
                    {
                        if (result != null && error == null)
                        {
                            //everything is ok
                            await LoginSuccess();

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

        private async void Register_Click(object sender, EventArgs e)
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

                await App.Instance.StaticServiceData.RegisterUser(App.Instance.User, (result, error) =>
                    {
                        if (result != null && error == null)
                        {
                            LoginSuccess();

                            txtMessageRegister.Text = AppResources.RegisterSuccess;
                        }
                        else
                        {
                            throw new Exception(error.Message);
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

        private async void ForgotPass_Click(object sender, EventArgs e)
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

                await App.Instance.StaticServiceData.ForgotPassword(App.Instance.User, (result, error) =>
                {
                    if (result != null && error == null)
                    {
                        txtMessageForgot.Text = AppResources.PasswordSentSuccess;
                    }
                    else
                    {
                        throw new Exception(error.Message);
                    }
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

        async Task LoginSuccess()
        {
            //if (App.Instance.IsOnline && !App.Instance.IsSync)
            //{
            //    txtMessage.Text = "Synchronizing Transactions...";
            //    await App.Instance.ServiceData.SyncTransactions();

            //    txtMessage.Text = "Synchronizing Budgets...";
            //    await App.Instance.ServiceData.SyncBudgets();

            //    txtMessage.Text = "Synchronizing Data...";
            //    //await App.Instance.StaticServiceData.SyncData();
            //}

            txtMessage.Text = "Loading Transactions...";
            await App.Instance.ServiceData.LoadTransactions();

            txtMessage.Text = "Loading Budgets...";
            await App.Instance.ServiceData.LoadBudgets();
            
            txtMessage.Text = "Loading Data...";
            await App.Instance.StaticServiceData.LoadStaticData();

            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
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

        
    }
}