using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVirtualization.Toolkit.Charting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DataVirtualization.Toolkit.Legends
{
    public class Legend:LegendBase
    {
        public StackPanel panel;
        private bool _contentLoaded = false;
        public Legend()
        {
            InitializeComponents();
            DefaultStyleKey = typeof(Legend);
            this.ApplyTemplate();
            this.Loaded += Legend_Loaded;
        }

        void Legend_Loaded(object sender, RoutedEventArgs e)
        {
            AddLegends();
        }

        public override void AddLegends()
        {
            if (ItemsSource == null)
                return;
            if (ItemsSource.Count == 0)
                return;
            int index = 0;
            panel.Children.Clear();
            foreach(DataItem item in ItemsSource)
            {
                LegendItem legendItem = new LegendItem();
                legendItem.Text = item.Name;
                if (Palette == null)
                    Palette = PaletteCollection.LoadDefaults();
                legendItem.SeriesColor = Palette.getPalette(index);
                panel.Children.Add(legendItem);
                index++;
            }
        }


        public void InitializeComponents()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;

            Application.LoadComponent(this, new System.Uri("ms-appx:///DataVirtualizationToolkit/Legends/Legend.xaml"),
                Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);

            panel = (StackPanel)this.FindName("panel");
            
        }

        #region Dependancy Properties
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("OrientationProperty", typeof(Orientation), typeof(Legend),
            new PropertyMetadata(0.0));

        /// <summary>
        /// Series Color
        /// </summary>
        public Orientation Orientation
        {
            get 
            { 
                return (Orientation)GetValue(OrientationProperty); 
            }
            set
            {
                SetValue(OrientationProperty, value);
                panel.Orientation = value;
            }
        }

        #endregion

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        
    }
}
