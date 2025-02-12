using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VPBANK.RMD.Data.Auth;
using VPBANK.RMD.Utils.Common.Assemblies;
using VPBANK.RMD.EFCore.Entities.SchemaInfos;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.EFCore.Abstractions;
using VPBANK.RMD.EFCore.Implements;
using VPBANK.RMD.Data.Auth.Entities.POCOs;
using VPBANK.RMD.Repositories.Auth.Interfaces;
using VPBANK.RMD.Repositories.Auth.Implements;
using VPBANK.RMD.Services.Auth.Interfaces;
using VPBANK.RMD.Services.Auth.Implements;
using VPBANK.RMD.API.Settings;

namespace VPBANK.RMD.API.Common.Features
{
    public static class DbContextConfigurator
    {
        internal static bool IsContextAdded { get; set; }

        public static IServiceCollection ConfigureBusinessFeatures(this IServiceCollection services, IConfiguration _configuration)
        {
            if (!IsContextAdded)
            {
                // Regix connection string
                services.AddDbContext<AuthContext>(c =>
                {
                    c.UseSqlServer(_configuration.GetConnectionString(nameof(AppSettings.ConnectionStrings.AuthConnection)), options =>
                    {
                        options.MigrationsAssembly(DefAssemblies.VBP_RMD_Data_Auth);
                    });
                });

                services.AddConfigureContext()
                        .AddConfigureAuth();

                IsContextAdded = true;
            }

            return services;
        }

        private static IServiceCollection AddConfigureContext(this IServiceCollection services)
        {
            // Data Context
            services.AddScoped<DbContext, AuthContext>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork<AuthContext>, UnitOfWork<AuthContext>>();

            // Generic repository, trackable, service pattern
            services.AddTransient(typeof(IDomainRepository<,,>), typeof(DomainRepository<,,>));
            services.AddTransient(typeof(ITrackableRepository<,,>), typeof(TrackableRepository<,,>));
            services.AddTransient(typeof(IGenericRepository<,,>), typeof(GenericRepository<,,>));

            // Queryable
            services.AddTransient(typeof(IQueryableRepository<,>), typeof(QueryableRepository<,>));

            return services;
        }

        private static IServiceCollection AddConfigureAuth(this IServiceCollection services)
        {

            // User
            services.AddScoped(typeof(IDomainRepository<AuthContext, User, int>), typeof(DomainRepository<AuthContext, User, int>));
            services.AddScoped(typeof(ITrackableRepository<AuthContext, User, int>), typeof(TrackableRepository<AuthContext, User, int>));
            services.AddScoped(typeof(IGenericRepository<AuthContext, User, int>), typeof(GenericRepository<AuthContext, User, int>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
