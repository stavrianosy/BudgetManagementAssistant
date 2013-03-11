using BMA.BusinessLogic;
using BMA.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace BMA.Pages.Settings
{
    public sealed partial class Security : LayoutAwarePage
    {
        public Security()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            txtOldPass.Password = "";
            txtNewPass.Password = "";
            txtConfirmPass.Password = "";
        }

        private async void btnSave_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog dialog = null;
            User user = App.Instance.User;

            string oldPass = txtOldPass.Password.Trim();
            string newPass = txtNewPass.Password.Trim();
            string confirmPass = txtConfirmPass.Password.Trim();

            if (oldPass != user.Password)
            {
                dialog = new MessageDialog(string.Format("Old password is not correct"), "Change password failed");
                await dialog.ShowAsync();

                ResetControls();

                return;
            }

            if (oldPass == newPass)
            {
                dialog = new MessageDialog(string.Format("New password must be different from your current"), "Change password failed");
                await dialog.ShowAsync();

                ResetControls();

                return;
            }

            if (newPass != confirmPass)
            {
                dialog = new MessageDialog(string.Format("Confirm password does not match with New password"), "Change password failed");
                await dialog.ShowAsync();

                ResetControls();

                return;
            }

            StringBuilder sb = new StringBuilder();
            string delim = "";
            foreach (var item in App.Instance.User.ValidatePassword(newPass))
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

                ResetControls();

                return;
            }

            user.Password = newPass;

            try
            {
                Progress.IsActive = true;
                txtMessage.Text = "Resetting password...";

                await App.Instance.StaticDataSource.ChangePassword(user);

                Progress.IsActive = false;
                txtMessage.Text = "Password reset successfully";
            }
            catch (Exception ex)
            {
                Progress.IsActive = false;

                dialog = new MessageDialog(string.Format("{0}", ex.Message, "Login Failed"));

                dialog.ShowAsync();

                txtMessage.Text = "Registration failed. Please try again.";
            }
        }

        private void ResetControls()
        {
            txtNewPass.Password = "";
            txtConfirmPass.Password = "";
            txtOldPass.Focus(Windows.UI.Xaml.FocusState.Pointer);
            txtOldPass.SelectAll();
        }
    }
}
