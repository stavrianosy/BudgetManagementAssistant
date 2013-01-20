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
        public DbSet<TransactionReason> TransactionReason { get; set; }
        public DbSet<Installment> Installment { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Security> Security { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TypeTransaction> TypeTransaction { get; set; }
        public DbSet<User> User { get; set; }

        public EntityContext()
            : base("name=primaryConn")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
            //This will create the table with default mappings.
            //Just run Add-Migration and then Update-Database
            modelBuilder.Entity<Budget>();
            modelBuilder.Entity<Category>();
            modelBuilder.Entity<TransactionReason>();
            modelBuilder.Entity<Installment>();
            modelBuilder.Entity<Notification>();
            modelBuilder.Entity<Security>();
            modelBuilder.Entity<Transaction>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Transaction>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<TypeTransaction>();
            modelBuilder.Entity<User>();
        }


    }
}
