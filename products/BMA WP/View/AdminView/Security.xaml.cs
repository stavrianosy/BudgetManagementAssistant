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
using BMA_WP.ViewModel.Admin;

namespace BMA_WP.View.AdminView
{
    public partial class Security : PhoneApplicationPage
    {
        #region Private Properties
        ApplicationBarIconButton changePass;
        ApplicationBarIconButton cancel;
        ApplicationBarIconButton save;
        #endregion

        #region Public Properties

        public SecurityViewModel vm
        {
            get { return (SecurityViewModel)DataContext; }
        }

        #endregion

        public Security()
        {
            InitializeComponent();
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string piName = (e.AddedItems[0] as PivotItem).Name;

            switch (piName)
            {
                case "piSecurity":
                    SetupAppBar_ChangePass();
                    svItem.ScrollToVerticalOffset(0d);
                    break;
                case "piDetails":
                    SetupAppBar_UserDetails();
                    break;
            }
        }

        void SetupAppBar_ChangePass()
        {
            ApplicationBar = new ApplicationBar();

            changePass = new ApplicationBarIconButton();
            changePass.IconUri = new Uri("/Assets/icons/Dark/check.png", UriKind.Relative);
            changePass.Text = AppResources.AppBarButtonSignIn;
            ApplicationBar.Buttons.Add(changePass);
            changePass.Click += new EventHandler(ChangePass_Click);

            SetupAppBar_Common();
        }

        void SetupAppBar_UserDetails()
        {
            ApplicationBar = new ApplicationBar();

            save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/check.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSignIn;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);

            SetupAppBar_Common();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            App.Instance.StaticServiceData.UpdateUser(vm.User, (error) =>
            {
                if (error != null)
                {

                }
            });
            //close keypad
            dpBirthdate.Focus();
        }

        void SetupAppBar_Common()
        {
            cancel = new ApplicationBarIconButton();
            cancel.IconUri = new Uri("/Assets/icons/Dark/cancel.png", UriKind.Relative);
            cancel.Text = AppResources.AppBarButtonCancel;
            ApplicationBar.Buttons.Add(cancel);
            cancel.Click += new EventHandler(Cancel_Click);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            ClearPass();
            ResetUserDetails();
        }

        private void ResetUserDetails()
        {
            txtFirstName.Text = App.Instance.User.FirstName;
            txtLastName.Text = App.Instance.User.LastName;
            txtEmail.Text = App.Instance.User.Email;
            dpBirthdate.Value = App.Instance.User.Birthdate;
        }

        private void ClearPass()
        {
            txtOldPass.Text = "";
            txtNewPass.Text = "";
            txtConfirmPass.Text = "";
        }

        private void ChangePass_Click(object sender, EventArgs e)
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

            vm.NewPass = newPass;

            App.Instance.StaticServiceData.ChangePassword(vm.User,  error =>
                    {
                        if (error == null)
                            ClearPass();
                    });
            //close keypad
            dpBirthdate.Focus();
        }
    }
}