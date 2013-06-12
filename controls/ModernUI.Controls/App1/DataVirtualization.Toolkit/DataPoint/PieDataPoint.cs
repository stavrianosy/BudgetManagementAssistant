using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace DataVirtualization.Toolkit.DataPoint
{
    public class PieDataPoint:DataPoint
    {
        #region dependency properties
     


        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(PieDataPoint),
            new PropertyMetadata(null));

        public Brush Fill
        {
            get{return (Brush)GetValue(FillProperty);}
            set { SetValue(FillProperty, value); }

        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("RadiusProperty", typeof(double), typeof(PieDataPoint),
            new PropertyMetadata(0.0));

        /// <summary>
        /// The radius of this pie piece
        /// </summary>
        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty PushOutProperty =
            DependencyProperty.Register("PushOutProperty", typeof(double), typeof(PieDataPoint),
            new PropertyMetadata(10.0));

        /// <summary>
        /// The distance to 'push' this pie piece out from the centre.
        /// </summary>
        public double PushOut
        {
            get { return (double)GetValue(PushOutProperty); }
            set { SetValue(PushOutProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register("InnerRadiusProperty", typeof(double), typeof(PieDataPoint),
            new PropertyMetadata(10.0));

        /// <summary>
        /// The inner radius of this pie piece
        /// </summary>
        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty WedgeAngleProperty =
            DependencyProperty.Register("WedgeAngleProperty", typeof(double), typeof(PieDataPoint),
            new PropertyMetadata(0.0));

        /// <summary>
        /// The wedge angle of this pie piece in degrees
        /// </summary>
        public double WedgeAngle
        {
            get { return (double)GetValue(WedgeAngleProperty); }
            set
            {
                SetValue(WedgeAngleProperty, value);
                this.Percentage = (value / 360.0);
            }
        }

        public static readonly DependencyProperty RotationAngleProperty =
            DependencyProperty.Register("RotationAngleProperty", typeof(double), typeof(PieDataPoint),
            new PropertyMetadata(0.0));

        /// <summary>
        /// The rotation, in degrees, from the Y axis vector of this pie piece.
        /// </summary>
        public double RotationAngle
        {
            get { return (double)GetValue(RotationAngleProperty); }
            set { SetValue(RotationAngleProperty, value); }
        }

        public static readonly DependencyProperty CentreXProperty =
            DependencyProperty.Register("CentreXProperty", typeof(double), typeof(PieDataPoint),
            new PropertyMetadata(0.0));

        /// <summary>
        /// The X coordinate of centre of the circle from which this pie piece is cut.
        /// </summary>
        public double CentreX
        {
            get { return (double)GetValue(CentreXProperty); }
            set { SetValue(CentreXProperty, value); }
        }

        public static readonly DependencyProperty CentreYProperty =
            DependencyProperty.Register("CentreYProperty", typeof(double), typeof(PieDataPoint),
            new PropertyMetadata(0.0));

        /// <summary>
        /// The Y coordinate of centre of the circle from which this pie piece is cut.
        /// </summary>
        public double CentreY
        {
            get { return (double)GetValue(CentreYProperty); }
            set { SetValue(CentreYProperty, value); }
        }

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("PercentageProperty", typeof(double), typeof(PieDataPoint),
            new PropertyMetadata(0.0));

        /// <summary>
        /// The percentage of a full pie that this piece occupies.
        /// </summary>
        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            private set { SetValue(PercentageProperty, value); }
        }

        public static readonly DependencyProperty PieceValueProperty =
            DependencyProperty.Register("PieceValueProperty", typeof(double), typeof(PieDataPoint),
            new PropertyMetadata(0.0));

        /// <summary>
        /// The value that this pie piece represents.
        /// </summary>
        public double PieceValue
        {
            get { return (double)GetValue(PieceValueProperty); }
            set { SetValue(PieceValueProperty, value); }
        }

        private Point PushoutPoint
        {
            get
            {
                return Utilities.ComputeCartesianCoordinate(RotationAngle + WedgeAngle / 2, PushOut);
            }
        }

        private DataItem _data;
        public DataItem Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }


        #endregion

        public bool Pushedout = false;
        
        public PieDataPoint()
        {
            DefaultStyleKey = typeof(PieDataPoint);
            this.ApplyTemplate();
        }

        void PieDataPoint_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Path myPath = (Path)this.GetTemplateChild("Slice");
            myPath.Data = DrawGeometry();
            if (this.Fill != null)
            {
                myPath.Fill = this.Fill;
            }
        }

        public void OutAnimation()
        {
            this.RenderTransform = new CompositeTransform();
            Storyboard story = new Storyboard();
            DoubleAnimation xAmination = new DoubleAnimation();
            xAmination.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            xAmination.To = this.PushoutPoint.X;
            story.Children.Add(xAmination);
            Storyboard.SetTarget(xAmination, this);
            Storyboard.SetTargetProperty(xAmination, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");

            DoubleAnimation yAnimation = new DoubleAnimation();
            yAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            yAnimation.To = this.PushoutPoint.Y;
            story.Children.Add(yAnimation);
            Storyboard.SetTarget(yAnimation, this);
            Storyboard.SetTargetProperty(yAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
            story.Begin();
        }

        public void InAnimation()
        {
            this.RenderTransform = new CompositeTransform();
            Storyboard story = new Storyboard();
            DoubleAnimation xAmination = new DoubleAnimation();
            xAmination.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            xAmination.To = 0;
            xAmination.From = this.PushoutPoint.X;
            story.Children.Add(xAmination);
            Storyboard.SetTarget(xAmination, this);
            Storyboard.SetTargetProperty(xAmination, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");

            DoubleAnimation yAnimation = new DoubleAnimation();
            yAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            yAnimation.To = 0;
            yAnimation.From = this.PushoutPoint.Y;
            story.Children.Add(yAnimation);
            Storyboard.SetTarget(yAnimation, this);
            Storyboard.SetTargetProperty(yAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
            story.Begin();
        }

        

        private PathGeometry DrawGeometry()
        {

            bool largeArc = WedgeAngle > 180.0;
            Size outerArcSize = new Size(Radius, Radius);
            Size innerArcSize = new Size(InnerRadius, InnerRadius);

            Point innerArcStartPoint = Utilities.ComputeCartesianCoordinate(RotationAngle, InnerRadius);
            Point ButtomLineEndPoint = Utilities.ComputeCartesianCoordinate(RotationAngle, Radius);
            Point OuterArcEndPoint = Utilities.ComputeCartesianCoordinate(RotationAngle + WedgeAngle, Radius);
            Point EndLineEndPoint = Utilities.ComputeCartesianCoordinate(RotationAngle + WedgeAngle, InnerRadius);


            innerArcStartPoint.X += CentreX;
            innerArcStartPoint.Y += CentreY;
            ButtomLineEndPoint.X += CentreX;
            ButtomLineEndPoint.Y += CentreY;
            OuterArcEndPoint.X += CentreX;
            OuterArcEndPoint.Y += CentreY;
            EndLineEndPoint.X += CentreX;
            EndLineEndPoint.Y += CentreY;


            PathFigure path = new PathFigure();
            path.StartPoint = innerArcStartPoint;


            ArcSegment InnerArc = new ArcSegment();
            InnerArc.Size = innerArcSize;
            InnerArc.SweepDirection = SweepDirection.Counterclockwise;
            InnerArc.Point = innerArcStartPoint;
            InnerArc.IsLargeArc = largeArc;
            
            LineSegment ButtomLine = new LineSegment();
            ButtomLine.Point = ButtomLineEndPoint;
            ArcSegment OuterArc = new ArcSegment();
            OuterArc.SweepDirection = SweepDirection.Clockwise;
            OuterArc.Point = OuterArcEndPoint;
            OuterArc.Size = outerArcSize;
            OuterArc.IsLargeArc = largeArc;
            LineSegment EndLine = new LineSegment();
            EndLine.Point = EndLineEndPoint;
            path.Segments.Add(ButtomLine);
            path.Segments.Add(OuterArc);
            path.Segments.Add(EndLine);
            path.Segments.Add(InnerArc);
            
            PathGeometry myPath = new PathGeometry();
            myPath.Figures.Add(path);
            return myPath;
        }
    }
}
