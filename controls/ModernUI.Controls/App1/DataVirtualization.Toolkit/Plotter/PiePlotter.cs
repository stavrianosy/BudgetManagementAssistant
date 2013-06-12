using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using DataVirtualization.Toolkit.Charting;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using DataVirtualization.Toolkit.DataPoint;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace DataVirtualization.Toolkit.Plotter
{
    public class PiePlotter:UserControl
    {
        #region dependency properties
        /// <summary>
        /// The size of the hole in the centre of circle (as a percentage)
        /// </summary>
        public double HoleSize
        {
            get { return (double)GetValue(PieChart.HoleSizeProperty); }
            set
            {
                SetValue(PieChart.HoleSizeProperty, value);
                ConstructPiePieces();
            }
        }

        

        public static readonly DependencyProperty HoleSizeProperty =
                       DependencyProperty.Register("HoleSize", typeof(double), typeof(PiePlotter), new PropertyMetadata(0.0));


        private DataItemCollection _ItemSourceource;
        public DataItemCollection ItemsSource
        {
            get { return _ItemSourceource; }
            set
            {
                _ItemSourceource = value;
                ConstructPiePieces();
            }

        }
        public static readonly DependencyProperty PaletteCollectionProperty =
                       DependencyProperty.Register("PiePalette", typeof(PaletteCollection), typeof(PiePlotter), new PropertyMetadata(null));
        private PaletteCollection _palette = new PaletteCollection();
        public PaletteCollection PiePalette
        {
            get 
            {
                return _palette;
            }
            set 
            {
                SetValue(PaletteCollectionProperty, value);
                _palette = value; 
            }
        }

        #endregion

        /// <summary>
        /// A list which contains the current piece pieces, where the piece index
        /// is the same as the index of the item within the collection view which 
        /// it represents.
        /// </summary>
        private PieDataPointCollection piePieces = new PieDataPointCollection();
        private Canvas Container;
        private bool _contentLoaded = false;  
        public PiePlotter()
        {
            InitializeComponents();
            //DefaultStyleKey = typeof(PiePlotter);
            
            this.Loaded += PiePlotter_Loaded;
        }

        public void InitializeComponents()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;

            Application.LoadComponent(this, new System.Uri("ms-appx:///DataVirtualizationToolkit/Plotter/PiePlotter.xaml"),
                Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);

            Container = (Canvas)this.FindName("Container");
            //text = (TextBlock)this.FindName("text");

            
        }

        void PiePlotter_Loaded(object sender, RoutedEventArgs e)
        {
            ConstructPiePieces();
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Container = (Canvas)this.GetTemplateChild("Container"); 
        }

        private void ConstructPiePieces()
        {

            if (ItemsSource == null)
                return;
            if (ItemsSource.Count == 0)
                return;

            double halfWidth = this.ActualWidth / 2;
            double innerRadius = halfWidth * HoleSize;

            // compute the total for the property which is being plotted
            double total = 0;
            foreach (DataItem item in ItemsSource)
            {
                total += GetPlottedPropertyValue(item);
            }

            // add the pie pieces
            Container.Children.Clear();
            double accumulativeAngle = 0;
            int index = 0;
            foreach (DataItem item in ItemsSource)
            {
                double wedgeAngle = GetPlottedPropertyValue(item) * 360 / total;
                if (wedgeAngle == 360)
                {
                    wedgeAngle = 359.9;
                }
                PieDataPoint piece = new PieDataPoint()
                {
                    Radius = halfWidth,
                    InnerRadius = innerRadius,
                    CentreX = halfWidth,
                    CentreY = halfWidth,
                    WedgeAngle = wedgeAngle,
                    RotationAngle = accumulativeAngle,
                };
                piece.Tapped += piece_Tapped;
                if (PiePalette == null)
                {
                    PiePalette = PaletteCollection.LoadDefaults();
                }
                piece.Fill = PiePalette.getPalette(index);

                item.Percentage = Math.Round(((item.Value / total) * 100)).ToString() + "%";
                item.Palette = PiePalette.getPalette(index);
                Container.Children.Insert(0, piece);
                piece.Data = item;

                accumulativeAngle += wedgeAngle;
                index++;
            }

        }

        void piece_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ToolTip tooltip = null;
            PieDataPoint piece = (PieDataPoint)sender;
            Grid parent = (Grid)this.Parent;
            if (!piece.Pushedout)
            {
                if (piePieces.Selected != null)
                {
                    tooltip = (ToolTip)parent.FindName("ToolTip"+piePieces.Selected.Data.ID.ToString());
                    if (tooltip != null)
                    {
                        tooltip.HideData();
                        
                    }
                    piePieces.Selected.InAnimation();
                    piePieces.Selected.Pushedout = false;
                }
                piePieces.Selected = piece;
                piece.OutAnimation();
                piece.Pushedout = true;

                tooltip = new ToolTip();
                tooltip.ParentGrid = parent;
                tooltip.Name = "ToolTip" + piece.Data.ID.ToString();
                try
                {
                    parent.Children.Add(tooltip);
                }
                catch { }
                Grid.SetColumn(tooltip, 0);
                Grid.SetColumnSpan(tooltip, 2);
                tooltip.Margin = new Thickness(0);
                tooltip.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
                tooltip.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                tooltip.ViewData(piece.Data);
                piePieces.Selected = piece;
                
                if(ItemSelected != null)
                {
                    ItemSelected(piece, piece.Data);
                }
                
                
            }
            else
            {
                tooltip = (ToolTip)parent.FindName("ToolTip"+piece.Data.ID.ToString());
                if (tooltip != null)
                {
                    tooltip.HideData();
                }
                piece.InAnimation();
                piece.Pushedout = false;
                piePieces.Selected = null;
                if (ItemDeSelected != null)
                {
                    ItemDeSelected(piece, piece.Data);
                }
            }
        }

        private double GetPlottedPropertyValue(DataItem item)
        {
            return item.Value;
        }

        #region Events

        public event EventHandler<DataItem> ItemSelected;
        public event EventHandler<DataItem> ItemDeSelected;

        #endregion 

    }
}
