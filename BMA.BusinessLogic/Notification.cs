using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMA.BusinessLogic
{
    public class Notification : BaseItem
    {
        public int NotificationId { get; set; }

        public string Name { get; set; }

        public DateTime Time { get; set; }

        public string Description { get; set; }
    }
}
