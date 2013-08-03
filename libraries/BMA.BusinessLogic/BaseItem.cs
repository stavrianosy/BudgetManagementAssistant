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
        bool hasChanges;
        DateTime createdDate;
        DateTime syncDate;

        [DataMember]
        public  DateTime ModifiedDate { get; set; }

        //[Required]
        [DataMember]
        public User ModifiedUser { get; set; }

        [DataMember]
        public bool IsDeleted { get { return isDeleted; } set { isDeleted = value; OnPropertyChanged("IsDeleted"); } }

        [DataMember]
        public DateTime CreatedDate { get { return createdDate; } set { createdDate = value; OnPropertyChanged("CreatedDate"); } }

        [DataMember]
        public DateTime SyncDate { get { return syncDate; } set { syncDate = value; OnPropertyChanged("SyncDate"); } }

        //[IgnoreDataMember]
        [DataMember]
        public bool HasChanges { get { return hasChanges; } set { hasChanges = value; OnPropertyChanged("HasChanges"); } }

        //[Required]
        [DataMember]
        public User CreatedUser { get; set; }

        #region Constructor
        public BaseItem()
        {
            ModifiedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
            IsDeleted = false;
            HasChanges = true;
        }
        public BaseItem(User user)
            : this()
        {
            CreatedUser = user;
            ModifiedUser = user;
        }
        #endregion

        #region Public Methods
        public BaseItem Clone()
        {
            return (BaseItem)this.MemberwiseClone();
        }

        public void AcceptChanges()
        {
            hasChanges = false;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                if (propName != "HasChanges")
                    hasChanges = true;
                ModifiedDate = DateTime.Now;
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
