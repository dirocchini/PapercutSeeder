
using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Options;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IServerCommandOperations, ServerCommandProxyService>();
            services.AddSingleton<IUserOperations, UserService>();
            services.AddSingleton<IPrinterOperations, PrinterService>();
            services.AddSingleton<ISharedAccountOperations, SharedAccountService>();
            services.AddSingleton<IOfficeOperations, OfficeService>();
            services.AddSingleton<IDepartmentOperations, DepartmentService>();

            return services;
        }

        public static void AddRetryOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new RetryPolicyOptions();
            var section = configuration.GetSection("RetryPolicy");
            section.Bind(options);
            services.Configure<RetryPolicyOptions>(section);
        }
    }
}
