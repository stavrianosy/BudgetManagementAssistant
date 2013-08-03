using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMA.BusinessLogic;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BMA.DataAccess
{
    public class EntityContext:DbContext, IDisposable
    {
        public DbSet<Budget> Budget { get; set; }
        public DbSet<Category> Category{get; set;}
        public DbSet<TypeTransactionReason> TransactionReason { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Security> Security { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TypeTransaction> TypeTransaction { get; set; }
        public DbSet<User> User { get; set; }
        //public DbSet<Target> Target { get; set; }
        public DbSet<TypeSavingsDencity> TypeSavingsDencity { get; set; }
        public DbSet<BudgetThreshold> BudgetThreshold { get; set; }
        public DbSet<TypeInterval> TypeInterval { get; set; }
        public DbSet<TypeFrequency> TypeFrequency { get; set; }
        public DbSet<TransactionImage> TransactionImage { get; set; }
        public DbSet<RecurrenceRule> RecurrenceRule { get; set; }
        public DbSet<RulePart> RulePart { get; set; }
        public DbSet<FieldType> FieldType { get; set; }
        public DbSet<RulePartValue> RulePartValue { get; set; }

        public EntityContext()
            : base("name=primaryConn")
        {
            this.Configuration.ProxyCreationEnabled = false; 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            //This will create the table with default mappings.
            //Just run Add-Migration and then Update-Database

            modelBuilder.Entity<Budget>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<Budget>().Ignore(i => i.Transactions);
            modelBuilder.Entity<Budget>().Ignore(i => i.SyncDate);
            modelBuilder.Entity<Budget>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Budget>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<BudgetThreshold>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<BudgetThreshold>().Ignore(i => i.SyncDate);

            modelBuilder.Entity<Category>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<Category>().Ignore(i => i.SyncDate);

            modelBuilder.Entity<TypeFrequency>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<TypeFrequency>().Ignore(i => i.SyncDate);

            modelBuilder.Entity<TypeTransactionReason>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<TypeTransactionReason>().Ignore(i => i.SyncDate);

            modelBuilder.Entity<Notification>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<Notification>().Ignore(i => i.SyncDate);

            modelBuilder.Entity<Security>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<Security>().Ignore(i => i.SyncDate);

            modelBuilder.Entity<Transaction>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<Transaction>().Ignore(i => i.SyncDate);
            ////modelBuilder.Entity<Transaction>().HasRequired(c => c.Category).WithMany(c => c.Transactions);
            ////modelBuilder.Entity<Transaction>().HasRequired(c => c.TransactionReason).WithRequiredDependent(c => c.Transaction);

            modelBuilder.Entity<TypeTransaction>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<TypeTransaction>().Ignore(i => i.SyncDate);

            modelBuilder.Entity<User>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<User>().Ignore(i => i.SyncDate);
            modelBuilder.Entity<User>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<Target>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<Target>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Target>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeInterval>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<TypeInterval>().Ignore(i => i.SyncDate);

            modelBuilder.Entity<TypeSavingsDencity>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<TypeSavingsDencity>().Ignore(i => i.SyncDate);

            modelBuilder.Entity<TransactionImage>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<TransactionImage>().Ignore(i => i.SyncDate);

            modelBuilder.Entity<RecurrenceRule>().Ignore(i => i.HasChanges);
            modelBuilder.Entity<RecurrenceRule>().Ignore(i => i.SyncDate);

        }


    }
}
