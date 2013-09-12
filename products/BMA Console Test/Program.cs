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
            //User user = new User();
            //user.UserName = "asa";
            //user.Password = "asa";
            //user.Email = "asa@www.com";

            ServiceReference1.MainClient a = new ServiceReference1.MainClient();
            ServiceReference2.StaticClient b = new ServiceReference2.StaticClient();

            var usr = new User() { UserId = 1006, UserName = "qqqq", Password = "wwww" };


            //var rep = Reports(a, usr);
            //var tic = b.GetAllTypeIntervals(usr.UserId);
            //SaveNotifications(b, usr);
            //var newuser = CreateUser(b);
            //var db = b.GetDBStatus();
            //SyncTransactions(a, usr);
            //a.GetLatestTransactionsLimit(10, 11);
            //var tt = b.GetAllTypeTransactions(4);
            //var cat = b.GetAllCategories(usr.UserId);
            //var allTrans = a.GetLatestTransactions(usr.UserId);
            //var bud = a.GetAllBudgets(usr.UserId);
            //b.GetAllStaticData();
            //ForgotPass(b);
            //SaveTypeTransactionReasons(b, usr);
            //UpdateTypeInterval(a, b, usr);
            //var typeTransReason = b.GetAllTypeTransactionReasons(usr.UserId);
            //var cat = SaveCategories(b, usr);
            //SaveTransactionImages(a, usr);
            //UpdateTransaction(a, usr);
            //UpdateBudget(a, usr);
            //var dd = a.GetLatestTransactionDate(usr.UserId);
        }

        private static object Reports(ServiceReference1.MainClient a, User usr)
        {
            var dateFrom = new DateTime(2013,7,1);
            var dateTo = new DateTime(2013, 8, 30);
            var transTypeId = 2;
            var amountFrom = 1d;
            var amountTo = 10d;

            //var result = a.ReportTransactionAmount(dateFrom, dateTo, transTypeId, amountFrom, amountTo, usr.UserId);
            //var result = a.ReportTransactionCategory(dateFrom, dateTo, transTypeId, usr.UserId);
            //var result = a.ReportTransactionNameOfPlace(dateFrom, dateTo, transTypeId, usr.UserId);
            var result = a.ReportTransactionByPeriod(dateFrom, dateTo, transTypeId, 
                                                    Const.ReportPeriod.Daily, usr.UserId);
            

            return "";
        }

        private static void SaveNotifications(ServiceReference2.StaticClient b, User usr)
        {
            var notes = b.GetAllNotifications(usr.UserId);

            var newNote1 = notes[0];

            newNote1.NotificationId= -1;
            newNote1.Name = "eee";
            newNote1.ModifiedDate = DateTime.Now;

            notes = b.SaveNotifications(new List<Notification>{newNote1});
        }

        private static User CreateUser(ServiceReference2.StaticClient b)
        {
            User usr = new User();

            usr.UserName = "aaaa";
            usr.Password = "bbbb";
            usr.Email = "bbbb@aaaa.com";

            return b.RegisterUser(usr);
        }

        private static void SyncTransactions(ServiceReference1.MainClient a, User usr)
        {
            var transactions = a.GetLatestTransactions(usr.UserId);
            
            transactions[0].TransactionId = 0;
            
            transactions[1].ModifiedDate = transactions[1].ModifiedDate.AddDays(-1);
            
            transactions[2].TipAmount = 0;
            transactions[2].ModifiedDate = transactions[2].ModifiedDate.AddDays(1);

            var onDate = a.SyncTransactions(transactions);
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

        private static List<Category> SaveCategories(ServiceReference2.StaticClient b, User usr)
        {
            var allCat = b.GetAllCategories(usr.UserId);
            var allReasons = b.GetAllTypeTransactionReasons(usr.UserId);

            var catList = new CategoryList();
            var reasonList = new TypeTransactionReasonList();

            allReasons.ForEach(x=>reasonList.Add(x));
            var newCat = new Category(usr, reasonList);
            newCat = allCat[2];
            newCat.TypeTransactionReasons[1].IsDeleted = true;
            newCat.TypeTransactionReasons[1].ModifiedDate = DateTime.Now;

            newCat.HasChanges = true;
            newCat.ModifiedDate = DateTime.Now;

            //newCat.Name = "CarTest";

            //if (newCat.TypeTransactionReasons == null)
            //    newCat.TypeTransactionReasons = new List<TypeTransactionReason>();

            //newCat.TypeTransactionReasons.Add(allReasons[0]);
            //newCat.TypeTransactionReasons.Add(allReasons[1]);

            //var transReasonList = new List<TypeTransactionReason>();
            //var transReason = new TypeTransactionReason(usr);
            //transReason.Name = "CarTest";


            catList.Add(newCat);

            catList.OptimizeOnTopLevel();

            var result = b.SaveCategories(catList.ToList());
            //var arrC = st.ToList();

            //var c = b.SaveCategories(st.Categories);

            return result;
        }

        private static void UpdateTypeInterval(ServiceReference1.MainClient a, ServiceReference2.StaticClient b, User usr)
        {
            var cat = b.GetAllCategories(usr.UserId);
            var typeIntervals = b.GetAllTypeIntervals(usr.UserId);
            var recurrenceRules = b.GetAllRecurrenceRules();
            var typeTransactions = b.GetAllTypeTransactions(usr.UserId);

            // UPDATE //
            typeIntervals[0].Amount = 19d;
            typeIntervals[0].TransactionType = typeTransactions[1];
            typeIntervals[0].RecurrenceRuleValue.RulePartValueList[0].Value = "9123";
            typeIntervals[0].ModifiedDate = DateTime.Now;
            //staticData.TypeIntervals[1].ModifiedDate = DateTime.Now;

            typeIntervals[0].RecurrenceRangeRuleValue.RecurrenceRule = recurrenceRules.FirstOrDefault(x => x.Name == Const.Rule.RuleRangeTotalOcurrences.ToString());
            typeIntervals[0].RecurrenceRangeRuleValue.RulePartValueList[0].Value = "20100202";
            typeIntervals[0].RecurrenceRangeRuleValue.RulePartValueList[1].Value = "3";
            //staticData.TypeIntervals[0].RecurrenceRangeRuleValue.RulePartValueList[1].Value = "b22";
            //staticData.TypeIntervals[0].RecurrenceRangeRuleValue.RulePartValueList[1].Value = "c22";

            var k = new List<TypeInterval>();
            k.Add(typeIntervals[0]);

            var update = b.SaveTypeIntervals(k);

        }

        private static void InsertTypeInterval(ServiceReference1.MainClient a,ServiceReference2.StaticClient b, User usr)
        {
            var cat = b.GetAllCategories(usr.UserId);
            var typeTransReason = b.GetAllTypeTransactionReasons(usr.UserId);
            var typeIntervals = b.GetAllTypeIntervals(usr.UserId);
            var recurrenceRules = b.GetAllRecurrenceRules();
            var typeTransactions = b.GetAllTypeTransactions(usr.UserId);

            var catList = new CategoryList();
            cat.ForEach(x => catList.Add(x));

            var typeTransReasonList = new TypeTransactionReasonList();
            typeTransReason.ForEach(x => typeTransReasonList.Add(x));

            var typeTransactionsList = new TypeTransactionList();
            typeTransactions.ForEach(x => typeTransactionsList.Add(x));


            // INSERT //
            var intervals = new TypeIntervalList { new TypeInterval(catList, typeTransReasonList, typeTransactionsList, usr) };
            //intervals[0].RecurrenceRule = staticData.RecurrenceRules.FirstOrDefault(x => x.Name == "RuleDailyEveryDays");


            var found = recurrenceRules.FirstOrDefault(x => x.Name == "RuleDailyEveryDays");
            intervals[0].RecurrenceRuleValue.RecurrenceRule = found;
            intervals[0].RecurrenceRuleValue.RulePartValueList[0].Value = "aaa";
            intervals[0].RecurrenceRuleValue.RulePartValueList[1].Value = "bbb";

            intervals[0].RecurrenceRangeRuleValue.RecurrenceRule = recurrenceRules.FirstOrDefault(x => x.Name == Const.Rule.RuleRangeTotalOcurrences.ToString());
            intervals[0].RecurrenceRangeRuleValue.RulePartValueList[0].Value = "20111111";
            intervals[0].RecurrenceRangeRuleValue.RulePartValueList[1].Value = "234";


            var result = b.SaveTypeIntervals(intervals.ToList());

        }

        private static void SaveTypeTransactionReasons(ServiceReference2.StaticClient client, User usr)
        {
            //var stData = client.GetAllStaticData();
            //var stDataCat = client.GetAllCategories();
            var stDataTR = client.GetAllTypeTransactionReasons(usr.UserId);

            var reasons = new List<TypeTransactionReason>();
            reasons.Add(stDataTR[6]);

            reasons[0].Name = "abc";
            //stDataTR[11].Categories.Add(stData.TypeTransactionReasons[0].Categories[0]);
            //stDataTR[11].Categories.RemoveAt(0);
            reasons[0].Categories[1].IsDeleted = true;
            reasons[0].Categories[2].IsDeleted = true;

            //stDataTR[1].TypeTransactionReasonId = -1;
            //stDataTR[1].Categories = null;
            //reasons.Add(stDataTR[1]);

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
