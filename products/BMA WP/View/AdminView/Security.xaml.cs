using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA_WP.Resources;
using System.Text;

namespace BMA_WP.View.AdminView
{
    public partial class Security : PhoneApplicationPage
    {
        public Security()
        {
            InitializeComponent();
            SetupAppBar_ChangePass();
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        void SetupAppBar_ChangePass()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton changePass = new ApplicationBarIconButton();
            changePass.IconUri = new Uri("/Assets/icons/Dark/check.png", UriKind.Relative);
            changePass.Text = AppResources.AppBarButtonSignIn;
            ApplicationBar.Buttons.Add(changePass);
            changePass.Click += new EventHandler(ChangePass_Click);

            //SetupAppBar_Cancel();
        }

        void SetupAppBar_Cancel()
        {
            ApplicationBarIconButton cancel = new ApplicationBarIconButton();
            cancel.IconUri = new Uri("/Assets/icons/Dark/cancel.png", UriKind.Relative);
            cancel.Text = AppResources.AppBarButtonCancel;
            ApplicationBar.Buttons.Add(cancel);
            cancel.Click += new EventHandler(Cancel_Click);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            ClearPass();
        }

        private void ClearPass()
        {
            txtOldPass.Text = "";
            txtNewPass.Text = "";
            txtConfirmPass.Text = "";
        }

        private async void ChangePass_Click(object sender, EventArgs e)
        {
            var oldPass = txtOldPass.Text.Trim();
            var newPass = txtNewPass.Text.Trim();
            var confirmPass = txtConfirmPass.Text.Trim();

            if (oldPass != App.Instance.User.Password)
            {
                MessageBox.Show("Old password is not correct");
                ClearPass();
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("New and confirm password do not match");
                ClearPass();
                return;
            }

            var errMsg = App.Instance.User.ValidatePassword(newPass);
            StringBuilder sb = new StringBuilder();
            string delim = "";
            foreach (var item in App.Instance.User.ValidatePassword(newPass))
            {
                sb.AppendFormat("{0}{1}", delim, item);
                delim = "\r";
            }

            if (sb.Length > 0)
            {
                MessageBox.Show(string.Format("{0}:\n\r{2}",
                                            AppResources.PasswordValidationFailMessage,
                                            sb.ToString()),
                                            AppResources.LoginFail, MessageBoxButton.OK);
                ClearPass();
                return;
            }

            App.Instance.User.Password = newPass;

            await App.Instance.StaticServiceData.ChangePassword(App.Instance.User, (result, error) =>
                    {
                        if (result != null && error == null)
                        {

                        }
                    });
        }
    }
}