using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace DataVirtualization.Toolkit
{
    public class Utilities
    {
        public static Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle - 90);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }

        public static Point[] GetLinePointsOnCircle(Point start, Point end, double diameter)
        {
            double deltaY = end.Y - start.Y;
            double deltaX = end.X - start.X;
            double radius = diameter / 2;
            double angleDeg = Math.Atan2(deltaY, deltaX) * (180 / Math.PI);

            Point[] points = new Point[2];
            points[0] = ComputeLinePointCoordinate(angleDeg, radius, start);
            points[1] = ComputeLinePointCoordinate(angleDeg+180, radius, end);
            return points;
        }

        public static Point ComputeLinePointCoordinate(double angle, double radius,Point cicleCenter)
        {
           
            // convert to radians
            double angleRad = (Math.PI / 180.0) * (angle);

            double x = radius * Math.Cos(angleRad);
            double y = radius * Math.Sin(angleRad);
            Point point = new Point(x, y);

            point.X += cicleCenter.X;
            point.Y += cicleCenter.Y;
            return point;

            
        }
    }
}
