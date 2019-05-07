﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Infrastructure.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountCategory> AccountCategory { get; set; }
        public virtual DbSet<AccountProvider> AccountProvider { get; set; }
        public virtual DbSet<Agency> Agency { get; set; }
        public virtual DbSet<Campaign> Campaign { get; set; }
        public virtual DbSet<CampaignAccount> CampaignAccount { get; set; }
        public virtual DbSet<CampaignOption> CampaignOption { get; set; }
        public virtual DbSet<CampaignType> CampaignType { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Account>(ConfigureAccount);
            builder.Entity<AccountCategory>(ConfigureAccountCategory);
            builder.Entity<AccountProvider>(ConfigureAccountProvider);

            builder.Entity<Agency>(ConfigureAgency);


            builder.Entity<Campaign>(ConfigureCampaign);
            builder.Entity<CampaignAccount>(ConfigureCampaignAccount);
            builder.Entity<CampaignOption>(ConfigureCampaignOption);
            builder.Entity<CampaignType>(ConfigureCampaignType);
            


            builder.Entity<Category>(ConfigureCategory);


            builder.Entity<Notification>(ConfigureNotification);
        }


        private void ConfigureAccount(EntityTypeBuilder<Account> builder)
        {
            builder.HasQueryFilter(p => !p.Deleted);


            builder.Metadata.FindNavigation(nameof(Core.Entities.Account.AccountCategory)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Core.Entities.Account.AccountProvider)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }


        private void ConfigureAccountCategory(EntityTypeBuilder<AccountCategory> builder)
        {

            builder.HasKey(m => new { m.AccountId, m.CategoryId });
            builder.HasOne(m => m.Category).WithMany(m => m.AccountCategory).HasForeignKey(m => m.CategoryId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.Account).WithMany(m => m.AccountCategory).HasForeignKey(m => m.AccountId).OnDelete(DeleteBehavior.ClientSetNull);
        }
        private void ConfigureAccountProvider(EntityTypeBuilder<AccountProvider> builder)
        {

            builder.HasOne(m => m.Account).WithMany(m => m.AccountProvider).HasForeignKey(m => m.AccountId).OnDelete(DeleteBehavior.ClientSetNull);

            //builder.Property(e => e.Provider).HasConversion(v => v.ToString(), v => (AccountProviderType)Enum.Parse(typeof(AccountProviderProvider), v));
        }


        private void ConfigureAgency(EntityTypeBuilder<Agency> builder)
        {
            builder.HasQueryFilter(p => !p.Deleted);
            builder.Metadata.FindNavigation(nameof(Core.Entities.Agency.Campaign)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureCategory(EntityTypeBuilder<Category> builder)
        {

            builder.HasQueryFilter(p => !p.Deleted);
            builder.Metadata.FindNavigation(nameof(Core.Entities.Category.AccountCategory)).SetPropertyAccessMode(PropertyAccessMode.Field);


        }

        private void ConfigureCampaign(EntityTypeBuilder<Campaign> builder)
        {
            builder.HasQueryFilter(p => !p.Deleted);

            builder.HasOne(m => m.Agency).WithMany(m => m.Campaign).HasForeignKey(m => m.AgencyId).OnDelete(DeleteBehavior.ClientSetNull);


            builder.Metadata.FindNavigation(nameof(Core.Entities.Campaign.CampaignAccount)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(Core.Entities.Campaign.CampaignOption)).SetPropertyAccessMode(PropertyAccessMode.Field);

        }
        private void ConfigureCampaignAccount(EntityTypeBuilder<CampaignAccount> builder)
        {

            builder.HasKey(m => new { m.CampaignId, m.AccountId });

            builder.HasOne(m => m.Campaign).WithMany(m => m.CampaignAccount).HasForeignKey(m => m.CampaignId).OnDelete(DeleteBehavior.ClientSetNull);

        }
        private void ConfigureCampaignOption(EntityTypeBuilder<CampaignOption> builder)
        {

            builder.HasOne(m => m.Campaign).WithMany(m => m.CampaignOption).HasForeignKey(m => m.CampaignId).OnDelete(DeleteBehavior.ClientSetNull);

        }
        
        private void ConfigureCampaignType(EntityTypeBuilder<CampaignType> builder)
        {


        }

        private void ConfigureNotification(EntityTypeBuilder<Notification> builder)
        {

        }



    }




}