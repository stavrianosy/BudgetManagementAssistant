using BMA.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BMAServiceLib
{
    [ServiceContract]
    public interface IStatic
    {
        [OperationContract]
        StaticTypeList GetAllStaticData();
    }
}
