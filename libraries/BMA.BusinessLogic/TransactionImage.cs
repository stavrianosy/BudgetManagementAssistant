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
            if (item.TransactionImageId <= 0 && this.Contains(item))
            {
                var minIndex = (from i in this
                               orderby i.TransactionImageId ascending
                               select i).ToList();

                item.TransactionImageId = minIndex[0].TransactionImageId - 1;

            }
            base.InsertItem(index, item);
        }

        public void AcceptChanges()
        {
            foreach (var item in Items)
                item.HasChanges = false;
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
