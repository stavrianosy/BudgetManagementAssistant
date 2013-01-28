using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class TypeTransaction : BaseItem
    {
        public int TypeTransactionId { get; set; }

        public string Name { get; set; }

    }
}
