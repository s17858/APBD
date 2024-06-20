using Zadanie4.Models;
using Microsoft.EntityFrameworkCore;

namespace Zadanie4.Data;

public class WarehouseContext : DbContext
{
    public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductWarehouse> ProductWarehouses { get; set; }
}