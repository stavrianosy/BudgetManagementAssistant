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
            //Transaction t1 = new Transaction();
            //Transaction t2 = new Transaction();
            //TransactionList tl = new TransactionList();
            //tl.Add(t1);
            //tl.Add(t2);

            //t1.HasChanges = true;

            //tl.GetChanges<TransactionList>();

            User user = new User();
            user.UserName = "asa";
            user.Password = "asa";
            user.Email = "asa@www.com";

            ServiceReference1.MainClient a = new ServiceReference1.MainClient();
            ServiceReference2.StaticClient b = new ServiceReference2.StaticClient();
            //BMAServiceLib.Main a = new BMAServiceLib.Main();

            //var auth = b.AuthenticateUser(new User() { UserName="1111", Password="2222"});
            var usr = b.GetUpcomingNotifications(DateTime.Now);
            var bud = a.GetAllBudgets();

            var trans = a.GetAllTransactions();
            var list = new TransactionList();

            //Transaction aaaa = trans[0];
            list.Add(trans[0]);

            trans[0].Amount = 16d;
            var arr = trans.ToArray();

            var aa = a.SyncTransactions(list.ToList());

            //var st = a.GetAllStaticData();

            //var b1 = a.SaveTransactions(trans);

           // var arrC = st.Categories.ToList();

           // var c = a.SaveCategories(st.Categories);
        }
    }
}
