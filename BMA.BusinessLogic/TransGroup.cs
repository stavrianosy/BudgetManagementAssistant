using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace BMA.BusinessLogic
{
    public class TransGroup:BaseItem
    {
        [DataMember]
        public  int Id{get; set;}

        public string ImagePath { get; set; }

        public int CategoryId { get; set; }

        [DataMember]
        public string Title { get; set; }


        public int NewItemCount
        {
            get { return 0; }
            //set
            //{
            //    SetProperty(ref _newItemCount, value);
            //}
        }
    }
}
