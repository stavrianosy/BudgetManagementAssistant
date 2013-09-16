using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class TypeTransactionList : BaseList<TypeTransaction>, IDataList
    {
        
        public void PrepareForServiceSerialization()
        {
            var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).OrderByDescending(x => x.index).ToList();

            foreach (var item in deletedIDs)
                this.RemoveAt(item.index);

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
    }

    public class TypeTransaction : BaseItem
    {
        #region Public Methods
        public override bool Equals(Object obj)
        {
            TypeTransaction typeTransaction = obj as TypeTransaction;
            if (typeTransaction == null)
                return false;
            else
                return TypeTransactionId.Equals(typeTransaction.TypeTransactionId);
        }

        public override int GetHashCode()
        {
            return this.TypeTransactionId.GetHashCode();
        }

        /// <summary>
        /// Hiding Clone is intended since the overrideen one was returning a BaseItem
        /// </summary>
        /// <returns></returns>
        public new TypeTransaction Clone()
        {
            return (TypeTransaction)this.MemberwiseClone();
        }
        
        #endregion

        #region Public Properties
        public int TypeTransactionId { get; set; }

        public string Name { get; set; }

        public bool IsIncome { get; set; }
        #endregion

        #region Constructors
        //parameterless ctor in order to be used in generic as T
        public TypeTransaction()
            : base(null)
        {}
        public TypeTransaction(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **//  
        }
        #endregion

    }
}
