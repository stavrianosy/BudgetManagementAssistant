using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class TransactionImage : BaseItem
    {
        #region Private Members
        string path;
        byte[] image;
        string name;
        Transaction transaction;
        #endregion

        #region Public Methods
        public override bool Equals(Object obj)
        {
            TransactionImage transactionImage = obj as TransactionImage;
            if (transactionImage == null)
                return false;
            else
                return TransactionImageId.Equals(transactionImage.TransactionImageId);
        }

        public override int GetHashCode()
        {
            return this.TransactionImageId.GetHashCode();
        }
        #endregion

        #region Public Properties
        public int TransactionImageId { get; set; }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        public string Path { get { return path; } set { path = value; OnPropertyChanged("Path"); } }

        public byte[] Image { get { return image; } set { image = value; OnPropertyChanged("Image"); } }

    [IgnoreDataMember]
        public Transaction Transaction { get; set; }
        #endregion

        #region Constructions
        public TransactionImage()
            : this(null)
        { }
        public TransactionImage(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **//
        }
        #endregion
    }
}
