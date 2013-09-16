using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ServiceModel;
using System.Collections;

namespace BMA.BusinessLogic
{
    public class TransactionList : BaseList<Transaction>, IDataList
    {
        public TransactionList()
        {
            //CollectionChanged += TransactionList_CollectionChanged;
        }

        public TransactionList(TypeIntervalList typeIntervalList, TypeIntervalConfiguration typeIntervalConfiguration, User user)
        {
            try
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

                    var lastGeneratedDate = intervalStartDate;
                    if (typeIntervalConfiguration != null)
                        lastGeneratedDate = typeIntervalConfiguration.GeneratedDate;

                    var recRule = (Const.Rule)Enum.Parse(typeof(Const.Rule), interval.RecurrenceRuleValue.RecurrenceRule.Name);

                    //## setup recurrence transactions
                    if (intervalStartDate <= DateTime.Now)
                    {
                        switch (recRule)
                        {
                            case Const.Rule.RuleDailyEveryDays:
                                ApplyRuleDailyEveryDays(interval, intervalStartDate, intervalEndDate, lastGeneratedDate, intervalTotalOcc, user);
                                break;
                            case Const.Rule.RuleWeeklyEveryWeek:
                                ApplyRuleWeeklyEveryWeek(interval, intervalStartDate, intervalEndDate, lastGeneratedDate, intervalTotalOcc, user);
                                break;
                            case Const.Rule.RuleMonthlyDayNum:
                                ApplyRuleMonthlyDayNum(interval, intervalStartDate, intervalEndDate, lastGeneratedDate, intervalTotalOcc, user);
                                break;
                            case Const.Rule.RuleMonthlyPrecise:
                                ApplyRuleMonthlyPrecise(interval, intervalStartDate, intervalEndDate, lastGeneratedDate, intervalTotalOcc, user);
                                break;
                            case Const.Rule.RuleYearlyOnMonth:
                                ApplyRuleYearlyOnMonth(interval, intervalStartDate, intervalEndDate, lastGeneratedDate, intervalTotalOcc, user);
                                break;
                            case Const.Rule.RuleYearlyOnTheWeekDay:
                                ApplyRuleYearlyOnTheWeekDay(interval, intervalStartDate, intervalEndDate, lastGeneratedDate, intervalTotalOcc, user);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Rules
        private void ApplyRuleYearlyOnTheWeekDay(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, DateTime lastGenerateDate, int ruleTotalOccurences, User user)
        {
            var totalOccurenceDates = new List<DateTime>();
            int calcTotalOccurences = 0;
            int calcTotalOccurencesSinceBeginning = 0;

            var dayPosOfMonthStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyPositions.ToString());
            var dayPosOfMonth = dayPosOfMonthStr != null ? int.Parse(dayPosOfMonthStr.Value) : 1;

            var dayOfTheWeekStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyDayName.ToString());
            var dayOfTheWeek = dayOfTheWeekStr != null ? int.Parse(dayOfTheWeekStr.Value) : 1;

            var monthOfYearStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyMonthNameSec.ToString());
            var monthOfYear = monthOfYearStr != null ? int.Parse(monthOfYearStr.Value) : 1;

            var frequency = int.Parse(interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyEveryYear.ToString()).Value);

            var startDay = Helper.GetDayOcurrenceOfMonth(ruleStartDate, dayOfTheWeek, dayPosOfMonth, monthOfYear);

            if (ruleEndDate > DateTime.Now)
                ruleEndDate = DateTime.Now;

            ////### TESING ADJ ###///
            //ruleEndDate = ruleEndDate.AddYears(13);
            //lastGenerateDate = startDay;
            //////////////////////

            calcTotalOccurencesSinceBeginning = Helper.YearRange(startDay, ruleEndDate) / frequency;
            calcTotalOccurences = Helper.YearRange(lastGenerateDate, ruleEndDate) / frequency;

            calcTotalOccurences = Helper.CalculateTotalOcurrences(calcTotalOccurencesSinceBeginning, calcTotalOccurences, ruleTotalOccurences);

            for (int i = 0; i < calcTotalOccurences; i++)
                totalOccurenceDates.Add(Helper.GetDayOcurrenceOfMonth(startDay.AddYears(i * frequency), dayOfTheWeek, dayPosOfMonth, monthOfYear));

            GenerateIntervalTransactions(interval, totalOccurenceDates, user);

        }

        private void ApplyRuleYearlyOnMonth(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, DateTime lastGenerateDate, int ruleTotalOccurences, User user)
        {
            var totalOccurenceDates = new List<DateTime>();
            int calcTotalOccurences = 0;
            int calcTotalOccurencesSinceBeginning = 0;

            var dayPosOfYearStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyOnDayPos.ToString());
            var dayPosOfYear = dayPosOfYearStr != null ? int.Parse(dayPosOfYearStr.Value) : 1;

            var monthOfYearStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyMonthName.ToString());
            var monthOfYear = monthOfYearStr != null ? int.Parse(monthOfYearStr.Value) : 1;

            var frequency = int.Parse(interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.YearlyEveryYear.ToString()).Value);

            var startDay = Helper.AdjustYearStatDay(ruleStartDate, monthOfYear, dayPosOfYear);

            if (ruleEndDate > DateTime.Now)
                ruleEndDate = DateTime.Now;

            ////### TESING ADJ ###///
            //ruleEndDate = ruleEndDate.AddYears(13);
            //lastGenerateDate = startDay;
            //////////////////////

            calcTotalOccurencesSinceBeginning = Helper.YearRange(startDay, ruleEndDate) / frequency;
            calcTotalOccurences = Helper.YearRange(lastGenerateDate, ruleEndDate) / frequency;

            calcTotalOccurences = Helper.CalculateTotalOcurrences(calcTotalOccurencesSinceBeginning, calcTotalOccurences, ruleTotalOccurences);

            for (int i = 0; i < calcTotalOccurences; i++)
                totalOccurenceDates.Add(Helper.AdjustYearStatDay(startDay.AddYears(i * frequency), monthOfYear, dayPosOfYear));

            GenerateIntervalTransactions(interval, totalOccurenceDates, user);
        }

        private void ApplyRuleMonthlyPrecise(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, DateTime lastGenerateDate, int ruleTotalOccurences, User user)
        {
            var totalOccurenceDates = new List<DateTime>();
            int calcTotalOccurences = 0;
            int calcTotalOccurencesSinceBeginning = 0;

            var countOfWeekDayStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyCountOfWeekDay.ToString());
            var countOfWeekDay = countOfWeekDayStr != null ? int.Parse(countOfWeekDayStr.Value) : 1;

            var dayOfTheWeekStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyDayName.ToString());
            var dayOfTheWeek = dayOfTheWeekStr != null ? int.Parse(dayOfTheWeekStr.Value) : 1;

            var frequency = int.Parse(interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyCountOfMonth.ToString()).Value);

            var startDay = Helper.GetDayOcurrenceOfMonth(ruleStartDate, dayOfTheWeek, countOfWeekDay);

            if (ruleEndDate > DateTime.Now)
                ruleEndDate = DateTime.Now;

            ////### TESING ADJ ###///
            //ruleEndDate = ruleEndDate.AddYears(3);
            //lastGenerateDate = startDay;
            //////////////////////

            calcTotalOccurencesSinceBeginning = Helper.MonthRange(startDay, ruleEndDate) / frequency;
            calcTotalOccurences = Helper.MonthRange(lastGenerateDate, ruleEndDate) / frequency;

            calcTotalOccurences = Helper.CalculateTotalOcurrences(calcTotalOccurencesSinceBeginning, calcTotalOccurences, ruleTotalOccurences);

            for (int i = 0; i < calcTotalOccurences; i++)
                totalOccurenceDates.Add(Helper.GetDayOcurrenceOfMonth(startDay.AddMonths(i * frequency), dayOfTheWeek, countOfWeekDay));

            GenerateIntervalTransactions(interval, totalOccurenceDates, user);
        }

        private void ApplyRuleMonthlyDayNum(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, DateTime lastGenerateDate, int ruleTotalOccurences, User user)
        {
            var totalOccurenceDates = new List<DateTime>();
            int calcTotalOccurencesSinceBeginning = 0;
            int calcTotalOccurences = 0;

            var dayNumStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyDayNumber.ToString());
            var dayNum = dayNumStr != null ? int.Parse(dayNumStr.Value) : 1;

            var frequency = int.Parse(interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.MonthlyEveryMonth.ToString()).Value);

            var startDay = Helper.AdjustDayOcurrenceMonthly(ruleStartDate, dayNum);

            if (ruleEndDate > DateTime.Now)
                ruleEndDate = DateTime.Now;

            //### TESING ADJ ###///
            //ruleEndDate = ruleEndDate.AddYears(3);
            //lastGenerateDate = startDay;
            ////////////////////

            calcTotalOccurencesSinceBeginning = Helper.MonthRange(startDay, ruleEndDate) / frequency;
            calcTotalOccurences = Helper.MonthRange(lastGenerateDate, ruleEndDate) / frequency;

            calcTotalOccurences = Helper.CalculateTotalOcurrences(calcTotalOccurencesSinceBeginning, calcTotalOccurences, ruleTotalOccurences);

            for (int i = 0; i < calcTotalOccurences; i++)
                totalOccurenceDates.Add(Helper.AdjustDayOcurrenceMonthly(startDay.AddMonths(i * frequency),dayNum));

            GenerateIntervalTransactions(interval, totalOccurenceDates, user);
        }

        private void ApplyRuleWeeklyEveryWeek(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, DateTime lastGenerateDate, int ruleTotalOccurences, User user)
        {
            var totalOccurenceDates = new List<DateTime>();
            int calcPastOccurences = 0;
            int calcTotalOccurencesSinceBeginning = 0;
            int newTotalOccurences = 0;
            int calcTotalOccurences = 0;

            var dayOfTheWeekStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.WeeklyDayName.ToString());
            var dayOfTheWeek = dayOfTheWeekStr != null ? int.Parse(dayOfTheWeekStr.Value) : 1;

            var frequency = int.Parse(interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.WeeklyEveryWeek.ToString()).Value);

            var startDay = Helper.AdjustDayOcurrenceWeekly(ruleStartDate, dayOfTheWeek);

            if (ruleEndDate > DateTime.Now)
                ruleEndDate = DateTime.Now;

            ////### TESING ADJ ###///
            //ruleEndDate = ruleEndDate.AddMonths(4);
            //lastGenerateDate = startDay;
            //////////////////////

            calcTotalOccurencesSinceBeginning = Helper.WeekRange(startDay, ruleEndDate) / frequency;
            calcTotalOccurences = Helper.WeekRange(lastGenerateDate, ruleEndDate) / frequency;

            calcPastOccurences = calcTotalOccurencesSinceBeginning - calcTotalOccurences;
            newTotalOccurences = ruleTotalOccurences - calcPastOccurences;

            if (ruleTotalOccurences > 0 && newTotalOccurences < calcTotalOccurences)
                calcTotalOccurences = newTotalOccurences;

            //No need to call the adjustment method, since it will always be the same day of the week
            for (int i = 0; i < calcTotalOccurences; i++)
                totalOccurenceDates.Add(startDay.AddDays(i * frequency * 7));

            GenerateIntervalTransactions(interval, totalOccurenceDates, user);
        }

        private void ApplyRuleDailyEveryDays(TypeInterval interval, DateTime ruleStartDate, DateTime ruleEndDate, DateTime lastGenerateDate, int ruleTotalOccurences, User user)
        {
            var totalOccurenceDates = new List<DateTime>();
            int calcTotalOccurences = 0;
            int calcPastOccurences = 0;
            int calcTotalOccurencesSinceBeginning = 0;
            int newTotalOccurences = 0;

            var onlyWeekDaysStr = interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.DailyOnlyWeekdays.ToString());
            var onlyWeekDays = false;
            bool.TryParse(onlyWeekDaysStr.Value, out onlyWeekDays);

            var frequency = int.Parse(interval.RecurrenceRuleValue.RulePartValueList.FirstOrDefault(x => x.RulePart.FieldName == Const.RuleField.DailyEveryDay.ToString()).Value);

            var startDay = Helper.AdjustDayOcurrenceDaily(ruleStartDate, onlyWeekDays);

            if (ruleEndDate > DateTime.Now)
                ruleEndDate = DateTime.Now;

            ////### TESING ADJ ###///
            //ruleEndDate = ruleEndDate.AddDays(28);
            //lastGenerateDate = startDay;
            //////////////////////

            calcTotalOccurencesSinceBeginning = Helper.DayRange(startDay, ruleEndDate) / frequency;
            calcTotalOccurences = Helper.DayRange(lastGenerateDate, ruleEndDate) / frequency;

            calcPastOccurences = calcTotalOccurencesSinceBeginning - calcTotalOccurences;
            newTotalOccurences = ruleTotalOccurences - calcPastOccurences;

            if (ruleTotalOccurences > 0 && newTotalOccurences < calcTotalOccurences)
                calcTotalOccurences = newTotalOccurences;

            for (int i = 0; i < calcTotalOccurences; i++)
                totalOccurenceDates.Add(Helper.AdjustDayOcurrenceDaily(startDay.AddDays(i * frequency), onlyWeekDays));

            GenerateIntervalTransactions(interval, totalOccurenceDates, user);
        }
        #endregion

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

        public TransactionList FilterOnDateRange(DateTime fromDate, DateTime toDate)
        {
            TransactionList result = new TransactionList();

            var query = from i in this
                        where i.TransactionDate >= fromDate && i.TransactionDate <= toDate
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
            RemoveDeleted();

            this.AcceptChanges();
        }

        public void GenerateIntervalTransactions(TypeInterval typeInterval, List<DateTime> occurenceDates, User user)
        {
            foreach (var item in occurenceDates)
            {
                var trans = new Transaction(typeInterval.Amount, typeInterval.Category, typeInterval.TransactionReasonType, typeInterval.Comments, typeInterval.Purpose, item, typeInterval.TransactionType, user);
                this.Add(trans);
            }
        }

        //public void AcceptChanges()
        //{
        //    foreach (var item in Items)
        //        item.HasChanges = false;
        //}

        //public void RemoveDeleted()
        //{
        //    var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).OrderByDescending(x => x.index).ToList();

        //    foreach (var item in deletedIDs)
        //        this.RemoveAt(item.index);
        //}

        public IEnumerable SplitComments()
        {
            //var query = this.SelectMany(x => x.NameOfPlace.Split(' ')).ToList();
            var query = this.GroupBy(x=>x.NameOfPlace).Select(x => x.Key).ToList();
            return query;
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

        public Transaction(double amount, Category category, TypeTransactionReason transactionReasonType, string comments, string nameOfPlace, DateTime transactionDate, TypeTransaction transactionType, User user)
            : base(user)
        {
            Amount = amount;
            Category = category;
            TransactionReasonType = transactionReasonType;
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
            if (this.Category!=null)
                this.Category.TypeTransactionReasons = null;

            if (this.TransactionReasonType != null)
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
