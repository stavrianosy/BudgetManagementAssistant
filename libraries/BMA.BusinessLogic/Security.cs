using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMA.BusinessLogic
{
    public class Security : BaseItem
    {
        public int SecurityId { get; set; }

        #region Constructors
        //parameterless ctor in order to be used in generic as T
        public Security()
            : base(null)
        {}
        public Security(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
        }
        #endregion
    }
}
