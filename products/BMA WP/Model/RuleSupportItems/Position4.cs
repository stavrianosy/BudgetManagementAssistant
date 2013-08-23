using BMA_WP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model.RuleSupportItems
{
    public static class Position4List
    {
        static List<BasicItem> position4 = new List<BasicItem> 
        { 
            new BasicItem(1, AppResources.PositionFirst), 
            new BasicItem(2, AppResources.PositionSecond), 
            new BasicItem(3, AppResources.PositionThird), 
            new BasicItem(4, AppResources.PositionForth), 
            new BasicItem(5, AppResources.PositionLast)};

        public static List<BasicItem> GetPositions()
        {
            return position4;
        }
    }
}
