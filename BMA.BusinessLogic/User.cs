using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BMA.BusinessLogic
{
    public class User //: BaseItem
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}
