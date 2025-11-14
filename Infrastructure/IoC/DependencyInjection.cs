using Application.Interface;
using Application.Service;
using Application.Service.Auth;
using Infrastructure.Data;
using Infrastructure.HealthCheck;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIoC(this IServiceCollection services, IConfiguration configuration)
        {
            // 🗄️ DbContext Oracle
            services.AddDbContext<AppDbContext>(options =>
                options.UseOracle(configuration.GetConnectionString("DefaultConnection")));

            // 👤 Usuario
            services.AddScoped<IUsuarioInterface, UsuarioService>();

            // 🎓 Habilidade
            services.AddScoped<IHabilidadeInterface, HabilidadeService>();

            // 🔄 Troca
            services.AddScoped<ITrocaInterface, TrocaService>();

            // 💸 Transacao
            services.AddScoped<ITransacaoInterface, TransacaoService>();

            // ⭐ Avaliacao
            services.AddScoped<IAvaliacaoInterface, AvaliacaoService>();

            // 🔵 HEALTH CHECKS (REGISTRAR APENAS AQUI!)
            services.AddHealthChecks()
                .AddCheck("api_alive", () => HealthCheckResult.Healthy(), tags: new[] { "live" })
                .AddCheck<OracleHealthCheck>("oracle", tags: new[] { "ready" });

            // 🔐 JWT Service
            services.AddScoped<JwtService>();


            return services;
        }
    }
}
