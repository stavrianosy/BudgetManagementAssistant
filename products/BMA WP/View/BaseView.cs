﻿using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.View
{
    public interface IView
    {
        bool NavigateTo(string uriString, System.UriKind uriKind);
    }
}