using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class TransactionList : ObservableCollection<Transaction>
    {
        
    }
    //[DataContractAttribute(IsReference = true)] 
    [DataContract]
    public class Transaction : BaseItem, INotifyPropertyChanged
    {
        double amount;
        string nameOfPlace;
        double tipAmount;
        string comments;
        Category category;
        TypeTransactionReason typeTransactionReason;
        TypeTransaction typeTransaction;

        [DataMember]
        [Required]
        public int TransactionId { get; set; }

        [DataMember]
        [Required]
        public double Amount { get { return amount; } set { amount = value; NotifyPropertyChanged("Amount"); } }

        [DataMember]
        public string NameOfPlace { get { return nameOfPlace; } set { nameOfPlace = value; NotifyPropertyChanged("NameOfPlace"); } }

        [DataMember]
        public double TipAmount { get { return tipAmount; } set { tipAmount = value; NotifyPropertyChanged("TipAmount"); } }

        [DataMember]
        public string Comments { get { return comments; } set { comments = value; NotifyPropertyChanged("Comments"); } }

        [DataMember]
        //[IgnoreDataMember]
        public Category Category { get { return category; } set { category = value; NotifyPropertyChanged("Category"); } }

        [DataMember]
        public TypeTransactionReason TransactionReasonType { get { return typeTransactionReason; } set { typeTransactionReason = value; NotifyPropertyChanged("TransactionReasonType"); } }

        [DataMember]
        public TypeTransaction TransactionType { get { return typeTransaction; } set { typeTransaction = value; NotifyPropertyChanged("TransactionType"); } }

        [IgnoreDataMember]
        public bool HasChanged { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                HasChanged = true;
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public Transaction()
        {
            TransactionId = -1;
            Amount = 0;
            Category = new Category();
            Comments = "";
            CreatedDate = DateTime.Now;
            //CreatedUser = new User();
            ModifiedDate = DateTime.Now;
            //ModifiedUser = new User();
            NameOfPlace = "";
            TipAmount = 0;
            TransactionReasonType = new TypeTransactionReason();
            TransactionType = new TypeTransaction(); 
        }
    }
}
