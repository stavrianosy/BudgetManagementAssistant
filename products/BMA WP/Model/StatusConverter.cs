using BMA.BusinessLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Windows.UI;

namespace BMA_WP.Model
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                string param = parameter.ToString().ToLower();

                double barSize = 1;
                if (value != null)
                    double.TryParse(value.ToString(), out barSize);
                string width = "1*";

                switch (param)
                {
                    case "daysleft":
                    case "balance":
                        return GetDouble(value);

                    case "daysleftpercent":
                    case "daysleftpercent_negative":
                        return GetDaysLeftWidth(width, param, barSize);

                    case "balancepercent":
                    case "balancepercent_negative":
                        return GetBalancePercentWidth(width, param, barSize);

                    case "balancecolor":
                        return GetBalanceColor(width, param, barSize);

                    case "timeformat":
                        return GetTimeFormat(value);

                    case "dateformat":
                        return GetDateFormat(value);

                    case "itemisdeleted":
                        return TrueCollapse(value);

                    case "truevisible":
                        return TrueVisible(value);

                    case "visibilityontipamount":
                        return TrueVisibleDouble(value);

                    case "intervalbool":
                        return TransTypeConverter(value);

                    case "bitconverter":
                        return BitConverter(value);

                    case "reversebit":
                        return ReverseBit(value);

                    case "nulltranimages":
                        return GetNullTransImages(value);

                    case "nulllist":
                        return GetNullList(value);

                    case "reasoncatlist":
                        return GetReasonCategoryList(value);

                    case "categoryreasonlist":
                        return GetCategoryReasonList(value);

                    case "categorynotdeleted":
                        return GetCategoryNotDeleted(value);

                    case "changedcolor":
                        return GetChangedColor(value);

                    case "bytestoimage":
                        return ConvertByteArrayToImage(value);

                    case "categorycloneinstance":
                        return CategoryCloneInstance(value);

                    case "deletedimagecolor":
                        return DeletedImageColor(value);

                    case "onlinestatustotext":
                        return OnlineStatusToText(value);

                    case "onlinestatustovisibility":
                        return OnlineStatusToVisibility(value);

                    case "offlinetovisibility":
                        return OfflineToVisibility(value);

                    case "updatetovisibility":
                        return UpdateToVisibility(value);

                    case "filtertransactioreasonbycategory":
                        return GetTypeTransactionReasonByCategory(value);

                    case "intervalimage":
                        return IntervalImage(value);

                    case "periodtodate":
                        return PeriodToDate(value);
                }
            }

            //#region IValueConverter Members
            if (value is TypeTransaction)
            {
                var transType = value as TypeTransaction;
                string path = string.Empty;
                switch (transType.Name)
                {
                    case "Income":
                        path = "wallet_in.png";
                        break;
                    case "Expense":
                        path = "wallet_out.png";
                        break;
                    default:
                        break;
                }

                Uri uri = new Uri(string.Format("/Assets/{0}", path), UriKind.Relative);
                return new BitmapImage(uri);
            }
            else if (value is bool)
            {
                var hasChanges = (bool)value;
                string color = hasChanges ? "AliceBlue" : "Green";
                return color;
            }

            return null;
        }

        private object GetCategoryNotDeleted(object value)
        {
            var result = value as ICollection<Category>;

            if (result != null)
                result = result.Where(x => !x.IsDeleted).ToObservableCollection();

            return result;
        }

        private object PeriodToDate(object value)
        {
            var result = value.ToString();
            var date = new DateTime();

            switch (result.Length)
            {
                //Daily yyyyMMdd
                case 8:
                    date = Helper.ConvertStringToDate(result);
                    result = date.ToShortDateString();
                    break;

                //Monthly yyyyMM
                case 6:
                    date = Helper.ConvertStringToDate(result + "01");
                    result = date.ToString("MMMM yyyy");
                    break;

                //Yearly yyyy
                case 4:
                    result = string.Format("Year {0}", result);
                    break;
            }

            return result;
        }

        private object IntervalImage(object value)
        {
            var typeTrans = (TypeTransaction)value;
            var isIncome = typeTrans.IsIncome;

            string path = isIncome ? "interval_income.png" : "interval_expense.png";

            Uri uri = new Uri(string.Format("/Assets/{0}", path), UriKind.Relative);
            return new BitmapImage(uri);
        }

        private object OfflineToVisibility(object value)
        {
            var status = (StaticServiceData.ServerStatus)value;
            var result = status == StaticServiceData.ServerStatus.Error ? "Visible" : "Collapsed";

            return result;
        }

        private object UpdateToVisibility(object value)
        {
            var status = (StaticServiceData.ServerStatus)value;
            var result = status == StaticServiceData.ServerStatus.Communicating ? "Visible" : "Collapsed";

            return result;
        }

        private object OnlineStatusToHeight(object value)
        {
            var status = (StaticServiceData.ServerStatus)value;
            var result = status != StaticServiceData.ServerStatus.Ok ? "Visible" : "Collapsed";

            return result;
        }

        private object OnlineStatusToVisibility(object value)
        {
            var status = (StaticServiceData.ServerStatus)value;
            var result = status != StaticServiceData.ServerStatus.Ok ? "Visible" : "Collapsed";

            return result;
        }

        private object OnlineStatusToText(object value)
        {
            var result = "";
            var status = (StaticServiceData.ServerStatus)value;

            switch (status)
            {
                case StaticServiceData.ServerStatus.Error:
                    result = "offline";
                    break;
                case StaticServiceData.ServerStatus.Communicating:
                    result = "updating";
                    break;
            }

            return result;
        }











        private object DeletedImageColor(object value)
        {
            var result = (bool)value ? "Red" : "Black";

            return result;
        }

        private object CategoryCloneInstance(object value)
        {
            var category = value as Category;
            return category == null ? null : category.Clone();
        }

        private object GetTypeTransactionReasonByCategory(object value)
        {
            ICollection<TypeTransactionReason> result = null;

            if (value == null)
                return null;

            var cat = value as Category;
            var query = App.Instance.StaticServiceData.CategoryList.Where(x => x.CategoryId == cat.CategoryId).FirstOrDefault();

            if (query != null && query.TypeTransactionReasons != null)
                result = query.TypeTransactionReasons.OrderBy(x => x.Name).ToObservableCollection();


            return result == null ? null : result.Where(x => !x.IsDeleted).ToObservableCollection();
        }

        private object ConvertByteArrayToImage(object value)
        {
            BitmapImage image = null;
            if (value != null && value is byte[])
            {
                byte[] bytes = value as byte[];
                MemoryStream stream = new MemoryStream(bytes);
                image = new BitmapImage();

                image.SetSource(stream);

                return image;
            }

            return image;

        }

        private object GetChangedColor(object value)
        {
            var test = IsLightThemeUsed();
            var color = (bool)value;
            return color ? "Blue" : IsLightThemeUsed() ? "Black" : "White";
        }

        private string GetReasonCategoryList(object value)
        {
            var delim = "";
            var result = new StringBuilder();
            if (value is ICollection<Category>)
            {
                foreach (var i in (ICollection<Category>)value)
                {
                    result.Append(delim);
                    result.Append(i.Name);
                    delim = ", ";
                }
            }
            return result.ToString();
        }

        private string GetCategoryReasonList(object value)
        {
            var delim = "";
            var result = new StringBuilder();
            if (value is ICollection<TypeTransactionReason>)
            {
                foreach (var i in (ICollection<TypeTransactionReason>)value)
                {
                    result.Append(delim);
                    result.Append(i.Name);
                    delim = ", ";
                }
            }
            return result.ToString();
        }

        private object GetNullList(object value)
        {
            var anyList = value;
            if (anyList == null)
                return null;
            else
                return value;
        }

        private object GetNullTransImages(object value)
        {
            var tranImgs = (ObservableCollection<TransactionImage>)value;
            if (tranImgs == null || tranImgs.Count == 0)
                return null;
            else
                return value;
        }

        private string BitConverter(object value)
        {
            var bit = (bool)value;
            return bit ? "true" : "false";
        }

        private string ReverseBit(object value)
        {
            var bit = (bool)value;
            return bit ? "false" : "true";
        }


        private string TransTypeConverter(object value)
        {
            var isIncome = (bool)value;
            return isIncome ? "Income" : "Expense";
        }

        private string TrueVisibleDouble(object value)
        {
            var hasTipAmount = (double)value > 0;
            return hasTipAmount ? "Visible" : "Collapsed";
        }

        private string TrueCollapse(object value)
        {
            var collapsed = (bool)value;
            return collapsed ? "Collapsed" : "Visible";
        }

        private string TrueVisible(object value)
        {
            var visible = (bool)value;
            return visible ? "Visible" : "Collapsed";
        }

        private string VisibilityConverter(object value)
        {
            var isDeleted = (bool)value;
            return !isDeleted ? "Collapsed" : "Visible";
        }

        private string GetDateFormat(object value)
        {
            DateTime date = DateTime.Now;

            DateTime.TryParse(value.ToString(), out date);

            return date.ToString("dd/MM/yyyy");
        }

        private string GetTimeFormat(object value)
        {
            DateTime time = DateTime.Now;
            DateTime.TryParse(value.ToString(), out time);

            return time.ToString("HH:mm");
        }

        private string GetBalanceColor(string width, string param, double barSize)
        {
            var positive = Math.Abs(1 - barSize);
            var negative = Math.Abs(1 - 1 / barSize);

            string color = "#FFFFFFFF";
            int alpha = 255;
            int red = 255;
            int green = 255;
            int blue = 0;

            if (barSize > 0d)
            {
                if (barSize < 1)
                {
                    red = int.Parse(Math.Round(Math.Abs(1 - barSize) * 255, 0).ToString());
                }
                else
                {
                    red = int.Parse(Math.Round(1 / Math.Abs(barSize + 1) * 255, 0).ToString());
                    red = Math.Abs(255 - red);
                }
            }
            else if (barSize < 0d)
                green = int.Parse(Math.Round(255 / (Math.Abs(barSize) + 1), 0).ToString());

            color = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", alpha, red, green, blue);

            return color;
        }

        private string GetBalancePercentWidth(string width, string param, double barSize)
        {
            if (barSize > 0d)
                width = string.Format("{0}*", param.Contains("negative") ? barSize + 1 : Math.Abs(1 - barSize));
            else if (barSize < 0d)
                width = string.Format("{0}*", param.Contains("negative") ? 1 : 2 * Math.Abs(barSize) + 1);

            return width;
        }

        private string GetDaysLeftWidth(string width, string param, double barSize)
        {
            width = string.Format("{0}*", param.Contains("negative") ? barSize : 1 - barSize);
            return width;
        }

        private string GetDouble(object value)
        {
            double result = 0d;
            double.TryParse(value.ToString(), out result);
            //no need of a break;
            return string.Format("{0:N0}", result);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                string param = parameter.ToString().ToLower();
                switch (param)
                {
                    case "categorycloneinstance":
                        return CategoryCloneInstance(value);

                    case "bitconverter":
                        return BitConverter(value);

                    case "reversebit":
                        return ReverseBit(value);

                    default:
                        break;
                }
            }
            return null;
        }

        private bool IsLightThemeUsed()
        {
            //var theme = (System.Windows.Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];
            var theme = (System.Windows.Visibility)Application.Current.Resources["PhoneLightThemeVisibility"];
            return theme == Visibility.Visible;
        }
    }
}
