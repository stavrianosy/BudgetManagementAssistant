﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class TypeTransactionReason : BaseItem
    {
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
        #endregion

        #region Public Properties
        public int TypeTransactionReasonId { get; set; }

        public string Name { get; set; }
        #endregion

        #region Constructors
        public TypeTransactionReason():this(null)
        { 
        }
        public TypeTransactionReason(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
        }
        #endregion
    }
}
