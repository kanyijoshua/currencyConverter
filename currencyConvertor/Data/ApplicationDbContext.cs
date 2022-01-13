using currencyConvertor.Models;
using Microsoft.EntityFrameworkCore;

namespace currencyConvertor.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Logs> Logs { get; set; }
}