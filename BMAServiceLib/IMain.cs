using BMA.BusinessLogic;
using System;
using System.Collections.Generic;
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
        List<Budget> GetAllBudgets();

        [OperationContract]
        Dictionary<Category, List<Transaction>> GetAllTransCategories();

        [OperationContract]
        List<TypeTransaction> GetAllTypeTransactions();

        [OperationContract]
        StartupInfo LoadItemCounts();

        [OperationContract]
        StaticTypeList GetAllStaticData();

    }
}
