using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMA.BusinessLogic;
using BMA.DataAccess;
using System.Collections.ObjectModel;

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

            //User user = new User();
            //user.UserName = "asa";
            //user.Password = "asa";
            //user.Email = "asa@www.com";

            ServiceReference1.MainClient a = new ServiceReference1.MainClient();
            ServiceReference2.StaticClient b = new ServiceReference2.StaticClient();

            var usr = new User() { UserId = 11, UserName = "qqqq", Password = "wwww" };

            //a.GetLatestTransactionsLimit(10, 11);
            //GetAllBudgets(a);
            //b.GetAllStaticData();
            //ForgotPass(b);
            //SaveTypeTransaction(b, usr);
            //SaveTypeInterval(a, b, usr);
            //b.GetAllTypeTransactionReasons();
            //SaveCategories(b, usr);
            //SaveTransactionImages(a, usr);
            //UpdateTransaction(a, usr);
            UpdateBudget(a, usr);
            //var dd = a.GetLatestTransactionDate();
        }

        private static void UpdateBudget(ServiceReference1.MainClient a, User usr)
        {
            var budgets = a.GetAllBudgets(usr.UserId);

            var budget = budgets[0];
            budget.Comments = "remove this comment";
            budget.ModifiedDate = DateTime.Now;

            var budget2 = budgets[1];
            budget2.Comments = "delete";
            budget2.ModifiedDate = DateTime.Now;
            budget2.BudgetId = -1;

            var budget3 = budgets[2];
            budget3.Comments = "remove from deleted";
            budget3.IsDeleted = true;
            budget3.ModifiedDate = DateTime.Now;

            var budgetList = new ObservableCollection<Budget> { budget, budget2, budget3 };

            var result = a.SaveBudgets(budgetList);
        }

        private static void UpdateTransaction(ServiceReference1.MainClient a, User usr)
        {
            var transactions = a.GetLatestTransactions(usr.UserId);
            
            var trans = transactions[0];
            trans.Comments = "";
            trans.ModifiedDate = DateTime.Now;

            var trans2 = transactions[1];
            trans2.Comments = "delete";
            trans2.ModifiedDate = DateTime.Now;
            trans2.TransactionId = -1;

            var trans3 = transactions[2];
            trans3.Comments = "remove from deleted";
            trans3.IsDeleted = true;
            trans3.ModifiedDate = DateTime.Now;
            //trans2.TransactionId = -1;

            var transList = new ObservableCollection<Transaction> { trans, trans2, trans3 };
            foreach (var item in transList)
                item.OptimizeOnTopLevel(Transaction.ImageRemovalStatus.Unchanged);

            var result = a.SaveTransactions(transList);
        }

        private static void SaveTransactionImages(ServiceReference1.MainClient a, User usr)
        {
            var transactions = a.GetAllTransactions(usr.UserId);
            var trans = transactions[0];
            var transWithImages = transactions.FirstOrDefault(x => x.TransactionId == 11132);
            var transImages = a.GetImagesForTransaction(transWithImages.TransactionId);
            
            var transImage = new TransactionImage(usr)
            {
                Transaction = trans,
                Path = "/Assets/login_white.png",
                Name = string.Format("{0} [{1}]", "aaaa", "bbbb"),
                Image = transImages[0].Image,
                Thumbnail = transImages[0].Image
            };

            var transImageList = new TransactionImageList();
            transImageList.Add(transImage);

            transImages.Add(transImage);
            trans.TransactionImages = (TransactionImageList)transImages;
            trans.ModifiedDate = DateTime.Now;
            trans.OptimizeOnTopLevel(Transaction.ImageRemovalStatus.Unchanged);

            a.SaveTransactions(new ObservableCollection<Transaction> { trans });
//            a.SaveTransactionImages(transImages);
        }

        private static void SaveCategories(ServiceReference2.StaticClient b, User usr)
        {
            var allCat = b.GetAllCategories();
            var allReasins = b.GetAllTypeTransactionReasons();

            var catList = new List<Category>();
            var newCat = new Category(usr);
            newCat.Name = "CarTest";

            if (newCat.TypeTransactionReasons == null)
                newCat.TypeTransactionReasons = new List<TypeTransactionReason>();

            newCat.TypeTransactionReasons.Add(allReasins[0]);
            newCat.TypeTransactionReasons.Add(allReasins[1]);

            //var transReasonList = new List<TypeTransactionReason>();
            //var transReason = new TypeTransactionReason(usr);
            //transReason.Name = "CarTest";


            catList.Add(newCat);

            b.SaveCategories(catList, usr);
            //var arrC = st.ToList();

            //var c = b.SaveCategories(st.Categories);
        }

        private static void SaveTypeInterval(ServiceReference1.MainClient a,ServiceReference2.StaticClient b, User usr)
        {
            var cat = b.GetAllCategories();
            var staticData = b.GetAllStaticData();

            staticData.TypeIntervals[0].Amount = 9d;
            staticData.TypeIntervals[0].RecurrenceRuleValue.RulePartValueList[0].Value = "9123";
            staticData.TypeIntervals[0].ModifiedDate = DateTime.Now;
            //staticData.TypeIntervals[1].ModifiedDate = DateTime.Now;

            staticData.TypeIntervals[0].RecurrenceRangeRuleValue.RecurrenceRule = staticData.RecurrenceRules.FirstOrDefault(x => x.Name == Const.Rule.RuleRangeTotalOcurrences.ToString());
            staticData.TypeIntervals[0].RecurrenceRangeRuleValue.RulePartValueList[0].Value = "20100202";
            staticData.TypeIntervals[0].RecurrenceRangeRuleValue.RulePartValueList[1].Value = "3";
            //staticData.TypeIntervals[0].RecurrenceRangeRuleValue.RulePartValueList[1].Value = "b22";
            //staticData.TypeIntervals[0].RecurrenceRangeRuleValue.RulePartValueList[1].Value = "c22";

            var k = new List<TypeInterval>();
            k.Add(staticData.TypeIntervals[0]);

            var update = b.SaveTypeIntervals(k);


            var intervals = new List<TypeInterval>{new TypeInterval(cat, staticData.TypeTransactions, usr)};
            //intervals[0].RecurrenceRule = staticData.RecurrenceRules.FirstOrDefault(x => x.Name == "RuleDailyEveryDays");


            intervals[0].RecurrenceRuleValue.RecurrenceRule = staticData.RecurrenceRules.FirstOrDefault(x => x.Name == "RuleDailyEveryDays") ;
            intervals[0].RecurrenceRuleValue.RulePartValueList[0].Value = "aaa";
            intervals[0].RecurrenceRuleValue.RulePartValueList[1].Value = "bbb";

            intervals[0].RecurrenceRangeRuleValue.RecurrenceRule = staticData.RecurrenceRules.FirstOrDefault(x => x.Name == Const.Rule.RuleRangeTotalOcurrences.ToString());
            intervals[0].RecurrenceRangeRuleValue.RulePartValueList[0].Value = "20111111";
            intervals[0].RecurrenceRangeRuleValue.RulePartValueList[1].Value = "234";


           var result = b.SaveTypeIntervals(intervals);

        }
        
        private static void SaveTypeTransaction(ServiceReference2.StaticClient client, User user)
        {
            //var stData = client.GetAllStaticData();
            //var stDataCat = client.GetAllCategories();
            var stDataTR = client.GetAllTypeTransactionReasons();

            var reasons = new List<TypeTransactionReason>();
            stDataTR[11].Name = "abc";
            //stDataTR[11].Categories.Add(stData.TypeTransactionReasons[0].Categories[0]);
            //stDataTR[11].Categories.RemoveAt(0);
            //stDataTR[11].Categories[0].IsDeleted = true;

            stDataTR[1].TypeTransactionReasonId = -1;
            //stDataTR[1].Categories = null;
            reasons.Add(stDataTR[1]);
            //reasons.Add(stDataTR[11]);

            reasons[0].ModifiedDate = DateTime.Now;
            client.SaveTypeTransactionReasons(reasons);
        }

        private static void ForgotPass(ServiceReference2.StaticClient client)
        {

            var user = new User();
            user.UserName = "qqqq";

            var forgot = client.ForgotPassword(user);
        }
    }
}
