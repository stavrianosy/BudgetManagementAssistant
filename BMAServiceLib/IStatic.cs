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
        List<Category> SaveCategories(Category[] categories);

        [OperationContract]
        List<TypeTransactionReason> SaveTypeTransactionReasons(TypeTransactionReason[] typeTransactionReason);

        [OperationContract]
        List<Notification> SaveNotifications(Notification[] notifications);

        [OperationContract]
        List<TypeTransaction> SaveTypeTransactions(TypeTransaction[] typeTransactions);

        [OperationContract]
        List<TypeFrequency> SaveTypeFrequencies(TypeFrequency[] typeFrequencies);

        [OperationContract]
        List<TypeInterval> SaveTypeIntervals(TypeInterval[] interval);

        [OperationContract]
        List<BudgetThreshold> SaveBudgetThresholds(BudgetThreshold[] budgetThreshold);

        [OperationContract]
        List<Notification> GetUpcomingNotifications(DateTime clientTime);
    }
}
