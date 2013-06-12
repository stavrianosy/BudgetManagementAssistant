using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace BMA.BusinessLogic
{
    //[DataContractAttribute(IsReference = true)] 
    [DataContract]
    public abstract class BaseItem : INotifyPropertyChanged //BindableBase
    {
        bool isDeleted;
        DateTime createdDate;

        [DataMember]
        public  DateTime ModifiedDate { get; set; }

        //[Required]
        [DataMember]
        public User ModifiedUser { get; set; }

        [DataMember]
        public bool IsDeleted { get { return isDeleted; } set { isDeleted = value; OnPropertyChanged("IsDeleted"); } }

        [DataMember]
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value; OnPropertyChanged("CreatedDate"); } }

        //[IgnoreDataMember]
        [DataMember]
        public bool HasChanges { get; set; }

        //[Required]
        [DataMember]
        public User CreatedUser { get; set; }

        #region Constructor
        public BaseItem()
        {
            ModifiedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
            IsDeleted = false;
            HasChanges = false;
        }
        public BaseItem(User user)
            : this()
        {
            CreatedUser = user;
            ModifiedUser = user;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
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
