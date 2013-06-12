using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DataVirtualization.Toolkit.DataPoint;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

namespace DataVirtualization.Toolkit.Plotter
{
    public class LinePlotter:Plotter
    {
        //private PieDataPointCollection piePieces = new PieDataPointCollection();
     
        public LinePlotter()
        {
            InitializeComponents();
            this.Loaded += LinePlotter_Loaded; 
            
        }

        void LinePlotter_Loaded(object sender, RoutedEventArgs e)
        {
            PlotPoints();
        }

        public void PlotPoints()
        {
            Container.Children.Clear();
            this.SeriesCollection = new Series.SeriesCollection();
            Series.LineDataSeries lineSeries = new Series.LineDataSeries(Container,40,DataItemCollection.DefaultList(),(Brush)PaletteCollection.defaultSolidBrush()[0]["Background"]);
            SeriesCollection.Add(lineSeries);

            foreach (Series.Series series in SeriesCollection)
            {
                lineSeries.DrawSeries();
            }

            double maxValue = 0;
            foreach (Series.Series series in SeriesCollection)
            {
                if (series.SeriesMaxValue > maxValue)
                    maxValue = series.SeriesMaxValue;
            }

            YAxis xaxis = new YAxis(YAxisContainer, maxValue, _numberogGridLines, margin);
            
        }

        private Point getLinePlottingPoint(LineDataPoint dataPoint)
        {
            double x = dataPoint.Margin.Left + (dataPoint.CircleRadius/2);
            double y = this.Height - (dataPoint.Margin.Bottom + (dataPoint.CircleRadius/2));
            return new Point(x, y);
        }
    }
}
