﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Actived");

                    b.Property<string>("Address");

                    b.Property<string>("Avatar");

                    b.Property<string>("BankAccountBank");

                    b.Property<string>("BankAccountBranch");

                    b.Property<string>("BankAccountName");

                    b.Property<string>("BankAccountNumber");

                    b.Property<DateTime?>("Birthday");

                    b.Property<int?>("CityId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("Deleted");

                    b.Property<int?>("DistrictId");

                    b.Property<string>("Email");

                    b.Property<int>("Gender");

                    b.Property<string>("IDCardCity");

                    b.Property<string>("IDCardImageBack");

                    b.Property<string>("IDCardImageFront");

                    b.Property<string>("IDCardName");

                    b.Property<string>("IDCardNumber");

                    b.Property<string>("IDCardTime");

                    b.Property<string>("IgnoreCampaignTypes");

                    b.Property<int>("MaritalStatus");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Phone");

                    b.Property<string>("Salt");

                    b.Property<int>("Type");

                    b.Property<string>("TypeData");

                    b.Property<string>("UserCreated");

                    b.Property<string>("UserModified");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("DistrictId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("Core.Entities.AccountCampaignCharge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountChargeAmount");

                    b.Property<int>("AccountId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("AccountCampaignCharge");
                });

            modelBuilder.Entity("Core.Entities.AccountCategory", b =>
                {
                    b.Property<int>("AccountId");

                    b.Property<int>("CategoryId");

                    b.HasKey("AccountId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("AccountCategory");
                });

            modelBuilder.Entity("Core.Entities.AccountFbPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<int>("CommentCount");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<int>("LikeCount");

                    b.Property<string>("Link");

                    b.Property<string>("Message");

                    b.Property<string>("Permalink");

                    b.Property<string>("Picture");

                    b.Property<string>("PostId");

                    b.Property<DateTime>("PostTime");

                    b.Property<int>("ShareCount");

                    b.Property<string>("UserCreated");

                    b.Property<string>("UserModified");

                    b.HasKey("Id");

                    b.ToTable("AccountFbPost");
                });

            modelBuilder.Entity("Core.Entities.AccountProvider", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken");

                    b.Property<int>("AccountId");

                    b.Property<string>("Email");

                    b.Property<DateTime>("Expired");

                    b.Property<int?>("FollowersCount");

                    b.Property<int?>("FriendsCount");

                    b.Property<string>("Name");

                    b.Property<string>("Provider")
                        .IsRequired();

                    b.Property<string>("ProviderId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("AccountProvider");
                });

            modelBuilder.Entity("Core.Entities.Agency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Actived");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("Salt");

                    b.Property<string>("TaxIdNumber");

                    b.Property<string>("UserCreated");

                    b.Property<string>("UserModified");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Agency");
                });

            modelBuilder.Entity("Core.Entities.Banner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<int>("DisplayOrder");

                    b.Property<string>("Image");

                    b.Property<int>("Position");

                    b.Property<bool>("Published");

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.Property<string>("UserCreated");

                    b.Property<string>("UserModified");

                    b.HasKey("Id");

                    b.ToTable("Banner");
                });

            modelBuilder.Entity("Core.Entities.Campaign", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountChargeAmount");

                    b.Property<int>("AccountChargeExtraPercent");

                    b.Property<int>("AccountChargeTime");

                    b.Property<int>("AgencyId");

                    b.Property<string>("Code");

                    b.Property<string>("Data");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime?>("DateEnd");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime?>("DateStart");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Description");

                    b.Property<bool>("EnabledAccountChargeExtra");

                    b.Property<int>("ExtraOptionChargePercent");

                    b.Property<string>("Image");

                    b.Property<bool>("Published");

                    b.Property<int>("Quantity");

                    b.Property<string>("Requirement");

                    b.Property<int>("ServiceChargeAmount");

                    b.Property<int>("ServiceChargePercent");

                    b.Property<int>("Status");

                    b.Property<string>("SystemNote");

                    b.Property<string>("Title");

                    b.Property<int>("Type");

                    b.Property<string>("UserCreated");

                    b.Property<string>("UserModified");

                    b.HasKey("Id");

                    b.HasIndex("AgencyId");

                    b.ToTable("Campaign");
                });

            modelBuilder.Entity("Core.Entities.CampaignAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountChargeAmount");

                    b.Property<int>("AccountId");

                    b.Property<int>("CampaignId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("RefContent");

                    b.Property<string>("RefData");

                    b.Property<string>("RefId");

                    b.Property<string>("RefImage");

                    b.Property<string>("RefUrl");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.Property<string>("UserCreated");

                    b.Property<string>("UserModified");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CampaignId");

                    b.ToTable("CampaignAccount");
                });

            modelBuilder.Entity("Core.Entities.CampaignAccountType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountType");

                    b.Property<int>("CampaignId");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("CampaignAccountType");
                });

            modelBuilder.Entity("Core.Entities.CampaignOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CampaignId");

                    b.Property<int>("Name");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CampaignId");

                    b.ToTable("CampaignOption");
                });

            modelBuilder.Entity("Core.Entities.CampaignTypeCharge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountChargeAmount");

                    b.Property<int>("AccountChargeExtraPercent");

                    b.Property<int>("ServiceChargeAmount");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("CampaignTypeCharge");
                });

            modelBuilder.Entity("Core.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name");

                    b.Property<bool>("Published");

                    b.Property<string>("UserCreated");

                    b.Property<string>("UserModified");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Core.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DisplayOrder");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("City");
                });

            modelBuilder.Entity("Core.Entities.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CityId");

                    b.Property<int?>("DisplayOrder");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("District");
                });

            modelBuilder.Entity("Core.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data");

                    b.Property<int>("DataId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("EntityId");

                    b.Property<int>("EntityType");

                    b.Property<string>("Image");

                    b.Property<string>("Message");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Core.Entities.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("Setting");
                });

            modelBuilder.Entity("Core.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdminNote");

                    b.Property<long>("Amount");

                    b.Property<string>("Code");

                    b.Property<string>("Data");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Note");

                    b.Property<int>("ReceiverId");

                    b.Property<string>("RefData");

                    b.Property<int?>("RefId");

                    b.Property<int>("SenderId");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.Property<string>("UserCreated");

                    b.Property<string>("UserModified");

                    b.HasKey("Id");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("Core.Entities.TransactionHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Amount");

                    b.Property<long>("Balance");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Note");

                    b.Property<int>("TransactionId");

                    b.Property<int>("WalletId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.HasIndex("WalletId");

                    b.ToTable("TransactionHistory");
                });

            modelBuilder.Entity("Core.Entities.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Balance");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<int>("EntityId");

                    b.Property<int>("EntityType");

                    b.Property<string>("UserCreated");

                    b.Property<string>("UserModified");

                    b.HasKey("Id");

                    b.ToTable("Wallet");
                });

            modelBuilder.Entity("Core.Entities.Account", b =>
                {
                    b.HasOne("Core.Entities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("Core.Entities.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId");
                });

            modelBuilder.Entity("Core.Entities.AccountCategory", b =>
                {
                    b.HasOne("Core.Entities.Account", "Account")
                        .WithMany("AccountCategory")
                        .HasForeignKey("AccountId");

                    b.HasOne("Core.Entities.Category", "Category")
                        .WithMany("AccountCategory")
                        .HasForeignKey("CategoryId");
                });

            modelBuilder.Entity("Core.Entities.AccountProvider", b =>
                {
                    b.HasOne("Core.Entities.Account", "Account")
                        .WithMany("AccountProvider")
                        .HasForeignKey("AccountId");
                });

            modelBuilder.Entity("Core.Entities.Campaign", b =>
                {
                    b.HasOne("Core.Entities.Agency", "Agency")
                        .WithMany("Campaign")
                        .HasForeignKey("AgencyId");
                });

            modelBuilder.Entity("Core.Entities.CampaignAccount", b =>
                {
                    b.HasOne("Core.Entities.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.Entities.Campaign", "Campaign")
                        .WithMany("CampaignAccount")
                        .HasForeignKey("CampaignId");
                });

            modelBuilder.Entity("Core.Entities.CampaignAccountType", b =>
                {
                    b.HasOne("Core.Entities.Campaign", "Campaign")
                        .WithMany("CampaignAccountType")
                        .HasForeignKey("CampaignId");
                });

            modelBuilder.Entity("Core.Entities.CampaignOption", b =>
                {
                    b.HasOne("Core.Entities.Campaign", "Campaign")
                        .WithMany("CampaignOption")
                        .HasForeignKey("CampaignId");
                });

            modelBuilder.Entity("Core.Entities.District", b =>
                {
                    b.HasOne("Core.Entities.City", "City")
                        .WithMany("District")
                        .HasForeignKey("CityId");
                });

            modelBuilder.Entity("Core.Entities.TransactionHistory", b =>
                {
                    b.HasOne("Core.Entities.Transaction", "Transaction")
                        .WithMany("TransactionHistory")
                        .HasForeignKey("TransactionId");

                    b.HasOne("Core.Entities.Wallet", "Wallet")
                        .WithMany("TransactionHistory")
                        .HasForeignKey("WalletId");
                });
#pragma warning restore 612, 618
        }
    }
}
