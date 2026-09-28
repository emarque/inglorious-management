using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SoftballManager.Api.Data;

public sealed class SoftballDbContextFactory : IDesignTimeDbContextFactory<SoftballDbContext>
{
    public SoftballDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Set ConnectionStrings__DefaultConnection when creating EF Core migrations.");
        }

        var options = new DbContextOptionsBuilder<SoftballDbContext>()
            .UseMySQL(connectionString)
            .Options;

        return new SoftballDbContext(options);
    }
}
