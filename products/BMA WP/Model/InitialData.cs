using BMA.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model
{
    public static class InitialData
    {
        public static CategoryList InitializeCategories(User user)
        {
            var result = new CategoryList { 
            new Category(user){Name="Other",CategoryId=1 }};

            return result;
        }


        public static TypeTransactionList InitializeTypeTransactions()
        {
            var user = new User{UserId = 2};
            var result = new TypeTransactionList { 
            new TypeTransaction(user){Name="Income",TypeTransactionId=1, },
            new TypeTransaction(user){Name="Expense",TypeTransactionId=2 }};

            return result;
        }

        public static TypeTransactionReasonList InitializeTypeTransactionReasons(User user)
        {
            var result = new TypeTransactionReasonList { 
            new TypeTransactionReason(user){Name="Other", TypeTransactionReasonId=1 }};

            return result;
        }
    }
}
