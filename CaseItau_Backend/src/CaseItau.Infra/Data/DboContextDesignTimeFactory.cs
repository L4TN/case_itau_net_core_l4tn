using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CaseItau.Infra.Data;

public class DboContextDesignTimeFactory : IDesignTimeDbContextFactory<DboContext>
{
    public DboContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DboContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=CaseItauDb_Design;TrustServerCertificate=true;");
        return new DboContext(optionsBuilder.Options);
    }
}
