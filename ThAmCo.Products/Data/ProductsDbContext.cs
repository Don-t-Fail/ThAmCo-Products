using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace ThAmCo.Products.Data
{
    public class ProductsDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Category { get; set; }

        private IHostingEnvironment HostEnv { get; }

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options, IHostingEnvironment env) : base(options)
        {
            HostEnv = env;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Brand>()
                .HasKey(b => b.Id);

            builder.Entity<Category>()
                .HasKey(c => c.Id);

            builder.Entity<Product>()
                .HasOne(b => b.Brand);

            builder.Entity<Product>()
                .HasOne(c => c.Category);

            builder.Entity<Product>()
                .HasKey(p => p.Id);

            if (HostEnv != null && HostEnv.IsDevelopment())
            {
                builder.Entity<Brand>()
                    .HasData(
                        new Brand { Id = 1, Name = "Brand 1"},
                        new Brand { Id = 2, Name = "Brand 2"},
                        new Brand { Id = 3, Name = "Brand 3"}
                    );

                builder.Entity<Category>()
                    .HasData(
                        new Category { Id = 1, Name = "Category 1"},
                        new Category { Id = 2, Name = "Category 2"},
                        new Category { Id = 3, Name = "Category 3"}
                    );

                builder.Entity<Product>()
                    .HasData(
                        new Product { Id = 1, Name = "Product 1", Description = "Description of product 1.", BrandId = 1, CategoryId = 2, Active = true },
                        new Product { Id = 2, Name = "Product 2", Description = "Description of product 2.", BrandId = 2, CategoryId = 3, Active = true },
                        new Product { Id = 3, Name = "Product 3", Description = "Description of product 3.", BrandId = 3, CategoryId = 1, Active = true },
                        new Product { Id = 4, Name = "Product 4", Description = "Description of product 4.", BrandId = 1, CategoryId = 2, Active = true },
                        new Product { Id = 5, Name = "Product 5", Description = "Description of product 5.", BrandId = 2, CategoryId = 3, Active = true },
                        new Product { Id = 6, Name = "Product 6", Description = "Description of product 6.", BrandId = 3, CategoryId = 1, Active = true },
                        new Product { Id = 7, Name = "Product 7", Description = "Description of product 7.", BrandId = 1, CategoryId = 2, Active = true }
                    );
            }
        }
    }
}
