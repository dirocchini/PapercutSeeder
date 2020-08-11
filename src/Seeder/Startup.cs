using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PaperCut;
using Seeder.Interfaces;
using Seeder.Options;
using Seeder.Services;
using Serilog;

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

            LoadDependencies(services);

            services.AddRetryOptions(config);
            services.AddPapercutOptions(config);
            services.AddUserOptions(config);

            services.AddSerilog(config, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            services.AddLogging(configure => configure
                .AddSerilog()
            );

            services.AddTransient<App>();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
        private static void LoadDependencies(IServiceCollection services)
        {
            services.AddSingleton<IServerCommandOperations, ServerCommandProxyService>();
            services.AddSingleton<IUserOperations, UserService>();
            services.AddSingleton<IPrinterOperations, PrinterService>();
            services.AddSingleton<ISharedAccountOperations, SharedAccountService>();
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
