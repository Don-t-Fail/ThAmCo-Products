﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ThAmCo.Products.Data;

namespace ThAmCo.Products.Data.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    partial class ProductsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ThAmCo.Products.Data.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AvailableProductCount");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Brands");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AvailableProductCount = 3,
                            Description = "Description 1",
                            Name = "Brand 1"
                        },
                        new
                        {
                            Id = 2,
                            AvailableProductCount = 2,
                            Description = "Description 2",
                            Name = "Brand 2"
                        },
                        new
                        {
                            Id = 3,
                            AvailableProductCount = 3,
                            Description = "Description 3",
                            Name = "Brand 3"
                        });
                });

            modelBuilder.Entity("ThAmCo.Products.Data.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AvailableProductCount");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Category");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AvailableProductCount = 4,
                            Name = "Category 1"
                        },
                        new
                        {
                            Id = 2,
                            AvailableProductCount = 3,
                            Name = "Category 2"
                        },
                        new
                        {
                            Id = 3,
                            AvailableProductCount = 2,
                            Name = "Category 3"
                        });
                });

            modelBuilder.Entity("ThAmCo.Products.Data.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("BrandId");

                    b.Property<int>("CategoryId");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            BrandId = 1,
                            CategoryId = 2,
                            Description = "Description of product 1.",
                            Name = "Product 1"
                        },
                        new
                        {
                            Id = 2,
                            Active = true,
                            BrandId = 2,
                            CategoryId = 3,
                            Description = "Description of product 2.",
                            Name = "Product 2"
                        },
                        new
                        {
                            Id = 3,
                            Active = true,
                            BrandId = 3,
                            CategoryId = 1,
                            Description = "Description of product 3.",
                            Name = "Product 3"
                        },
                        new
                        {
                            Id = 4,
                            Active = true,
                            BrandId = 1,
                            CategoryId = 2,
                            Description = "Description of product 4.",
                            Name = "Product 4"
                        },
                        new
                        {
                            Id = 5,
                            Active = true,
                            BrandId = 2,
                            CategoryId = 3,
                            Description = "Description of product 5.",
                            Name = "Product 5"
                        },
                        new
                        {
                            Id = 6,
                            Active = true,
                            BrandId = 3,
                            CategoryId = 1,
                            Description = "Description of product 6.",
                            Name = "Product 6"
                        },
                        new
                        {
                            Id = 7,
                            Active = true,
                            BrandId = 1,
                            CategoryId = 2,
                            Description = "Description of product 7.",
                            Name = "Product 7"
                        });
                });

            modelBuilder.Entity("ThAmCo.Products.Data.Product", b =>
                {
                    b.HasOne("ThAmCo.Products.Data.Brand", "Brand")
                        .WithMany()
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ThAmCo.Products.Data.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
