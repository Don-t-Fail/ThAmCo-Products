using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;

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
                //SEED DATA
            }
        }
    }
}
