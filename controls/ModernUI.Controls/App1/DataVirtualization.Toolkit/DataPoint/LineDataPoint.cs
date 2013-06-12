using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DataVirtualization.Toolkit.DataPoint
{
    public class LineDataPoint:DataPoint
    {
        private Point _point;
        Ellipse ellipse = null;

        public Point Point
        {
            get { return _point; }
            set { _point = value; }
        }

        private double _circleRadius;

        public double CircleRadius
        {
            get { return _circleRadius; }
            set { _circleRadius = value; }
        }

        private double _strockThickness;

        public double StrockThickness
        {
            get { return _strockThickness; }
            set {
                _strockThickness = value;
                
            }
        }

        private bool _fillPoint;

        public bool FillPoint
        {
            get { return _fillPoint; }
            set 
            {
                _fillPoint = value;
            }
        }

        private Brush _palette;

        public Brush Palette
        {
            get { return _palette; }
            set { _palette = value; }
        }
        





        public LineDataPoint()
        {
            DefaultStyleKey = typeof(LineDataPoint);
            this.ApplyTemplate();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ellipse = (Ellipse)this.GetTemplateChild("Point");

            this.Height = CircleRadius;
            this.Width = CircleRadius;
            Point = new Point(this.Height / 2, this.Height / 2);
            ellipse.StrokeThickness = this.StrockThickness;
            ellipse.Stroke = this.Palette;
            
            if (FillPoint)
                ellipse.Fill = this.Palette;
            else
                ellipse.Fill = null;

        }
    }
}
