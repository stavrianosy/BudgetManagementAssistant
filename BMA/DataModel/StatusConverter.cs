using BMA.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace BMA.DataModel
{
    class StatusConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            if (parameter != null) 
            {
                string param = parameter.ToString().ToLower();

                double barSize = 1;
                double.TryParse(value.ToString(), out barSize);
                string width = "1*";

                switch (param)
                {
                    case "daysleft":
                    case "balance":
                        double result = 0d;
                        double.TryParse(value.ToString(), out result);
                        //no need of a break;
                        return string.Format("{0:N0}", result);
                    case "daysleftpercent":
                    case "daysleftpercent_negative":
                        width = string.Format("{0}*", param.Contains("negative") ? barSize : 1 - barSize);

                        return width;
                    case "balancepercent":
                    case "balancepercent_negative":
                        if (barSize > 0d)
                            width = string.Format("{0}*", param.Contains("negative") ? barSize + 1 : Math.Abs(1 - barSize));
                        else if (barSize < 0d)
                            width = string.Format("{0}*", param.Contains("negative") ? 1 : 2 * Math.Abs(barSize) + 1);

                        return width;

                    case "balancecolor":
                        var positive = Math.Abs(1 - barSize);
                        var negative = Math.Abs(1-1 / barSize);

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
                    case "timeformat":
                        DateTime time = DateTime.Now;
                        DateTime.TryParse(value.ToString(), out time);
                        
                        return time.ToString("HH:mm");

                    case "dateformat":
                        DateTime date = DateTime.Now;

                        DateTime.TryParse(value.ToString(), out date);

                        return date.ToString("dd/MM/yyyy");
                    case "listviewfilter":
                        var item = value as BaseItem;
                        if (item == null || item.IsDeleted)
                            item = null;

                        return item;
                    case "transisdeleted":
                        var isDeleted = (bool)value;
                        return !isDeleted ? "Collapsed" : "Visible";
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
                        path = "income.png";
                        break;
                    case "Expense":
                        path = "outcome.png";
                        break;
                    default:
                        break;
                }

                Uri uri = new Uri(string.Format("ms-appx:///Assets/{0}", path));
                return new BitmapImage(uri);
            }
            else if(value is bool)
            {
                var hasChanges = (bool)value;
                string color = hasChanges ? "AliceBlue" : "Green";
                return color;
            }
            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
