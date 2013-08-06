using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class RecurrenceRuleList : ObservableCollection<RecurrenceRule>, IDataList
    {
        public void AcceptChanges()
        {
            foreach (var item in Items)
                item.HasChanges = false;
        }

        public void PrepareForServiceSerialization()
        {
            var deletedIDs = this.Select((x, i) => new { item = x, index = i }).Where(x => x.item.IsDeleted).ToList();

            foreach (var item in deletedIDs)
                this.RemoveAt(item.index);

            this.AcceptChanges();
        }

        public bool HasItemsWithChanges()
        {
            bool result = false;

            result = this.FirstOrDefault(x => x.HasChanges) != null;

            return result;
        }
    }


    public class RecurrenceRule:BaseItem
    {
        List<RulePart> ruleParts;
        public int RecurrenceRuleId { get; set; }

        public string Name { get; set; }

        public List<RulePart> RuleParts { get { return ruleParts; } set { ruleParts = value; OnPropertyChanged("RuleParts"); } }

        public RecurrenceRule()
            : base(null)
        {

        }
        public RecurrenceRule(User user)
            : base(user)
        {
            RuleParts = new List<RulePart>();
        }
    }
}
