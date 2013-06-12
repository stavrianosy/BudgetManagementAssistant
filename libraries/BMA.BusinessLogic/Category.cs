using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BMA.BusinessLogic
{
    public class Category : BaseItem
    {
        public override bool Equals(Object obj)
        {
            Category category = obj as Category;
            if (category == null)
                return false;
            else
                return CategoryId.Equals(category.CategoryId);
        }

        public override int GetHashCode()
        {
            return this.CategoryId.GetHashCode();
        }

        #region Private Members
        string name;
        DateTime fromDate;
        DateTime toDate;
        #endregion

        #region Public Properties
        public int CategoryId { get; set; }

        public string Name { get { return name; } set { name = value; OnPropertyChanged("Name"); } }

        
        /// <summary>
        /// Although this property should be TimeSpan, it is set as DateTime due to WCF serialization issues.
        /// </summary>
        public DateTime FromDate { get { return fromDate; } set { fromDate = value; OnPropertyChanged("FromDate"); } }

        /// <summary>
        /// Although this property should be TimeSpan, it is set as DateTime due to WCF serialization issues.
        /// </summary>
        public DateTime ToDate { get { return toDate; } set { toDate = value; OnPropertyChanged("ToDate"); } }
        #endregion

        #region Constructions
        public Category():this(null)
        {
            
        }
        public Category(User user)
            : base(user)
        {
            //** DONT INSTANTIATE CREATED AND MODIFIED USER WITH EMPTY VALUES **//
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 0, 0);
            ToDate = FromDate.AddHours(1);
        }
        #endregion
    }
}
