using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BMA.BusinessLogic
{
    public class BudgetList : ObservableCollection<Budget>, IDataList
    {
        public BudgetList GetChanges()
        {
            BudgetList result = new BudgetList();

            var query = this.Where(t => t.HasChanges).ToList();
            foreach (var item in query)
                result.Add(item);

            return result;
        }

        public void AcceptChanges()
        {
            foreach (var item in Items)
                item.HasChanges = false;
        }

        public void PrepareForServiceSerialization()
        {
            //one way to handle circular referenceis to explicitly set the child to null
            var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).ToList();
            foreach (var item in deletedIDs)
                this.RemoveAt(item.index);

            this.AcceptChanges();
        }

        public bool HasItemsWithChanges()
        {
            bool result = false;

            result = this.FirstOrDefault(x => x.HasChanges) != null;

            return result;
        }
    }

    public class Budget:BaseItem
    {
        #region Private Members
        private string name;
        private double amount;
        private DateTime fromDate;
        private DateTime toDate;
        private string comments;
        private bool repeat;
        private TransactionList transactions;
        #endregion

        #region Public Properties
        
        public int BudgetId { get; set; }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        public double Amount 
        { 
            get { return amount; } 
            set 
            { 
                amount = value;
                OnPropertyChanged("Amount");
                OnPropertyChanged("Balance");
                OnPropertyChanged("BalancePercent");                 
            } 
        }

        public DateTime FromDate 
        { 
            get { return fromDate; } 
            set 
            { 
                fromDate = value;
                OnPropertyChanged("FromDate");
                OnPropertyChanged("DurrationDays");
                OnPropertyChanged("DaysLeft");
                OnPropertyChanged("DaysLeftPercent"); 
            } 
        }

        public DateTime ToDate 
        { 
            get { return toDate; } 
            set 
            { 
                toDate = value;
                OnPropertyChanged("ToDate");
                OnPropertyChanged("DurrationDays");
                OnPropertyChanged("DaysLeft");
                OnPropertyChanged("DaysLeftPercent");
            } 
        }

        public string Comments { get { return comments; } set { comments = value; OnPropertyChanged("Comments"); } }

        /// <summary>
        /// Recreate on expiration
        /// </summary>
        public bool Repeat { get { return repeat; } set { repeat = value; OnPropertyChanged("Rrepeat"); } }

        /// <summary>
        /// Get - Display the duration of the budget in days.
        /// </summary>
        public double DurrationDays
        {
            get
            {
                TimeSpan timeSpan = ToDate.Subtract(FromDate);
                return timeSpan.Duration().TotalDays;
            }
        }

        /// <summary>
        /// Get - Days left to expire
        /// </summary>
        public double DaysLeft
        {
            get
            {
                TimeSpan timeSpan = ToDate.Subtract(DateTime.Now);
                var days = timeSpan.TotalDays;
                return days > 0 ?  days : 0;
            }
        }

        /// <summary>
        /// Get - Percentage of the days left to expire
        /// </summary>
        public double DaysLeftPercent
        {
            get{return DaysLeft / DurrationDays;}
        }

        /// <summary>
        /// Get - List of transactions based on the dates of budget
        /// </summary>
        public TransactionList Transactions
        {
            get{
                if (transactions == null)
                    transactions = new TransactionList();

                    return transactions.FilterOnDateRange(FromDate, ToDate);
                }
            set { transactions = value; }
        }

        /// <summary>
        /// Get - Using the list of transactions display balance
        /// </summary>
        public double Balance
        {
            get
            {
                double income = Transactions.Where(t => t.TransactionType.Name == "Income").Sum(t => t.Amount);
                double expense = Transactions.Where(t => t.TransactionType.Name == "Expense").Sum(t => t.Amount);

                return Amount + income - expense;
            }
        }

        /// <summary>
        /// Get - Using the list of transactions display balance percentage
        /// </summary>
        public double BalancePercent
        {
            get 
            {
                return Balance/Amount; 
            }
        }

        /// <summary>
        /// Hiding Clone is intended since the overrideen one was returning a BaseItem
        /// </summary>
        /// <returns></returns>
        public new Budget Clone()
        {
            var bud = new Budget(this.ModifiedUser);
            
            bud.Amount = this.Amount;
            bud.Comments = this.Comments;
            bud.FromDate = this.FromDate;
            bud.ToDate = this.ToDate;
            bud.Transactions = this.Transactions;
            bud.Repeat = this.Repeat;
            bud.Name = this.Name;

            return bud;
        }
        #endregion

        #region Constructor
        //parameterless ctor in order to be used in generic as T
        public Budget()
            : base(null)
        {}
        public Budget(User user):base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **//
            BudgetId = -1;
            Name = "";
            Amount = 0d;
            FromDate = DateTime.Now;
            ToDate = FromDate.AddDays(1);
            Comments = "";
        }
        #endregion

        #region Public Events
        #endregion

        #region Private Properties
        #endregion
    }
}
