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
        public DbSet<Installment> Installment { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Security> Security { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TypeTransaction> TypeTransaction { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Target> Target { get; set; }
        public DbSet<TypeSavingsDencity> TypeSavingsDencity { get; set; }

        public EntityContext()
            : base("name=primaryConn")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            //This will create the table with default mappings.
            //Just run Add-Migration and then Update-Database
            modelBuilder.Entity<Budget>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Budget>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Category>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<TransactionReason>().HasRequired(c => c.Transaction).WithMany(c => c.TransactionReason);
            modelBuilder.Entity<TypeTransactionReason>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<TypeTransactionReason>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);
            
            modelBuilder.Entity<Installment>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Installment>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);
            
            modelBuilder.Entity<Notification>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Notification>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);
            
            modelBuilder.Entity<Security>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Security>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<Transaction>().HasRequired(c => c.Category).WithMany(c => c.Transactions);
            //modelBuilder.Entity<Transaction>().HasRequired(c => c.TransactionReason).WithRequiredDependent(c => c.Transaction);
            modelBuilder.Entity<Transaction>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Transaction>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeTransaction>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<TypeTransaction>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<User>().HasRequired(c => c.UserName).WithMany().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<User>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Target>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Target>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeSavingsDencity>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<TypeSavingsDencity>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);
        }


    }
}
