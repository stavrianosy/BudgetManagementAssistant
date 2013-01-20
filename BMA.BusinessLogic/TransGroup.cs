using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class TransGroup:BaseItem
    {

        private int _newItemCount;

        public string ImagePath { get; set; }

        public int CategoryId { get; set; }

        public int NewItemCount
        {
            get { return _newItemCount; }
            //set
            //{
            //    SetProperty(ref _newItemCount, value);
            //}
        }
    }
}
