using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

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
                if (item.CreatedDate > Items[idx].CreatedDate)
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

    [DataContract]
    public class Transaction : BaseItem, INotifyPropertyChanged
    {
        double amount;
        string nameOfPlace;
        double tipAmount;
        string comments;
        Category category;
        TypeTransactionReason typeTransactionReason;
        TypeTransaction typeTransaction;
        DateTime transactionDate;

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

        [DataMember]
        [Required]
        public int TransactionId { get; set; }

        [DataMember]
        [Required]
        public double Amount { get { return amount; } set { amount = value; NotifyPropertyChanged("Amount"); } }

        [DataMember]
        public string NameOfPlace { get { return nameOfPlace; } set { nameOfPlace = value; NotifyPropertyChanged("NameOfPlace"); } }

        [DataMember]
        public double TipAmount { get { return tipAmount; } set { tipAmount = value; NotifyPropertyChanged("TipAmount"); } }

        public double TotalAmount { get { return Amount + TipAmount; } }

        [DataMember]
        public string Comments { get { return comments; } set { comments = value; NotifyPropertyChanged("Comments"); } }

        [DataMember]
        //[IgnoreDataMember]
        public Category Category { get { return category; } set { category = value; NotifyPropertyChanged("Category"); } }

        [DataMember]
        public TypeTransactionReason TransactionReasonType { get { return typeTransactionReason; } set { typeTransactionReason = value; NotifyPropertyChanged("TransactionReasonType"); } }

        [DataMember]
        public TypeTransaction TransactionType { get { return typeTransaction; } set { typeTransaction = value; NotifyPropertyChanged("TransactionType"); } }

        [DataMember]
        public DateTime TransactionDate { get{return transactionDate;} set{transactionDate = value;NotifyPropertyChanged("TransactionDate"); } }

        #region Contructors
        public Transaction()
        {
            TransactionId = -1;
            Amount = 0;
            Category = new Category();
            Comments = "";
            CreatedDate = DateTime.Now;
            CreatedUser = new User();
            ModifiedDate = DateTime.Now;
            ModifiedUser = new User();
            NameOfPlace = "";
            TipAmount = 0;
            TransactionReasonType = new TypeTransactionReason();
            TransactionType = new TypeTransaction();
            TransactionDate = DateTime.Now;
        }

        public Transaction(User user):this()
        {
            CreatedUser = user;
            ModifiedUser = user;
        }

        //Simple contructor with rules applied
        public Transaction(List<Category> categoryList, List<TypeTransaction> typeTransactionList, List<TypeTransactionReason> typeTransactionReasonList, User user)
            : this(user)
        {
            //Category c = new Category();
            //bool isCrossDay;
            
            Category = categoryList.Where(
                c => {
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
            TransactionType = typeTransactionList.Single(t => t.Name == "Expense");
            TransactionReasonType = typeTransactionReasonList.Single(t => t.Name == "Other");
        }
        #endregion


    }
}
