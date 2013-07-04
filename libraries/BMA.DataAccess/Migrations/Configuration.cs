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

                //  This method will be called after migrating to the latest version.
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [User] ON");
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Category ON;");
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT TransactionReason ON");
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT TypeTransaction ON");
                //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [Transaction] ON");
                ///context.Database.ExecuteSqlCommand("insert into category (categoryId) values (1)");
                //context.User.SqlQuery("INSERT INTO [User] (UserId, UserName, ModifiedDate, CreatedDate,ModifiedUser_UserId, CreatedUser_UserId) VALUES (1, 'stavrianosy', GETDATE(), GETDATE(), 1, 1)");


                #region User
                ////Must add the first user in a more T-Sql way since there are fields in User table that references its self.
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [User] ON " +
                    "if not exists(select * from [User] where Username = {0}) BEGIN " +
                    "INSERT INTO [User] (UserId, UserName, Password, Email, ModifiedDate, CreatedDate,ModifiedUser_UserId, CreatedUser_UserId, IsDeleted) VALUES " +
                    "(1, {0}, {1}, {2}, GETDATE(), GETDATE(), 1, 1, 0) " +
                    "END " +
                    "SET IDENTITY_INSERT [User] OFF", buildInAdmin, buildInAdminPass, buildInAdminEmail);

                context.User.AddOrUpdate(u => u.UserName, new User
                {
                    UserId = 2,
                    UserName = userName,
                    Email = buildInAdminEmail,
                    CreatedDate = DateTime.Now,
                    CreatedUser = context.User.Single(u => u.UserName == buildInAdmin),
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = context.User.Single(u => u.UserName == buildInAdmin),
                });
                #endregion

                #region Category
                context.Category.AddOrUpdate(c => c.Name,
                                            new Category(context.User.Local.Single(u => u.UserName == userName))
                                            {
                                                Name = "Work",
                                                FromDate = new DateTime(2000, 1, 1, 8, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 17, 0, 0),
                                            },
                                            new Category(context.User.Local.Single(u => u.UserName == userName))
                                            {
                                                Name = "Restaurant",
                                                FromDate = new DateTime(2000, 1, 1, 20, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 22, 0, 0),
                                            },
                                            new Category(context.User.Local.Single(u => u.UserName == userName))
                                            {
                                                Name = "Coffee shop",
                                                FromDate = new DateTime(2000, 1, 1, 19, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 23, 0, 0),
                                            },
                                            new Category(context.User.Local.Single(u => u.UserName == userName))
                                            {
                                                Name = "Supermarket",
                                                FromDate = new DateTime(2000, 1, 1, 17, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 19, 0, 0),
                                            },
                                            new Category(context.User.Local.Single(u => u.UserName == userName))
                                            {
                                                Name = "Entertainment",
                                                FromDate = new DateTime(2000, 1, 1, 19, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 22, 0, 0),
                                            },
                                            new Category(context.User.Local.Single(u => u.UserName == userName))
                                            {
                                                Name = "Club",
                                                FromDate = new DateTime(2000, 1, 1, 1, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 4, 0, 0),
                                            },
                                            new Category(context.User.Local.Single(u => u.UserName == userName))
                                            {
                                                Name = "Other",
                                                FromDate = new DateTime(2000, 1, 1, 0, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 0, 0, 0),
                                            });

                #endregion

                #region TypeSavingsDencity
                context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                    new TypeSavingsDencity(context.User.Local.Single(u => u.UserName == userName))
                    {
                        TypeSavingsDencityId = 1,
                        Name = "Daily",
                    });

                context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                    new TypeSavingsDencity(context.User.Local.Single(u => u.UserName == userName))
                    {
                        TypeSavingsDencityId = 2,
                        Name = "Weekly",
                    });

                context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                    new TypeSavingsDencity(context.User.Local.Single(u => u.UserName == userName))
                    {
                        TypeSavingsDencityId = 3,
                        Name = "Monthly",
                    });

                context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                    new TypeSavingsDencity(context.User.Local.Single(u => u.UserName == userName))
                    {
                        TypeSavingsDencityId = 4,
                        Name = "Yearly",
                    });
                #endregion

                #region TransactionReason
                context.TransactionReason.AddOrUpdate(e => e.Name,
                    new TypeTransactionReason(context.User.Local.Single(u => u.UserName == userName))
                    {
                        Name = "Lunch",
                    },
                    new TypeTransactionReason(context.User.Local.Single(u => u.UserName == userName))
                    {
                        Name = "Salary",
                    },
                    new TypeTransactionReason(context.User.Local.Single(u => u.UserName == userName))
                    {
                        Name = "Dinner",
                    },
                    new TypeTransactionReason(context.User.Local.Single(u => u.UserName == userName))
                    {
                        Name = "Futsal",
                    }, new TypeTransactionReason(context.User.Local.Single(u => u.UserName == userName))
                    {
                        Name = "Other",
                    });
                #endregion

                #region TypeTransaction
                context.TypeTransaction.AddOrUpdate(t => t.Name,
                    new TypeTransaction(context.User.Local.Single(u => u.UserName == userName))
                    {
                        Name = "Income",
                    },
                    new TypeTransaction(context.User.Local.Single(u => u.UserName == userName))
                    {
                        Name = "Expense",
                    }
                );
                #endregion

                #region Budget
                context.Budget.AddOrUpdate(b => b.Name,
                    new Budget(context.User.Local.Single(u => u.UserName == userName))
                    {
                        Amount = 100,
                        BudgetId = 1,
                        FromDate = new DateTime(2013, 1, 1),
                        ToDate = new DateTime(2013, 1, 28),
                        IncludeInstallments = false,
                        Name = "Budget 1"
                    },
                    new Budget(context.User.Local.Single(u => u.UserName == userName))
                    {
                        Amount = 250,
                        BudgetId = 2,
                        FromDate = new DateTime(2013, 2, 7),
                        ToDate = new DateTime(2013, 2, 20),
                        IncludeInstallments = false,
                        Name = "Budget 2"
                    }
                    );
                #endregion

                #region Budget Thrushold
                context.BudgetThreshold.AddOrUpdate(bt => bt.BudgetThresholdId,
                    new BudgetThreshold(context.User.Local.Single(u => u.UserName == userName))
                    {
                        BudgetThresholdId = 1,
                        Amount = 10d,
                    }
                    );
                #endregion

                #region Frequency
                context.TypeFrequency.AddOrUpdate(f => f.Name,
                    new TypeFrequency(context.User.Local.Single(u => u.UserName == userName))
                {
                    Name = "Hourly",
                    Count = 1,
                });
                context.TypeFrequency.AddOrUpdate(f => f.Name,
                    new TypeFrequency(context.User.Local.Single(u => u.UserName == userName))
                {
                    Name = "Daily",
                    Count = 24,
                });
                context.TypeFrequency.AddOrUpdate(f => f.Name,
                    new TypeFrequency(context.User.Local.Single(u => u.UserName == userName))
                {
                    Name = "Weekly",
                    Count = 168,
                });
                context.TypeFrequency.AddOrUpdate(f => f.Name,
                    new TypeFrequency(context.User.Local.Single(u => u.UserName == userName))
                {
                    Name = "Monthly",
                    Count = 672,
                });
                context.TypeFrequency.AddOrUpdate(f => f.Name,
                    new TypeFrequency(context.User.Local.Single(u => u.UserName == userName))
                {
                    Name = "Yearly",
                    Count = 8736,
                });
                #endregion

                #region Transaction
                //## No need to seed transactions anymore
                //context.Transaction.AddOrUpdate(t => t.NameOfPlace,
                //    new Transaction(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        TransactionId = 1,
                //        Amount = 6.5d,
                //        NameOfPlace = "Four Seassons",
                //        TipAmount = 0d,
                //        Category = context.Category.Local.Single(c => c.Name == "Restaurant"),
                //        TransactionReasonType = context.TransactionReason.Local.Single(tr => tr.Name == "Lunch"),
                //        TransactionType = context.TypeTransaction.Local.Single(c => c.Name == "Expense"),
                //        Comments = "Juice and Perrier"
                //    },
                //    new Transaction(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        TransactionId = 2,
                //        Amount = 475d,
                //        NameOfPlace = "BBD",
                //        TipAmount = 0d,
                //        Category = context.Category.Local.Single(c => c.Name == "Work"),
                //        TransactionReasonType = context.TransactionReason.Local.Single(tr => tr.Name == "Salary"),
                //        TransactionType = context.TypeTransaction.Local.Single(c => c.Name == "Income"),
                //        Comments = "Allowances"
                //    },
                //    new Transaction(context.User.Local.Single(u => u.UserName == userName))
                //    {
                //        TransactionId = 3,
                //        Amount = 5d,
                //        NameOfPlace = "Strovolos Municipality",
                //        TipAmount = 0d,
                //        Category = context.Category.Local.Single(c => c.Name == "Entertainment"),
                //        TransactionReasonType = context.TransactionReason.Local.Single(tr => tr.Name == "Futsal"),
                //        TransactionType = context.TypeTransaction.Local.Single(c => c.Name == "Expense"),
                //        Comments = "Statndard fee"
                //    });
                #endregion

                #region FieldType
                context.FieldType.AddOrUpdate(x => x.Name,
                    new FieldType { Name = "label", Type = Const.FieldType.Label.ToString(), DefaultValue = "" },
                    new FieldType { Name = "int", Type = Const.FieldType.Int.ToString(), DefaultValue = "1" },
                    new FieldType { Name = "dayNumber", Type = Const.FieldType.DayNum.ToString(), DefaultValue = "1" },
                    new FieldType { Name = "date", Type = Const.FieldType.DateInt.ToString(), DefaultValue = "20000101" },
                    new FieldType { Name = "truefalse", Type = Const.FieldType.Bit.ToString(), DefaultValue = "False" },
                    new FieldType { Name = "text", Type = Const.FieldType.String.ToString(), DefaultValue = "" },
                    new FieldType { Name = "position", Type = Const.FieldType.Position.ToString(), DefaultValue = "1" }
                );
                #endregion

                #region RulePart
                context.RulePart.AddOrUpdate(x => x.FieldName,
                    new RulePart { FieldName = Const.RuleField.RangeStartDate.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.DateInt.ToString()) },
                    new RulePart { FieldName = Const.RuleField.RangeNoEndDate.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Label.ToString()) },
                    new RulePart { FieldName = Const.RuleField.RangeTotalOcurrences.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { FieldName = Const.RuleField.RangeEndBy.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.DateInt.ToString()) },
                    new RulePart { FieldName = Const.RuleField.DailyEveryDay.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { FieldName = Const.RuleField.DailyOnlyWeekdays.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Bit.ToString()) },
                    new RulePart { FieldName = Const.RuleField.WeeklyEveryWeek.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { FieldName = Const.RuleField.WeeklyDayName.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.String.ToString()) },
                    new RulePart { FieldName = Const.RuleField.MonthlyDayNumber.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.DayNum.ToString()) },
                    new RulePart { FieldName = Const.RuleField.MonthlyEveryMonth.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { FieldName = Const.RuleField.MonthlyCountOfWeekDay.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Position.ToString()) },
                    new RulePart { FieldName = Const.RuleField.MonthlyDayName.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.DayNum.ToString()) },
                    new RulePart { FieldName = Const.RuleField.MonthlyCountOfMonth.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { FieldName = Const.RuleField.YearlyEveryYear.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Int.ToString()) },
                    new RulePart { FieldName = Const.RuleField.YearlyOnDayPos.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Position.ToString()) },
                    new RulePart { FieldName = Const.RuleField.YearlyMonthName.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.String.ToString()) },
                    new RulePart { FieldName = Const.RuleField.YearlyPositions.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.Position.ToString()) },
                    new RulePart { FieldName = Const.RuleField.YearlyDayName.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.String.ToString()) },
                    new RulePart { FieldName = Const.RuleField.YearlyMonthNameSec.ToString(), FieldType = context.FieldType.Local.Single(x => x.Type == Const.FieldType.String.ToString()) }
                );
                #endregion

                #region RecurrenceRule
                context.RecurrenceRule.AddOrUpdate(t => t.Name,
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleRangeNoEndDate.ToString(),
                        RuleParts = new List<RulePart>{
                            context.RulePart.Local.Single(x=>x.FieldName==Const.RuleField.RangeStartDate.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleRangeTotalOcurrences.ToString(),
                        RuleParts = new List<RulePart>{
                            context.RulePart.Local.Single(x=>x.FieldName==Const.RuleField.RangeTotalOcurrences.ToString()),
                            context.RulePart.Local.Single(x=>x.FieldName==Const.RuleField.RangeStartDate.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleRangeEndBy.ToString(),
                        RuleParts = new List<RulePart> { 
                            context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.RangeEndBy.ToString()) ,
                            context.RulePart.Local.Single(x=>x.FieldName==Const.RuleField.RangeStartDate.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleDailyEveryDays.ToString(),
                        RuleParts = new List<RulePart>{ 
                            context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.DailyEveryDay.ToString()),
                            context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.DailyOnlyWeekdays.ToString()) }
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleWeeklyEveryWeek.ToString(),
                        RuleParts = new List<RulePart>{ 
                                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.WeeklyEveryWeek.ToString()),
                                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.WeeklyDayName.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleMonthlyDayNum.ToString(),
                        RuleParts = new List<RulePart>{ 
                                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.MonthlyDayNumber.ToString()),
                                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.MonthlyEveryMonth.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleMonthlyPrecise.ToString(),
                        RuleParts = new List<RulePart>{ 
                                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.MonthlyCountOfWeekDay.ToString()),
                                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.MonthlyDayName.ToString()),
                                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.MonthlyCountOfMonth.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleYearlyOnMonth.ToString(),
                        RuleParts = new List<RulePart>{ 
                                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyEveryYear.ToString()),
                                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyOnDayPos.ToString()),
                                        context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyMonthName.ToString())}
                    },
                    new RecurrenceRule
                    {
                        Name = Const.Rule.RuleYearlyOnTheWeekDay.ToString(),
                        RuleParts = new List<RulePart>{ 
                                      context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyEveryYear.ToString()),
                                      context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyPositions.ToString()),
                                      context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyDayName.ToString()),
                                      context.RulePart.Local.Single(x => x.FieldName==Const.RuleField.YearlyMonthNameSec.ToString())}
                    }

                );
                #endregion

                #region Target
                ////context.Target.AddOrUpdate(t => t.EndDate, new Target { });
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
