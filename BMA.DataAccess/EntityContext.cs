﻿using System;
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
            modelBuilder.Entity<Budget>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Budget>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<BudgetThreshold>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<BudgetThreshold>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<BudgetThreshold>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<Category>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Category>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeFrequency>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<TypeFrequency>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<TypeFrequency>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeTransactionReason>().Ignore(i => i.HasChanges);
            ////modelBuilder.Entity<TransactionReason>().HasRequired(c => c.Transaction).WithMany(c => c.TransactionReason);
            //modelBuilder.Entity<TypeTransactionReason>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<TypeTransactionReason>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Notification>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<Notification>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Notification>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);            

            modelBuilder.Entity<Security>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<Security>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Security>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Transaction>().Ignore(i => i.HasChanges);
            ////modelBuilder.Entity<Transaction>().HasRequired(c => c.Category).WithMany(c => c.Transactions);
            ////modelBuilder.Entity<Transaction>().HasRequired(c => c.TransactionReason).WithRequiredDependent(c => c.Transaction);
            //modelBuilder.Entity<Transaction>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Transaction>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeTransaction>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<TypeTransaction>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<TypeTransaction>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<User>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<User>().HasRequired(c => c.UserName).WithMany().HasForeignKey(c => c.UserId);
            modelBuilder.Entity<User>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<Target>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<Target>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Target>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeInterval>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<TypePeriodicInOut>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<TypePeriodicInOut>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeSavingsDencity>().Ignore(i => i.HasChanges);
            //modelBuilder.Entity<TypeSavingsDencity>().HasRequired(c => c.CreatedUser).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<TypeSavingsDencity>().HasRequired(c => c.ModifiedUser).WithMany().WillCascadeOnDelete(false);
        }


    }
}