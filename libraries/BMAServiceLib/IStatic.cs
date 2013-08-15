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
        #region Read
        [OperationContract]
        bool GetDBStatus();

        [OperationContract]
        User AuthenticateUser(User user);

        [OperationContract]
            User RegisterUser(User user);

        [OperationContract]
        User UpdateUser(User user);

        [OperationContract]
        User ChangePassword(User user);

        [OperationContract]
        User ForgotPassword(User user);

        //[OperationContract]
        //StaticTypeList GetAllStaticData(int userId);

        [OperationContract]
        CategoryList GetAllCategories(int userId);

        [OperationContract]
        TypeTransactionReasonList GetAllTypeTransactionReasons(int userId);

        [OperationContract]
        List<TypeTransaction> GetAllTypeTransactions(int userId);

        [OperationContract]
        List<TypeSavingsDencity> GetAllTypeSavingsDencities(int userId);

        [OperationContract]
        List<Notification> GetAllNotifications(int userId);

        [OperationContract]
        TypeIntervalList GetAllTypeIntervals(int userId);

        [OperationContract]
        List<TypeFrequency> GetAllTypeFrequencies(int userId);

        [OperationContract]
        List<BudgetThreshold> GetAllBudgetThresholds(int userId);

        [OperationContract]
        RecurrenceRuleList GetAllRecurrenceRules();

        [OperationContract]
        List<Notification> GetUpcomingNotifications(DateTime clientTime, int userId);

        [OperationContract]
        TypeIntervalConfiguration GetTypeIntervalConfiguration(int userId);

        #endregion

        #region Update
        //[OperationContract]
        //StaticTypeList SyncStaticData(StaticTypeList staticData);

        //[OperationContract]
        //List<Category> SyncCategories(List<Category> categories);

        //[OperationContract]
        //List<TypeTransactionReason> SyncTypeTransactionReasons(List<TypeTransactionReason> typeTransactionReason);

        //[OperationContract]
        //List<Notification> SyncNotifications(List<Notification> notifications);

        //[OperationContract]
        //List<TypeTransaction> SyncTypeTransactions(List<TypeTransaction> typeTransactions);

        //[OperationContract]
        //List<TypeFrequency> SyncTypeFrequencies(List<TypeFrequency> typeFrequencies);

        //[OperationContract]
        //List<TypeInterval> SyncTypeIntervals(List<TypeInterval> interval);

        //[OperationContract]
        //List<BudgetThreshold> SyncBudgetThresholds(List<BudgetThreshold> budgetThreshold);

        [OperationContract]
        CategoryList SaveCategories(CategoryList categories);

        [OperationContract]
        TypeTransactionReasonList SaveTypeTransactionReasons(TypeTransactionReasonList typeTransactionReason);

        [OperationContract]
        NotificationList SaveNotifications(NotificationList notifications);

        [OperationContract]
        List<TypeTransaction> SaveTypeTransactions(List<TypeTransaction> typeTransactions);

        [OperationContract]
        List<TypeFrequency> SaveTypeFrequencies(List<TypeFrequency> typeFrequencies);

        [OperationContract]
        TypeIntervalList SaveTypeIntervals(TypeIntervalList interval);

        [OperationContract]
        List<BudgetThreshold> SaveBudgetThresholds(List<BudgetThreshold> budgetThreshold);
        #endregion
    }
}
