using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class Configuration : BaseItem
    {
        #region Public Properties
        public int ConfigurationId { get; set; }
        public int MaxCategories { get; set; }
        public int MaxTransactionReasons { get; set; }
        public DateTime TypeIntervalLastRun { get; set; }
        #endregion

        #region Constructors
        //parameterless ctor in order to be used in generic as T
        public Configuration()
            : base(null)
        {}
        public Configuration(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **// 
        }

        #endregion
    }
}
