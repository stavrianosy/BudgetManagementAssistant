using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.DataModel
{
    public static class Config
    {
        public static bool TestOnly = false;
        public static Uri RootUri = new Uri("http://www.ys_test.com.cy/", UriKind.Absolute);
    }
}
