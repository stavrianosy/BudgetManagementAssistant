using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class TransactionImageList : ObservableCollection<TransactionImage>
    {
        public TransactionImageList()
        {
            //CollectionChanged += TransactionList_CollectionChanged;
        }

        protected override void InsertItem(int index, TransactionImage item)
        {
            if (item.TransactionImageId <= 0)
            {
                var minIndex = (from i in this
                               orderby i.TransactionImageId ascending
                               select i).ToList();

                if (minIndex.Count > 0 && minIndex[0].TransactionImageId <= 0)
                    item.TransactionImageId = minIndex[0].TransactionImageId - 1;
                else
                    item.TransactionImageId = 0;

            }
            base.InsertItem(index, item);
        }

    }

    public class TransactionImage : BaseItem
    {
        #region Private Members
        string path;
        byte[] image;
        byte[] thumbnail;
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

        public byte[] Thumbnail { get { return thumbnail; } set { thumbnail = value; OnPropertyChanged("Thumbnail"); } }
        
        //[IgnoreDataMember]
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
