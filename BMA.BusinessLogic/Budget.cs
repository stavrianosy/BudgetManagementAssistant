using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BMA.BusinessLogic
{
    public class BudgetList : ObservableCollection<Budget>
    {
        public BudgetList GetChanges()
        {
            BudgetList result = new BudgetList();

            var query = this.Where(t => t.HasChanges).ToList();
            foreach (var item in query)
                result.Add(item);

            return result;
        }
    }
    public class Budget:BaseItem, INotifyPropertyChanged
    {
        #region Private Members
        private string name;
        private double amount;
        private DateTime fromDate;
        private DateTime toDate;
        private string comments;
        private bool includeInstallemnts;
        private TransactionList transactions;
        #endregion

        #region Public Properties
        
        public int BudgetId { get; set; }

        [Required]
        public string Name { get { return name; } set { name = value; NotifyPropertyChanged("Name"); } }

        public double Amount 
        { 
            get { return amount; } 
            set 
            { 
                amount = value; 
                NotifyPropertyChanged("Amount"); 
                NotifyPropertyChanged("Balance");
                NotifyPropertyChanged("BalancePercent");                 
            } 
        }

        public DateTime FromDate 
        { 
            get { return fromDate; } 
            set 
            { 
                fromDate = value; 
                NotifyPropertyChanged("FromDate");
                NotifyPropertyChanged("DurrationDays");
                NotifyPropertyChanged("DaysLeft");
                NotifyPropertyChanged("DaysLeftPercent"); 
            } 
        }

        public DateTime ToDate 
        { 
            get { return toDate; } 
            set 
            { 
                toDate = value; 
                NotifyPropertyChanged("ToDate"); 
                NotifyPropertyChanged("DurrationDays");
                NotifyPropertyChanged("DaysLeft");
                NotifyPropertyChanged("DaysLeftPercent");
            } 
        }

        public string Comments { get { return comments; } set { comments = value; NotifyPropertyChanged("Comments"); } }

        /// <summary>
        /// Subtract the installment amount when its date come.
        /// </summary>
        public bool IncludeInstallments { get { return includeInstallemnts; } set { includeInstallemnts = value; NotifyPropertyChanged("IncludeInstallemnts"); } }

        /// <summary>
        /// Display the duration of the budget in days.
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
        /// Days left to expire
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
        /// Percentage of the days left to expire
        /// </summary>
        public double DaysLeftPercent
        {
            get{return DaysLeft / DurrationDays;}
        }

        /// <summary>
        /// Get a list of transactions based on the dates of budget
        /// </summary>
        public TransactionList Transactions
        {
            get{return transactions.FilterOnDateRange(FromDate, ToDate);}
            set { transactions = value; }
        }

        /// <summary>
        /// Using the list of transactions display balance
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
        /// Using the list of transactions display balance percentage
        /// </summary>
        public double BalancePercent
        {
            get 
            {
                return Balance/Amount; 
            }
        }
        #endregion

        #region Constructor
        public Budget()
        {
            BudgetId = -1;
            Name = "";
            Amount = 0d;
            FromDate = DateTime.Now;
            ToDate = DateTime.Now.AddDays(1);
            Comments = "";
            IncludeInstallments = false;
            CreatedDate = DateTime.Now;
            CreatedUser = new User();
            ModifiedDate = DateTime.Now;
            ModifiedUser = new User();
        }
        public Budget(User user)
        {
            CreatedUser = user;
            ModifiedUser = user;
            
        }
        #endregion

        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Private Properties
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                HasChanges = true;
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion
    }
}
