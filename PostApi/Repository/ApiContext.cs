using Microsoft.EntityFrameworkCore;
using PostApi.Models;

namespace PostApi.Repository;

public class ApiContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "PostDb");
    }

    public DbSet<Post> Posts { get; set; }
}