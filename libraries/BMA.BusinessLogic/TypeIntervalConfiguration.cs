using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class TypeIntervalConfiguration:BaseItem
    {
        private DateTime generatedDate;

        public int TypeIntervalConfigurationId { get; set; }

        public DateTime GeneratedDate { get { return generatedDate; } set { generatedDate = value;  } }

        public TypeIntervalConfiguration()
            : this(null)
        {
        }
        public TypeIntervalConfiguration(User user)
            : base(user)
        {
        }
    }
}
