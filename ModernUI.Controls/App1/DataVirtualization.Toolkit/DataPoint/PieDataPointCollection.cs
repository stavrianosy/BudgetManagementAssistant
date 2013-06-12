using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataVirtualization.Toolkit.DataPoint
{
    class PieDataPointCollection:List<PieDataPoint>
    {
        private PieDataPoint _pieDataPoint;
        public PieDataPoint Selected
        {
            get
            {
                return _pieDataPoint;
            }
            set
            {
                _pieDataPoint = value;
            }
        }

    }
}
