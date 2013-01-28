using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    //[DataContractAttribute(IsReference = true)] 
    [DataContract]
    public class Category : BaseItem
    {
        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [IgnoreDataMember]
        public List<Transaction> Transactions { get; set; }
    }
}
