using DataAccess.Generic;

namespace ProyectoEncodeApi.Middleware
{
    public static class IoC
    {
        public static IServiceCollection AddDependencia(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return services;
        }
    }
}
