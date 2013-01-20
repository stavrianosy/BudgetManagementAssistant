using System;
using System.Runtime.Serialization;

namespace BMA.BusinessLogic
{
    [DataContract]
    public abstract class BaseItem //: BindableBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public DateTime? ModifiedDate { get; set; }

        [DataMember]
        public int ModifiedUserId { get; set; }

        [DataMember]
        public DateTime? CreatedDate { get; set; }

        [DataMember]
        public int CreatedUserId { get; set; }
    }
}
