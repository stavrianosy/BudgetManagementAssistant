using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace DataVirtualization.Toolkit.Series
{
    public abstract class Series
    {
        public SeriesType Type;

        private DataItemCollection _ItemSource;
        
        
        public DataItemCollection ItemSource
        {
            get { return _ItemSource; }
            
        }

        private Grid _container;

        public Grid Container
        {
            get { return _container; }
            
        }

        private double margin;

        public double Margin
        {
            get { return margin; }
        }
        private Brush _palette;

        public Brush Palette
        {
            get { return _palette; }
        }

        private double _seriesMaxValue = double.NaN;

        internal double SeriesMaxValue
        {
            get 
            {
                if (double.IsNaN(_seriesMaxValue))
                {
                    _seriesMaxValue = getSeriesMaxValue();
                }
                return _seriesMaxValue; 
            }
            
        }



        public abstract void DrawSeries();

        public Series(Grid container, double margin, DataItemCollection dataItem,Brush palette)
        {
            this._container = container;
            this._ItemSource = dataItem;
            this.margin = margin;
            this._palette = palette;
        }

        private double getSeriesMaxValue()
        {
            double maxValue = 0;

            foreach (DataItem item in ItemSource)
            {
                if (item.Value > maxValue)
                    maxValue = item.Value;
            }
            return maxValue;
        }


    }
}
