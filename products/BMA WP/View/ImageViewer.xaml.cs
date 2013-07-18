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
            int.TryParse(this.NavigationContext.QueryString["transImageId"], out transimageId);

            var TransImage = App.Instance.ServiceData.TransactionImageList.FirstOrDefault(x => x.TransactionImageId == transimageId);

            BitmapImage newImage = new BitmapImage();

            using (MemoryStream ms = new MemoryStream(TransImage.Image, 0, TransImage.Image.Length))
            {
                ms.Write(TransImage.Image, 0, TransImage.Image.Length);
                newImage.SetSource(ms);

                transImage.Source = newImage;
            } 
        }
    }
}