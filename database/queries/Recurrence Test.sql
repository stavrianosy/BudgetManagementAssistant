/*

delete from TypeInterval
delete from RecurrenceRuleRulePart where RecurrenceRule_RecurrenceRuleId > 9
delete from RulePartValue
delete from RecurrenceRulePart
delete from RecurrenceRule where RecurrenceRuleId > 9 
delete from RulePart where RulePartId > 19
delete from FieldType where FieldTypeId > 7

*/
select 
RulePart = (select case when count(*) = 19 then '=' else 'NO' end from RulePart),
RecurrenceRuleRulePart = (select case when count(*) = 21 then '=' else 'NO' end from RecurrenceRuleRulePart),
RecurrenceRule = (select case when count(*) = 9 then '=' else 'NO' end from RecurrenceRule)