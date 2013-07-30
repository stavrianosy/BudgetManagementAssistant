using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    interface IDataList
    {
        void AcceptChanges();

        void PrepareForServiceSerialization();
    }
}
