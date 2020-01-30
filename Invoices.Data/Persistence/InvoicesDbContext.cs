using Invoices.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Invoices.Data.Persistence
{
	public class InvoicesDbContext : DbContext
	{

		public DbSet<Order> Orders { get; set; }

		public DbSet<Invoice> Invoices { get; set; }

		public InvoicesDbContext(DbContextOptions<InvoicesDbContext> options) : base(options)
		{
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			var converter = new EnumToStringConverter<InvoiceState>();

			modelBuilder.Entity<Invoice>(e =>
			{
				e.Property(i => i.UserId).IsRequired();
				e.Property(i => i.State).HasConversion(converter).IsRequired();
				e.Property(i => i.Address).IsRequired();
				e.Property(i => i.Name).IsRequired();
				e.Property(i => i.Email).IsRequired();
				e.Property(i => i.InvoicedAt).IsRequired();
				e.HasMany(i => i.Orders).WithOne(o => o.Invoice).HasForeignKey(o => o.InvoiceId);
			});

			modelBuilder.Entity<Order>(e =>
			{
				e.Property(o => o.ProductName).IsRequired();
				e.Property(o => o.Name).IsRequired();
				e.Property(o => o.Address).IsRequired();
				e.Property(o => o.Price).IsRequired();
				e.Property(o => o.PurchaseDateTime).IsRequired();
			});
		}
	}
}
