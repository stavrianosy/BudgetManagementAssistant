using System;
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

            //var usr = new User() { UserId = 2, UserName = "stavrianosy", Password = "1234" };
            var usr = new User() { UserId = 4, UserName = "qqqq", Password = "wwww" };
            var auth = b.AuthenticateUser(usr);
            var usr11 = b.GetUpcomingNotifications(DateTime.Now);
            var bud = a.GetAllBudgets();

            var st = b.GetAllStaticData();


            var trans1 = a.GetLatestTransactions();
            var trans = a.GetLatestTransactions();
            var list = new TransactionList();

            Transaction aaaa = trans[0];
            aaaa.TransactionId = -1;
            aaaa.CreatedUser = usr;
            aaaa.ModifiedUser = usr;
            //var transImg = new TransactionImage(usr) { Path = "111asas" };
            var ss = "22 111asas";
            aaaa.TransactionImages[0].Path = ss;
            aaaa.TransactionImages[0].Name = "ss";
            aaaa.TransactionImages[0].ModifiedUser = usr;
            aaaa.TransactionImages.Add(new TransactionImage(usr) { Path = "111asas" });
            list.Add(aaaa);

            aaaa.Amount = 516d;
            aaaa.ModifiedDate = DateTime.Now;
            //var arr = trans.ToArray();

            var newbudList = new List<Budget>();
            var newbud = new Budget(usr);
            newbud.Amount = 512;
            newbud.Name = "qqaaww";
            newbud.ModifiedDate = DateTime.Now;

            newbudList.Add(newbud);

            bud[0].Amount = 102;
            bud[0].Name = "bbbbb";
            bud[0].ModifiedDate = DateTime.Now;

            //var aa = a.SyncTransactions(list.ToList());

            var b1 = a.SaveTransactions(trans);
            var b2 = a.SaveBudgets(newbudList);

            st.Categories[7].Name = "asd";
            st.Categories[7].ModifiedDate = DateTime.Now;

            b.SaveCategories(st.Categories);
            //var arrC = st.Categories.ToList();

           // var c = a.SaveCategories(st.Categories);
        }
    }
}
