using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class CategoryList : BaseList<Category>, IDataList
    {
        public const int DEVICE_MAX_COUNT = 30;
        protected override void InsertItem(int index, Category item)
        {
            bool added = false;

            //logic for new unueqe id 
            if (item.CategoryId <= 0 && this.Contains(item))
            {
                var minIndex = (from i in this
                                orderby i.CategoryId ascending
                                select i).ToList();

                item.CategoryId = minIndex[0].CategoryId - 1;
            }

            for (int idx = 0; idx < Count; idx++)
            {
                //immediate sorting !
                if (item.Name != null && item.Name.CompareTo(Items[idx].Name) <= 0)
                {
                    base.InsertItem(idx, item);
                    added = true;
                    break;
                }
            }

            if (!added)
                base.InsertItem(index, item);
        }

        public void PrepareForServiceSerialization()
        {
            this.OptimizeOnTopLevel();

            var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).OrderByDescending(x => x.index).ToList();
            
            foreach (var item in deletedIDs)
                this.RemoveAt(item.index);

            this.AcceptChanges();
        }

        public void OptimizeOnTopLevel()
        {
            foreach (var item in this)
                item.OptimizeOnTopLevel();
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
    }

    public class Category : BaseItem
    {
        #region Public Methods
        public override bool Equals(Object obj)
        {
            Category category = obj as Category;
            if (category == null)
                return false;
            else
                return CategoryId.Equals(category.CategoryId);
        }

        public override int GetHashCode()
        {
            return this.CategoryId.GetHashCode();
        }

        #endregion

        #region Private Members
        string name;
        DateTime fromDate;
        DateTime toDate;
        #endregion

        #region Public Properties
        public int CategoryId { get; set; }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        
        /// <summary>
        /// Although this property should be TimeSpan, it is set as DateTime due to WCF serialization issues.
        /// </summary>
        public DateTime FromDate { get { return fromDate; } set { fromDate = value; OnPropertyChanged("FromDate"); } }

        /// <summary>
        /// Although this property should be TimeSpan, it is set as DateTime due to WCF serialization issues.
        /// </summary>
        public DateTime ToDate { get { return toDate; } set { toDate = value; OnPropertyChanged("ToDate"); } }

        //[IgnoreDataMember]
        public virtual List<TypeTransactionReason> TypeTransactionReasons { get; set; }
        #endregion

        #region Constructions
        //parameterless ctor in order to be used in generic as T
        public Category()
            : this(null, null)
        {}
        public Category(User user)
            : this(user, null)
        {
            
        }
        public Category(User user, TypeTransactionReasonList typeTransactionList)
            : base(user)
        {

            if (typeTransactionList != null) 
                TypeTransactionReasons = typeTransactionList.Where(x => x.Name == "Other").ToList();

            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **//
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0);
            ToDate = FromDate.AddHours(1);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Hiding Clone is intended since the overrideen one was returning a BaseItem
        /// </summary>
        /// <returns></returns>
        public new Category Clone()
        {
            return (Category)this.MemberwiseClone();
        }

        public void OptimizeOnTopLevel()
        {
            if(this.TypeTransactionReasons != null)
                foreach (var item in this.TypeTransactionReasons)
                    item.Categories = null;
        }
        #endregion

        
    }
}
