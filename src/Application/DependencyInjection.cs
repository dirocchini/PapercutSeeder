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

    }
}
