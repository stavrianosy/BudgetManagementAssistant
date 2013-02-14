using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class BaseItemComparer : IEqualityComparer<BaseItem>
    {
        public bool Equals(BaseItem x, BaseItem y)
        {
            return true;// x.Id.Equals(y.Id);
        }

        public int GetHashCode(BaseItem obj)
        {
            return 0;// obj.Id.GetHashCode();
        }
    }
}
