using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace BMA.BusinessLogic
{
    public class TransItem : BaseItem
    {
        public TransGroup Group { get; set; }

    }
}
