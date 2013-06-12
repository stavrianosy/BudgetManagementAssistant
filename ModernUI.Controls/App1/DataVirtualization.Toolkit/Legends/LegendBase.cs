using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DataVirtualization.Toolkit.Legends
{
    public abstract class LegendBase:UserControl
    {
        public abstract void AddLegends();
        public LegendBase()
        {
            
        }
        private DataItemCollection _itemSource;
        public DataItemCollection ItemsSource
        {
            get { return _itemSource; }
            set
            {
                _itemSource = value;
                AddLegends();
            }

        }
        private PaletteCollection _palette;
        public PaletteCollection Palette
        {
            get { return _palette; }
            set { _palette =value; }
        }

        

    }
}
