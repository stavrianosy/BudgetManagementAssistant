using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    //[DataContractAttribute(IsReference = true)] 
    [DataContract]
    public class Category : BaseItem, INotifyPropertyChanged
    {
        #region Private Members
    string name;
    DateTime fromDate;
    DateTime toDate;
    #endregion

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public string Name { get { return name; } set { name = value; NotifyPropertyChanged("Name"); } }

        /// <summary>
        /// Although this property should be TimeSpan, it is set as DateTime due to WCF serialization issues.
        /// </summary>
        [DataMember]
        public DateTime FromDate { get { return fromDate; } set { fromDate = value; NotifyPropertyChanged("FromDate"); } }

        /// <summary>
        /// Although this property should be TimeSpan, it is set as DateTime due to WCF serialization issues.
        /// </summary>
        [DataMember]
        public DateTime ToDate { get { return toDate; } set { toDate = value; NotifyPropertyChanged("ToDate"); } }


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

    }
}
