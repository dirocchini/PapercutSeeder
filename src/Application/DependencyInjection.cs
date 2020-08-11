using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Options;

namespace Application
{
   public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
           

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
