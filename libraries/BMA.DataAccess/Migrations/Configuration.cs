namespace BMA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using BMA.BusinessLogic;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;

    internal sealed class Configuration : DbMigrationsConfiguration<BMA.DataAccess.EntityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BMA.DataAccess.EntityContext context)
        {
            try
            {
                string buildInAdmin = "admin";
                string buildInAdminEmail = "admin@bma.com";
                string buildInAdminPass = "1234";
                string userName = "stavrianosy";

                #region Initial DB Setup
                ////Must add the first user in a more T-Sql way since there are fields in User table that references its self.

                //## User ##//
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [User] ON if not exists(select * from [User] where Username = {1}) BEGIN " +
                    "INSERT INTO [User] (UserId, UserName, Password, Email, Birthdate, FirstName, LastName, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                    "({0}, {1}, {2}, {3}, GETDATE(), {4}, {5}, GETDATE(), GETDATE(), 1, 1, 0) END ", 
                    1, buildInAdmin, buildInAdminPass, buildInAdminEmail, "AdminName", "AdminSurname");

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [User] ON if not exists(select * from [User] where Username = {1}) BEGIN " +
                    "INSERT INTO [User] (UserId, UserName, Password, Email, Birthdate, FirstName, LastName, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                    "({0}, {1}, {2}, {3}, GETDATE(), {4}, {5}, GETDATE(), GETDATE(), 1, 1, 0) END ", 
                    2, "System", "system", "system@system.com", "SysName", "SysSurname");

                //## Category ##//
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [Category] ON if not exists(select * from [Category] where Name = {1}) BEGIN " +
                    "INSERT INTO [Category] (CategoryId, Name, FromDate, ToDate, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                    "({0}, {1}, {2}, {3}, GETDATE(), GETDATE(), 2, 2, 0) END ", 
                    1, "Other", "2000-01-01", "2000-01-01");

                //## TypeTransactionReason ##//
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [TypeTransactionReason] ON if not exists(select * from [TypeTransactionReason] where Name = {1}) BEGIN " +
                    "INSERT INTO [TypeTransactionReason] (TypeTransactionReasonId, Name, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                    "({0}, {1}, GETDATE(), GETDATE(), 2, 2, 0) END ", 
                    1, "Other");

                //## Category - TypeTransactionReason ##//
                context.Database.ExecuteSqlCommand("if not exists(select * from [TypeTransactionReasonCategory]  " +
                     "where TypeTransactionReason_TypeTransactionReasonId = 1 AND Category_CategoryId = 1) BEGIN " +
                     "INSERT INTO [TypeTransactionReasonCategory] (TypeTransactionReason_TypeTransactionReasonId, Category_CategoryId) VALUES ( " +
                "(SELECT CategoryId FROM Category WHERE Name = {0}), (SELECT TypeTransactionReasonId FROM TypeTransactionReason WHERE Name = {1})) END ",
                "Other", "Other");

                //## TypeTransaction ##//
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [TypeTransaction] ON if not exists(select * from [TypeTransaction] where Name = {1}) BEGIN " +
                                    "INSERT INTO [TypeTransaction] (TypeTransactionId, Name, IsIncome, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, {2}, GETDATE(), GETDATE(), 2, 2, 0) END ", 
                                    1, "Income", true);

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [TypeTransaction] ON if not exists(select * from [TypeTransaction] where Name = {1}) BEGIN " +
                                    "INSERT INTO [TypeTransaction] (TypeTransactionId, Name, IsIncome, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, {2}, GETDATE(), GETDATE(), 2, 2, 0) END ", 
                                    2, "Expense", false);

                //## TypeFrequency ##//
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [TypeFrequency] ON if not exists(select * from [TypeFrequency] where Name = {1}) BEGIN " +
                                    "INSERT INTO [TypeFrequency] (TypeFrequencyId, Name, Count, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, {2}, GETDATE(), GETDATE(), 2, 2, 0) END ", 
                                    1, "Hourly", 1);

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [TypeFrequency] ON if not exists(select * from [TypeFrequency] where Name = {1}) BEGIN " +
                                    "INSERT INTO [TypeFrequency] (TypeFrequencyId, Name, Count, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, {2}, GETDATE(), GETDATE(), 2, 2, 0) END ", 
                                    2, "Daily", 24);

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [TypeFrequency] ON if not exists(select * from [TypeFrequency] where Name = {1}) BEGIN " +
                                    "INSERT INTO [TypeFrequency] (TypeFrequencyId, Name, Count, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, {2}, GETDATE(), GETDATE(), 2, 2, 0) END ", 
                                    3, "Weekly", 168);

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [TypeFrequency] ON if not exists(select * from [TypeFrequency] where Name = {1}) BEGIN " +
                                    "INSERT INTO [TypeFrequency] (TypeFrequencyId, Name, Count, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, {2}, GETDATE(), GETDATE(), 2, 2, 0) END ", 
                                    4, "Monthly", 672);

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [TypeFrequency] ON if not exists(select * from [TypeFrequency] where Name = {1}) BEGIN " +
                                    "INSERT INTO [TypeFrequency] (TypeFrequencyId, Name, Count, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, {2}, GETDATE(), GETDATE(), 2, 2, 0) END ", 
                                    5, "Yearly", 8736);

                //## FieldType ##//
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [FieldType] ON if not exists(select * from [FieldType] where Name = {1}) BEGIN " +
                                    "INSERT INTO [FieldType] (FieldTypeId, Name, Type, DefaultValue) VALUES " +
                                    "({0}, {1}, {2}, {3}) END ",
                                    1, "label", Const.FieldType.Label.ToString(), "");

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [FieldType] ON if not exists(select * from [FieldType] where Name = {1}) BEGIN " +
                                    "INSERT INTO [FieldType] (FieldTypeId, Name, Type, DefaultValue) VALUES " +
                                    "({0}, {1}, {2}, {3}) END ",
                                    2, "int", Const.FieldType.Int.ToString(), "1");

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [FieldType] ON if not exists(select * from [FieldType] where Name = {1}) BEGIN " +
                                    "INSERT INTO [FieldType] (FieldTypeId, Name, Type, DefaultValue) VALUES " +
                                    "({0}, {1}, {2}, {3}) END ",
                                    3, "dayNumber", Const.FieldType.DayNum.ToString(), "");

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [FieldType] ON if not exists(select * from [FieldType] where Name = {1}) BEGIN " +
                                    "INSERT INTO [FieldType] (FieldTypeId, Name, Type, DefaultValue) VALUES " +
                                    "({0}, {1}, {2}, {3}) END ",
                                    4, "date", Const.FieldType.DateInt.ToString(), "20000101");

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [FieldType] ON if not exists(select * from [FieldType] where Name = {1}) BEGIN " +
                                    "INSERT INTO [FieldType] (FieldTypeId, Name, Type, DefaultValue) VALUES " +
                                    "({0}, {1}, {2}, {3}) END ",
                                    5, "truefalse", Const.FieldType.Bit.ToString(), "False");

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [FieldType] ON if not exists(select * from [FieldType] where Name = {1}) BEGIN " +
                                    "INSERT INTO [FieldType] (FieldTypeId, Name, Type, DefaultValue) VALUES " +
                                    "({0}, {1}, {2}, {3}) END ",
                                    6, "text", Const.FieldType.String.ToString(), "");

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [FieldType] ON if not exists(select * from [FieldType] where Name = {1}) BEGIN " +
                                    "INSERT INTO [FieldType] (FieldTypeId, Name, Type, DefaultValue) VALUES " +
                                    "({0}, {1}, {2}, {3}) END ",
                                    7, "position", Const.FieldType.Position.ToString(), "1");


                //## RulePart ##//
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    1, Const.RuleField.RangeStartDate.ToString(), Const.FieldType.DateInt.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    2, Const.RuleField.RangeNoEndDate.ToString(), Const.FieldType.Label.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    3, Const.RuleField.RangeTotalOcurrences.ToString(), Const.FieldType.Int.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    4, Const.RuleField.RangeEndBy.ToString(), Const.FieldType.DateInt.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    5, Const.RuleField.DailyEveryDay.ToString(), Const.FieldType.Int.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    6, Const.RuleField.DailyOnlyWeekdays.ToString(), Const.FieldType.Bit.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    7, Const.RuleField.WeeklyEveryWeek.ToString(), Const.FieldType.Int.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    8, Const.RuleField.WeeklyDayName.ToString(), Const.FieldType.String.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    9, Const.RuleField.MonthlyDayNumber.ToString(), Const.FieldType.DayNum.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    10, Const.RuleField.MonthlyEveryMonth.ToString(), Const.FieldType.Int.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    11, Const.RuleField.MonthlyCountOfWeekDay.ToString(), Const.FieldType.Position.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    12, Const.RuleField.MonthlyDayName.ToString(), Const.FieldType.DayNum.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    13, Const.RuleField.MonthlyCountOfMonth.ToString(), Const.FieldType.Int.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    14, Const.RuleField.YearlyEveryYear.ToString(), Const.FieldType.Int.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    15, Const.RuleField.YearlyOnDayPos.ToString(), Const.FieldType.Position.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    16, Const.RuleField.YearlyMonthName.ToString(), Const.FieldType.String.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    17, Const.RuleField.YearlyPositions.ToString(), Const.FieldType.Position.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    18, Const.RuleField.YearlyDayName.ToString(), Const.FieldType.String.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RulePart] ON if not exists(select * from [RulePart] where FieldName = {1}) BEGIN " +
                                    "INSERT INTO [RulePart] (RulePartId, FieldName, FieldType_FieldTypeId) VALUES " +
                                    "({0}, {1}, (SELECT FieldTypeId FROM FieldType WHERE Type = {2})) END ",
                                    19, Const.RuleField.YearlyMonthNameSec.ToString(), Const.FieldType.String.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RecurrenceRule] ON if not exists(select * from [RecurrenceRule] where Name = {1}) BEGIN " +
                                    "INSERT INTO [RecurrenceRule] (RecurrenceRuleId, Name, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, GETDATE(), GETDATE(), 2, 2, 0) END ",
                                    1, Const.Rule.RuleRangeNoEndDate.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RecurrenceRule] ON if not exists(select * from [RecurrenceRule] where Name = {1}) BEGIN " +
                                    "INSERT INTO [RecurrenceRule] (RecurrenceRuleId, Name, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, GETDATE(), GETDATE(), 2, 2, 0) END ",
                                    2, Const.Rule.RuleRangeTotalOcurrences.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RecurrenceRule] ON if not exists(select * from [RecurrenceRule] where Name = {1}) BEGIN " +
                                    "INSERT INTO [RecurrenceRule] (RecurrenceRuleId, Name, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, GETDATE(), GETDATE(), 2, 2, 0) END ",
                                    3, Const.Rule.RuleRangeEndBy.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RecurrenceRule] ON if not exists(select * from [RecurrenceRule] where Name = {1}) BEGIN " +
                                    "INSERT INTO [RecurrenceRule] (RecurrenceRuleId, Name, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, GETDATE(), GETDATE(), 2, 2, 0) END ",
                                    4, Const.Rule.RuleDailyEveryDays.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RecurrenceRule] ON if not exists(select * from [RecurrenceRule] where Name = {1}) BEGIN " +
                                    "INSERT INTO [RecurrenceRule] (RecurrenceRuleId, Name, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, GETDATE(), GETDATE(), 2, 2, 0) END ",
                                    5, Const.Rule.RuleWeeklyEveryWeek.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RecurrenceRule] ON if not exists(select * from [RecurrenceRule] where Name = {1}) BEGIN " +
                                    "INSERT INTO [RecurrenceRule] (RecurrenceRuleId, Name, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, GETDATE(), GETDATE(), 2, 2, 0) END ",
                                    6, Const.Rule.RuleMonthlyDayNum.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RecurrenceRule] ON if not exists(select * from [RecurrenceRule] where Name = {1}) BEGIN " +
                                    "INSERT INTO [RecurrenceRule] (RecurrenceRuleId, Name, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, GETDATE(), GETDATE(), 2, 2, 0) END ",
                                    7, Const.Rule.RuleMonthlyPrecise.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RecurrenceRule] ON if not exists(select * from [RecurrenceRule] where Name = {1}) BEGIN " +
                                    "INSERT INTO [RecurrenceRule] (RecurrenceRuleId, Name, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, GETDATE(), GETDATE(), 2, 2, 0) END ",
                                    8, Const.Rule.RuleYearlyOnMonth.ToString());

                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [RecurrenceRule] ON if not exists(select * from [RecurrenceRule] where Name = {1}) BEGIN " +
                                    "INSERT INTO [RecurrenceRule] (RecurrenceRuleId, Name, ModifiedDate, CreatedDate, ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                                    "({0}, {1}, GETDATE(), GETDATE(), 2, 2, 0) END ",
                                    9, Const.Rule.RuleYearlyOnTheWeekDay.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleRangeNoEndDate.ToString(), Const.RuleField.RangeStartDate.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleRangeTotalOcurrences.ToString(), Const.RuleField.RangeTotalOcurrences.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleRangeTotalOcurrences.ToString(), Const.RuleField.RangeStartDate.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleRangeEndBy.ToString(), Const.RuleField.RangeEndBy.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleRangeEndBy.ToString(), Const.RuleField.RangeStartDate.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleDailyEveryDays.ToString(), Const.RuleField.DailyEveryDay.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleDailyEveryDays.ToString(), Const.RuleField.DailyOnlyWeekdays.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleWeeklyEveryWeek.ToString(), Const.RuleField.WeeklyEveryWeek.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleWeeklyEveryWeek.ToString(), Const.RuleField.WeeklyDayName.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleMonthlyDayNum.ToString(), Const.RuleField.MonthlyDayNumber.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleMonthlyDayNum.ToString(), Const.RuleField.MonthlyEveryMonth.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleMonthlyPrecise.ToString(), Const.RuleField.MonthlyCountOfWeekDay.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleMonthlyPrecise.ToString(), Const.RuleField.MonthlyDayName.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleMonthlyPrecise.ToString(), Const.RuleField.MonthlyCountOfMonth.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleYearlyOnMonth.ToString(), Const.RuleField.YearlyEveryYear.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleYearlyOnMonth.ToString(), Const.RuleField.YearlyOnDayPos.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleYearlyOnMonth.ToString(), Const.RuleField.YearlyMonthName.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleYearlyOnTheWeekDay.ToString(), Const.RuleField.YearlyEveryYear.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleYearlyOnTheWeekDay.ToString(), Const.RuleField.YearlyPositions.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleYearlyOnTheWeekDay.ToString(), Const.RuleField.YearlyDayName.ToString());

                context.Database.ExecuteSqlCommand("INSERT INTO [RecurrenceRuleRulePart] (RecurrenceRule_RecurrenceRuleId, RulePart_RulePartId) VALUES " +
                                    "((SELECT RecurrenceRuleId FROM RecurrenceRule WHERE Name = {0}), (SELECT RulePartId FROM RulePart WHERE FieldName = {1})) ",
                                    Const.Rule.RuleYearlyOnTheWeekDay.ToString(), Const.RuleField.YearlyMonthNameSec.ToString());


                
                #endregion

                #region aaaaa

                //#region User

                ////** find the way to explicitely set the ID of the autonumber fields
                //context.User.AddOrUpdate(u => u.UserName, new User
                //{
                //    UserName = userName,
                //    Email = buildInAdminEmail,
                //    Birthdate = new DateTime(1979, 11, 5),
                //    FirstName = "Yiannis",
                //    LastName = "Stavrianos",
                //    CreatedDate = DateTime.Now,
                //    CreatedUser = context.User.Single(u => u.UserName == buildInAdmin),
                //    ModifiedDate = DateTime.Now,
                //    ModifiedUser = context.User.Single(u => u.UserName == buildInAdmin),
                //});
                //#endregion

                //#region Category
                //context.Category.AddOrUpdate(c => c.Name,
                //                            new Category(context.User.Local.Single(u => u.UserName == userName))
                //                            {
                //                                Name = "Work",
                //                                FromDate = new DateTime(2000, 1, 1, 8, 0, 0),
                //                                ToDate = new DateTime(2000, 1, 1, 17, 0, 0),
                //                            },
                //                            new Category(context.User.Local.Single(u => u.UserName == userName))
                //                            {
                //                                Name = "Other",
                //                                FromDate = new DateTime(2000, 1, 1, 0, 0, 0),
                //                                ToDate = new DateTime(2000, 1, 1, 0, 0, 0),
                //                            },
                //                            new Category(context.User.Local.Single(u => u.UserName == userName))
                //                            {
                //                                Name = "Restaurant",
                //                                FromDate = new DateTime(2000, 1, 1, 20, 0, 0),
                //                                ToDate = new DateTime(2000, 1, 1, 22, 0, 0),
                //                            },
                //                            new Category(context.User.Local.Single(u => u.UserName == userName))
                //                            {
                //                                Name = "Coffee shop",
                //                                FromDate = new DateTime(2000, 1, 1, 19, 0, 0),
                //                                ToDate = new DateTime(2000, 1, 1, 23, 0, 0),
                //                            },
                //                            new Category(context.User.Local.Single(u => u.UserName == userName))
                //                            {
                //                                Name = "Supermarket",
                //                                FromDate = new DateTime(2000, 1, 1, 17, 0, 0),
                //                                ToDate = new DateTime(2000, 1, 1, 19, 0, 0),
                //                            },
                //                            new Category(context.User.Local.Single(u => u.UserName == userName))
                //                            {
                //                                Name = "Entertainment",
                //                                FromDate = new DateTime(2000, 1, 1, 19, 0, 0),
                //                                ToDate = new DateTime(2000, 1, 1, 22, 0, 0),
                //                            },
                //                            new Category(context.User.Local.Single(u => u.UserName == userName))
                //                            {
                //                                Name = "Club",
                //                                FromDate = new DateTime(2000, 1, 1, 1, 0, 0),
                //                                ToDate = new DateTime(2000, 1, 1, 4, 0, 0),
                //                            });

                //#endregion

                //#region TypeSavingsDencity
                //context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                //    new TypeSavingsDencity(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        TypeSavingsDencityId = 1,
                //        Name = "Daily",
                //    });

                //context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                //    new TypeSavingsDencity(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        TypeSavingsDencityId = 2,
                //        Name = "Weekly",
                //    });

                //context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                //    new TypeSavingsDencity(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        TypeSavingsDencityId = 3,
                //        Name = "Monthly",
                //    });

                //context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                //    new TypeSavingsDencity(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        TypeSavingsDencityId = 4,
                //        Name = "Yearly",
                //    });
                //#endregion

                //#region TransactionReason
                //context.TransactionReason.AddOrUpdate(e => e.Name,
                //    new TypeTransactionReason(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        Name = "Lunch",
                //    },
                //    new TypeTransactionReason(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        Name = "Salary",
                //    },
                //    new TypeTransactionReason(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        Name = "Dinner",
                //    },
                //    new TypeTransactionReason(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        Name = "Futsal",
                //    }, new TypeTransactionReason(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        Name = "Other",
                //    });
                //#endregion

                //#region TypeTransaction
                //context.TypeTransaction.AddOrUpdate(t => t.Name,
                //    new TypeTransaction(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        Name = "Income",
                //    },
                //    new TypeTransaction(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        Name = "Expense",
                //    }
                //);
                //#endregion

                //#region Budget
                //context.Budget.AddOrUpdate(b => b.Name,
                //    new Budget(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        Amount = 100,
                //        BudgetId = 1,
                //        FromDate = new DateTime(2013, 1, 1),
                //        ToDate = new DateTime(2013, 1, 28),
                //        IncludeInstallments = false,
                //        Name = "Budget 1"
                //    },
                //    new Budget(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        Amount = 250,
                //        BudgetId = 2,
                //        FromDate = new DateTime(2013, 2, 7),
                //        ToDate = new DateTime(2013, 2, 20),
                //        IncludeInstallments = false,
                //        Name = "Budget 2"
                //    }
                //    );
                //#endregion

                //#region Budget Thrushold
                //context.BudgetThreshold.AddOrUpdate(bt => bt.BudgetThresholdId,
                //    new BudgetThreshold(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        BudgetThresholdId = 1,
                //        Amount = 10d,
                //    }
                //    );
                //#endregion

                //#region Frequency
                //context.TypeFrequency.AddOrUpdate(f => f.Name,
                //    new TypeFrequency(context.User.Local.Single(u => u.UserName == userName))
                //{
                //    Name = "Hourly",
                //    Count = 1,
                //});
                //context.TypeFrequency.AddOrUpdate(f => f.Name,
                //    new TypeFrequency(context.User.Local.Single(u => u.UserName == userName))
                //{
                //    Name = "Daily",
                //    Count = 24,
                //});
                //context.TypeFrequency.AddOrUpdate(f => f.Name,
                //    new TypeFrequency(context.User.Local.Single(u => u.UserName == userName))
                //{
                //    Name = "Weekly",
                //    Count = 168,
                //});
                //context.TypeFrequency.AddOrUpdate(f => f.Name,
                //    new TypeFrequency(context.User.Local.Single(u => u.UserName == userName))
                //{
                //    Name = "Monthly",
                //    Count = 672,
                //});
                //context.TypeFrequency.AddOrUpdate(f => f.Name,
                //    new TypeFrequency(context.User.Local.Single(u => u.UserName == userName))
                //{
                //    Name = "Yearly",
                //    Count = 8736,
                //});
                //#endregion

                //#region Transaction
                ////## No need to seed transactions anymore
                ////context.Transaction.AddOrUpdate(t => t.NameOfPlace,
                ////    new Transaction(context.User.Local.Single(u => u.UserName == userName))
                ////    {
                ////        TransactionId = 1,
                ////        Amount = 6.5d,
                ////        NameOfPlace = "Four Seassons",
                ////        TipAmount = 0d,
                ////        Category = context.Category.Local.Single(c => c.Name == "Restaurant"),
                ////        TransactionReasonType = context.TransactionReason.Local.Single(tr => tr.Name == "Lunch"),
                ////        TransactionType = context.TypeTransaction.Local.Single(c => c.Name == "Expense"),
                ////        Comments = "Juice and Perrier"
                ////    },
                ////    new Transaction(context.User.Local.Single(u => u.UserName == userName))
                ////    {
                ////        TransactionId = 2,
                ////        Amount = 475d,
                ////        NameOfPlace = "BBD",
                ////        TipAmount = 0d,
                ////        Category = context.Category.Local.Single(c => c.Name == "Work"),
                ////        TransactionReasonType = context.TransactionReason.Local.Single(tr => tr.Name == "Salary"),
                ////        TransactionType = context.TypeTransaction.Local.Single(c => c.Name == "Income"),
                ////        Comments = "Allowances"
                ////    },
                ////    new Transaction(context.User.Local.Single(u => u.UserName == userName))
                ////    {
                ////        TransactionId = 3,
                ////        Amount = 5d,
                ////        NameOfPlace = "Strovolos Municipality",
                ////        TipAmount = 0d,
                ////        Category = context.Category.Local.Single(c => c.Name == "Entertainment"),
                ////        TransactionReasonType = context.TransactionReason.Local.Single(tr => tr.Name == "Futsal"),
                ////        TransactionType = context.TypeTransaction.Local.Single(c => c.Name == "Expense"),
                ////        Comments = "Statndard fee"
                ////    });
                //#endregion

                //#region FieldType
                //context.FieldType.AddOrUpdate(x => x.Name,
                //    new FieldType { Name = "label", Type = Const.FieldType.Label.ToString(), DefaultValue = "" },
                //    new FieldType { Name = "int", Type = Const.FieldType.Int.ToString(), DefaultValue = "1" },
                //    new FieldType { Name = "dayNumber", Type = Const.FieldType.DayNum.ToString(), DefaultValue = "1" },
                //    new FieldType { Name = "date", Type = Const.FieldType.DateInt.ToString(), DefaultValue = "20000101" },
                //    new FieldType { Name = "truefalse", Type = Const.FieldType.Bit.ToString(), DefaultValue = "False" },
                //    new FieldType { Name = "text", Type = Const.FieldType.String.ToString(), DefaultValue = "" },
                //    new FieldType { Name = "position", Type = Const.FieldType.Position.ToString(), DefaultValue = "1" }
                //);
                //#endregion

                //#region RulePart
                //context.RulePart.AddOrUpdate(x => x.FieldName,
                //    new RulePart { FieldName = Const.RuleField.RangeStartDate.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.DateInt.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.RangeNoEndDate.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Label.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.RangeTotalOcurrences.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.RangeEndBy.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.DateInt.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.DailyEveryDay.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.DailyOnlyWeekdays.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Bit.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.WeeklyEveryWeek.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.WeeklyDayName.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.String.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.MonthlyDayNumber.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.DayNum.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.MonthlyEveryMonth.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.MonthlyCountOfWeekDay.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Position.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.MonthlyDayName.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.DayNum.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.MonthlyCountOfMonth.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.YearlyEveryYear.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.YearlyOnDayPos.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Position.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.YearlyMonthName.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.String.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.YearlyPositions.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Position.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.YearlyDayName.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.String.ToString()) },
                //    new RulePart { FieldName = Const.RuleField.YearlyMonthNameSec.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.String.ToString()) }
                //);
                //#endregion

                //#region RecurrenceRule
                //context.RecurrenceRule.AddOrUpdate(t => t.Name,
                //    new RecurrenceRule
                //    {
                //        Name = Const.Rule.RuleRangeNoEndDate.ToString(),
                //        RuleParts = new List<RulePart>{
                //            context.RulePart.Local.Single(x=>x.FieldName==Const.RuleField.RangeStartDate.ToString())}
                //    },
                //    new RecurrenceRule
                //    {
                //        Name = Const.Rule.RuleRangeTotalOcurrences.ToString(),
                //        RuleParts = new List<RulePart>{
                //            context.RulePart.Local.Single(x=>x.FieldName==Const.RuleField.RangeTotalOcurrences.ToString()),
                //            context.RulePart.Local.Single(x=>x.FieldName==Const.RuleField.RangeStartDate.ToString())}
                //    },
                //    new RecurrenceRule
                //    {
                //        Name = Const.Rule.RuleRangeEndBy.ToString(),
                //        RuleParts = new List<RulePart> { 
                //            context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.RangeEndBy.ToString()) ,
                //            context.RulePart.Local.Single(x=>x.FieldName==Const.RuleField.RangeStartDate.ToString())}
                //    },
                //    new RecurrenceRule
                //    {
                //        Name = Const.Rule.RuleDailyEveryDays.ToString(),
                //        RuleParts = new List<RulePart>{ 
                //            context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.DailyEveryDay.ToString()),
                //            context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.DailyOnlyWeekdays.ToString()) }
                //    },
                //    new RecurrenceRule
                //    {
                //        Name = Const.Rule.RuleWeeklyEveryWeek.ToString(),
                //        RuleParts = new List<RulePart>{ 
                //                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.WeeklyEveryWeek.ToString()),
                //                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.WeeklyDayName.ToString())}
                //    },
                //    new RecurrenceRule
                //    {
                //        Name = Const.Rule.RuleMonthlyDayNum.ToString(),
                //        RuleParts = new List<RulePart>{ 
                //                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.MonthlyDayNumber.ToString()),
                //                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.MonthlyEveryMonth.ToString())}
                //    },
                //    new RecurrenceRule
                //    {
                //        Name = Const.Rule.RuleMonthlyPrecise.ToString(),
                //        RuleParts = new List<RulePart>{ 
                //                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.MonthlyCountOfWeekDay.ToString()),
                //                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.MonthlyDayName.ToString()),
                //                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.MonthlyCountOfMonth.ToString())}
                //    },
                //    new RecurrenceRule
                //    {
                //        Name = Const.Rule.RuleYearlyOnMonth.ToString(),
                //        RuleParts = new List<RulePart>{ 
                //                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyEveryYear.ToString()),
                //                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyOnDayPos.ToString()),
                //                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyMonthName.ToString())}
                //    },
                //    new RecurrenceRule
                //    {
                //        Name = Const.Rule.RuleYearlyOnTheWeekDay.ToString(),
                //        RuleParts = new List<RulePart>{ 
                //                      context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyEveryYear.ToString()),
                //                      context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyPositions.ToString()),
                //                      context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyDayName.ToString()),
                //                      context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyMonthNameSec.ToString())}
                //    }

                //);
                //#endregion

                //#region Target
                //////context.Target.AddOrUpdate(t => t.EndDate, new Target { });
                //#endregion

                #endregion

                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [User] OFF");
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Category OFF");
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT TransactionReason OFF");
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT TypeTransaction OFF");
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [Transaction] OFF");
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
        }
    }
}
