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
        [OperationContract]
        TransactionList GetAllTransactions();

        [OperationContract]
        TransactionList GetLatestTransactions();

        [OperationContract]
        TransactionList GetLatestTransactionsLimit(int latestRecs);

        [OperationContract]
        TransactionList GetTransactionsForBudget(int budgetId);

        [OperationContract]
        BudgetList GetAllBudgets();

        [OperationContract]
        List<TypeTransaction> GetAllTypeTransactions();

        [OperationContract]
        StartupInfo LoadItemCounts();

        [OperationContract]
        TransactionList SaveTransactions(TransactionList transactions);

        [OperationContract]
        TransactionList SyncTransactions(TransactionList transactions);

        [OperationContract]
        BudgetList SaveBudgets(BudgetList transactions);

        [OperationContract]
        BudgetList SyncBudgets(BudgetList budgets);
    }
}
