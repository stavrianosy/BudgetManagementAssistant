using BMA.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMA_WP.Model
{
    public static class InitialData
    {
        public static CategoryList InitializeCategories()
        {
            var user = new User { UserId = 2 }; 
            var result = new CategoryList { 
            new Category(user){Name="Other",CategoryId=1 }};

            return result;
        }


        public static TypeTransactionList InitializeTypeTransactions()
        {
            var user = new User{UserId = 2};
            var result = new TypeTransactionList { 
            new TypeTransaction(user){Name="Income",TypeTransactionId=1, },
            new TypeTransaction(user){Name="Expense",TypeTransactionId=2 }};

            return result;
        }

        public static TypeTransactionReasonList InitializeTypeTransactionReasons()
        {
            var user = new User { UserId = 2 };
            var result = new TypeTransactionReasonList { 
            new TypeTransactionReason(user){Name="Other", TypeTransactionReasonId=1 }};

            return result;
        }

        public static TypeFrequencyList InitializeTypeFrequencies()
        {
            var user = new User { UserId = 2 };
            var result = new TypeFrequencyList { 
            new TypeFrequency(user){TypeFrequencyId=1 , Name="Hourly", Count=1},
            new TypeFrequency(user){TypeFrequencyId=2 , Name="Daily", Count=24},
            new TypeFrequency(user){TypeFrequencyId=3 , Name="Weekly", Count=168},
            new TypeFrequency(user){TypeFrequencyId=4 , Name="Monthly", Count=672},
            new TypeFrequency(user){TypeFrequencyId=5 , Name="Yearly", Count=8736}};

            return result;
        }

        public static FieldTypeList InitializeFieldType()
        {
            var user = new User { UserId = 2 };
            var result = new FieldTypeList { 
            new FieldType(){FieldTypeId=1 , Name="label", Type=Const.FieldType.Label.ToString(), DefaultValue=""},
            new FieldType(){FieldTypeId=2 , Name="int", Type=Const.FieldType.Int.ToString(), DefaultValue="1"},
            new FieldType(){FieldTypeId=3 , Name="dayNumber", Type=Const.FieldType.DayNum.ToString(), DefaultValue=""},
            new FieldType(){FieldTypeId=4 , Name="date", Type=Const.FieldType.DateInt.ToString(), DefaultValue="20000101"},
            new FieldType(){FieldTypeId=5 , Name="truefalse", Type=Const.FieldType.Bit.ToString(), DefaultValue="true"},
            new FieldType(){FieldTypeId=6 , Name="text", Type=Const.FieldType.String.ToString(), DefaultValue=""},
            new FieldType(){FieldTypeId=7 , Name="position", Type=Const.FieldType.Position.ToString(), DefaultValue="1"}};

            return result;
        }
        
        public static RulePartList InitializeRulePart()
        {
            var user = new User { UserId = 2 };
            var result = new RulePartList
            {
                new RulePart{RulePartId=1, FieldName=Const.RuleField.RangeStartDate.ToString(), FieldType=InitializeFieldType().FirstOrDefault(x=>x.FieldTypeId == 4)},
                    new RulePart { RulePartId=2, FieldName = Const.RuleField.RangeNoEndDate.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Label.ToString()) },
                    new RulePart { RulePartId=3, FieldName = Const.RuleField.RangeTotalOcurrences.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { RulePartId=4, FieldName = Const.RuleField.RangeEndBy.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.DateInt.ToString()) },
                    new RulePart { RulePartId=5, FieldName = Const.RuleField.DailyEveryDay.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { RulePartId=6, FieldName = Const.RuleField.DailyOnlyWeekdays.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Bit.ToString()) },
                    new RulePart { RulePartId=7, FieldName = Const.RuleField.WeeklyEveryWeek.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { RulePartId=8, FieldName = Const.RuleField.WeeklyDayName.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.String.ToString()) },
                    new RulePart { RulePartId=9, FieldName = Const.RuleField.MonthlyDayNumber.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.DayNum.ToString()) },
                    new RulePart { RulePartId=10, FieldName = Const.RuleField.MonthlyEveryMonth.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { RulePartId=11, FieldName = Const.RuleField.MonthlyCountOfWeekDay.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Position.ToString()) },
                    new RulePart { RulePartId=12, FieldName = Const.RuleField.MonthlyDayName.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.DayNum.ToString()) },
                    new RulePart { RulePartId=13, FieldName = Const.RuleField.MonthlyCountOfMonth.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { RulePartId=14, FieldName = Const.RuleField.YearlyEveryYear.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { RulePartId=15, FieldName = Const.RuleField.YearlyOnDayPos.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Position.ToString()) },
                    new RulePart { RulePartId=16, FieldName = Const.RuleField.YearlyMonthName.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.String.ToString()) },
                    new RulePart { RulePartId=17, FieldName = Const.RuleField.YearlyPositions.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.Position.ToString()) },
                    new RulePart { RulePartId=18, FieldName = Const.RuleField.YearlyDayName.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.String.ToString()) },
                    new RulePart { RulePartId=19, FieldName = Const.RuleField.YearlyMonthNameSec.ToString(), FieldType = InitializeFieldType().FirstOrDefault(x => x.Type == Const.FieldType.String.ToString()) }
            };

            return result;
        }


        public static RecurrenceRuleList InitializeRecurrenceRule()
        {
            var user = new User { UserId = 2 };
            var result = new RecurrenceRuleList
            {
                new RecurrenceRule
                    {
                        Name = Const.Rule.RuleRangeNoEndDate.ToString(),
                        RuleParts = new List<RulePart>{
                            InitializeRulePart().FirstOrDefault(x=>x.FieldName==Const.RuleField.RangeStartDate.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleRangeTotalOcurrences.ToString(),
                        RuleParts = new List<RulePart>{
                            InitializeRulePart().FirstOrDefault(x=>x.FieldName==Const.RuleField.RangeTotalOcurrences.ToString()),
                            InitializeRulePart().FirstOrDefault(x=>x.FieldName==Const.RuleField.RangeStartDate.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleRangeEndBy.ToString(),
                        RuleParts = new List<RulePart> { 
                            InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.RangeEndBy.ToString()) ,
                            InitializeRulePart().FirstOrDefault(x=>x.FieldName==Const.RuleField.RangeStartDate.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleDailyEveryDays.ToString(),
                        RuleParts = new List<RulePart>{ 
                            InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.DailyEveryDay.ToString()),
                            InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.DailyOnlyWeekdays.ToString()) }
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleWeeklyEveryWeek.ToString(),
                        RuleParts = new List<RulePart>{ 
                                        InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.WeeklyEveryWeek.ToString()),
                                        InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.WeeklyDayName.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleMonthlyDayNum.ToString(),
                        RuleParts = new List<RulePart>{ 
                                        InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.MonthlyDayNumber.ToString()),
                                        InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.MonthlyEveryMonth.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleMonthlyPrecise.ToString(),
                        RuleParts = new List<RulePart>{ 
                                        InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.MonthlyCountOfWeekDay.ToString()),
                                        InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.MonthlyDayName.ToString()),
                                        InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.MonthlyCountOfMonth.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleYearlyOnMonth.ToString(),
                        RuleParts = new List<RulePart>{ 
                                        InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.YearlyEveryYear.ToString()),
                                        InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.YearlyOnDayPos.ToString()),
                                        InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.YearlyMonthName.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleYearlyOnTheWeekDay.ToString(),
                        RuleParts = new List<RulePart>{ 
                                      InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.YearlyEveryYear.ToString()),
                                      InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.YearlyPositions.ToString()),
                                      InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.YearlyDayName.ToString()),
                                      InitializeRulePart().FirstOrDefault(x => x.FieldName==Const.RuleField.YearlyMonthNameSec.ToString())}
                    }
            };

            return result;
        }
        public static string GenerateSqlScript()
        {
            var result = new StringBuilder();

            string buildInAdmin = "admin";
            string buildInAdminEmail = "admin@bma.com";
            string buildInAdminPass = "1234";
            string userName = "stavrianosy";


            result.Append(string.Format("SET IDENTITY_INSERT [User] ON if not exists(select * from [User] where Username = {1}) BEGIN " +
                    "INSERT INTO [User] (UserId, UserName, Password, Email, Birthdate, FirstName, LastName, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                    "({0}, {1}, {2}, {3}, GETDATE(), {4}, {5}, GETDATE(), GETDATE(), 1, 1, 0) END ",
                    1, buildInAdmin, buildInAdminPass, buildInAdminEmail, "AdminName", "AdminSurname"));

            result.Append(string.Format("SET IDENTITY_INSERT [User] ON if not exists(select * from [User] where Username = {1}) BEGIN " +
                    "INSERT INTO [User] (UserId, UserName, Password, Email, Birthdate, FirstName, LastName, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                    "({0}, {1}, {2}, {3}, GETDATE(), {4}, {5}, GETDATE(), GETDATE(), 1, 1, 0) END ",
                    2, "System", "system", "system@system.com", "SysName", "SysSurname"));

            return result.ToString();
        }
    }
}
