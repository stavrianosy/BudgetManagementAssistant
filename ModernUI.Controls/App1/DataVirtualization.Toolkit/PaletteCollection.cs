using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace DataVirtualization.Toolkit
{
    public class PaletteCollection:ObservableCollection<ResourceDictionary>
    {
        public LinearGradientBrush getPalette(int index)
        {
            if (this.Count == 0)
            {
                foreach (ResourceDictionary rs in LoadDefaults())
                {
                    this.Add(rs);
                }
            }
            if (this.Count > index)
            {
                return (LinearGradientBrush)this[index]["Background"];
            }
            return (LinearGradientBrush)this[0]["Background"];
        }

        public static PaletteCollection defaultSolidBrush()
        {
            PaletteCollection pallete = new PaletteCollection();
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Windows.UI.Color.FromArgb(255,19,127,208);
            ResourceDictionary rs1 = new ResourceDictionary();
            rs1.Add("Background",brush);
            pallete.Add(rs1);
            return pallete;
        }

        public static PaletteCollection LoadDefaults()
        {
            PaletteCollection pallete = new PaletteCollection();
            LinearGradientBrush brush1 = new LinearGradientBrush();
            brush1.StartPoint = new Windows.Foundation.Point(0.5, 0);
            brush1.EndPoint = new Windows.Foundation.Point(0.5, 1);
            brush1.GradientStops.Add(new GradientStop());
            brush1.GradientStops.Add(new GradientStop());
            brush1.GradientStops[0].Color = Windows.UI.Color.FromArgb(255, 225, 31, 31);
            brush1.GradientStops[1].Color = Windows.UI.Color.FromArgb(255, 154, 34, 34);
            brush1.GradientStops[1].Offset = 1;
            ResourceDictionary rs1 = new ResourceDictionary();
            rs1.Add("Background", brush1);
            pallete.Add(rs1);

            LinearGradientBrush brush4 = new LinearGradientBrush();
            brush4.StartPoint = new Windows.Foundation.Point(0.5, 0);
            brush4.EndPoint = new Windows.Foundation.Point(0.5, 1);
            brush4.GradientStops.Add(new GradientStop());
            brush4.GradientStops.Add(new GradientStop());
            brush4.GradientStops[0].Color = Windows.UI.Color.FromArgb(255, 13, 95, 167);
            brush4.GradientStops[1].Color = Windows.UI.Color.FromArgb(255, 36, 152, 225);
            brush4.GradientStops[1].Offset = 1;
            ResourceDictionary rs4 = new ResourceDictionary();
            rs4.Add("Background", brush4);
            pallete.Add(rs4);

            LinearGradientBrush brush2 = new LinearGradientBrush();
            brush2.StartPoint = new Windows.Foundation.Point(0.5, 0);
            brush2.EndPoint = new Windows.Foundation.Point(0.5, 1);
            brush2.GradientStops.Add(new GradientStop());
            brush2.GradientStops.Add(new GradientStop());
            brush2.GradientStops[0].Color = Windows.UI.Color.FromArgb(255, 85, 224, 31);
            brush2.GradientStops[1].Color = Windows.UI.Color.FromArgb(255, 67, 150, 34);
            brush2.GradientStops[1].Offset = 1;
            ResourceDictionary rs2 = new ResourceDictionary();
            rs2.Add("Background", brush2);
            pallete.Add(rs2);

            LinearGradientBrush brush3 = new LinearGradientBrush();
            brush3.StartPoint = new Windows.Foundation.Point(0.5, 0);
            brush3.EndPoint = new Windows.Foundation.Point(0.5, 1);
            brush3.GradientStops.Add(new GradientStop());
            brush3.GradientStops.Add(new GradientStop());
            brush3.GradientStops[0].Color = Windows.UI.Color.FromArgb(255, 224, 222, 31);
            brush3.GradientStops[1].Color = Windows.UI.Color.FromArgb(255, 147, 150, 34);
            brush3.GradientStops[1].Offset = 1;
            ResourceDictionary rs3 = new ResourceDictionary();
            rs3.Add("Background", brush3);
            pallete.Add(rs3);

            

            LinearGradientBrush brush6 = new LinearGradientBrush();
            brush6.StartPoint = new Windows.Foundation.Point(0.5, 0);
            brush6.EndPoint = new Windows.Foundation.Point(0.5, 1);
            brush6.GradientStops.Add(new GradientStop());
            brush6.GradientStops.Add(new GradientStop());
            brush6.GradientStops[0].Color = Windows.UI.Color.FromArgb(255, 255, 255, 255);
            brush6.GradientStops[1].Color = Windows.UI.Color.FromArgb(255, 194, 194, 194);
            brush6.GradientStops[1].Offset = 1;
            ResourceDictionary rs6 = new ResourceDictionary();
            rs6.Add("Background", brush6);
            pallete.Add(rs6);

            LinearGradientBrush brush5 = new LinearGradientBrush();
            brush5.StartPoint = new Windows.Foundation.Point(0.5, 0);
            brush5.EndPoint = new Windows.Foundation.Point(0.5, 1);
            brush5.GradientStops.Add(new GradientStop());
            brush5.GradientStops.Add(new GradientStop());
            brush5.GradientStops[0].Color = Windows.UI.Color.FromArgb(255, 111, 16, 168);
            brush5.GradientStops[1].Color = Windows.UI.Color.FromArgb(255, 169, 25, 255);
            brush5.GradientStops[1].Offset = 1;
            ResourceDictionary rs5 = new ResourceDictionary();
            rs5.Add("Background", brush5);
            pallete.Add(rs5);

            
            return pallete;
        }
    }
}
