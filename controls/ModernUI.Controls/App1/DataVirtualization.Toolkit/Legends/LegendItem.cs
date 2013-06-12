using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DataVirtualization.Toolkit.Legends
{
    public class LegendItem:UserControl
    {
        public Rectangle rec;
        public TextBlock text;
        private bool _contentLoaded = false;

        public LegendItem()
        {
            
            DefaultStyleKey = typeof(LegendItem);
            InitializeComponents();
            this.ApplyTemplate();
        }

        public void InitializeComponents()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;

            Application.LoadComponent(this, new System.Uri("ms-appx:///DataVirtualizationToolkit/Legends/LegendItem.xaml"),
                Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);

            rec = (Rectangle)this.FindName("rec");
            text = (TextBlock)this.FindName("text");
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
                
        }

        #region Dependancy Properties
        public static readonly DependencyProperty SeriesColorProperty =
            DependencyProperty.Register("SeriesColorProperty", typeof(Brush), typeof(LegendItem),
            new PropertyMetadata(0.0));

        /// <summary>
        /// Series Color
        /// </summary>
        public Brush SeriesColor
        {
            get { return (Brush)GetValue(SeriesColorProperty); }
            set 
            {
                SetValue(SeriesColorProperty, value);
                rec.Fill = value;
            }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("SeriesColorProperty", typeof(string), typeof(LegendItem),
            new PropertyMetadata(0.0));

        /// <summary>
        /// Series Color
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
                text.Text = value;
            }
        }
        #endregion

    }
}
