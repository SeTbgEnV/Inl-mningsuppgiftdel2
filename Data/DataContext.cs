using Microsoft.EntityFrameworkCore;
using MormorDagnysDel2.Entities;

namespace MormorDagnysDel2.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<PostalAddress> PostalAddresses { get; set; }
    public DbSet<AddressType> AddressTypes { get; set; }
    public DbSet<CustomerAddress> CustomerAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderItem>()
        .HasKey(o => new { o.ProductId, o.SalesOrderId });
        
        modelBuilder.Entity<CustomerAddress>()
        .HasKey(c => new { c.AddressId, c.CustomerId });

        modelBuilder.Entity<Product>()
        .HasMany(p => p.OrderItems)
        .WithOne(oi => oi.Product)
        .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<SalesOrder>()
        .HasMany(so => so.OrderItems)
        .WithOne(oi => oi.SalesOrder)
        .HasForeignKey(oi => oi.SalesOrderId);

        modelBuilder.Entity<Customer>()
        .HasMany(c => c.Orders)
        .WithOne(so => so.Customer)
        .HasForeignKey(so => so.CustomerId);

        base.OnModelCreating(modelBuilder);
    }
}