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
    public sealed partial class CategoryDetailFrame : BMA.Common.LayoutAwarePage
    {
        Category currCategory;

        public CategoryDetailFrame()
        {
            this.InitializeComponent();
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            App.Instance.Share = null;

            currCategory = navigationParameter as Category;

            DefaultViewModel["Category"] = currCategory;

            this.IsEnabled = currCategory != null;

            InitControls();

            this.UpdateLayout();
        }

        private void InitControls()
        {
            if (currCategory == null)
                return;

            DateTime dateNowFrom = currCategory == null ? new DateTime() : currCategory.FromDate;
            DateTime dateNowTo = currCategory == null ? new DateTime() : currCategory.ToDate;            

            for (int i = 0; i < 24; i++)
            {
                int itemHour = i;

                ComboBoxItem cbItemFrom = new ComboBoxItem();
                ComboBoxItem cbItemTo = new ComboBoxItem();

                cbItemFrom.Content = cbItemTo.Content = string.Format("{0:00}", itemHour) ;
                cbItemFrom.Tag = cbItemTo.Tag = itemHour;

                cbItemFrom.IsSelected = itemHour == dateNowFrom.Hour;
                cbItemTo.IsSelected = itemHour == dateNowTo.Hour;

                cbFromHour.Items.Add(cbItemFrom);
                cbToHour.Items.Add(cbItemTo);
            }

            for (int i = 0; i < 60; i++)
            {
                int itemMinute = i;

                ComboBoxItem cbItemFrom = new ComboBoxItem();
                ComboBoxItem cbItemTo = new ComboBoxItem();
                
                cbItemFrom.Content = cbItemTo.Content = string.Format("{0:00}", itemMinute);
                cbItemFrom.Tag = cbItemTo.Tag = itemMinute;

                cbItemFrom.IsSelected = dateNowFrom.Minute == itemMinute;
                cbItemTo.IsSelected = dateNowTo.Minute == itemMinute;

                cbFromMinute.Items.Add(cbItemFrom);
                cbToMinute.Items.Add(cbItemTo);
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            currCategory.Name = txtName.Text;
        }

        private void cbFromHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currCategory == null)
                return;

            int tagH = (int)(cbFromHour.SelectedItem as ComboBoxItem).Tag;
            int tagM = currCategory.FromDate.Minute;

            currCategory.FromDate = new DateTime(2000, 1, 1, tagH, tagM, 0);
        }

        private void cbFromMinute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currCategory == null)
                return;

            int tagH = currCategory.FromDate.Hour;
            int tagM = (int)(cbFromMinute.SelectedItem as ComboBoxItem).Tag;

            currCategory.FromDate = new DateTime(2000, 1, 1, tagH, tagM, 0);
        }

        private void cbToHour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currCategory == null)
                return;

            int tagH = (int)(cbToHour.SelectedItem as ComboBoxItem).Tag;
            int tagM = currCategory.FromDate.Minute;

            currCategory.ToDate = new DateTime(2000, 1, 1, tagH, tagM, 0);
        }

        private void cbToMinute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currCategory == null)
                return;

            int tagH = currCategory.FromDate.Hour;
            int tagM = (int)(cbToMinute.SelectedItem as ComboBoxItem).Tag;

            currCategory.ToDate = new DateTime(2000, 1, 1, tagH, tagM, 0);
        }

        private void txtComments_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
