﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class TypeFrequency:BaseItem
    {
        public int TypeFrequencyId { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }
    }
}
