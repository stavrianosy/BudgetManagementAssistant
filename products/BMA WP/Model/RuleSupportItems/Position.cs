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
        static List<BasicItem> positions = new List<BasicItem> { new BasicItem(1, AppResources.First), new BasicItem(2, "2nd Second"), new BasicItem(3, "3rd Third"), 
                            new BasicItem(4, "4th Forth"), new BasicItem(5, "5th Fifth"), new BasicItem(6, "6th Sisth"), new BasicItem(7, "7th Seventh"), 
                            new BasicItem(8, "8th Eightth"), new BasicItem(9, "9th Nineth"), new BasicItem(10, "10th Tenth"), new BasicItem(11, "11th Eleventh"), 
                            new BasicItem(12, "12th Twelveth"), new BasicItem(13, "13th Thirteenth"), new BasicItem(14, "14th Fourteenth"), new BasicItem(15, "15th Fifteenth"),
                            new BasicItem(16, "16th Sisteenth"),new BasicItem(17, "17th Seventeenth"), new BasicItem(18, "18th Eighteenth"), new BasicItem(19, "19th Nineteenth"), 
                            new BasicItem(20, "20th Twentieth"), new BasicItem(21, "21st Twenty first"), new BasicItem(22, "22nd Twenty secondth"), new BasicItem(23, "23rd Twenty third"), 
                            new BasicItem(24, "24th Twenty forth"), new BasicItem(25, "25th Twenty fifth"), new BasicItem(26, "26th Twenty sixth"), new BasicItem(27, "27th Twenty seventh"), 
                            new BasicItem(28, "28th Twenty eightth"), new BasicItem(29, "29th Twenty nineth"), new BasicItem(30, "30th Thirtyeth"), new BasicItem(31, "31st Thirty first"), 
        };

        public static List<BasicItem> GetPositions()
        {
            return positions;
        }
    }
}
