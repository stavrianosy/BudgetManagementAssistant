using BMA_WP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model.RuleSupportItems
{
    public static class Position12List
    {
        static List<BasicItem> positions = new List<BasicItem> 
        { 
            new BasicItem(1, AppResources.PositionFirst), 
            new BasicItem(2, AppResources.PositionSecond), 
            new BasicItem(3, AppResources.PositionThird), 
            new BasicItem(4, AppResources.PositionForth), 
            new BasicItem(5, AppResources.PositionFifth), 
            new BasicItem(6, AppResources.PositionSisth), 
            new BasicItem(7, AppResources.PositionSeventh), 
            new BasicItem(8, AppResources.PositionEightth), 
            new BasicItem(9, AppResources.PositionNineth), 
            new BasicItem(10, AppResources.PositionTenth), 
            new BasicItem(11, AppResources.PositionEleventh), 
            new BasicItem(12, AppResources.PositionTwelveth) 
        };

        public static List<BasicItem> GetPositions()
        {
            return positions;
        }
    }
}
