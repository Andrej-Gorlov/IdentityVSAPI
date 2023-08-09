using IdentityVSAPI.DAL;
using IdentityVSAPI.DAL.Initializer;
using IdentityVSAPI.DAL.Interfaces;
using IdentityVSAPI.DAL.Repository;
using IdentityVSAPI.Domain.Response;
using IdentityVSAPI.Service.Implementations;
using IdentityVSAPI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityVSAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
           IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            // Add services to the container.
            services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(connectionString));

            services.Configure<JwtOptions>(config.GetSection("ApiSettings:JwtOptions"));
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IDbBaseUserInitializer, DbBaseUserInitializer>();
            services.AddScoped<IAccountServicesAuthAsync, AccountServicesAuthAsync>();

            return services;
        }
    }       
}