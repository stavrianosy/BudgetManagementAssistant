using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    //[DataContractAttribute(IsReference = true)] 
    [DataContract]
    public class Transaction : BaseItem
    {
        [DataMember]
        [Required]
        public int TransactionId { get; set; }

        [DataMember]
        [Required]
        public double Amount { get; set; }

        [DataMember]
        public string NameOfPlace { get; set; }

        [DataMember]
        public double TipAmount { get; set; }

        [DataMember]
        public string Comments { get; set; }

        [DataMember]
        //[IgnoreDataMember]
        public Category Category { get; set; }

        [DataMember]
        public TypeTransactionReason TransactionReasonType { get; set; }

        [DataMember]
        public TypeTransaction TransactionType { get; set; }
    }
}
