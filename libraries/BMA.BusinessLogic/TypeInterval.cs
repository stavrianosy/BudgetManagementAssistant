﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace BMA.BusinessLogic
{
    public class TypeIntervalList : BaseList<TypeInterval>, IDataList
    {
        public const int DEVICE_MAX_COUNT = 30;
        
        public void PrepareForServiceSerialization()
        {
            RemoveDeleted();

            this.AcceptChanges();
        }

        //public void AcceptChanges()
        //{
        //    foreach (var item in Items)
        //        item.HasChanges = false;
        //}

        //public bool HasItemsWithChanges()
        //{
        //    bool result = false;

        //    result = this.FirstOrDefault(x => x.HasChanges) != null;

        //    return result;
        //}

        //public void RemoveDeleted()
        //{
        //    var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).OrderByDescending(x => x.index).ToList();

        //    foreach (var item in deletedIDs)
        //        this.RemoveAt(item.index);
        //}
    }


    public class TypeInterval : BaseItem
    {
        #region Private Members
        string name;
        string purpose;
        TypeTransaction transactionType;
        Category category;
        TypeTransactionReason transactionReasonType;
        Double amount;
        string comments;
        RecurrenceRulePart recurrenceRuleValue;
        RecurrenceRulePart recurrenceRangeRuleValue;
        #endregion

        #region Public Properties
        public int TypeIntervalId { get; set; }

        public TypeTransaction TransactionType { get { return transactionType; } set { transactionType = value; OnPropertyChanged("TransactionType"); } }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        public string Purpose { get { return purpose; } set { purpose = value; OnPropertyChanged("Purpose"); } }

        public Category Category { get { return category; } set { category = value; OnPropertyChanged("Category"); } }

        public TypeTransactionReason TransactionReasonType { get { return transactionReasonType; } set { transactionReasonType = value; OnPropertyChanged("TransactionReasonType"); } }

        public double Amount { get { return amount; } set { amount = value; OnPropertyChanged("Amount"); } }

        public string Comments { get { return comments; } set { comments = value; OnPropertyChanged("Comments"); } }

        public RecurrenceRulePart RecurrenceRuleValue { get { return recurrenceRuleValue; } set { recurrenceRuleValue = value; OnPropertyChanged("RecurrenceRuleValue"); } }

        public RecurrenceRulePart RecurrenceRangeRuleValue { get { return recurrenceRangeRuleValue; } set { recurrenceRangeRuleValue = value; OnPropertyChanged("RecurrenceRangeRuleValue"); } }
        #endregion

        #region Constructors
        //parameterless ctor in order to be used in generic as T
        public TypeInterval()
            : this(null,null,null, null)
        {}
        public TypeInterval(User user)
            : this(null, null, null, user)
        { }
        public TypeInterval(CategoryList categoryList, TypeTransactionReasonList transactionReasonTypeList, TypeTransactionList typeTransactionList, User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
            if (categoryList == null)
                Category = new Category(user);
            else
                Category = categoryList.Single(t => t.Name == "Other");

            if (transactionReasonTypeList == null)
                transactionReasonType = new TypeTransactionReason(user);
            else
                transactionReasonType = transactionReasonTypeList.Single(t => t.Name == "Other");

            if (typeTransactionList == null)
                TransactionType = new TypeTransaction(user);
            else
                TransactionType = typeTransactionList.Single(t => t.Name == "Expense");

            RecurrenceRuleValue = new RecurrenceRulePart();
            
            RecurrenceRangeRuleValue = new RecurrenceRulePart();
        }
        #endregion

        #region Public Events
        public StringBuilder SelfValidation()
        {
            List<string> result = new List<string>();
            var errorMessage = new StringBuilder();

            if (this.Name == null || this.Name.Trim().Length == 0)
                errorMessage.Append(string.Format("Interval {0} doesn't have a Name\n", this.Name));

            if(this.Amount <= 0)
                errorMessage.Append(string.Format("Interval {0} doesn't have an Amount\n", this.Name));

            if (this.RecurrenceRuleValue == null || this.RecurrenceRuleValue.RecurrenceRule == null || this.RecurrenceRuleValue.RulePartValueList == null)
            {
                errorMessage.Append(string.Format("Interval {0} doesn't have a Recurrence Rule\n", this.Name));
            }

            if (this.RecurrenceRuleValue.RulePartValueList != null)
            {
                foreach(var item in this.RecurrenceRuleValue.RulePartValueList)
                {
                    if (item.Value == null || item.Value == "")
                    {
                        errorMessage.Append(string.Format("Rule {0} of Interval {1} cannot be empty\n", item.RulePart.FieldName, this.Name));
                    }
                    if (item.Value == "0")
                    {
                        errorMessage.Append(string.Format("Rule {0} of Interval {1} cannot be 0\n", item.RulePart.FieldName, this.Name));
                    }
                }
            }

            if (this.RecurrenceRangeRuleValue == null || this.RecurrenceRangeRuleValue.RecurrenceRule == null || this.RecurrenceRangeRuleValue.RulePartValueList == null)
            {
                errorMessage.Append(string.Format("Interval {0} doesn't have a Recurrence Range Rule\n", this.Name));
            }

            if (this.RecurrenceRangeRuleValue.RulePartValueList != null)
            {
                foreach(var item in this.RecurrenceRangeRuleValue.RulePartValueList)
                {
                    if (item.Value == null || item.Value == "")
                    {
                        errorMessage.Append(string.Format("Rule {0} of Interval {1} cannot be empty\n", item.RulePart.FieldName, this.Name));
                    }
                    if (item.Value == "0")
                    {
                        errorMessage.Append(string.Format("Rule {0} of Interval {1} cannot be 0\n", item.RulePart.FieldName, this.Name));
                    }
                }
            }

            return errorMessage;
        }
        #endregion

        #region Private Properties
        #endregion
    }
}
