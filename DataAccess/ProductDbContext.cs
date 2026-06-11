using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
namespace DataAccess
{
	public class ProductDbContext(DbContextOptions<ProductDbContext> options): DbContext(options)
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<Client> Clients { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.Property(x => x.Name).IsRequired();
				entity.Property(x => x.Price).HasPrecision(18, 2);
				entity.Property(x => x.Count).HasDefaultValue(0);

				entity.HasOne(p => p.Client)
					  .WithMany(c => c.Products)
					  .HasForeignKey(p => p.ClientId)
					  .OnDelete(DeleteBehavior.SetNull);
			});
			modelBuilder.Entity<Client>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.Property(x => x.Name).IsRequired();

				entity.HasMany(c => c.Products)
					  .WithOne(p => p.Client)
					  .HasForeignKey(p => p.ClientId);
			});
			base.OnModelCreating(modelBuilder);
		}
	}
}
