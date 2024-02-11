﻿// <auto-generated />
using System;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations.ProductCatalogDB
{
    [DbContext(typeof(ProductCatalogDBContext))]
    partial class ProductCatalogDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Entities.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AddressId"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("StreetName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.HasKey("AddressId")
                        .HasName("PK__Addresse__091C2AFB2B3A5224");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Infrastructure.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CategoryId")
                        .HasName("PK__Categori__19093A0B6040E846");

                    b.HasIndex(new[] { "CategoryName" }, "UQ__Categori__8517B2E0F2B72418")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Infrastructure.Entities.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("CustomerId")
                        .HasName("PK__Customer__A4AE64D84AC5CB15");

                    b.HasIndex(new[] { "Email" }, "UQ__Customer__A9D1053495839DBE")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Infrastructure.Entities.CustomersAddress", b =>
                {
                    b.Property<int>("CustomerAddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerAddressId"));

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.HasKey("CustomerAddressId")
                        .HasName("PK__Customer__DB891B780BE87D3A");

                    b.HasIndex("AddressId");

                    b.HasIndex(new[] { "CustomerId", "AddressId" }, "UQ__Customer__043FA676DA3DBAC3")
                        .IsUnique();

                    b.ToTable("CustomersAddresses");
                });

            modelBuilder.Entity("Infrastructure.Entities.CustomersPhoneNumber", b =>
                {
                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("CustomerId", "PhoneNumber")
                        .HasName("PK__Customer__2CF1D03B4B6B6229");

                    b.ToTable("CustomersPhoneNumbers");
                });

            modelBuilder.Entity("Infrastructure.Entities.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("OrderDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime?>("ShippedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<decimal?>("TotalAmount")
                        .HasColumnType("money");

                    b.HasKey("OrderId")
                        .HasName("PK__Orders__C3905BCF38665E38");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Infrastructure.Entities.OrderItem", b =>
                {
                    b.Property<int>("OrderItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderItemId"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("ArticleNumber")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ProductPriceId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("money");

                    b.HasKey("OrderItemId", "OrderId")
                        .HasName("PK__OrderIte__BBD4033D1F487787");

                    b.HasIndex("ArticleNumber");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductPriceId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Infrastructure.Entities.Product", b =>
                {
                    b.Property<string>("ArticleNumber")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ingress")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("ProductImageId")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("money");

                    b.HasKey("ArticleNumber")
                        .HasName("PK__Products__3C991143460BCA14");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ProductImageId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductCategory", b =>
                {
                    b.Property<int>("ProductCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductCategoryId"));

                    b.Property<string>("ArticleNumber")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasKey("ProductCategoryId")
                        .HasName("PK__ProductC__3224ECCE31A1F48E");

                    b.HasIndex("CategoryId");

                    b.HasIndex(new[] { "ArticleNumber", "CategoryId" }, "UQ__ProductC__9D0982E20ABAA23C")
                        .IsUnique();

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductImage", b =>
                {
                    b.Property<int>("ProductImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductImageId"));

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductImageId")
                        .HasName("PK__ProductI__07B2B1B82C90DA53");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductPrice", b =>
                {
                    b.Property<int>("ProductPriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductPriceId"));

                    b.Property<string>("ArticleNumber")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal?>("DiscountPrice")
                        .HasColumnType("money");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("money");

                    b.HasKey("ProductPriceId")
                        .HasName("PK__ProductP__92B9436F3D5EFD0C");

                    b.HasIndex("ArticleNumber");

                    b.ToTable("ProductPrices");
                });

            modelBuilder.Entity("Infrastructure.Entities.Supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SupplierId"));

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SupplierEmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SupplierNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SupplierPhone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("SupplierId")
                        .HasName("PK__Supplier__4BE666B4A82399A7");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("Infrastructure.Entities.CustomersAddress", b =>
                {
                    b.HasOne("Infrastructure.Entities.Address", "Address")
                        .WithMany("CustomersAddresses")
                        .HasForeignKey("AddressId")
                        .IsRequired()
                        .HasConstraintName("FK__Customers__Addre__30D918B3");

                    b.HasOne("Infrastructure.Entities.Customer", "Customer")
                        .WithMany("CustomersAddresses")
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK__Customers__Custo__2FE4F47A");

                    b.Navigation("Address");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Infrastructure.Entities.Order", b =>
                {
                    b.HasOne("Infrastructure.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK__Orders__Customer__401B5C43");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Infrastructure.Entities.OrderItem", b =>
                {
                    b.HasOne("Infrastructure.Entities.Product", "ArticleNumberNavigation")
                        .WithMany("OrderItems")
                        .HasForeignKey("ArticleNumber")
                        .IsRequired()
                        .HasConstraintName("FK__OrderItem__Artic__45D43599");

                    b.HasOne("Infrastructure.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK__OrderItem__Order__44E01160");

                    b.HasOne("Infrastructure.Entities.ProductPrice", "ProductPrice")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductPriceId")
                        .IsRequired()
                        .HasConstraintName("FK__OrderItem__Produ__46C859D2");

                    b.Navigation("ArticleNumberNavigation");

                    b.Navigation("Order");

                    b.Navigation("ProductPrice");
                });

            modelBuilder.Entity("Infrastructure.Entities.Product", b =>
                {
                    b.HasOne("Infrastructure.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK__Products__Catego__33B5855E");

                    b.HasOne("Infrastructure.Entities.ProductImage", "ProductImage")
                        .WithMany("Products")
                        .HasForeignKey("ProductImageId")
                        .HasConstraintName("FK__Products__Produc__359DCDD0");

                    b.HasOne("Infrastructure.Entities.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .IsRequired()
                        .HasConstraintName("FK__Products__Suppli__34A9A997");

                    b.Navigation("Category");

                    b.Navigation("ProductImage");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductCategory", b =>
                {
                    b.HasOne("Infrastructure.Entities.Product", "ArticleNumberNavigation")
                        .WithMany("ProductCategories")
                        .HasForeignKey("ArticleNumber")
                        .IsRequired()
                        .HasConstraintName("FK__ProductCa__Artic__3C4ACB5F");

                    b.HasOne("Infrastructure.Entities.Category", "Category")
                        .WithMany("ProductCategories")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK__ProductCa__Categ__3D3EEF98");

                    b.Navigation("ArticleNumberNavigation");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductPrice", b =>
                {
                    b.HasOne("Infrastructure.Entities.Product", "ArticleNumberNavigation")
                        .WithMany("ProductPrices")
                        .HasForeignKey("ArticleNumber")
                        .IsRequired()
                        .HasConstraintName("FK__ProductPr__Artic__387A3A7B");

                    b.Navigation("ArticleNumberNavigation");
                });

            modelBuilder.Entity("Infrastructure.Entities.Address", b =>
                {
                    b.Navigation("CustomersAddresses");
                });

            modelBuilder.Entity("Infrastructure.Entities.Category", b =>
                {
                    b.Navigation("ProductCategories");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("Infrastructure.Entities.Customer", b =>
                {
                    b.Navigation("CustomersAddresses");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Infrastructure.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Infrastructure.Entities.Product", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("ProductCategories");

                    b.Navigation("ProductPrices");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductImage", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProductPrice", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Infrastructure.Entities.Supplier", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}