﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18046
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.VisualStudio.ServiceReference.Platforms, version 11.0.50727.1
// 
namespace BMA.BMAStaticDataService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BMAStaticDataService.IStatic")]
    public interface IStatic {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/AuthenticateUser", ReplyAction="http://tempuri.org/IStatic/AuthenticateUserResponse")]
        System.Threading.Tasks.Task<BMA.BusinessLogic.User> AuthenticateUserAsync(BMA.BusinessLogic.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/RegisterUser", ReplyAction="http://tempuri.org/IStatic/RegisterUserResponse")]
        System.Threading.Tasks.Task<BMA.BusinessLogic.User> RegisterUserAsync(BMA.BusinessLogic.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/ChangePassword", ReplyAction="http://tempuri.org/IStatic/ChangePasswordResponse")]
        System.Threading.Tasks.Task<BMA.BusinessLogic.User> ChangePasswordAsync(BMA.BusinessLogic.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/ForgotPassword", ReplyAction="http://tempuri.org/IStatic/ForgotPasswordResponse")]
        System.Threading.Tasks.Task<BMA.BusinessLogic.User> ForgotPasswordAsync(BMA.BusinessLogic.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/GetAllStaticData", ReplyAction="http://tempuri.org/IStatic/GetAllStaticDataResponse")]
        System.Threading.Tasks.Task<BMA.BusinessLogic.StaticTypeList> GetAllStaticDataAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/GetAllCategories", ReplyAction="http://tempuri.org/IStatic/GetAllCategoriesResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Category>> GetAllCategoriesAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/GetAllTypeTransactionReasons", ReplyAction="http://tempuri.org/IStatic/GetAllTypeTransactionReasonsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransactionReason>> GetAllTypeTransactionReasonsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/GetAllTypeTransactions", ReplyAction="http://tempuri.org/IStatic/GetAllTypeTransactionsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransaction>> GetAllTypeTransactionsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/GetUpcomingNotifications", ReplyAction="http://tempuri.org/IStatic/GetUpcomingNotificationsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Notification>> GetUpcomingNotificationsAsync(System.DateTime clientTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SyncStaticData", ReplyAction="http://tempuri.org/IStatic/SyncStaticDataResponse")]
        System.Threading.Tasks.Task<BMA.BusinessLogic.StaticTypeList> SyncStaticDataAsync(BMA.BusinessLogic.StaticTypeList staticData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SyncCategories", ReplyAction="http://tempuri.org/IStatic/SyncCategoriesResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Category>> SyncCategoriesAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Category> categories);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SyncTypeTransactionReasons", ReplyAction="http://tempuri.org/IStatic/SyncTypeTransactionReasonsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransactionReason>> SyncTypeTransactionReasonsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransactionReason> typeTransactionReason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SyncNotifications", ReplyAction="http://tempuri.org/IStatic/SyncNotificationsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Notification>> SyncNotificationsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Notification> notifications);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SyncTypeTransactions", ReplyAction="http://tempuri.org/IStatic/SyncTypeTransactionsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransaction>> SyncTypeTransactionsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransaction> typeTransactions);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SyncTypeFrequencies", ReplyAction="http://tempuri.org/IStatic/SyncTypeFrequenciesResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeFrequency>> SyncTypeFrequenciesAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeFrequency> typeFrequencies);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SyncTypeIntervals", ReplyAction="http://tempuri.org/IStatic/SyncTypeIntervalsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeInterval>> SyncTypeIntervalsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeInterval> interval);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SyncBudgetThresholds", ReplyAction="http://tempuri.org/IStatic/SyncBudgetThresholdsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.BudgetThreshold>> SyncBudgetThresholdsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.BudgetThreshold> budgetThreshold);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SaveCategories", ReplyAction="http://tempuri.org/IStatic/SaveCategoriesResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Category>> SaveCategoriesAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Category> categories);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SaveTypeTransactionReasons", ReplyAction="http://tempuri.org/IStatic/SaveTypeTransactionReasonsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransactionReason>> SaveTypeTransactionReasonsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransactionReason> typeTransactionReason);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SaveNotifications", ReplyAction="http://tempuri.org/IStatic/SaveNotificationsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Notification>> SaveNotificationsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Notification> notifications);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SaveTypeTransactions", ReplyAction="http://tempuri.org/IStatic/SaveTypeTransactionsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransaction>> SaveTypeTransactionsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransaction> typeTransactions);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SaveTypeFrequencies", ReplyAction="http://tempuri.org/IStatic/SaveTypeFrequenciesResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeFrequency>> SaveTypeFrequenciesAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeFrequency> typeFrequencies);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SaveTypeIntervals", ReplyAction="http://tempuri.org/IStatic/SaveTypeIntervalsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeInterval>> SaveTypeIntervalsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeInterval> interval);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStatic/SaveBudgetThresholds", ReplyAction="http://tempuri.org/IStatic/SaveBudgetThresholdsResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.BudgetThreshold>> SaveBudgetThresholdsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.BudgetThreshold> budgetThreshold);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IStaticChannel : BMA.BMAStaticDataService.IStatic, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StaticClient : System.ServiceModel.ClientBase<BMA.BMAStaticDataService.IStatic>, BMA.BMAStaticDataService.IStatic {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public StaticClient() : 
                base(StaticClient.GetDefaultBinding(), StaticClient.GetDefaultEndpointAddress()) {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IStatic.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public StaticClient(EndpointConfiguration endpointConfiguration) : 
                base(StaticClient.GetBindingForEndpoint(endpointConfiguration), StaticClient.GetEndpointAddress(endpointConfiguration)) {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public StaticClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(StaticClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress)) {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public StaticClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(StaticClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress) {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public StaticClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Threading.Tasks.Task<BMA.BusinessLogic.User> AuthenticateUserAsync(BMA.BusinessLogic.User user) {
            return base.Channel.AuthenticateUserAsync(user);
        }
        
        public System.Threading.Tasks.Task<BMA.BusinessLogic.User> RegisterUserAsync(BMA.BusinessLogic.User user) {
            return base.Channel.RegisterUserAsync(user);
        }
        
        public System.Threading.Tasks.Task<BMA.BusinessLogic.User> ChangePasswordAsync(BMA.BusinessLogic.User user) {
            return base.Channel.ChangePasswordAsync(user);
        }
        
        public System.Threading.Tasks.Task<BMA.BusinessLogic.User> ForgotPasswordAsync(BMA.BusinessLogic.User user) {
            return base.Channel.ForgotPasswordAsync(user);
        }
        
        public System.Threading.Tasks.Task<BMA.BusinessLogic.StaticTypeList> GetAllStaticDataAsync() {
            return base.Channel.GetAllStaticDataAsync();
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Category>> GetAllCategoriesAsync() {
            return base.Channel.GetAllCategoriesAsync();
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransactionReason>> GetAllTypeTransactionReasonsAsync() {
            return base.Channel.GetAllTypeTransactionReasonsAsync();
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransaction>> GetAllTypeTransactionsAsync() {
            return base.Channel.GetAllTypeTransactionsAsync();
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Notification>> GetUpcomingNotificationsAsync(System.DateTime clientTime) {
            return base.Channel.GetUpcomingNotificationsAsync(clientTime);
        }
        
        public System.Threading.Tasks.Task<BMA.BusinessLogic.StaticTypeList> SyncStaticDataAsync(BMA.BusinessLogic.StaticTypeList staticData) {
            return base.Channel.SyncStaticDataAsync(staticData);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Category>> SyncCategoriesAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Category> categories) {
            return base.Channel.SyncCategoriesAsync(categories);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransactionReason>> SyncTypeTransactionReasonsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransactionReason> typeTransactionReason) {
            return base.Channel.SyncTypeTransactionReasonsAsync(typeTransactionReason);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Notification>> SyncNotificationsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Notification> notifications) {
            return base.Channel.SyncNotificationsAsync(notifications);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransaction>> SyncTypeTransactionsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransaction> typeTransactions) {
            return base.Channel.SyncTypeTransactionsAsync(typeTransactions);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeFrequency>> SyncTypeFrequenciesAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeFrequency> typeFrequencies) {
            return base.Channel.SyncTypeFrequenciesAsync(typeFrequencies);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeInterval>> SyncTypeIntervalsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeInterval> interval) {
            return base.Channel.SyncTypeIntervalsAsync(interval);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.BudgetThreshold>> SyncBudgetThresholdsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.BudgetThreshold> budgetThreshold) {
            return base.Channel.SyncBudgetThresholdsAsync(budgetThreshold);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Category>> SaveCategoriesAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Category> categories) {
            return base.Channel.SaveCategoriesAsync(categories);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransactionReason>> SaveTypeTransactionReasonsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransactionReason> typeTransactionReason) {
            return base.Channel.SaveTypeTransactionReasonsAsync(typeTransactionReason);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Notification>> SaveNotificationsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.Notification> notifications) {
            return base.Channel.SaveNotificationsAsync(notifications);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransaction>> SaveTypeTransactionsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeTransaction> typeTransactions) {
            return base.Channel.SaveTypeTransactionsAsync(typeTransactions);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeFrequency>> SaveTypeFrequenciesAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeFrequency> typeFrequencies) {
            return base.Channel.SaveTypeFrequenciesAsync(typeFrequencies);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeInterval>> SaveTypeIntervalsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.TypeInterval> interval) {
            return base.Channel.SaveTypeIntervalsAsync(interval);
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.BudgetThreshold>> SaveBudgetThresholdsAsync(System.Collections.ObjectModel.ObservableCollection<BMA.BusinessLogic.BudgetThreshold> budgetThreshold) {
            return base.Channel.SaveBudgetThresholdsAsync(budgetThreshold);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync() {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync() {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IStatic)) {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IStatic)) {
                return new System.ServiceModel.EndpointAddress("http://win-vvthsubpmki/BMAService/BMAStaticDataService.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding() {
            return StaticClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IStatic);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress() {
            return StaticClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IStatic);
        }
        
        public enum EndpointConfiguration {
            
            BasicHttpBinding_IStatic,
        }
    }
}
