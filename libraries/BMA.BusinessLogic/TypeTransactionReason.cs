using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class TypeTransactionReasonList : BaseList<TypeTransactionReason>, IDataList
    {
        public const int DEVICE_MAX_COUNT = 50;

        protected override void InsertItem(int index, TypeTransactionReason item)
        {

            bool added = false;

            //logic for new unueqe id 
            if (item.TypeTransactionReasonId <= 0 && this.Contains(item))
            {
                var minIndex = (from i in this
                                orderby i.TypeTransactionReasonId ascending
                                select i).ToList();

                item.TypeTransactionReasonId = minIndex[0].TypeTransactionReasonId - 1;
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
    public class TypeTransactionReason : BaseItem
    {
        #region Private Members
        string name;
        List<Category> categories;
        #endregion

        #region Public Methods
        public override bool Equals(Object obj)
        {
            TypeTransactionReason typeTransactionReason = obj as TypeTransactionReason;
            if (typeTransactionReason == null)
                return false;
            else
                return TypeTransactionReasonId.Equals(typeTransactionReason.TypeTransactionReasonId);
        }

        public override int GetHashCode()
        {
            return this.TypeTransactionReasonId.GetHashCode();
        }

        /// <summary>
        /// Hiding Clone is intended since the overrideen one was returning a BaseItem
        /// </summary>
        /// <returns></returns>
        public new TypeTransactionReason Clone()
        {
            return (TypeTransactionReason)this.MemberwiseClone();
        }

        public void OptimizeOnTopLevel()
        {
            if (this.Categories != null)
                foreach (var item in this.Categories)
                    item.TypeTransactionReasons = null;
        }

        #endregion

        #region Public Properties
        public int TypeTransactionReasonId { get; set; }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        public virtual List<Category> Categories { get { return categories; } set { categories = value; OnPropertyChanged("Categories"); } }
        #endregion

        #region Constructors
        //parameterless ctor in order to be used in generic as T
        public TypeTransactionReason(): this(null)
        {}
        public TypeTransactionReason(User user): this(user, null)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
        }
        public TypeTransactionReason(User user, CategoryList categoryList)
            : base(user)
        {
            if(categoryList!=null)
                Categories = categoryList.Where(x => x.Name == "Other").ToList();
        }
        #endregion
    }
}
