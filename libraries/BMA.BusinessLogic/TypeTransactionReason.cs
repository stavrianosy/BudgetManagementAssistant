using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class TypeTransactionReasonList : ObservableCollection<TypeTransactionReason>, IDataList
    {
        public void AcceptChanges()
        {
            foreach (var item in Items)
                item.HasChanges = false;
        }

        public void PrepareForServiceSerialization()
        {
            this.OptimizeOnTopLevel();

            var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).ToList();

            foreach (var item in deletedIDs)
                this.RemoveAt(item.index);

            this.AcceptChanges();
        }

        public void OptimizeOnTopLevel()
        {
            foreach (var item in this)
                item.OptimizeOnTopLevel();
        }

        public bool HasItemsWithChanges()
        {
            bool result = false;

            result = this.FirstOrDefault(x => x.HasChanges) != null;

            return result;
        }
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

        public TypeTransactionReason Clone()
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
        public TypeTransactionReason(): base(null)
        {}
        public TypeTransactionReason(User user): base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
        }
        #endregion
    }
}
