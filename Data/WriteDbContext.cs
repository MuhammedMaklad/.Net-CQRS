using Microsoft.EntityFrameworkCore;
using OrderApi.Models;

namespace OrderApi;

public class WriteDbContext : DbContext
{
  public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options) { }
  public DbSet<Order> Orders { get; set; }
}
