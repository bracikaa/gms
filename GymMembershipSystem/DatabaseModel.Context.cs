﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GymMembershipSystem
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class gymDatabaseEntities : DbContext
    {
        public gymDatabaseEntities()
            : base("name=gymDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ShopPayment> ShopPayments { get; set; }
        public DbSet<TrainingEnrollment> TrainingEnrollments { get; set; }
    }
}
