using Application.Interface;
using Application.Service;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
