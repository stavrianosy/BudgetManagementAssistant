using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BMA.BusinessLogic
{
    //[DataContractAttribute(IsReference = true)] 
    [DataContract]
    public abstract class BaseItem : INotifyPropertyChanged
    {

        bool isDeleted;
        //bool hasChanges;
        DateTime createdDate;
        //[DataMember]
        //public int Id { get; set; }

        [DataMember]
        [Required]
        public  DateTime ModifiedDate { get; set; }

        [DataMember]
        //[Required]
        public User ModifiedUser { get; set; }

        [DataMember]
        [Required]
        public bool IsDeleted { get { return isDeleted; } set { isDeleted = value; NotifyPropertyChanged("IsDeleted"); } }

        [DataMember]
        [Required]
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value; NotifyPropertyChanged("CreatedDate"); } }

        //[IgnoreDataMember]
        [DataMember]
        [Required]
        public bool HasChanges { get; set; }

        [DataMember]
        //[Required]
        public User CreatedUser { get; set; }

        #region Constructor
        public BaseItem()
        {
            ModifiedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
            IsDeleted = false;
            HasChanges = false;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                HasChanges = true;
                ModifiedDate = DateTime.Now;
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
