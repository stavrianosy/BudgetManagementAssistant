using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA.BusinessLogic
{
    public class RecurrenceRulePart
    {
        private RecurrenceRule recurrenceRule;

        public int RecurrenceRulePartId { get; set; }

        public List<RulePartValue> RulePartValueList { get; set; }

        /// <summary>
        /// Only when the assigned value is different with the existing, the RulePartValue in RulePartValueList will be cleared.
        /// </summary>
        public RecurrenceRule RecurrenceRule
        {
            get { return recurrenceRule; }
            set
            {
                if (value == null)
                {
                    recurrenceRule = value;
                    //RulePartValueList = new List<RulePartValue>();
                }
                else
                {
                    if (RecurrenceRule == null || value.Name != RecurrenceRule.Name)
                    {
                        recurrenceRule = value;
                        RulePartValueList = new List<RulePartValue>();
                        if (RecurrenceRule != null && RecurrenceRule.RuleParts != null)
                        {
                            foreach (var item in RecurrenceRule.RuleParts)
                            {
                                string defaultValue = item.FieldType.Type == Const.FieldType.DateInt.ToString() ? 
                                                            DateTime.Now.ToString("yyyyMMdd") : item.FieldType.DefaultValue;
                                
                                var tempRulePartValue = new RulePartValue { RulePart = item, Value = defaultValue};
                                RulePartValueList.Add(tempRulePartValue);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Crazy hack because i had issue with save and the rules applied on setter of RecurrenceRule
        /// Use this method only for saving.
        /// </summary>
        /// <param name="recRule"></param>
        public void SetPrivateRecurrenceRuleForSave(RecurrenceRule recRule)
        {
            recurrenceRule = recRule;
        }
    }
}
