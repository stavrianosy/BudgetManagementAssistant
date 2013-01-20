using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BMA.BusinessLogic
{
    public class Transaction
    {
        [Required]
        public int TransactionId { get; set; }

        [Required]
        public double Amount { get; set; }

        public string NameOfPlace { get; set; }

        public double TipAmount { get; set; }

        public string Comments { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        public User ModifiedUser { get; set; }

        public DateTime? CreatedDate { get; set; }

        [Required]
        public User CreatedUser { get; set; }

        public Category Category { get; set; }

        public TransactionReason TransactionReason { get; set; }

        public TypeTransaction TypeTransaction { get; set; }
    }
}
