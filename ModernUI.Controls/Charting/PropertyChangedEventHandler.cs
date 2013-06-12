using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernUI.Toolkit.Data.Charting
{
    public delegate void PropertChangedEventHander(object sender, object OldValue,object NewValue);
    public delegate void PropertChangedEventHander<T>(object sender, T OldValue, T NewValue);
}
