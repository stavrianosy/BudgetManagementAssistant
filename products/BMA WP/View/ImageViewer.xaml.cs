using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.IO;
using BMA.BusinessLogic;
using System.Windows.Input;
using System.Windows.Media;

namespace BMA_WP.View
{
    public partial class ImageViewer : PhoneApplicationPage
    {
        public ImageViewer()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var transimageId = 0;
            var transactionId = 0;

            int.TryParse(this.NavigationContext.QueryString["transImageId"], out transimageId);
            int.TryParse(this.NavigationContext.QueryString["transId"], out transactionId);

            var Transaction = App.Instance.ServiceData.TransactionList.FirstOrDefault(x => x.TransactionId == transactionId);
            TransactionImage TransImage = null;

            if (Transaction != null && Transaction.TransactionImages != null)
                TransImage = Transaction.TransactionImages.FirstOrDefault(x=>x.TransactionImageId == transimageId);

            if (TransImage == null)
                return;

            BitmapImage newImage = new BitmapImage();

            using (MemoryStream ms = new MemoryStream(TransImage.Image, 0, TransImage.Image.Length))
            {
                ms.Write(TransImage.Image, 0, TransImage.Image.Length);
                newImage.SetSource(ms);

                transImage.Source = newImage;
            } 
        }

        private void Image_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            if (e.PinchManipulation != null)
            {
                var transform = (CompositeTransform)transImage.RenderTransform;

                // Scale Manipulation
                transform.ScaleX = e.PinchManipulation.CumulativeScale;
                transform.ScaleY = e.PinchManipulation.CumulativeScale;

                // Translate manipulation
                var originalCenter = e.PinchManipulation.Original.Center;
                var newCenter = e.PinchManipulation.Current.Center;
                transform.TranslateX = newCenter.X - originalCenter.X;
                transform.TranslateY = newCenter.Y - originalCenter.Y;

                // Rotation manipulation
                //transform.Rotation = angleBetween2Lines(
                //    e.PinchManipulation.Current,
                //    e.PinchManipulation.Original);

                // end 
                e.Handled = true;
            }
        }
        
        // copied from http://www.developer.nokia.com/Community/Wiki/Real-time_rotation_of_the_Windows_Phone_8_Map_Control
        public static double angleBetween2Lines(PinchContactPoints line1, PinchContactPoints line2)
        {
            if (line1 != null && line2 != null)
            {
                double angle1 = Math.Atan2(line1.PrimaryContact.Y - line1.SecondaryContact.Y,
                                           line1.PrimaryContact.X - line1.SecondaryContact.X);
                double angle2 = Math.Atan2(line2.PrimaryContact.Y - line2.SecondaryContact.Y,
                                           line2.PrimaryContact.X - line2.SecondaryContact.X);
                return (angle1 - angle2) * 180 / Math.PI;
            }
            else { return 0.0; }
        }
    }
}