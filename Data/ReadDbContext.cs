using Microsoft.EntityFrameworkCore;
using OrderApi.Models;

namespace OrderApi;

public class ReadDbContext : DbContext
{
  public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options){  }
  public DbSet<Order> Orders { get; set; }
}
