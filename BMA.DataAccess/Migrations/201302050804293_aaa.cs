namespace BMA.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Budget",
                c => new
                    {
                        BudgetId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Amount = c.Double(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        Comments = c.String(),
                        IncludeInstallments = c.Boolean(nullable: false),
                        HasChanged = c.Boolean(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetId);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        HasChanged = c.Boolean(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.TypeTransactionReason",
                c => new
                    {
                        TypeTransactionReasonId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TypeTransactionReasonId);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        NotificationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Time = c.DateTime(nullable: false),
                        Description = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.NotificationId);
            
            CreateTable(
                "dbo.Security",
                c => new
                    {
                        SecurityId = c.Int(nullable: false, identity: true),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SecurityId);
            
            CreateTable(
                "dbo.Transaction",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        NameOfPlace = c.String(),
                        TipAmount = c.Double(nullable: false),
                        Comments = c.String(),
                        HasChanged = c.Boolean(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Category_CategoryId = c.Int(),
                        TransactionReasonType_TypeTransactionReasonId = c.Int(),
                        TransactionType_TypeTransactionId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Category", t => t.Category_CategoryId)
                .ForeignKey("dbo.TypeTransactionReason", t => t.TransactionReasonType_TypeTransactionReasonId)
                .ForeignKey("dbo.TypeTransaction", t => t.TransactionType_TypeTransactionId)
                .Index(t => t.Category_CategoryId)
                .Index(t => t.TransactionReasonType_TypeTransactionReasonId)
                .Index(t => t.TransactionType_TypeTransactionId);
            
            CreateTable(
                "dbo.TypeTransaction",
                c => new
                    {
                        TypeTransactionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TypeTransactionId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Target",
                c => new
                    {
                        TargetId = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Comments = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        SavingsDencityType_TypeSavingsDencityId = c.Int(),
                    })
                .PrimaryKey(t => t.TargetId)
                .ForeignKey("dbo.TypeSavingsDencity", t => t.SavingsDencityType_TypeSavingsDencityId)
                .Index(t => t.SavingsDencityType_TypeSavingsDencityId);
            
            CreateTable(
                "dbo.TypeSavingsDencity",
                c => new
                    {
                        TypeSavingsDencityId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TypeSavingsDencityId);
            
            CreateTable(
                "dbo.BudgetThreshold",
                c => new
                    {
                        BudgetThresholdId = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetThresholdId);
            
            CreateTable(
                "dbo.PeriodicInOut",
                c => new
                    {
                        PeriodicInOutId = c.Int(nullable: false, identity: true),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PeriodicInOutId);
            
            CreateTable(
                "dbo.TypePeriodicInOut",
                c => new
                    {
                        TypePeriodicInOutId = c.Int(nullable: false, identity: true),
                        isIncome = c.Boolean(nullable: false),
                        Name = c.String(),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TypePeriodicInOutId);
            
            CreateTable(
                "dbo.TypeFrequency",
                c => new
                    {
                        TypeFrequencyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Count = c.Int(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TypeFrequencyId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Target", new[] { "SavingsDencityType_TypeSavingsDencityId" });
            DropIndex("dbo.Transaction", new[] { "TransactionType_TypeTransactionId" });
            DropIndex("dbo.Transaction", new[] { "TransactionReasonType_TypeTransactionReasonId" });
            DropIndex("dbo.Transaction", new[] { "Category_CategoryId" });
            DropForeignKey("dbo.Target", "SavingsDencityType_TypeSavingsDencityId", "dbo.TypeSavingsDencity");
            DropForeignKey("dbo.Transaction", "TransactionType_TypeTransactionId", "dbo.TypeTransaction");
            DropForeignKey("dbo.Transaction", "TransactionReasonType_TypeTransactionReasonId", "dbo.TypeTransactionReason");
            DropForeignKey("dbo.Transaction", "Category_CategoryId", "dbo.Category");
            DropTable("dbo.TypeFrequency");
            DropTable("dbo.TypePeriodicInOut");
            DropTable("dbo.PeriodicInOut");
            DropTable("dbo.BudgetThreshold");
            DropTable("dbo.TypeSavingsDencity");
            DropTable("dbo.Target");
            DropTable("dbo.User");
            DropTable("dbo.TypeTransaction");
            DropTable("dbo.Transaction");
            DropTable("dbo.Security");
            DropTable("dbo.Notification");
            DropTable("dbo.TypeTransactionReason");
            DropTable("dbo.Category");
            DropTable("dbo.Budget");
        }
    }
}
