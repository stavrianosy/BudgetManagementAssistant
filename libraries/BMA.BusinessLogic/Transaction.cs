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
    public class TransactionList : ObservableCollection<Transaction>
    {
        public TransactionList()
        {
            //CollectionChanged += TransactionList_CollectionChanged;
        }

        protected override void InsertItem(int index, Transaction item)
        {

            bool added = false;

            for (int idx = 0; idx < Count; idx++)
            {
                if (item.TransactionDate > Items[idx].TransactionDate)
                {
                    base.InsertItem(idx, item);
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                base.InsertItem(index, item);
            }
        }

        //private void TransactionList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    SortByCreatedDate();
        //}

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
                            //Items[i] = o;
                            //Items[k] = n;
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
        //create a method that accepts a budget for input and will give you all transactions for that budget
        //apply the same 
        //where i.CreatedDate >= context.Budget.Where(b => b.BudgetId == budgetId).Select(b => b.FromDate).FirstOrDefault()
        //&& i.CreatedDate <= context.Budget.Where(b => b.BudgetId == budgetId).Select(b => b.ToDate).FirstOrDefault()
    }

    //[DataContract]
    public class Transaction : BaseItem
    {
        #region Private Members
        double amount;
        string nameOfPlace;
        double tipAmount;
        string comments;
        Category category;
        TypeTransactionReason typeTransactionReason;
        TypeTransaction typeTransaction;
        DateTime transactionDate;
        List<TransactionImage> transactionImages;
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
        public List<TransactionImage> TransactionImages { get { return transactionImages; } set { transactionImages = value; OnPropertyChanged("TransactionImages"); OnPropertyChanged("HasChanges"); } }

        #endregion

        #region Contructors
        //parameterless ctor in order to be used in generic as T
        public Transaction()
            : this(null)
        {}
        public Transaction(User user):this(null,null,null, user)
        { }

        //Simple contructor with rules applied
        public Transaction(List<Category> categoryList, List<TypeTransaction> typeTransactionList, List<TypeTransactionReason> typeTransactionReasonList, User user)
            : base(user)
        {
            TransactionId = -1;
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
                Category = categoryList.Where(
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
                    }
                ).FirstOrDefault();
            }

            if (typeTransactionList == null)
                TransactionType = new TypeTransaction(user);
            else
                TransactionType = typeTransactionList.Single(t => t.Name == "Expense");

            if (typeTransactionReasonList == null)
                TransactionReasonType = new TypeTransactionReason(user);
            else
                TransactionReasonType = typeTransactionReasonList.Single(t => t.Name == "Other");

            TransactionImages = new List<TransactionImage>();
        }
        #endregion

        #region Public Methods
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
