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
    }
}