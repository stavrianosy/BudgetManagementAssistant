using System;
using BMAServiceLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMA.BusinessLogic;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.MainClient a = new ServiceReference1.MainClient();
            //BMAServiceLib.Main a = new BMAServiceLib.Main();
            var t = a.GetAllStaticData();
        }
    }
}
