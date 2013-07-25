﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA.BusinessLogic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BMA_WP.ViewModel;
using BMA_WP.Resources;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;
using Microsoft.Phone.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using BMA_WP.Model;
using Microsoft.Phone;
using System.IO;
using Microsoft.Devices;

namespace BMA_WP.View
{
    public partial class Transactions : PhoneApplicationPage
    {
        #region Private Members
        ApplicationBarIconButton save;
        ApplicationBarIconButton delete;
        ApplicationBarIconButton add;
        ApplicationBarMenuItem mainMenu;
        ApplicationBarMenuItem budget;
        #endregion

        #region Public Properties

        public TransactionViewModel vm
        {
            get { return (TransactionViewModel)DataContext; }
        }

        #endregion

        #region Constructors
        public Transactions()
        {
            InitializeComponent();
        }
        #endregion

        #region Binding
        //workaround for the ListPicker issue when binding object becomes null
        private void SetBindings(bool isEnabled)
        {
            if (isEnabled)
            {
                Binding bindTransType = new Binding("TransactionType");
                bindTransType.Mode = BindingMode.TwoWay;
                bindTransType.Source = vm.CurrTransaction;
                if (vm.CurrTransaction.TransactionType != null &&
                    ((ObservableCollection<TypeTransaction>)cmbType.ItemsSource)
                                                .FirstOrDefault(x => x.TypeTransactionId == vm.CurrTransaction.TransactionType.TypeTransactionId)!=null)
                    cmbType.SetBinding(ListPicker.SelectedItemProperty, bindTransType);

                Binding bindCategory = new Binding("Category");
                bindCategory.Mode = BindingMode.TwoWay;
                bindCategory.Source = vm.CurrTransaction;
                
                bindCategory.Converter = new StatusConverter();
                bindCategory.ConverterParameter = "categoryCloneInstance";
//                bindCategory.UpdateSourceTrigger(

                if (vm.CurrTransaction.Category != null &&
                    ((ObservableCollection<Category>)cmbCategory.ItemsSource)
                        .FirstOrDefault(x => x.CategoryId == vm.CurrTransaction.Category.CategoryId) != null)
                    cmbCategory.SetBinding(ListPicker.SelectedItemProperty, bindCategory);

                Binding bindTransReasonType = new Binding("TransactionReasonType");
                bindTransReasonType.Mode = BindingMode.TwoWay;
                bindTransReasonType.Source = vm.CurrTransaction;
                if (vm.CurrTransaction.TransactionReasonType != null && 
                    ((List<TypeTransactionReason>)cmbReason.ItemsSource)
                        .FirstOrDefault(x => x.TypeTransactionReasonId == vm.CurrTransaction.TransactionReasonType.TypeTransactionReasonId) != null)
                    cmbReason.SetBinding(ListPicker.SelectedItemProperty, bindTransReasonType);
            }
            else
            {
                if(cmbType.GetBindingExpression(ListPicker.SelectedIndexProperty) != null)
                    cmbType.ClearValue(ListPicker.SelectedItemProperty);
            }
        }
        #endregion

        #region Events
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);            
        }
        #endregion

        private void Transactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string piName = (e.AddedItems[0] as PivotItem).Name;
            
            switch (piName)
            {
                case "piTransaction":
                    ItemSelected();
                    SetupAppBar_Transaction();
                   svItem.ScrollToVerticalOffset(0d);
                    break;
                case "piTransactionList":
                    TransactionMultiSelect.SelectedItem = null;
                    vm.CurrTransaction = null;
                    SetupAppBar_TransactionList();
                    break;
            }

            if (vm.CurrTransaction != null)
            {
                vm.CurrTransaction.PropertyChanged += (o, changedEventArgs) =>
                {
                    var items = vm.Transactions.Where(t => t.HasChanges).ToObservableCollection();
                    if (items.Count > 0)
                    {
                        save.IsEnabled = vm.Transactions.HasItemsWithChanges() && vm.IsLoading == false;
                        delete.IsEnabled = true;
                    }
                };
                delete.IsEnabled = true;
            }

        }

        private async void ItemSelected()
        {
            var trans = (Transaction)TransactionMultiSelect.SelectedItem;
            SetBindings(false);

            vm.CurrTransaction = trans;

            if (trans != null)
                SetBindings(true);

            if (vm.CurrTransaction == null || vm.CurrTransaction.IsDeleted)
            {
                vm.IsEnabled = false;
            }
            else
            {
                
                spProgressImages.Visibility = System.Windows.Visibility.Visible;
                await App.Instance.ServiceData.LoadAllTransactionImages(vm.CurrTransaction.TransactionId, (error) =>
                {
                    if (error == null)
                    {
                        //everything is ok
                        //vm.CurrTransaction.TransactionImages.ForEach(x =>
                        //    {
                        //        vm.CurrTransactionImages.Add(x);
                        //    });
                        //var expression = listSelectTransImages.GetBindingExpression(LongListSelector.ItemsSourceProperty);
                        //expression.UpdateSource();
                        //listSelectTransImages.UpdateLayout();
                        
                        spProgressImages.Visibility = System.Windows.Visibility.Collapsed;
                    }
                });

                vm.IsEnabled = vm.IsLoading ? false :true;
            }

        }

        private void SetupAppBar_TransactionList()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            add = new ApplicationBarIconButton();
            add.IconUri = new Uri("/Assets/icons/Dark/add.png", UriKind.Relative);
            add.Text = AppResources.AppBarButtonAdd;
            add.IsEnabled = true;
            ApplicationBar.Buttons.Add(add);
            add.Click += new EventHandler(Add_Click);

            save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/save.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSave;
            save.IsEnabled = vm.Transactions.HasItemsWithChanges() && vm.IsLoading == false;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);
            

            mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            mainMenu.IsEnabled = true;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);

            budget = new ApplicationBarMenuItem();
            budget.Text = AppResources.AppBarButtonBudget;
            budget.IsEnabled = true;
            ApplicationBar.MenuItems.Add(budget);
            budget.Click += new EventHandler(Budget_Click);
        }

        void SetupAppBar_Transaction()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/save.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSave;
            save.IsEnabled = vm.Transactions.HasItemsWithChanges() && vm.IsLoading == false;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);

            delete = new ApplicationBarIconButton();
            delete.IconUri = new Uri("/Assets/icons/Dark/delete.png", UriKind.Relative);
            delete.Text = AppResources.AppBarButtonDelete;
            delete.IsEnabled = false;
            ApplicationBar.Buttons.Add(delete);
            delete.Click += new EventHandler(Delete_Click);

            add = new ApplicationBarIconButton();
            add.IconUri = new Uri("/Assets/icons/Dark/add.png", UriKind.Relative);
            add.Text = AppResources.AppBarButtonAdd;
            add.IsEnabled = true;
            ApplicationBar.Buttons.Add(add);
            add.Click += new EventHandler(Add_Click);

            mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            mainMenu.IsEnabled = true;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);

            budget = new ApplicationBarMenuItem();
            budget.Text = AppResources.AppBarButtonBudget;
            budget.IsEnabled = true;
            ApplicationBar.MenuItems.Add(budget);
            budget.Click += new EventHandler(Budget_Click);
        }

        void MainMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(AppResources.DeleteMessage, AppResources.ConfirmDeletion, MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                vm.CurrTransaction.IsDeleted = true;
                vm.PivotIndex = 1;
                //SaveTransaction();
            }
        }

        private void Budget_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/Budgets.xaml", UriKind.Relative));
        }

        void Save_Click(object sender, EventArgs e)
        {
            SaveTransaction();
        }

        private async void SaveTransaction()
        {
            ManualUpdate();
            if (!ValidateTransaction())
                return;

            vm.IsLoading = true;

            var saveOC = vm.Transactions.Where(t => t.HasChanges).ToObservableCollection();
            //var saveImg = vm.CurrTransactionImages.Where(t => t.HasChanges).ToObservableCollection();


            await App.Instance.ServiceData.SaveTransaction(saveOC, (error) => 
            {
                    vm.IsLoading = false;
            });
            
            //##Images WITH CHANGES will be saved with their transactions but wont be retruned again. 
            //Only after you select specific transaction.
            //await App.Instance.ServiceData.SaveTransactionImages(saveImg); //leave this as comment

            pivotContainer.SelectedIndex = 1;
            save.IsEnabled = vm.Transactions.HasItemsWithChanges() && vm.IsLoading == false;

        }

        private void ManualUpdate()
        {
            //manually update model. textbox dont work well with numeric bindings
            var amount = 0d;
            var tipAmount = 0d;

            double.TryParse(txtAmount.Text, out amount);
            double.TryParse(txtTip.Text, out tipAmount);

            if (vm.CurrTransaction != null)
            {
                vm.CurrTransaction.Amount = amount;
                vm.CurrTransaction.TipAmount = tipAmount;
            }
            //set the focus to a control without keyboard
            cmbType.Focus();

            //end of - manually update model
        }

        private void Add_Click(object sender, EventArgs e)
        {
            ManualUpdate();
            if (!ValidateTransaction())
                return;

            Transaction item = new Transaction(App.Instance.StaticServiceData.CategoryList.ToList(),
                                                App.Instance.StaticServiceData.TypeTransactionList.ToList(),
                                                App.Instance.StaticServiceData.TypeTransactionReasonList.ToList(),
                                                App.Instance.User);

            vm.PivotIndex = 0;

            svItem.ScrollToVerticalOffset(0d);

            vm.Transactions.Add(item);
            TransactionMultiSelect.SelectedItem = item;
            vm.CurrTransaction = item;
            
            SetBindings(true);

            save.IsEnabled = vm.Transactions.HasItemsWithChanges() && vm.IsLoading == false;
            delete.IsEnabled = vm.Transactions.HasItemsWithChanges() && vm.IsLoading == false;
            vm.IsEnabled = vm.IsLoading ? false : true;
        }

        private bool ValidateTransaction()
        {
            var result = true;
            if (vm.CurrTransaction == null)
                return result;

            SolidColorBrush okColor = new SolidColorBrush(new Color() { A = 255, B = 255, G = 255, R = 255 });
            SolidColorBrush errColor = new SolidColorBrush(new Color() { A = 255, B = 75, G = 75, R = 240});

            txtAmount.Background = okColor;
            txtTip.Background = okColor;

            if (vm.CurrTransaction.Amount <= 0 && vm.CurrTransaction.TipAmount <= 0)
            {
                result = false;
                txtAmount.Background = errColor;
                txtTip.Background = errColor;
            }

            if (vm.CurrTransaction.Amount < 0)
            {
                result = false;
                txtAmount.Background = errColor;
            }

            if (vm.CurrTransaction.TipAmount < 0)
            {
                result = false;
                txtTip.Background = errColor;
            }

            if (!result)
                svItem.ScrollToVerticalOffset(0);
            else
            {
                var tempTrans = vm.Transactions.Where(x => !x.IsDeleted && ((x.Amount <= 0 && x.TipAmount <= 0) || (x.Amount < 0 || x.TipAmount < 0))).ToList();
                if (tempTrans.Count>0)
                {
                    result = false;
                    //for more specific message
                    if(tempTrans.Count ==1)
                        MessageBox.Show(string.Format("There is another transaction that failed validation.\nUpdate it from the list and save again."));
                    else
                        MessageBox.Show(string.Format("There are another {0} transactions that failed validation.\nUpdate them from the list and save again.", tempTrans.Count));
                }
            }

            return result;
        }

        private void btnTest_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            PhotoCamera camera = new PhotoCamera();
            //Set the VideoBrush source to the camera 
            // a rectangle in xaml
            //viewfinderBrush.SetSource(camera);

            //camera.CaptureImageAvailable += camera_CaptureImageAvailable;

            //camera.CaptureImage();

            //return;

            CameraCaptureTask cameraTask = new CameraCaptureTask();

            cameraTask.Completed += cameraTask_Completed;

            cameraTask.Show();
        }

        private void camera_CaptureImageAvailable(object sender, ContentReadyEventArgs e)
        {
            var bytes = ReadImageBytes(e.ImageStream);
        }

        private void cameraTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                var bitmap = new BitmapImage();
                bitmap.SetSource(e.ChosenPhoto);

                //byte[] m_Bytes = ReadToEnd(e.ChosenPhoto);

                //imgReceipt.Source = new BitmapImage(new Uri(e.OriginalFileName));
                //vm.CurrTransaction.TransactionImages.Add(new TransactionImage(App.Instance.User) { Path = e.OriginalFileName });

                using (var stream = new MemoryStream())
                {
                     //PictureDecoder.DecodeJpeg(stream);
                }

                WriteableBitmap wBitmap = new WriteableBitmap(bitmap);
                
                double factorThumb = 1l;
                double factorImage = 1l;

                if (wBitmap.PixelHeight > wBitmap.PixelWidth)
                {
                    factorThumb = wBitmap.PixelHeight / 100;
                    factorImage = wBitmap.PixelHeight / 600;
                }
                else
                {
                    factorThumb = wBitmap.PixelWidth / 100;
                    factorImage = wBitmap.PixelWidth / 600;
                }

                int heightThumb = Convert.ToInt32(wBitmap.PixelHeight / factorThumb);
                int widthThumb = Convert.ToInt32(wBitmap.PixelWidth / factorThumb);

                int heightImage = Convert.ToInt32(wBitmap.PixelHeight / factorImage);
                int widthImage = Convert.ToInt32(wBitmap.PixelWidth / factorImage);

                MemoryStream msThumb = new MemoryStream();
                wBitmap.SaveJpeg(msThumb, widthThumb, heightThumb, 0, 100);
                byte[] tn_Bytes = ReadToEnd(msThumb);

                MemoryStream msImage = new MemoryStream();
                wBitmap.SaveJpeg(msImage, widthImage, heightImage, 0, 100);
                byte[] m_Bytes = ReadToEnd(msImage);

                var transImage = new TransactionImage(App.Instance.User) { 
                                                Transaction = vm.CurrTransaction,
                                                Path = "/Assets/login_white.png",
                                                Name = string.Format("{0} [{1}]", vm.CurrTransaction.NameOfPlace, vm.CurrTransaction.TotalAmount),
                                                Image = m_Bytes,
                                                Thumbnail = tn_Bytes
                };
                //vm.CurrTransactionImages.Add(transImage);
                vm.CurrTransaction.TransactionImages.Add(transImage);
                vm.CurrTransaction.HasChanges = true;
                save.IsEnabled = vm.Transactions.HasItemsWithChanges() && vm.IsLoading == false;
                //imgReceipt.Source = bitmap;
            }
        }

        private void deletePhoto_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var transImage = (TransactionImage)((Microsoft.Phone.Controls.MenuItem)sender).DataContext;
            transImage.IsDeleted = true;
            vm.CurrTransaction.HasChanges = true;

            save.IsEnabled = vm.Transactions.HasItemsWithChanges() && vm.IsLoading == false;
        }

        private byte[] ReadImageBytes(BinaryReader brImage)
        {
            byte[] imgByteArray = brImage.ReadBytes((int)(brImage.BaseStream.Length));

            return imgByteArray;
        }

        private byte[] ReadImageBytes(Stream imageStream)
        {
            byte[] imageBytes = new byte[imageStream.Length];
            imageStream.Read(imageBytes, 0, imageBytes.Length);

            return imageBytes;
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        private void btnImage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Image thumbnail = e.OriginalSource as Image;
            var transImageId = thumbnail.Tag.ToString();

            var uri = string.Format("/View/ImageViewer.xaml?transId={0}&transImageId={1}", vm.CurrTransaction.TransactionId,  transImageId);
            NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}