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
        DateTime GetLatestTransactionDate();

        [OperationContract]
        double GetLatestTransactionDateDouble(int userId);

        [OperationContract]
        TransactionList GetAllTransactions(int userId);

        [OperationContract]
        TransactionList GetLatestTransactions(int userId);

        [OperationContract]
        TransactionList GetLatestTransactionsOnDate(int userId);

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

        [OperationContract]
        bool SyncTransactions(TransactionList transactions);

        [OperationContract]
        bool SyncBudgets(BudgetList budgets);

        #endregion

        #region Update
        [OperationContract]
        TransactionList SaveTransactions(TransactionList transactions);

        [OperationContract]
        BudgetList SaveBudgets(BudgetList budgets);

        [OperationContract]
        bool SaveTransactionImages(TransactionImageList transactionImages);

        #endregion

        #region Reports
        [OperationContract]
        List<Transaction> ReportTransactionAmount(DateTime dateFrom, DateTime dateTo, int transTypeId, double amountFrom, double amountTo, int userId);

        [OperationContract]
        Dictionary<Category, double> ReportTransactionCategory(DateTime dateFrom, DateTime dateTo, int transTypeId, int userId);

        [OperationContract]
        Dictionary<TypeTransactionReason, double> ReportTransactionReason(DateTime dateFrom, DateTime dateTo, int transTypeId, int userId);

        [OperationContract]
        List<Budget> ReportTransactionBudget(DateTime dateFrom, DateTime dateTo, int transTypeId, int userId);

        [OperationContract]
        Dictionary<string, double> ReportTransactionNameOfPlace(DateTime dateFrom, DateTime dateTo, int transTypeId, int userId);
        #endregion
    }
}
