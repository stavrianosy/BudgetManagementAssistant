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
        List<Transaction> GetAllTransactions();

        [OperationContract]
        TransactionList GetLatestTransactions();

        [OperationContract]
        TransactionList GetLatestTransactionsLimit(int latestRecs);

        [OperationContract]
        List<Transaction> GetTransactionsForBudget(int budgetId);

        [OperationContract]
        List<Budget> GetAllBudgets();

        //[OperationContract]
        //Dictionary<Category, List<Transaction>> GetAllTransCategories();

        [OperationContract]
        List<TypeTransaction> GetAllTypeTransactions();

        [OperationContract]
        StartupInfo LoadItemCounts();

        [OperationContract]
        StaticTypeList GetAllStaticData();

        [OperationContract]
        TransactionList SaveTransactions(Transaction[] transactions);

        [OperationContract]
        TransactionList SaveTransaction(Transaction transaction);

        [OperationContract]
        List<Category> SaveCategories(Category[] categories);

    }
}
