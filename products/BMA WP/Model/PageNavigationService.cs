using BMA_WP.View;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model
{
    public static class PageNavigationService
    {
        public static void NavigateTo(Type page, string url)
        {
            if (page == typeof(MainPage))
                MainPage.NavigateTo(url, UriKind.Relative);
            else if(page == typeof(MainPage))
                MainPage.NavigateTo(url, UriKind.Relative);
            
        }

    }
}
