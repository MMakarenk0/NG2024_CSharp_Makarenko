﻿// <auto-generated />
using System;
using DataLayer.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataLayer.Migrations
{
    [DbContext(typeof(DeliveryServiceDbContext))]
    [Migration("20240805093031_AddedItemCategoryTable")]
    partial class AddedItemCategoryTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DataLayer.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("710f409a-fec3-4dea-b561-60dcd302b65b"),
                            Description = "Electronics"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Clients");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a1cd570b-7d18-4f5b-84fe-fc54045b2b78"),
                            Name = "John",
                            Phone = "1111"
                        },
                        new
                        {
                            Id = new Guid("debd53b3-c423-4127-a669-e39d1f06b301"),
                            Name = "Kyle",
                            Phone = "2222"
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Salary")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("StorageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StorageId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DataLayer.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("StorageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.HasIndex("StorageId");

                    b.ToTable("Items");

                    b.HasData(
                        new
                        {
                            Id = new Guid("81509cc6-0481-4979-b279-1f6d7263e43c"),
                            Description = "Laptop ACER Nitro 5",
                            Price = 1200m,
                            ReceiverId = new Guid("debd53b3-c423-4127-a669-e39d1f06b301"),
                            SenderId = new Guid("a1cd570b-7d18-4f5b-84fe-fc54045b2b78"),
                            StorageId = new Guid("1591f96b-e048-46c4-9a23-ba3beff8fafe"),
                            Weight = 2.5f
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.ItemCategory", b =>
                {
                    b.Property<Guid>("ItemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ItemId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ItemCategories");

                    b.HasData(
                        new
                        {
                            ItemId = new Guid("81509cc6-0481-4979-b279-1f6d7263e43c"),
                            CategoryId = new Guid("710f409a-fec3-4dea-b561-60dcd302b65b"),
                            Id = new Guid("00000000-0000-0000-0000-000000000000")
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Manager", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("StorageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Managers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0afcfdf2-62e5-4ef2-b8b7-2822575e53e4"),
                            Name = "Michael",
                            StorageId = new Guid("1591f96b-e048-46c4-9a23-ba3beff8fafe")
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Storage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("DirectorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DirectorId")
                        .IsUnique()
                        .HasFilter("[DirectorId] IS NOT NULL");

                    b.ToTable("Storages");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1591f96b-e048-46c4-9a23-ba3beff8fafe"),
                            Address = "456 Maple Ave, Apt 12, Newtown, USA",
                            DirectorId = new Guid("0afcfdf2-62e5-4ef2-b8b7-2822575e53e4"),
                            Number = 235
                        });
                });

            modelBuilder.Entity("DataLayer.Entities.Employee", b =>
                {
                    b.HasOne("DataLayer.Entities.Storage", "Storage")
                        .WithMany("Employees")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Storage");
                });

            modelBuilder.Entity("DataLayer.Entities.Item", b =>
                {
                    b.HasOne("DataLayer.Entities.Client", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId");

                    b.HasOne("DataLayer.Entities.Client", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.HasOne("DataLayer.Entities.Storage", "Storage")
                        .WithMany("Items")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Receiver");

                    b.Navigation("Sender");

                    b.Navigation("Storage");
                });

            modelBuilder.Entity("DataLayer.Entities.ItemCategory", b =>
                {
                    b.HasOne("DataLayer.Entities.Category", "Category")
                        .WithMany("ItemCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataLayer.Entities.Item", "Item")
                        .WithMany("ItemCategories")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("DataLayer.Entities.Storage", b =>
                {
                    b.HasOne("DataLayer.Entities.Manager", "Director")
                        .WithOne("Storage")
                        .HasForeignKey("DataLayer.Entities.Storage", "DirectorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Director");
                });

            modelBuilder.Entity("DataLayer.Entities.Category", b =>
                {
                    b.Navigation("ItemCategories");
                });

            modelBuilder.Entity("DataLayer.Entities.Item", b =>
                {
                    b.Navigation("ItemCategories");
                });

            modelBuilder.Entity("DataLayer.Entities.Manager", b =>
                {
                    b.Navigation("Storage");
                });

            modelBuilder.Entity("DataLayer.Entities.Storage", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}