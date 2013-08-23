using BMA_WP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model.RuleSupportItems
{
    public static class PositionList
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
            new BasicItem(12, AppResources.PositionTwelveth), 
            new BasicItem(13, AppResources.PositionThirteenth), 
            new BasicItem(14, AppResources.PositionFourteenth), 
            new BasicItem(15, AppResources.PositionFifteenth),
            new BasicItem(16, AppResources.PositionSixteenth),
            new BasicItem(17, AppResources.PositionSeventeenth), 
            new BasicItem(18, AppResources.PositionEighteenth), 
            new BasicItem(19, AppResources.PositionNineteenth), 
            new BasicItem(20, AppResources.PositionTwentieth), 
            new BasicItem(21, AppResources.PositionTwentyFirst), 
            new BasicItem(22, AppResources.PositionTwentySecond), 
            new BasicItem(23, AppResources.PositionTwentyThird), 
            new BasicItem(24, AppResources.PositionTwentyForth), 
            new BasicItem(25, AppResources.PositionTwentyFifth), 
            new BasicItem(26, AppResources.PositionTwentySixth), 
            new BasicItem(27, AppResources.PositionTwentySeventh), 
            new BasicItem(28, AppResources.PositionTwentyEightth), 
            new BasicItem(29, AppResources.PositionTwentyNineth), 
            new BasicItem(30, AppResources.PositionThirtyeth), 
            new BasicItem(31, AppResources.PositionThirtyFirst), 
        };

        public static List<BasicItem> GetPositions()
        {
            return positions;
        }
    }
}
