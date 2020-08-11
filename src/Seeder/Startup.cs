using Microsoft.Extensions.Configuration;
using System;
using Application;
using Microsoft.Extensions.DependencyInjection;
using Seeder.Options;
using Serilog;
using Shared;
using Infrastructure;

namespace Seeder
{
  public  class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static ServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            var config = LoadConfiguration(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            services.AddSingleton(config);

            services.AddApplication();
            services.AddShared();
            services.AddInfrastructure();

            services.AddPapercutOptions(config);
            services.AddUserOptions(config);
            services.AddRetryOptions(config);

            services.AddSerilog(config, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            services.AddLogging(configure => configure
                .AddSerilog()
            );

            services.AddTransient<App>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    

        public static IConfiguration LoadConfiguration(string environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);


            return builder.Build();
        }
  }
}
