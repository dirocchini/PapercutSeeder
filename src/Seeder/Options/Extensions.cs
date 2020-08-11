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
