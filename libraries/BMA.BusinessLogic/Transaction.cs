using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ServiceModel;

namespace BMA.BusinessLogic
{
    public class TransactionList : ObservableCollection<Transaction>, IDataList
    {
        public TransactionList()
        {
            //CollectionChanged += TransactionList_CollectionChanged;
        }

        public TransactionList(TypeIntervalList typeIntervalList, User user)
        {
            foreach (var interval in typeIntervalList)
            {
                //## setup all range elements
                var intervalStartDateStr = interval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeStartDate.ToString()).Value;
                var intervalStartDate = Helper.ConvertStringToDate(intervalStartDateStr);

                var intervalEndDateStr = interval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeEndBy.ToString());
                var intervalEndDate = intervalEndDateStr != null ? Helper.ConvertStringToDate(intervalEndDateStr.Value) : DateTime.Now.AddYears(1);

                var intervalTotalOccStr = interval.RecurrenceRangeRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.RangeTotalOcurrences.ToString());
                var intervalTotalOcc = intervalTotalOccStr != null ? int.Parse(intervalTotalOccStr.Value) : -1;

                var recRule = (Const.Rule)Enum.Parse(typeof(Const.Rule), interval.RecurrenceRuleValue.RecurrenceRule.Name);

                //## setup recurrence transactions
                if (intervalStartDate <= DateTime.Now)
                {
                    switch (recRule)
                    {
                        case Const.Rule.RuleDailyEveryDays:
                            ApplyRuleDailyEveryDays(interval, intervalStartDate, intervalEndDate, intervalTotalOcc, user);
                            break;
                        case Const.Rule.RuleWeeklyEveryWeek:
                            ApplyRuleWeeklyEveryWeek(interval, intervalStartDate, intervalEndDate, intervalTotalOcc, user);
                            break;
                        case Const.Rule.RuleMonthlyDayNum:
                            ApplyRuleMonthlyDayNum(interval, intervalStartDate, intervalEndDate, intervalTotalOcc, user);
                            break;
                        case Const.Rule.RuleMonthlyPrecise:
                            ApplyRuleMonthlyPrecise(interval, intervalStartDate, intervalEndDate, intervalTotalOcc, user);
                            break;
                        case Const.Rule.RuleYearlyOnMonth:
                            ApplyRuleYearlyOnMonth(interval, intervalStartDate, intervalEndDate, intervalTotalOcc, user);
                            break;
                        case Const.Rule.RuleYearlyOnTheWeekDay:
                            ApplyRuleYearlyOnTheWeekDay(interval, intervalStartDate, intervalEndDate, intervalTotalOcc, user);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ApplyRuleYearlyOnTheWeekDay(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, int ruleTotalOccurences, User user)
        {
        }

        private void ApplyRuleYearlyOnMonth(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, int ruleTotalOccurences, User user)
        {
        }

        private void ApplyRuleMonthlyPrecise(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, int ruleTotalOccurences, User user)
        {
            var totalOccurenceDates = new List<DateTime>();
            TimeSpan daysSpan = new TimeSpan();
            int calcTotalOccurences = 0;

            var countOfWeekDayStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyCountOfWeekDay.ToString());
            var countOfWeekDay = countOfWeekDayStr != null ? int.Parse(countOfWeekDayStr.Value) : 1;

            var dayOfTheWeekStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyDayName.ToString());
            var dayOfTheWeek = dayOfTheWeekStr != null ? int.Parse(dayOfTheWeekStr.Value) : 1;

            var frequency = int.Parse(interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyCountOfMonth.ToString()).Value);

            var startDay = ruleStartDate;

            startDay = Helper.GetDayOcurrenceOfMonth(startDay, dayOfTheWeek, countOfWeekDay);


                //find the ending from the range
                if (ruleTotalOccurences > 0)
                {
                    calcTotalOccurences = ruleTotalOccurences;
                }
                else
                {
                    if (ruleEndDate < DateTime.Now)
                        daysSpan = DateTime.Now.Subtract(startDay);
                    else
                        daysSpan = ruleEndDate.Subtract(startDay);

                    calcTotalOccurences = Helper.MonthRange(startDay, ruleEndDate) / frequency + 1;
                }

                var tempStartDay = new DateTime(startDay.Year, startDay.Month, 1);
            for (int i = 0; i < calcTotalOccurences; i++)
                totalOccurenceDates.Add(Helper.GetDayOcurrenceOfMonth(tempStartDay.AddMonths(i * frequency), dayOfTheWeek, countOfWeekDay));

            foreach (var item in totalOccurenceDates)
            {
                //gen transactions
                var trans = new Transaction(interval.Amount, interval.Category, interval.Comments, interval.Purpose, item, interval.TransactionType, user);
                this.Add(trans);
            }
        }

        private void ApplyRuleMonthlyDayNum(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, int ruleTotalOccurences, User user)
        {
            var totalOccurenceDates = new List<DateTime>();
            TimeSpan daysSpan = new TimeSpan();
            int calcTotalOccurences = 0;

            var dayNumStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyDayNumber.ToString());
            var dayNum = dayNumStr != null ? int.Parse(dayNumStr.Value) : 1;

            var frequency = int.Parse(interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyEveryMonth.ToString()).Value);

            var startDay = ruleStartDate;

            while (startDay.Day != dayNum)
                startDay = startDay.AddDays(1);

            //find the ending from the range
            if (ruleTotalOccurences > 0)
            {
                calcTotalOccurences = ruleTotalOccurences;
            }
            else
            {
                if (ruleEndDate > DateTime.Now)
                    daysSpan = DateTime.Now.Subtract(startDay);
                else
                    daysSpan = ruleEndDate.Subtract(startDay);

                calcTotalOccurences = Helper.MonthRange(startDay, ruleEndDate) / frequency + 1;
            }

            for (int i = 0; i < calcTotalOccurences; i++)
                totalOccurenceDates.Add(ruleStartDate.AddDays(i * frequency));

            foreach (var item in totalOccurenceDates)
            {
                //gen transactions
                var trans = new Transaction(interval.Amount, interval.Category, interval.Comments, interval.Purpose, item, interval.TransactionType, user);
                this.Add(trans);
            }

        }

        private void ApplyRuleWeeklyEveryWeek(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, int ruleTotalOccurences, User user)
        {
            var totalOccurenceDates = new List<DateTime>();
            TimeSpan daysSpan = new TimeSpan();
            int calcTotalOccurences = 0;

            var dayOfTheWeekStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.WeeklyDayName.ToString());
            var dayOfTheWeek = dayOfTheWeekStr != null ? int.Parse(dayOfTheWeekStr.Value) : 1;

            var frequency = int.Parse(interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.WeeklyEveryWeek.ToString()).Value);

            Dictionary<string, int> dayOfTheWeekNumber = new Dictionary<string, int>();
            dayOfTheWeekNumber.Add("Monday",1);
            dayOfTheWeekNumber.Add("Tuesday",2);
            dayOfTheWeekNumber.Add("Wednesday",3);
            dayOfTheWeekNumber.Add("Thursday",4);
            dayOfTheWeekNumber.Add("Friday",5);
            dayOfTheWeekNumber.Add("Saturday",6);
            dayOfTheWeekNumber.Add("Sunday",7);

            var startDay = ruleStartDate;

            while (dayOfTheWeekNumber[startDay.DayOfWeek.ToString()] != dayOfTheWeek)
                startDay = startDay.AddDays(1);

            //find the ending from the range
            if (ruleTotalOccurences > 0)
            {
                calcTotalOccurences = ruleTotalOccurences;
            }
            else
            {
                if (ruleEndDate > DateTime.Now)
                    daysSpan = DateTime.Now.Subtract(startDay);
                else
                    daysSpan = ruleEndDate.Subtract(startDay);

                calcTotalOccurences = daysSpan.Days / 7 / frequency + 1;
            }

            for (int i = 0; i < calcTotalOccurences; i++)
                totalOccurenceDates.Add(startDay.AddDays(i * frequency * 7));

            foreach (var item in totalOccurenceDates)
            {
                //gen transactions
                var trans = new Transaction(interval.Amount, interval.Category, interval.Comments, interval.Purpose, item, interval.TransactionType, user);
                this.Add(trans);
            }
        }

        private void ApplyRuleDailyEveryDays(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, int ruleTotalOccurences, User user)
        {
            var totalOccurenceDates = new List<DateTime>();
            TimeSpan daysSpan = new TimeSpan();
            int calcTotalOccurences = 0;

            var onlyWeekDaysStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x=>x.RulePart.FieldName == Const.RuleField.DailyOnlyWeekdays.ToString());
            var onlyWeekDays = onlyWeekDaysStr != null ? bool.Parse(onlyWeekDaysStr.Value) : false;


            //find the ending from the range
            if (ruleTotalOccurences > 0)
            {
                calcTotalOccurences = ruleTotalOccurences;
            }
            else
            {
                if (ruleEndDate > DateTime.Now)
                    daysSpan = DateTime.Now.Subtract(ruleStartDate);
                else
                    daysSpan = ruleEndDate.Subtract(ruleStartDate);

                var frequency = int.Parse(interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.DailyEveryDay.ToString()).Value);
                var calc = daysSpan.Days / frequency;
                calcTotalOccurences = calc;
                
            }

            for (int i = 0; i < calcTotalOccurences; i++)
                totalOccurenceDates.Add(ruleStartDate.AddDays(i));

            foreach (var item in totalOccurenceDates)
            {
                if (onlyWeekDays && (item.DayOfWeek == DayOfWeek.Saturday || item.DayOfWeek == DayOfWeek.Sunday))
                    continue;

                //gen transactions
                var trans = new Transaction(interval.Amount, interval.Category, interval.Comments, interval.Purpose, item, interval.TransactionType, user);
                this.Add(trans);
            }
        }

        protected override void InsertItem(int index, Transaction item)
        {

            bool added = false;

            //logic for new unueqe id 
            if (item.TransactionId <= 0 && this.Contains(item))
            {
                var minIndex = (from i in this
                                orderby i.TransactionId ascending
                                select i).ToList();

                item.TransactionId = minIndex[0].TransactionId - 1;
            }

            for (int idx = 0; idx < Count; idx++)
            {
                //immediate sorting !
                if (item.TransactionDate > Items[idx].TransactionDate)
                {
                    base.InsertItem(idx, item);
                    added = true;
                    break;
                }
            }

            if (!added)
                base.InsertItem(index, item);
        }

        [Obsolete]
        public void SortByCreatedDate()
        {
            for (int z = 0; z < Items.Count; z++)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    for (int k = i; k < Items.Count; k++)
                    {
                        Transaction n = Items[i];
                        Transaction o = Items[k];

                        if (Items[i].CreatedDate < Items[k].CreatedDate)
                        {
                            base.MoveItem(k, i);
                            break;
                        }
                    }
                }
            }
        }

        public TransactionList GetChanges()
        {
            TransactionList result = new TransactionList();
            
            var query = this.Where(t => t.HasChanges).ToList();
            foreach (var item in query)
                result.Add(item);

            return result;
        }

        public bool HasItemsWithChanges()
        {
            bool result = false;

            result = this.FirstOrDefault(x => x.HasChanges) != null;

            return result;
        }

        public void AcceptChanges()
        {
            foreach (var item in Items)
                item.HasChanges = false;
        }

        public TransactionList FilterOnDateRange(DateTime fromDate, DateTime toDate)
        {
            TransactionList result = new TransactionList();

            var query = from i in this
                        where i.CreatedDate >= fromDate && i.CreatedDate <= toDate
                        select i;

            foreach (var item in query)
                result.Add(item);

            return result;
        }
        
        public void OptimizeOnTopLevel(Transaction.ImageRemovalStatus removeImages)
        {
            foreach (var item in this)
                item.OptimizeOnTopLevel(removeImages);   
        }

        public void PrepareForServiceSerialization()
        {
            //one way to handle circular referenceis to explicitly set the child to null
            this.OptimizeOnTopLevel(Transaction.ImageRemovalStatus.All);
            var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).ToList();
            foreach (var item in deletedIDs)
                this.RemoveAt(item.index);

            this.AcceptChanges();
        }

        public static void GenerateIntervalTransactions(TypeIntervalList typeIntervalList, DateTime overwriteStartDate)
        { 
        }
    }

    //[DataContract]
    public class Transaction : BaseItem
    {
        #region Enumarators
        public enum ImageRemovalStatus
        {
            None,
            All,
            Unchanged,
            Changed
        }
        #endregion

        #region Private Members
        double amount;
        string nameOfPlace;
        double tipAmount;
        string comments;
        Category category;
        TypeTransactionReason typeTransactionReason;
        TypeTransaction typeTransaction;
        DateTime transactionDate;
        TransactionImageList transactionImages;
        #endregion

        public class PlaceComparer : IEqualityComparer<Transaction>
        {
            #region IEqualityComparer Members
            public bool Equals(Transaction x, Transaction y)
            {
                if (x.NameOfPlace == y.NameOfPlace)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(Transaction obj)
            {
                return base.GetHashCode();
            }
            #endregion
        }

        public class IDComparer : IEqualityComparer<Transaction>
        {
            #region IEqualityComparer Members
            public bool Equals(Transaction x, Transaction y)
            {
                if (x.TransactionId == y.TransactionId)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(Transaction obj)
            {
                return base.GetHashCode();
            }
            #endregion
        }

        #region Public Properties
        //[DataMember]
        public int TransactionId { get; set; }

        //[DataMember]
        public double Amount { get { return amount; } set { amount = value; OnPropertyChanged("Amount"); OnPropertyChanged("HasChanges"); OnPropertyChanged("TotalAmount"); } }
        
        //[DataMember]
        public double TipAmount { get { return tipAmount; } set { tipAmount = value; OnPropertyChanged("TipAmount"); OnPropertyChanged("HasChanges"); OnPropertyChanged("TotalAmount"); } }
        
        //[DataMember]
        public double TotalAmount { get { return Amount + TipAmount; } }
        
        //[DataMember]
        public string NameOfPlace { get { return nameOfPlace; } set { nameOfPlace = value; OnPropertyChanged("NameOfPlace"); OnPropertyChanged("HasChanges"); } }

        //[DataMember]
        public string Comments { get { return comments; } set { comments = value; OnPropertyChanged("Comments"); OnPropertyChanged("HasChanges"); } }

        //[IgnoreDataMember]
        //[DataMember]
        public Category Category { get { return category; } set { category = value; OnPropertyChanged("Category"); OnPropertyChanged("HasChanges"); } }

        //[DataMember]
        public TypeTransactionReason TransactionReasonType { get { return typeTransactionReason; } set { typeTransactionReason = value; OnPropertyChanged("TransactionReasonType"); OnPropertyChanged("HasChanges"); } }

        //[DataMember]
        public TypeTransaction TransactionType { get { return typeTransaction; } set { typeTransaction = value; OnPropertyChanged("TransactionType"); OnPropertyChanged("HasChanges"); } }

        //[DataMember]
        public DateTime TransactionDate { get { return transactionDate; } set { transactionDate = value; OnPropertyChanged("TransactionDate"); OnPropertyChanged("HasChanges"); } }

        //[DataMember]
        //[IgnoreDataMember]
        public TransactionImageList TransactionImages { get { return transactionImages; } set { transactionImages = value; OnPropertyChanged("TransactionImages"); OnPropertyChanged("HasChanges"); } }

        #endregion

        #region Contructors
        //parameterless ctor in order to be used in generic as T
        public Transaction()
            : this(null)
        {}
        public Transaction(User user):this(null,null,null, user)
        { }

        //Simple contructor with rules applied
        public Transaction(CategoryList categoryList, TypeTransactionList typeTransactionList, TypeTransactionReasonList typeTransactionReasonList, User user)
            : base(user)
        {
            TransactionId = 0;
            Amount = 0;
            
            Comments = "";
            NameOfPlace = "";
            TipAmount = 0;
            TransactionDate = DateTime.Now;

            if (categoryList == null)
            {
                Category = new Category(user);
            }
            else
            {
                
                var categoryTemp = categoryList.FirstOrDefault(
                    c =>
                    {
                        bool found = false;
                        if (c.FromDate.Hour <= c.ToDate.Hour)
                            found = c.FromDate.Hour <= DateTime.Now.Hour && c.ToDate.Hour >= DateTime.Now.Hour;
                        else
                        {
                            //if it is a cross day there are 2 cases
                            found = (c.FromDate.Hour <= DateTime.Now.Hour && c.ToDate.Hour <= DateTime.Now.Hour) ||
                                (c.FromDate.Hour >= DateTime.Now.Hour && c.ToDate.Hour >= DateTime.Now.Hour);
                        }
                        return found ? found : c.Name == "Other";
                    });

                Category = categoryTemp.Clone();
            }

            if (typeTransactionList == null)
                TransactionType = new TypeTransaction(user);
            else
            {
                var typeTransactionTemp = typeTransactionList.Single(t => t.Name == "Expense");
                TransactionType = typeTransactionTemp.Clone();
            }

            if (typeTransactionReasonList == null)
                TransactionReasonType = new TypeTransactionReason(user);
            else
            {
                var typeTransReasonTemp = typeTransactionReasonList.Single(t => t.Name == "Other");
                TransactionReasonType = typeTransReasonTemp.Clone();
            }

            TransactionImages = new TransactionImageList();
        }

        public Transaction(double amount, Category category, string comments, string nameOfPlace, DateTime transactionDate, TypeTransaction transactionType, User user)
            : base(user)
        {
            Amount = amount;
            Category = category;
            Comments = comments;
            HasChanges = false;
            NameOfPlace = nameOfPlace;
            TransactionDate = transactionDate;
            TransactionType = transactionType;
        }
        #endregion

        #region Public Methods
        public void OptimizeOnTopLevel(ImageRemovalStatus removeImages)
        {
            this.Category.TypeTransactionReasons = null;
            this.TransactionReasonType.Categories = null;

            if (this.TransactionImages != null)
            {
                switch (removeImages)
                {
                    case Transaction.ImageRemovalStatus.All:
                        this.TransactionImages = null;
                        break;
                    case Transaction.ImageRemovalStatus.Changed:
                        var transImagesNoChange = this.TransactionImages.Where(x => x.HasChanges == false).ToList();
                        this.TransactionImages = new TransactionImageList();

                        foreach (var img in transImagesNoChange)
                            this.TransactionImages.Add(img);

                        break;
                    case Transaction.ImageRemovalStatus.Unchanged:
                        var transImagesChange = this.TransactionImages.Where(x => x.HasChanges == true).ToList();
                        this.TransactionImages = new TransactionImageList();

                        foreach (var img in transImagesChange)
                            this.TransactionImages.Add(img);

                        break;
                    case Transaction.ImageRemovalStatus.None:
                        break;
                }

                if (this.TransactionImages != null)
                {
                    foreach (var transImage in this.TransactionImages)
                        transImage.Transaction = null;
                }
            }
        }

        public override bool Equals(Object obj)
        {
            Transaction transaction = obj as Transaction;
            if (transaction == null)
                return false;
            else
                return TransactionId.Equals(transaction.TransactionId);
        }

        public override int GetHashCode()
        {
            return this.TransactionId.GetHashCode();
        }
        #endregion
    }
}
