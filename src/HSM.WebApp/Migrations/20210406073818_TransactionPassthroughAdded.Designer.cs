﻿// <auto-generated />
using System;
using HSM.WebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HSM.WebApp.Migrations
{
    [DbContext(typeof(HsmDbContext))]
    [Migration("20210406073818_TransactionPassthroughAdded")]
    partial class TransactionPassthroughAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("HSM.WebApp.Data.Models.Ledger", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<double>("Balance")
                        .HasColumnType("double");

                    b.Property<DateTime?>("CalculatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDue")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Ledgers");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.Member", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("EMail")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastUpdatedBy")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("LastUpdatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("MemberFrom")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Occupation")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OriginallyFrom")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.MemberAccount", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("ActivatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("Balance")
                        .HasColumnType("double");

                    b.Property<DateTime?>("CalculatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("MemberId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("MemberId")
                        .IsUnique();

                    b.ToTable("MemberAccounts");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.Transaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("AccountId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<double>("Amount")
                        .HasColumnType("double");

                    b.Property<string>("CreatedByUser")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("CreatedByUserOn")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsContra")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDue")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LedgerId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("PassthroughId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Tags")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UnitId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("LedgerId");

                    b.HasIndex("PassthroughId");

                    b.HasIndex("UnitId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.TransactionPassthrough", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<double>("Balance")
                        .HasColumnType("double");

                    b.Property<DateTime?>("CalculatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("TransactionPassthrough");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.Unit", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<long?>("Area")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsRented")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OwnerId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("RentedFrom")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RentedPersonName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RentedPersonPhone")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.MemberAccount", b =>
                {
                    b.HasOne("HSM.WebApp.Data.Models.Member", "Member")
                        .WithOne("Account")
                        .HasForeignKey("HSM.WebApp.Data.Models.MemberAccount", "MemberId");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.Transaction", b =>
                {
                    b.HasOne("HSM.WebApp.Data.Models.MemberAccount", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId");

                    b.HasOne("HSM.WebApp.Data.Models.Ledger", "Ledger")
                        .WithMany("Transactions")
                        .HasForeignKey("LedgerId");

                    b.HasOne("HSM.WebApp.Data.Models.TransactionPassthrough", "Passthrough")
                        .WithMany("Transactions")
                        .HasForeignKey("PassthroughId");

                    b.HasOne("HSM.WebApp.Data.Models.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId");

                    b.Navigation("Account");

                    b.Navigation("Ledger");

                    b.Navigation("Passthrough");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.Unit", b =>
                {
                    b.HasOne("HSM.WebApp.Data.Models.Member", "Owner")
                        .WithMany("OwnedUnits")
                        .HasForeignKey("OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.Ledger", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.Member", b =>
                {
                    b.Navigation("Account");

                    b.Navigation("OwnedUnits");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.MemberAccount", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("HSM.WebApp.Data.Models.TransactionPassthrough", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
