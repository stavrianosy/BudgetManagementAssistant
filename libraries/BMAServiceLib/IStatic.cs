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
    public interface IStatic
    {
        [OperationContract]
        User AuthenticateUser(User user);

        [OperationContract]
        User RegisterUser(User user);

        [OperationContract]
        User ChangePassword(User user);

        [OperationContract]
        User ForgotPassword(User user);

        [OperationContract]
        StaticTypeList GetAllStaticData();

        [OperationContract]
        List<Category> GetAllCategories();

        [OperationContract]
        List<TypeTransactionReason> GetAllTypeTransactionReasons();

        [OperationContract]
        List<TypeTransaction> GetAllTypeTransactions();

        [OperationContract]
        List<Notification> GetUpcomingNotifications(DateTime clientTime);

        [OperationContract]
        StaticTypeList SyncStaticData(StaticTypeList staticData, User user);

        [OperationContract]
        List<Category> SyncCategories(List<Category> categories, User user);

        [OperationContract]
        List<TypeTransactionReason> SyncTypeTransactionReasons(List<TypeTransactionReason> typeTransactionReason);

        [OperationContract]
        List<Notification> SyncNotifications(List<Notification> notifications);

        [OperationContract]
        List<TypeTransaction> SyncTypeTransactions(List<TypeTransaction> typeTransactions);

        [OperationContract]
        List<TypeFrequency> SyncTypeFrequencies(List<TypeFrequency> typeFrequencies);

        [OperationContract]
        List<TypeInterval> SyncTypeIntervals(List<TypeInterval> interval);

        [OperationContract]
        List<BudgetThreshold> SyncBudgetThresholds(List<BudgetThreshold> budgetThreshold);

        [OperationContract]
        List<Category> SaveCategories(List<Category> categories, User user);

        [OperationContract]
        List<TypeTransactionReason> SaveTypeTransactionReasons(List<TypeTransactionReason> typeTransactionReason);

        [OperationContract]
        List<Notification> SaveNotifications(List<Notification> notifications);

        [OperationContract]
        List<TypeTransaction> SaveTypeTransactions(List<TypeTransaction> typeTransactions);

        [OperationContract]
        List<TypeFrequency> SaveTypeFrequencies(List<TypeFrequency> typeFrequencies);

        [OperationContract]
        List<TypeInterval> SaveTypeIntervals(List<TypeInterval> interval);

        [OperationContract]
        List<BudgetThreshold> SaveBudgetThresholds(List<BudgetThreshold> budgetThreshold);

    }
}
