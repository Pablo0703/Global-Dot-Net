using Microsoft.Extensions.Diagnostics.HealthChecks;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.HealthCheck
{
    public class OracleHealthCheck : IHealthCheck
    {
        private readonly AppDbContext _context;

        public OracleHealthCheck(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Abre a conexão com Oracle
                var connection = _context.Database.GetDbConnection();

                await connection.OpenAsync(cancellationToken);

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT 1 FROM DUAL";
                await command.ExecuteScalarAsync(cancellationToken);

                return HealthCheckResult.Healthy("Oracle OK");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Oracle FAILED", ex);
            }
        }
    }
}