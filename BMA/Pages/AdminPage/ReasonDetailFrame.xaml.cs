using BMA.BusinessLogic;
using BMA.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BMA.Pages.AdminPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReasonDetailFrame : LayoutAwarePage
    {
        TypeTransactionReason currTypeTransReason;

        public ReasonDetailFrame()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            App.Instance.Share = null;

            currTypeTransReason = navigationParameter as TypeTransactionReason;

            DefaultViewModel["TypeTransactionReason"] = currTypeTransReason;

            this.IsEnabled = currTypeTransReason != null;

            this.UpdateLayout();
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            currTypeTransReason.Name = txtName.Text;
        }
    }
}
