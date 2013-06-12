using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
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
        #endregion

        #region Public Properties
        public int TypeTransactionId { get; set; }

        public string Name { get; set; }
        #endregion

        #region Constructors
        public TypeTransaction():this(null)
        { 
        }
        public TypeTransaction(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **//  
        }
        #endregion
    }
}
