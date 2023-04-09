using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Medilive.Assessment.Affiliate.Data
{
    public class DesignTimeDatabaseContextFactory : IDesignTimeDbContextFactory<MediliveAffiliateDatabaseContext>
    {
        public MediliveAffiliateDatabaseContext CreateDbContext(string[] args)
        {
            try
            {
                var connectionStr = "Server=localhost;Database=AffiliateProgram;User Id=sa;Password=*******;TrustServerCertificate=True";
                var optionsBuilder = new DbContextOptionsBuilder<MediliveAffiliateDatabaseContext>();

                optionsBuilder.UseSqlServer(connectionStr);
                optionsBuilder.EnableSensitiveDataLogging();

                return new MediliveAffiliateDatabaseContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
