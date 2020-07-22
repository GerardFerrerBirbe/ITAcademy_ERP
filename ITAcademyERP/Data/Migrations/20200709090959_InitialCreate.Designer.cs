﻿// <auto-generated />
using System;
using ITAcademyERP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ITAcademyERP.Data.Migrations
{
    [DbContext(typeof(ITAcademyERPContext))]
    [Migration("20200709090959_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ITAcademyERP.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("ITAcademyERP.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique()
                        .HasFilter("[PersonId] IS NOT NULL");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("ITAcademyERP.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Salary")
                        .HasColumnType("float");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique()
                        .HasFilter("[PersonId] IS NOT NULL");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("ITAcademyERP.Models.OrderHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AssignToEmployeeDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeliveryAddressId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FinalisationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderPriorityId")
                        .HasColumnType("int");

                    b.Property<int>("OrderStateId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("DeliveryAddressId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("OrderPriorityId");

                    b.HasIndex("OrderStateId");

                    b.ToTable("OrderHeader");
                });

            modelBuilder.Entity("ITAcademyERP.Models.OrderLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("OrderHeaderId")
                        .HasColumnType("int");

                    b.Property<string>("ProductId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProductId1")
                        .HasColumnType("int");

                    b.Property<double?>("Quantity")
                        .HasColumnType("float");

                    b.Property<double?>("TotalNetPrice")
                        .HasColumnType("float");

                    b.Property<double?>("TotalVatPrice")
                        .HasColumnType("float");

                    b.Property<double?>("UnitNetPrice")
                        .HasColumnType("float");

                    b.Property<double?>("UnitVatPrice")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("OrderHeaderId");

                    b.HasIndex("ProductId1");

                    b.ToTable("OrderLine");
                });

            modelBuilder.Entity("ITAcademyERP.Models.OrderPriority", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Priority")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderPriority");
                });

            modelBuilder.Entity("ITAcademyERP.Models.OrderState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderState");
                });

            modelBuilder.Entity("ITAcademyERP.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PersonalAddressId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonalAddressId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("ITAcademyERP.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProductCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductCategoryId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("ITAcademyERP.Models.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProductCategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProductCategory");
                });

            modelBuilder.Entity("ITAcademyERP.Models.Client", b =>
                {
                    b.HasOne("ITAcademyERP.Models.Person", "Person")
                        .WithOne("Client")
                        .HasForeignKey("ITAcademyERP.Models.Client", "PersonId");
                });

            modelBuilder.Entity("ITAcademyERP.Models.Employee", b =>
                {
                    b.HasOne("ITAcademyERP.Models.Person", "Person")
                        .WithOne("Employee")
                        .HasForeignKey("ITAcademyERP.Models.Employee", "PersonId");
                });

            modelBuilder.Entity("ITAcademyERP.Models.OrderHeader", b =>
                {
                    b.HasOne("ITAcademyERP.Models.Client", "Client")
                        .WithMany("OrderHeader")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ITAcademyERP.Models.Address", "DeliveryAddress")
                        .WithMany("OrderHeader")
                        .HasForeignKey("DeliveryAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ITAcademyERP.Models.Employee", "Employee")
                        .WithMany("OrderHeader")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ITAcademyERP.Models.OrderPriority", "OrderPriority")
                        .WithMany("OrderHeader")
                        .HasForeignKey("OrderPriorityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ITAcademyERP.Models.OrderState", "OrderState")
                        .WithMany("OrderHeader")
                        .HasForeignKey("OrderStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ITAcademyERP.Models.OrderLine", b =>
                {
                    b.HasOne("ITAcademyERP.Models.OrderHeader", "OrderHeader")
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderHeaderId");

                    b.HasOne("ITAcademyERP.Models.Product", "Product")
                        .WithMany("OrderLines")
                        .HasForeignKey("ProductId1");
                });

            modelBuilder.Entity("ITAcademyERP.Models.Person", b =>
                {
                    b.HasOne("ITAcademyERP.Models.Address", "PersonalAddress")
                        .WithMany("Person")
                        .HasForeignKey("PersonalAddressId");
                });

            modelBuilder.Entity("ITAcademyERP.Models.Product", b =>
                {
                    b.HasOne("ITAcademyERP.Models.ProductCategory", "ProductCategory")
                        .WithMany("Product")
                        .HasForeignKey("ProductCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}