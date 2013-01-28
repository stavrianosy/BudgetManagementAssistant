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

using BMA.Common;
using BMA.BusinessLogic;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BMA.Pages.BudgetPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BudgetDetailFrame : LayoutAwarePage
    {
        public BudgetDetailFrame()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            App.Instance.Share = null;
            
            DefaultViewModel["Budgets"] = App.Instance.TransDataSource.BudgetList;

            this.UpdateLayout();

            this.IsEnabled = false;

            if (navigationParameter is Budget)
            {
                this.IsEnabled = true;

                var budget = navigationParameter as Budget;

                txtName.Text = budget.Name ?? string.Empty;
                txtAmount.Text = budget.Amount.ToString();
                dtFrom.Text = budget.FromDate.ToString();
                dtTo.Text = budget.ToDate.ToString();
                cbInstallment.IsChecked = budget.IncludeInstallments;
                txtComments.Text = budget.Comments??string.Empty;
                txtDuration.Text = budget.DurrationDays().ToString();
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void cbInstallment_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtComments_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
