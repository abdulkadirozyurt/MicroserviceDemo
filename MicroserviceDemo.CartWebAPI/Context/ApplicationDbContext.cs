using MicroserviceDemo.CartWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroserviceDemo.CartWebAPI.Context;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Cart> Carts { get; set; }
}
