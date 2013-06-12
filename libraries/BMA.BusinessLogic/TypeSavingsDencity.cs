using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
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
        public TypeSavingsDencity():this(null)
        { 
        }
        public TypeSavingsDencity(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
        }
        #endregion
    }
}
