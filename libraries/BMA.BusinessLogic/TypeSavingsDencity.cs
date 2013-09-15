using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class TypeSavingsDencityList : ObservableCollection<TypeSavingsDencity>, IDataList
    {
        public void AcceptChanges()
        {
            foreach (var item in Items)
                item.HasChanges = false;
        }

        public void PrepareForServiceSerialization()
        {
            var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).OrderByDescending(x => x.index).ToList();

            foreach (var item in deletedIDs)
                this.RemoveAt(item.index);

            this.AcceptChanges();
        }

        public bool HasItemsWithChanges()
        {
            bool result = false;

            result = this.FirstOrDefault(x => x.HasChanges) != null;

            return result;
        }
    }


    [DataContract]
    public class TypeSavingsDencity : BaseItem
    {
        #region Public Methods
        public override bool Equals(Object obj)
        {
            TypeSavingsDencity typeSavingsDencity = obj as TypeSavingsDencity;
            if (typeSavingsDencity == null)
                return false;
            else
                return TypeSavingsDencityId.Equals(typeSavingsDencity.TypeSavingsDencityId);
        }

        public override int GetHashCode()
        {
            return this.TypeSavingsDencityId.GetHashCode();
        }
        #endregion

        #region Public Properties
        public int TypeSavingsDencityId { get; set; }
        
        public string Name { get; set; }
        #endregion

        #region Constructors
        //parameterless ctor in order to be used in generic as T
        public TypeSavingsDencity()
            : base(null)
        {}
        public TypeSavingsDencity(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
        }
        #endregion
    }
}
