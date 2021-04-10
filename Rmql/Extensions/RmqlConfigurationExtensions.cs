using System;
using Microsoft.Extensions.DependencyInjection;
using Rmql.Utils;

namespace Rmql.Extensions
{
    public static class RmqlConfigurationExtensions
    {
        public static void AddRmql(this IServiceCollection services, Action<RmqlConfigurations> configure)
        {
            var configBuilder = new RmqlConfigurations(services);
            configure(configBuilder);

            services.AddScoped<RmqlClient>();
        }
    }
}