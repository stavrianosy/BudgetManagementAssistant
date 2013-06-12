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

namespace BMA_WP.View
{
    public partial class Login : PhoneApplicationPage
    {
        //public delegate void LoginDelegate();
        //public event LoginDelegate LoginSuccess;

        public Login()
        {
            //Explicitely set the language and use localized resources
            SetUILanguage("el-GR");

            InitializeComponent();
            SetupAppBar_Signin();



            //var screenWidth = System.Windows.Application.Current.Host.Content.ActualWidth;
            //var screenHeight = System.Windows.Application.Current.Host.Content.ActualHeight;
            
            //Progress.Width = screenWidth - (cnvProgress.Margin.Left + (cnvProgress.Margin.Right * 2));
            //Progress.Margin = new Thickness(0, -screenHeight/2, 0, 0);
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
            string piName = (e.AddedItems[0] as PanoramaItem).Name;

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
        }

        void SetupAppBar_Cancel()
        {
            ApplicationBarIconButton cancel = new ApplicationBarIconButton();
            cancel.IconUri = new Uri("/Assets/icons/Dark/cancel.png", UriKind.Relative);
            cancel.Text = AppResources.AppBarButtonCancel;
            ApplicationBar.Buttons.Add(cancel);
            cancel.Click += new EventHandler(Cancel_Click);
        }

        private async void SignIn_Click(object sender, EventArgs e)
        {
            App.Instance.User.UserName = txtUsername.Text.Trim();
            App.Instance.User.Password = txtPassword.Password.Trim();

            StringBuilder sb = new StringBuilder();
            string delim = "";
            foreach (var item in App.Instance.User.SelfValidation(false))
            {
                sb.AppendFormat("{0}{1}", delim, item);
                delim = "\r";
            }

            if (sb.Length > 0)
            {
                MessageBox.Show(string.Format("{0}{1}:\n\r{2}",
                                            AppResources.LoginFailMessage,
                                            App.Instance.User.SelfValidation(false).Count > 1 ? "s" : "",
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

                await App.Instance.StaticServiceData.LoadUser(App.Instance.User, (result, error) =>
                    {
                        if (result != null && error == null)
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
                            LoginSuccess();
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
                txtMessage.Text = "Registering ...";

                //await App.Instance.StaticServiceData.RegisterUser(App.Instance.User);
                LoginSuccess();

                ProgressShow(false);
                txtMessage.Text = "Registered successfully";
            }
            catch (Exception ex)
            {
                ProgressShow(false);

                MessageBox.Show(string.Format("{0}", ex.Message, "Login Failed"));

                txtMessage.Text = "Registration failed. Please try again.";
            }
        }

        private async void ForgotPass_Click(object sender, EventArgs e)
        {
            App.Instance.User.UserName = txtForgotUsername.Text.Trim();
            App.Instance.User.Email = txtForgotEmail.Text.Trim();

            try
            {
                ProgressShow(true);
                txtMessage.Text = "Sending password...";

                //await App.Instance.StaticServiceData.ForgotPassword(App.Instance.User);

                ProgressShow(false);
                txtMessage.Text = "Password sent to the given email";
            }
            catch (Exception ex)
            {
                ProgressShow(false);

                MessageBox.Show(string.Format("{0}", ex.Message, "Password is not sent"));

                txtMessage.Text = "Password sent failed. Please try again.";
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
            txtForgotUsername.Text = "";
        }

        private void btnCancel_Tap(object sender, EventArgs e)
        {

        }

        private void btnForgot_Tap(object sender, EventArgs e)
        {

        }

        async void LoginSuccess()
        {
            await App.Instance.ServiceData.LoadTransactions();
            await App.Instance.ServiceData.LoadBudgets();
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
    }
}