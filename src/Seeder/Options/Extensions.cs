using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Seeder.Options
{
    public static class Extensions
    {
        public static void AddRetryOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new RetryPolicyOptions();
            var section = configuration.GetSection("RetryPolicy");
            section.Bind(options);
            services.Configure<RetryPolicyOptions>(section);
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
        public static IServiceCollection AddSerilog(this IServiceCollection services, IConfiguration configuration, string environment)
        {
            var template = "{Timestamp:yyyy-MM-dd HH:mm:ss.ffffff}::{Level:u4}:{SourceContext} - {Message} {Exception}{NewLine}";

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.File(path: Path.GetFullPath("logs/log.log"), restrictedToMinimumLevel: LogEventLevel.Debug, outputTemplate: template, fileSizeLimitBytes: 5242880,
                    rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true, retainedFileCountLimit: 5)
                .CreateLogger();

            return services;
        }
    }
}
