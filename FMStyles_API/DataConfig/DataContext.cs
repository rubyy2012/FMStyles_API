using FMStyles_API.Models;
using Microsoft.EntityFrameworkCore;

namespace FMStyles_API.DataConfig
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierCategory> SuppliersCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>(
                entity =>
                {
                    entity.HasOne(s => s.SupplierCategory).WithMany(s => s.Suppliers)
                    .HasForeignKey(s => s.CategoryId).OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity<SupplierCategory>(entity =>
            {
               
            });
             

            base.OnModelCreating(modelBuilder);
        }

    }
}
