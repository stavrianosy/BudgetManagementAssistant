using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DataVirtualization.Toolkit.Charting
{
    public class PieChart:Chart
    {
        public Plotter.PiePlotter plotter;
        public Legends.Legend legend;

        private bool _contentLoaded = false;   
        public PieChart()
        {
            //DefaultStyleKey = typeof(PieChart);
            InitializeComponents();
            this.Loaded += PieChart_Loaded;
        }

        void PieChart_Loaded(object sender, RoutedEventArgs e)
        {
            
        }


        public void InitializeComponents()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;

            Application.LoadComponent(this, new System.Uri("ms-appx:///DataVirtualizationToolkit/Charting/PieChart.xaml"),
                Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);

            plotter = (Plotter.PiePlotter)this.FindName("Pie");
            legend = (Legends.Legend)this.FindName("Legend");
            plotter.ItemSelected += plotter_ItemSelected;
            plotter.ItemDeSelected += plotter_ItemDeSelected;
            SetValue(ItemsSourceProperty,DataItemCollection.DefaultList());
            SetValue(PaletteProperty, PaletteCollection.LoadDefaults());

        }


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
        }

        #region dependancy properties
        public DataItemCollection ItemsSource
        {
            get { 
                return (DataItemCollection)GetValue(ItemsSourceProperty); 
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
                plotter.ItemsSource = value;
                legend.ItemsSource = value;
                
            }
            
        }
        public static readonly DependencyProperty ItemsSourceProperty =
                       DependencyProperty.Register("ItemsSource", typeof(DataItemCollection), typeof(PieChart), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a palette of ResourceDictionaries used by the children of the Chart.
        /// </summary>
        public PaletteCollection Palette
        {
            get { return GetValue(PaletteProperty) as PaletteCollection; }
            set { 
                SetValue(PaletteProperty, value);
                legend.Palette = value;
                plotter.PiePalette = value;
            }
        }

        /// <summary>
        /// Identifies the Palette dependency property.
        /// </summary>
        public static readonly DependencyProperty PaletteProperty =
            DependencyProperty.Register(
                "Palette",
                typeof(PaletteCollection),
                typeof(PieChart),
                new PropertyMetadata(null));

       
        /// <summary>
        /// Sets the Hole in the Middle of the Chart Size
        /// </summary>
        public static readonly DependencyProperty HoleSizeProperty =
                       DependencyProperty.Register("HoleSize", typeof(double), typeof(PieChart), new PropertyMetadata(0.3));

        /// <summary>
        /// The size of the hole in the centre of circle (as a percentage)
        /// </summary>
        public double HoleSize
        {
            get { return (double)GetValue(HoleSizeProperty); }
            set
            {
                SetValue(HoleSizeProperty, value);
            }
        }
        #endregion

        #region Events
            public event EventHandler<DataItem> ItemSelected;
            public event EventHandler<DataItem> ItemDeSelected;
            void plotter_ItemSelected(object sender, DataItem e)
            {
                if (ItemSelected != null)
                {
                    ItemSelected(sender, e);
                }
            }
            void plotter_ItemDeSelected(object sender, DataItem e)
            {
                if (ItemDeSelected != null)
                {
                    ItemDeSelected(sender, e);
                }
            }
        #endregion
    }
}
