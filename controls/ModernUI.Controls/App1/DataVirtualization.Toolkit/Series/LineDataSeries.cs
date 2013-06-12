using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVirtualization.Toolkit.DataPoint;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DataVirtualization.Toolkit.Series
{
    class LineDataSeries:Series
    {
        private double radius = 7;

        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        private double lineThickness = 2;

        public double LineThickness
        {
            get { return lineThickness; }
            set { lineThickness = value; }
        }


        public LineDataSeries(Grid container, double margin, DataItemCollection dataItem,Brush palette)
            : base(container, margin, dataItem,palette)
        {

        }

        public override void DrawSeries()
        {
            
            if (ItemSource.Count == 0)
                return;

            double XStep = (this.Container.ActualWidth - Margin) / ItemSource.Count + 1;
            double CurrentStep = 1;
            double YMaxValue = 0;
            foreach (DataItem item in ItemSource)
            {
                if (item.Value > YMaxValue)
                    YMaxValue = item.Value;
            }
            double YStep = (Container.ActualHeight - Margin) / YMaxValue;
            Point prevPoint;
            bool FirstPoint = true;
            foreach (DataItem item in ItemSource)
            {
                LineDataPoint DataPoint = new LineDataPoint()
                {
                    CircleRadius = Radius*2,
                    Palette = Palette,
                    FillPoint = false,
                    StrockThickness = LineThickness
                };
                DataPoint.Margin = new Thickness(CurrentStep * XStep, 0, 0, YStep * item.Value);
                DataPoint.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
                DataPoint.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                Container.Children.Add(DataPoint);
                if (FirstPoint)
                {
                    prevPoint = getLinePlottingPoint(DataPoint);
                    FirstPoint = false;
                }
                else
                {
                    prevPoint = DrawLine(Container, Palette, LineThickness, prevPoint, DataPoint);
                }
                CurrentStep++;
            }
        }

        private Point DrawLine(Grid container, Brush Palette, double linethickness, Point prevPoint, LineDataPoint DataPoint)
        {
            Point Current = getLinePlottingPoint(DataPoint);
            Point[] LinePoints = Utilities.GetLinePointsOnCircle(prevPoint, Current, DataPoint.CircleRadius);


            PathFigure path = new PathFigure();
            path.StartPoint = LinePoints[0];
            path.IsClosed = false;
            LineSegment Line = new LineSegment();
            Line.Point = LinePoints[1];
            path.Segments.Add(Line);
            PathGeometry myPath = new PathGeometry();
            myPath.Figures.Add(path);
            Path output = new Path();
            output.Data = myPath;
            output.StrokeThickness = linethickness;
            output.Stroke = Palette;
            container.Children.Add(output);
            prevPoint = Current;
            return prevPoint;
        }

        private Point getLinePlottingPoint(LineDataPoint dataPoint)
        {
            double x = dataPoint.Margin.Left + (dataPoint.CircleRadius / 2);
            double y = this.Container.ActualHeight - (dataPoint.Margin.Bottom + (dataPoint.CircleRadius / 2));
            return new Point(x, y);
        }
    }
}
