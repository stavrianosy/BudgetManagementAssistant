using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace DataVirtualization.Toolkit
{
    public class DataItem
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private double _value;

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private List<string> _ExtraData;

        public List<string> ExtraData
        {
            get { 
                return _ExtraData; 
            }
            set {
                if (_ExtraData == null)
                    _ExtraData = new List<string>();

                _ExtraData = value; 
            }
        }

        private Brush _palette;
        internal Brush Palette
        {
            get
            {
                return _palette;
            }
            set
            {
                _palette = value;
            }
        }

        private string _percentage;
        internal string Percentage
        {
            get
            {
                return _percentage;
            }
            set
            {
                _percentage = value;
            }
        }

        private string _dataSymbol;
        
        public string DataSymbol
        {
            get
            {
                return _dataSymbol;
            }
            set
            {
                _dataSymbol = value;
            }
        }


    }
}
