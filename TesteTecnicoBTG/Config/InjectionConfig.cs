using TesteTecnicoBTG.Data;
using TesteTecnicoBTG.Data.Interfaces;
using TesteTecnicoBTG.Data.Repositories;
using TesteTecnicoBTG.Services;
using TesteTecnicoBTG.Services.Interfaces;

namespace TesteTecnicoBTG.Config
{
    public static class InjectionConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            // Database
            services.AddSingleton<DBConnectionFactory>();

            // Services
            services.AddScoped<IUsuarioService, UsuarioService>();

            // Repositories
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            return services;
        }
    }
}
