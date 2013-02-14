using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BMA.BusinessLogic
{
    //[DataContractAttribute(IsReference = true)] 
    [DataContract]
    public abstract class BaseItem : BindableBase
    {
        //[DataMember]
        //public int Id { get; set; }

        [DataMember]
        [Required]
        public  DateTime ModifiedDate { get; set; }

        //[DataMember]
        //[Required]
        //public User ModifiedUser { get; set; }

        [DataMember]
        [Required]
        public DateTime CreatedDate { get; set; }

        //[DataMember]
        //[Required]
        //public User CreatedUser { get; set; }

        #region Constructor
        public BaseItem()
        {
            ModifiedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
        }
        #endregion

    }
}
