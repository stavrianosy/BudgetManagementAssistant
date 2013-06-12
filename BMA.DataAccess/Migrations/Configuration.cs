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
                                            new Category
                                            {
                                                Name = "Work",
                                                FromDate = new DateTime(2000, 1, 1, 8, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 17, 0, 0),
                                                CreatedDate = DateTime.Now,
                                                CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                                                ModifiedDate = DateTime.Now,
                                                ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                                            },
                                            new Category
                                            {
                                                Name = "Restaurant",
                                                FromDate = new DateTime(2000, 1, 1, 20, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 22, 0, 0),
                                                CreatedDate = DateTime.Now,
                                                CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                                                ModifiedDate = DateTime.Now,
                                                ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                                            },
                                            new Category
                                            {
                                                Name = "Coffee shop",
                                                FromDate = new DateTime(2000, 1, 1, 19, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 23, 0, 0),
                                                CreatedDate = DateTime.Now,
                                                CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                                                ModifiedDate = DateTime.Now,
                                                ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                                            },
                                            new Category
                                            {
                                                Name = "Supermarket",
                                                FromDate = new DateTime(2000, 1, 1, 17, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 19, 0, 0),
                                                CreatedDate = DateTime.Now,
                                                CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                                                ModifiedDate = DateTime.Now,
                                                ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                                            },
                                            new Category
                                            {
                                                Name = "Entertainment",
                                                FromDate = new DateTime(2000, 1, 1, 19, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 22, 0, 0),
                                                CreatedDate = DateTime.Now,
                                                CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                                                ModifiedDate = DateTime.Now,
                                                ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                                            },
                                            new Category
                                            {
                                                Name = "Club",
                                                FromDate = new DateTime(2000, 1, 1, 1, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 4, 0, 0),
                                                CreatedDate = DateTime.Now,
                                                CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                                                ModifiedDate = DateTime.Now,
                                                ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                                            },
                                            new Category
                                            {
                                                Name = "Other",
                                                FromDate = new DateTime(2000, 1, 1, 0, 0, 0),
                                                ToDate = new DateTime(2000, 1, 1, 0, 0, 0),
                                                CreatedDate = DateTime.Now,
                                                CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                                                ModifiedDate = DateTime.Now,
                                                ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                                            });

                #endregion

                #region TypeSavingsDencity
                context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                    new TypeSavingsDencity
                    {
                        TypeSavingsDencityId = 1,
                        Name = "Daily",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Single(u => u.UserName == userName)
                    });

                context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                    new TypeSavingsDencity
                    {
                        TypeSavingsDencityId = 2,
                        Name = "Weekly",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Single(u => u.UserName == userName)
                    });

                context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                    new TypeSavingsDencity
                    {
                        TypeSavingsDencityId = 3,
                        Name = "Monthly",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Single(u => u.UserName == userName)
                    });

                context.TypeSavingsDencity.AddOrUpdate(t => t.Name,
                    new TypeSavingsDencity
                    {
                        TypeSavingsDencityId = 4,
                        Name = "Yearly",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Single(u => u.UserName == userName)
                    });
                #endregion

                #region TransactionReason
                context.TransactionReason.AddOrUpdate(e => e.Name,
                    new TypeTransactionReason
                    {
                        Name = "Lunch",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                    },
                    new TypeTransactionReason
                    {
                        Name = "Salary",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                    },
                    new TypeTransactionReason
                    {
                        Name = "Dinner",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        // ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                    },
                    new TypeTransactionReason
                    {
                        Name = "Futsal",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                    }, new TypeTransactionReason
                    {
                        Name = "Other",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                    });
                #endregion

                #region TypeTransaction
                context.TypeTransaction.AddOrUpdate(t => t.Name,
                    new TypeTransaction
                    {
                        Name = "Income",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                    },
                    new TypeTransaction
                    {
                        Name = "Expense",
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                    }
                );
                #endregion

                #region Budget
                context.Budget.AddOrUpdate(b => b.Name,
                    new Budget
                    {
                        Amount = 100,
                        BudgetId = 1,
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                        FromDate = new DateTime(2013, 1, 1),
                        ToDate = new DateTime(2013, 1, 28),
                        IncludeInstallments = false,
                        Name = "Budget 1"
                    },
                    new Budget
                    {
                        Amount = 250,
                        BudgetId = 2,
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                        FromDate = new DateTime(2013, 2, 7),
                        ToDate = new DateTime(2013, 2, 20),
                        IncludeInstallments = false,
                        Name = "Budget 2"
                    }
                    );
                #endregion

                #region Budget Thrushold
                context.BudgetThreshold.AddOrUpdate(bt => bt.BudgetThresholdId,
                    new BudgetThreshold
                    {
                        BudgetThresholdId = 1,
                        Amount = 10d,
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                    }
                    );
                #endregion

                #region Frequency
                context.TypeFrequency.AddOrUpdate(f => f.TypeFrequencyId, new TypeFrequency
                {
                    Name = "Hourly",
                    Count = 1,
                    CreatedDate = DateTime.Now,
                    CreatedUser = context.User.Single(u => u.UserName == userName),
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = context.User.Single(u => u.UserName == userName)
                });
                context.TypeFrequency.AddOrUpdate(f => f.TypeFrequencyId, new TypeFrequency
                {
                    Name = "Daily",
                    Count = 24,
                    CreatedDate = DateTime.Now,
                    CreatedUser = context.User.Single(u => u.UserName == userName),
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = context.User.Single(u => u.UserName == userName)
                });
                context.TypeFrequency.AddOrUpdate(f => f.TypeFrequencyId, new TypeFrequency
                {
                    Name = "Weekly",
                    Count = 168,
                    CreatedDate = DateTime.Now,
                    CreatedUser = context.User.Single(u => u.UserName == userName),
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = context.User.Single(u => u.UserName == userName)
                });
                context.TypeFrequency.AddOrUpdate(f => f.TypeFrequencyId, new TypeFrequency
                {
                    Name = "Monthly",
                    Count = 672,
                    CreatedDate = DateTime.Now,
                    CreatedUser = context.User.Single(u => u.UserName == userName),
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = context.User.Single(u => u.UserName == userName)
                });
                context.TypeFrequency.AddOrUpdate(f => f.TypeFrequencyId, new TypeFrequency
                {
                    Name = "Yearly",
                    Count = 8736,
                    CreatedDate = DateTime.Now,
                    CreatedUser = context.User.Single(u => u.UserName == userName),
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = context.User.Single(u => u.UserName == userName)
                });
                #endregion

                #region Transaction
                context.Transaction.AddOrUpdate(t => t.NameOfPlace,
                    new Transaction()
                    {
                        TransactionId = 1,
                        Amount = 6.5d,
                        NameOfPlace = "Four Seassons",
                        TipAmount = 0d,
                        TransactionDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        Category = context.Category.Local.Single(c => c.Name == "Restaurant"),
                        TransactionReasonType = context.TransactionReason.Local.Single(tr => tr.Name == "Lunch"),
                        TransactionType = context.TypeTransaction.Local.Single(c => c.Name == "Expense"),
                        Comments = "Juice and Perrier"
                    },
                    new Transaction
                    {
                        TransactionId = 2,
                        Amount = 475d,
                        NameOfPlace = "BBD",
                        TipAmount = 0d,
                        TransactionDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        Category = context.Category.Local.Single(c => c.Name == "Work"),
                        TransactionReasonType = context.TransactionReason.Local.Single(tr => tr.Name == "Salary"),
                        TransactionType = context.TypeTransaction.Local.Single(c => c.Name == "Income"),
                        Comments = "Allowances"
                    },
                    new Transaction
                    {
                        TransactionId = 3,
                        Amount = 5d,
                        NameOfPlace = "Strovolos Municipality",
                        TipAmount = 0d,
                        TransactionDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        ModifiedUser = context.User.Local.Single(u => u.UserName == userName),
                        CreatedDate = DateTime.Now,
                        CreatedUser = context.User.Local.Single(u => u.UserName == userName),
                        Category = context.Category.Local.Single(c => c.Name == "Entertainment"),
                        TransactionReasonType = context.TransactionReason.Local.Single(tr => tr.Name == "Futsal"),
                        TransactionType = context.TypeTransaction.Local.Single(c => c.Name == "Expense"),
                        Comments = "Statndard fee"
                    });
                #endregion

                //#region Target
                ////context.Target.AddOrUpdate(t => t.EndDate, new Target { });
                //#endregion


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
