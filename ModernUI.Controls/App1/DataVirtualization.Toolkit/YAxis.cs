using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DataVirtualization.Toolkit
{
    public class YAxis
    {
        private Grid Container;
        private double MaxValue;
        private int Gridlines;
        private double Margin;
        public YAxis(Grid container, double MaxValue, int gridLines,double margin)
        {
            this.Container = container;
            this.MaxValue = MaxValue;
            this.Gridlines = gridLines;
            this.Margin = margin;
            DrawAxis();
        }

        private void DrawAxis()
        {
            double YStep = (Container.ActualHeight) / (Gridlines+1);
            double ValueStep = (Container.ActualHeight - Margin) / MaxValue;
            double y = 0;
            for (int i = 0; i < Gridlines; i++)
            {
                y += YStep;
                TextBlock text = new TextBlock();
                text.Text = Math.Round(y/ValueStep).ToString();
                text.Margin = new Windows.UI.Xaml.Thickness(5, 0, 0, y);
                text.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
                text.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(100, 150, 150, 150));
                text.FontWeight = Windows.UI.Text.FontWeights.Bold;
                Container.Children.Add(text);
            }
        }

    }
}
