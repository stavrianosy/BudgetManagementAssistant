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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace DataVirtualization.Toolkit
{
    public sealed partial class ToolTip : UserControl
    {
        
        public ToolTip()
        {
            this.InitializeComponent();
        }


        public void ViewData(DataItem item)
        {
            this.recPalette.Fill = item.Palette;
            this.txtName.Text = item.Name;
            this.txtValue.Text = item.Value.ToString() + " " + item.DataSymbol;
            this.txtPercent.Text = item.Percentage;
            this.Visibility = Windows.UI.Xaml.Visibility.Visible;
            this.Show.Begin();
        }

        public void HideData()
        {
            this.Hide.Begin();
            this.Hide.Completed += Hide_Completed;
            
        }

        void Hide_Completed(object sender, object e)
        {
            this.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Grid Parent = (Grid)this.Parent;
            if (Parent.Children.Contains(this))
            {
                Parent.Children.Remove(this);
            }
            
        }

        private Grid _parent;
        public Grid ParentGrid
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
    }
}
