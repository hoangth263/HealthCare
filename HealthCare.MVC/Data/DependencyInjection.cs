using HealthCare.MVC.Repositories;
using HealthCare.MVC.Repositories.IRepositories;
using HealthCare.MVC.Services;
using HealthCare.MVC.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.MVC.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HealthCareContext>(option => option.UseSqlServer(configuration.GetConnectionString("HealthCare")));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            #region entity
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<INoteRepository, NoteRepository>();
            services.AddTransient<INoteService, NoteService>();

            services.AddTransient<IAgentRepository, AgentRepository>();
            services.AddTransient<IAgentService, AgentService>();

            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ICustomerService, CustomerService>();

            services.AddTransient<IAsignRepository, AsignRepository>();
            services.AddTransient<IAsignService, AsignService>();
            #endregion
            return services;
        }
    }
}
