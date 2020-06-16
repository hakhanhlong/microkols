using System;
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
        public virtual DbSet<AccountFbPost> AccountFbPost { get; set; }

        public virtual DbSet<AccountCampaignCharge> AccountCampaignCharge { get; set; }
        
        public virtual DbSet<CampaignTypeCharge> CampaignTypeCharge { get; set; }

        public virtual DbSet<Agency> Agency { get; set; }
        public virtual DbSet<Campaign> Campaign { get; set; }
        public virtual DbSet<CampaignAccount> CampaignAccount { get; set; }
        public virtual DbSet<CampaignOption> CampaignOption { get; set; }
        public virtual DbSet<CampaignAccountCaption> CampaignAccountCaption { get; set; }
        public virtual DbSet<CampaignAccountContent> CampaignAccountContent { get; set; }
        public virtual DbSet<CampaignAccountStatistic> CampaignAccountStatistic { get; set; }


        public virtual DbSet<CampaignAccountType> CampaignAccountType { get; set; }


        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }

        public virtual DbSet<PayoutExport> PayoutExport { get; set; }


        public virtual DbSet<Setting> Setting { get; set; }

        public virtual DbSet<QnA> QnA { get; set; }

        public virtual DbSet<QnAVideo> QnAVideo { get; set; }
        public virtual DbSet<QnAImage> QnAImage { get; set; }

        public virtual DbSet<Bank> Bank { get; set; }


        public virtual DbSet<VideoGallery> VideoGallery { get; set; }




        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionHistory> TransactionHistory { get; set; }
        public virtual DbSet<Wallet> Wallet { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Account>(ConfigureAccount);
            builder.Entity<AccountCategory>(ConfigureAccountCategory);
            builder.Entity<AccountProvider>(ConfigureAccountProvider);

            builder.Entity<Agency>(ConfigureAgency);

            builder.Entity<Campaign>(ConfigureCampaign);
            builder.Entity<CampaignAccount>(ConfigureCampaignAccount);
            builder.Entity<CampaignAccountType>(ConfigureCampaignAccountType);
            builder.Entity<CampaignOption>(ConfigureCampaignOption);


            builder.Entity<Category>(ConfigureCategory);
            builder.Entity<Notification>(ConfigureNotification);
            builder.Entity<Setting>(ConfigureSetting);

            builder.Entity<Transaction>(ConfigureTransaction);
            builder.Entity<TransactionHistory>(ConfigureTransactionHistory);
            builder.Entity<Wallet>(ConfigureWallet);

            builder.Entity<QnAImage>(ConfigureQnAImage);
            builder.Entity<QnAVideo>(ConfigureQnAVideo);
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

            builder.Property(e => e.Provider).HasConversion(v => v.ToString(), v => (AccountProviderNames)Enum.Parse(typeof(AccountProviderNames), v));
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
            builder.Metadata.FindNavigation(nameof(Core.Entities.Campaign.CampaignAccountType)).SetPropertyAccessMode(PropertyAccessMode.Field);

        }
        private void ConfigureCampaignAccount(EntityTypeBuilder<CampaignAccount> builder)
        {
            builder.HasOne(m => m.Campaign).WithMany(m => m.CampaignAccount).HasForeignKey(m => m.CampaignId).OnDelete(DeleteBehavior.ClientSetNull);
        }

        private void ConfigureQnAImage(EntityTypeBuilder<QnAImage> builder)
        {
            builder.HasOne(m => m.QnA).WithMany(m => m.QnAImage).HasForeignKey(m => m.QAId).OnDelete(DeleteBehavior.ClientSetNull);
        }

        private void ConfigureQnAVideo(EntityTypeBuilder<QnAVideo> builder)
        {
            builder.HasOne(m => m.QnA).WithMany(m => m.QnAVideo).HasForeignKey(m => m.QAId).OnDelete(DeleteBehavior.ClientSetNull);
        }

        private void ConfigureCampaignAccountType(EntityTypeBuilder<CampaignAccountType> builder)
        {

            builder.HasOne(m => m.Campaign).WithMany(m => m.CampaignAccountType).HasForeignKey(m => m.CampaignId).OnDelete(DeleteBehavior.ClientSetNull);

        }
        private void ConfigureCampaignOption(EntityTypeBuilder<CampaignOption> builder)
        {

            builder.HasOne(m => m.Campaign).WithMany(m => m.CampaignOption).HasForeignKey(m => m.CampaignId).OnDelete(DeleteBehavior.ClientSetNull);

        }

        private void ConfigureCampaignTypeCharge(EntityTypeBuilder<CampaignTypeCharge> builder)
        {

        }

        private void ConfigureNotification(EntityTypeBuilder<Notification> builder)
        {

        }

        private void ConfigureSetting(EntityTypeBuilder<Setting> builder)
        {
            builder.Property(e => e.Name).HasConversion(v => v.ToString(), v => (SettingName)Enum.Parse(typeof(SettingName), v));

        }


        private void ConfigureTransaction(EntityTypeBuilder<Transaction> builder)
        {

            builder.Metadata.FindNavigation(nameof(Core.Entities.Transaction.TransactionHistory)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
        private void ConfigureTransactionHistory(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.HasOne(m => m.Transaction).WithMany(m => m.TransactionHistory).HasForeignKey(m => m.TransactionId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(m => m.Wallet).WithMany(m => m.TransactionHistory).HasForeignKey(m => m.WalletId).OnDelete(DeleteBehavior.ClientSetNull);

        }
        private void ConfigureWallet(EntityTypeBuilder<Wallet> builder)
        {
            builder.Metadata.FindNavigation(nameof(Core.Entities.Wallet.TransactionHistory)).SetPropertyAccessMode(PropertyAccessMode.Field);

        }


    }




}
