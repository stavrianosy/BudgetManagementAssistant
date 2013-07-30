using BMA.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BMAServiceLib
{
    [ServiceContract]
    public interface IMain
    {
        #region Read
        [OperationContract]
        bool GetDBStatus();

        [OperationContract]
        DateTime GetLatestTransactionDate(int userId);

        [OperationContract]
        double GetLatestTransactionDateDouble(int userId);

        [OperationContract]
        TransactionList GetAllTransactions(int userId);

        [OperationContract]
        TransactionList GetLatestTransactions(int userId);

        [OperationContract]
        TransactionList GetLatestTransactionsLimit(int latestRecs, int userId);

        [OperationContract]
        TransactionList GetTransactionsForBudget(int budgetId);

        [OperationContract]
        TransactionImageList GetImagesForTransaction(int transactionId);

        [OperationContract]
        BudgetList GetAllBudgets(int userId);

        [OperationContract]
        StartupInfo LoadItemCounts(int userId);
        #endregion

        #region Update
        [OperationContract]
        TransactionList SaveTransactions(TransactionList transactions);

        [OperationContract]
        BudgetList SaveBudgets(BudgetList budgets);

        [OperationContract]
        bool SaveTransactionImages(TransactionImageList transactionImages);

        #endregion
    }
}
