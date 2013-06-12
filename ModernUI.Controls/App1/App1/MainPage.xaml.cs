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

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            DataVirtualization.Toolkit.DataItemCollection collection = new DataVirtualization.Toolkit.DataItemCollection();
            collection.Add(new DataVirtualization.Toolkit.DataItem()
            {
                DataSymbol = "Dollars",
                ID = 1,
                Name = "Transporation",
                Value = 500
            });
            collection.Add(new DataVirtualization.Toolkit.DataItem()
            {
                DataSymbol = "Dollars",
                ID = 2,
                Name = "Meals",
                Value = 200
            });
            collection.Add(new DataVirtualization.Toolkit.DataItem()
            {
                DataSymbol = "Dollars",
                ID = 3,
                Name = "Meetings",
                Value = 100
            });
            collection.Add(new DataVirtualization.Toolkit.DataItem()
            {
                DataSymbol = "Dollars",
                ID = 4,
                Name = "Outings",
                Value = 700
            });

            chart.ItemsSource = collection;

            chart.ItemSelected += chart_ItemSelected;
            chart.ItemDeSelected += chart_ItemDeSelected;
        }

        void chart_ItemDeSelected(object sender, DataVirtualization.Toolkit.DataItem e)
        {
            txtPageHeader.Text = string.Format("{0} Has been Selected with Value {1}", e.Name, e.Value.ToString());
            
        }

        void chart_ItemSelected(object sender, DataVirtualization.Toolkit.DataItem e)
        {
            txtPageHeader.Text = string.Format("{0} Has been Selected with Value {1}", e.Name, e.Value.ToString());
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
