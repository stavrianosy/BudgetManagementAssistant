namespace BMA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAllTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Budget",
                c => new
                    {
                        BudgetId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Amount = c.Double(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        Comments = c.String(),
                        IncludeInstallments = c.Boolean(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(nullable: false),
                        CreatedUser_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(nullable: false),
                        CreatedUser_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(),
                        CreatedUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.TypeTransactionReason",
                c => new
                    {
                        TypeTransactionReasonId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(),
                        CreatedUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TypeTransactionReasonId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Time = c.DateTime(nullable: false),
                        Description = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(),
                        CreatedUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.NotificationId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.Security",
                c => new
                    {
                        SecurityId = c.Int(nullable: false, identity: true),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(),
                        CreatedUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.SecurityId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        NameOfPlace = c.String(),
                        TipAmount = c.Double(nullable: false),
                        Comments = c.String(),
                        TransactionDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Category_CategoryId = c.Int(),
                        TransactionReasonType_TypeTransactionReasonId = c.Int(),
                        TransactionType_TypeTransactionId = c.Int(),
                        ModifiedUser_UserId = c.Int(),
                        CreatedUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Category", t => t.Category_CategoryId)
                .ForeignKey("dbo.TypeTransactionReason", t => t.TransactionReasonType_TypeTransactionReasonId)
                .ForeignKey("dbo.TypeTransaction", t => t.TransactionType_TypeTransactionId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.Category_CategoryId)
                .Index(t => t.TransactionReasonType_TypeTransactionReasonId)
                .Index(t => t.TransactionType_TypeTransactionId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.TypeTransaction",
                c => new
                    {
                        TypeTransactionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(),
                        CreatedUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TypeTransactionId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.TypeSavingsDencity",
                c => new
                    {
                        TypeSavingsDencityId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(),
                        CreatedUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TypeSavingsDencityId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.BudgetThreshold",
                c => new
                    {
                        BudgetThresholdId = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(),
                        CreatedUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.BudgetThresholdId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.TypeInterval",
                c => new
                    {
                        TypeIntervalId = c.Int(nullable: false, identity: true),
                        IsIncome = c.Boolean(nullable: false),
                        Name = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(),
                        CreatedUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TypeIntervalId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
            CreateTable(
                "dbo.TypeFrequency",
                c => new
                    {
                        TypeFrequencyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Count = c.Int(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedUser_UserId = c.Int(),
                        CreatedUser_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TypeFrequencyId)
                .ForeignKey("dbo.User", t => t.ModifiedUser_UserId)
                .ForeignKey("dbo.User", t => t.CreatedUser_UserId)
                .Index(t => t.ModifiedUser_UserId)
                .Index(t => t.CreatedUser_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TypeFrequency", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.TypeFrequency", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.TypeInterval", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.TypeInterval", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.BudgetThreshold", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.BudgetThreshold", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.TypeSavingsDencity", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.TypeSavingsDencity", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.TypeTransaction", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.TypeTransaction", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.Transaction", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.Transaction", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.Transaction", new[] { "TransactionType_TypeTransactionId" });
            DropIndex("dbo.Transaction", new[] { "TransactionReasonType_TypeTransactionReasonId" });
            DropIndex("dbo.Transaction", new[] { "Category_CategoryId" });
            DropIndex("dbo.Security", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.Security", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.Notification", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.Notification", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.TypeTransactionReason", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.TypeTransactionReason", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.Category", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.Category", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.User", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.User", new[] { "ModifiedUser_UserId" });
            DropIndex("dbo.Budget", new[] { "CreatedUser_UserId" });
            DropIndex("dbo.Budget", new[] { "ModifiedUser_UserId" });
            DropForeignKey("dbo.TypeFrequency", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.TypeFrequency", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.TypeInterval", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.TypeInterval", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.BudgetThreshold", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.BudgetThreshold", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.TypeSavingsDencity", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.TypeSavingsDencity", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.TypeTransaction", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.TypeTransaction", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.Transaction", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.Transaction", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.Transaction", "TransactionType_TypeTransactionId", "dbo.TypeTransaction");
            DropForeignKey("dbo.Transaction", "TransactionReasonType_TypeTransactionReasonId", "dbo.TypeTransactionReason");
            DropForeignKey("dbo.Transaction", "Category_CategoryId", "dbo.Category");
            DropForeignKey("dbo.Security", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.Security", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.Notification", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.Notification", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.TypeTransactionReason", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.TypeTransactionReason", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.Category", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.Category", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.User", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.User", "ModifiedUser_UserId", "dbo.User");
            DropForeignKey("dbo.Budget", "CreatedUser_UserId", "dbo.User");
            DropForeignKey("dbo.Budget", "ModifiedUser_UserId", "dbo.User");
            DropTable("dbo.TypeFrequency");
            DropTable("dbo.TypeInterval");
            DropTable("dbo.BudgetThreshold");
            DropTable("dbo.TypeSavingsDencity");
            DropTable("dbo.TypeTransaction");
            DropTable("dbo.Transaction");
            DropTable("dbo.Security");
            DropTable("dbo.Notification");
            DropTable("dbo.TypeTransactionReason");
            DropTable("dbo.Category");
            DropTable("dbo.User");
            DropTable("dbo.Budget");
        }
    }
}
