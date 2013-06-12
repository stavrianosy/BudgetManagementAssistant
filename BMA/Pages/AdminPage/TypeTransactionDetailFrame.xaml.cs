using BMA.BusinessLogic;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BMA.Pages.AdminPage
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TypeTransactionDetailFrame : BMA.Common.LayoutAwarePage
    {
        TypeTransaction currTypeTransaction;

        public TypeTransactionDetailFrame()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            App.Instance.Share = null;

            currTypeTransaction = navigationParameter as TypeTransaction;

            DefaultViewModel["TypeTransaction"] = currTypeTransaction;

            this.IsEnabled = false;// currTypeTransaction != null;

            this.UpdateLayout();
        }


        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            currTypeTransaction.Name = txtName.Text;
        }

        private void txtComments_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cbFromHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbFromMinute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbToHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbToMinute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
