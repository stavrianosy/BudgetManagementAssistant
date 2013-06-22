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
            throw new NotImplementedException();
        }

        private void ChangePass_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}