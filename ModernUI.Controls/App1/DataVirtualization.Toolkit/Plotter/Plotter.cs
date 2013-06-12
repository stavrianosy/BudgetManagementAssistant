using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DataVirtualization.Toolkit.Plotter
{
    public class Plotter:UserControl
    {
        internal int _numberogGridLines = 5;
        internal double margin = 30;
        internal Grid Container;
        internal Grid GridLines;
        internal Grid YAxisContainer;
        internal Grid XAxisContainer;
        internal bool _contentLoaded = false;

        private Series.SeriesCollection _seriesCollection;

        internal Series.SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set { _seriesCollection = value; }
        }


        public Plotter()
        {
            InitializeComponents();
            this.Loaded += LinePlotter_Loaded;
        }
        void LinePlotter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!double.IsNaN(this.Width) && !double.IsNaN(this.Height))
            
            PlotGrid();
        }

        void LinePlotter_Loaded(object sender, RoutedEventArgs e)
        {
            this.SizeChanged += LinePlotter_SizeChanged;
            PlotGrid();
        }


        public void InitializeComponents()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;

            Application.LoadComponent(this, new System.Uri("ms-appx:///DataVirtualizationToolkit/Plotter/Plotter.xaml"),
                Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);

            Container = (Grid)this.FindName("Container");
            GridLines = (Grid)this.FindName("GridLines");
            YAxisContainer = (Grid)this.FindName("YAxisContainer");
            XAxisContainer = (Grid)this.FindName("XAxisContainer");
            //text = (TextBlock)this.FindName("text");

        }

        public void PlotGrid()
        {
            GridLines.Children.Clear();
            double xStart = 5;
            double xEnd = this.ActualWidth - 5;

            double y = 0;
            double step = GridLines.ActualHeight / (_numberogGridLines + 1);

            for (int i = 0; i < _numberogGridLines; i++)
            {
                y += step;
                PathFigure path = new PathFigure();
                path.StartPoint = new Point(xStart, y);
                path.IsClosed = false;
                LineSegment Line = new LineSegment();
                Line.Point = new Point(xEnd, y);
                path.Segments.Add(Line);
                PathGeometry myPath = new PathGeometry();
                myPath.Figures.Add(path);
                Path output = new Path();
                output.Data = myPath;
                output.StrokeThickness = 1;
                output.Stroke = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 150, 150, 150));
                GridLines.Children.Add(output);
            }

            
        }
    }
}
