namespace BMA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using BMA.BusinessLogic;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<BMA.DataAccess.EntityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BMA.DataAccess.EntityContext context)
        {
            //  This method will be called after migrating to the latest version.
            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Category ON;");
            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT TransactionReason ON");
            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT TypeTransaction ON");
            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [Transaction] ON");
            ///context.Database.ExecuteSqlCommand("insert into category (categoryId) values (1)");

            context.User.AddOrUpdate(u => u.UserName, new User { UserName = "stavrianosy" });

            context.Category.AddOrUpdate(c => c.Name,
                                        new Category { Name = "Work" },
                                        new Category { Name = "Restaurant" },
                                        new Category {Name = "Caffee shop"},
                                        new Category {Name = "Supermarket"});

            context.TransactionReason.AddOrUpdate(e => e.Name,
                new TransactionReason { Name = "Lunch" },
                new TransactionReason { Name = "Salary" },
                new TransactionReason { Name = "Dinner" }
                );

            context.TypeTransaction.AddOrUpdate(t => t.Name,
                new TypeTransaction { Name = "Income" },
                new TypeTransaction { Name = "Expence" }
            );

            context.Transaction.AddOrUpdate(t => t.NameOfPlace,
                new Transaction
                {
                    TransactionId = 1,
                    Amount = 5d,
                    NameOfPlace = "Four Seassons",
                    TipAmount = 0d,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = context.User.Local.Single(u=>u.UserName == "stavrianosy"),
                    CreatedDate = DateTime.Now,
                    CreatedUser = context.User.Local.Single(u => u.UserName == "stavrianosy"),
                    Category = context.Category.Local.Single(c => c.Name == "Restaurant"),
                    TransactionReason = context.TransactionReason.Local.Single(tr => tr.Name == "Lunch"),
                    TypeTransaction = context.TypeTransaction.Local.Single(c => c.Name == "Expence")
                });

            context.Transaction.AddOrUpdate(t => t.NameOfPlace,
                new Transaction
                {
                    TransactionId = 2,
                    Amount = 1000d,
                    NameOfPlace = "BBD",
                    TipAmount = 0d,
                    ModifiedDate = DateTime.Now,
                    ModifiedUser = context.User.Local.Single(u => u.UserName == "stavrianosy"),
                    CreatedDate = DateTime.Now,
                    CreatedUser = context.User.Local.Single(u => u.UserName == "stavrianosy"),
                    Category = context.Category.Local.Single(c => c.Name == "Work"),
                    TransactionReason = context.TransactionReason.Local.Single(tr => tr.Name == "Salary"),
                    TypeTransaction = context.TypeTransaction.Local.Single(c => c.Name == "Income")
                });

            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Category OFF");
            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT TransactionReason OFF");
            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT TypeTransaction OFF");
            //context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT [Transaction] OFF");
        }
    }
}
