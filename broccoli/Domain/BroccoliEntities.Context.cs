﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BroccoliTrade.Domain
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BroccoliEntities : DbContext
    {
        public BroccoliEntities()
            : base("name=BroccoliEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Users> Users { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<AccountPool> AccountPool { get; set; }
        public DbSet<Systems> Systems { get; set; }
        public DbSet<TradingSystemPool> TradingSystemPool { get; set; }
        public DbSet<TradingSystems> TradingSystems { get; set; }
        public DbSet<Referrer> Referrer { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
