using ApiUsuarioKRT.Data;
using ApiUsuarioKRT.Data.Interfaces;
using ApiUsuarioKRT.Data.Repositories;
using ApiUsuarioKRT.Services;
using ApiUsuarioKRT.Services.Interfaces;

namespace ApiUsuarioKRT.Config
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
