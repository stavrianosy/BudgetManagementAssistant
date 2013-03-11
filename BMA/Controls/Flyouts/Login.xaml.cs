using BMA.BusinessLogic;
using BMA.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BMA.Controls.Flyouts
{
    public sealed partial class Login : UserControl
    {
        bool isRegistration;

        public delegate void LoginDelegate();
        public event LoginDelegate LoginSuccess;

        public Login()
        {
            this.InitializeComponent();
            LoginVisible();
        }

        private void btnForgotPass_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HidePanels();
            spForgotPass.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void btnRegisterView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txtMessage.Text = "Capture all fields to register";

            HidePanels();
            spEmail.Visibility = Windows.UI.Xaml.Visibility.Visible;
            spRegister.Visibility = Windows.UI.Xaml.Visibility.Visible;
            spPassword.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        private void LoginVisible()
        {
            HidePanels();
            spLogin.Visibility = Windows.UI.Xaml.Visibility.Visible;
            spPassword.Visibility = Windows.UI.Xaml.Visibility.Visible;

        }

        private void HidePanels()
        {
            txtMessage.Text = "";
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtEmail.Text = "";

            spRegister.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            spEmail.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            spForgotPass.Visibility = Windows.UI.Xaml.Visibility.Collapsed; 
            spLogin.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            spPassword.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void btnLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog dialog = null;

            App.Instance.User.UserName = txtUsername.Text;
            App.Instance.User.Password = txtPassword.Password;

            StringBuilder sb = new StringBuilder();
            string delim = "";
            foreach (var item in App.Instance.User.SelfValidation(false))
            {
                sb.AppendFormat("{0}{1}", delim, item);
                delim = "\r";
            }

            if (sb.Length > 0)
            {
                dialog = new MessageDialog(string.Format("Please review the following validation error{0}:\n\r{1}",
                                                            App.Instance.User.SelfValidation(false).Count > 1 ? "s" : "",
                                                            sb.ToString()),
                                                            "Login Failed");
                await dialog.ShowAsync();

                txtPassword.Password = "";
                txtUsername.Focus(Windows.UI.Xaml.FocusState.Pointer);
                txtUsername.Select(0, txtUsername.Text.Length);

                return;
            }

            try
            {
                Progress.IsActive = true;
                txtMessage.Text = "Loging in...";

                await App.Instance.StaticDataSource.LoadUser(App.Instance.User);
                LoginSuccess();

                Progress.IsActive = false;
                txtMessage.Text = "Logged in successfully";
            }
            catch (Exception ex)
            {
                Progress.IsActive = false;

                dialog = new MessageDialog(string.Format("{0}",ex.Message,"Login Failed"));

                dialog.ShowAsync();
                
                txtMessage.Text = "Last login attempt failed. Please try again.";
            }
        }

        private void btnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LoginVisible();
        }

        private async void btnRegister_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog dialog = null;

            App.Instance.User.UserName = txtUsername.Text.Trim();
            App.Instance.User.Email = txtEmail.Text.Trim();
            App.Instance.User.Password = txtPassword.Password.Trim();

            StringBuilder sb = new StringBuilder();
            string delim = "";
            foreach (var item in App.Instance.User.SelfValidation(true))
            {
                sb.AppendFormat("{0}{1}", delim, item);
                delim = "\r";
            }

            if (sb.Length > 0)
            {
                dialog = new MessageDialog(string.Format("Please review the following validation error{0}:\n\r{1}",
                                                            App.Instance.User.SelfValidation(true).Count > 1 ? "s" : "",
                                                            sb.ToString()),
                                                            "Registration Failed");
                await dialog.ShowAsync();

                txtPassword.Password = "";
                txtUsername.Focus(Windows.UI.Xaml.FocusState.Pointer);
                txtUsername.Select(0, txtUsername.Text.Length);

                return;
            }

            try
            {
                Progress.IsActive = true;
                txtMessage.Text = "Registering ...";

                await App.Instance.StaticDataSource.RegisterUser(App.Instance.User);
                LoginSuccess();

                Progress.IsActive = false;
                txtMessage.Text = "Registered successfully";
            }
            catch (Exception ex)
            {
                Progress.IsActive = false;

                dialog = new MessageDialog(string.Format("{0}", ex.Message, "Login Failed"));

                dialog.ShowAsync();

                txtMessage.Text = "Registration failed. Please try again.";
            }
        }

        private async void btnSendPass_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog dialog = null;

            App.Instance.User.UserName = txtUsername.Text.Trim();
            App.Instance.User.Email = txtEmail.Text.Trim();

            try
            {
                Progress.IsActive = true;
                txtMessage.Text = "Sending password...";

                await App.Instance.StaticDataSource.ForgotPassword(App.Instance.User);

                Progress.IsActive = false;
                txtMessage.Text = "Password sent to the given email";
            }
            catch (Exception ex)
            {
                Progress.IsActive = false;

                dialog = new MessageDialog(string.Format("{0}", ex.Message, "Password is not sent"));

                dialog.ShowAsync();

                txtMessage.Text = "Password sent failed. Please try again.";
            }
        }

        private void btnCancelRegister_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LoginVisible();
        }

        private void btnCancelPass_Tapped(object sender, TappedRoutedEventArgs e)
        {
            LoginVisible();
        }

        private void KeyUp_Event(object sender, KeyRoutedEventArgs e)
        {
            switch(e.Key)
            {
                case VirtualKey.Enter:
                    btnLogin_Tapped(null, null);
                    break;
                case VirtualKey.Escape:
                    btnCancelRegister_Tapped(null, null);
                    btnCancelPass_Tapped(null, null);
                    btnCancel_Tapped(null, null);
                    break;
            }
        }
    }
}
