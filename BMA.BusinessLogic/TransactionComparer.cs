using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMA.BusinessLogic
{
    public class TransactionComparer : IEqualityComparer<Transaction>
    {
        public bool Equals(Transaction x, Transaction y)
        {
            return x.TransactionId.Equals(y.TransactionId);
        }
        public int GetHashCode(Transaction obj)
        {
            return obj.TransactionId.GetHashCode();
        }
    }
}
