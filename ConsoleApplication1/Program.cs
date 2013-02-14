using System;
using BMAServiceLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMA.BusinessLogic;
using BMA.DataAccess;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ServiceReference1.MainClient a = new ServiceReference1.MainClient();
            //BMAServiceLib.Main a = new BMAServiceLib.Main();
            var trans = a.GetAllTransactions();
            trans[0].Amount = 16d;
            var arr = trans.ToArray();

            var st = a.GetAllStaticData();

            var b = a.SaveTransactions(trans);

            var arrC = st.Categories.ToList();

            var c = a.SaveCategories(st.Categories);
        }
    }
}
