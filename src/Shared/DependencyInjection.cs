using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Options;

namespace Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShared(this IServiceCollection services)
        {


            return services;
        }

        public static void AddPapercutOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new PapercutOptions();
            var section = configuration.GetSection("Papercut");
            section.Bind(options);
            services.Configure<PapercutOptions>(section);
        }

        public static void AddUserOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new UserOptions();
            var section = configuration.GetSection("UserOptions");
            section.Bind(options);
            services.Configure<UserOptions>(section);
        }
    }
}
