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
