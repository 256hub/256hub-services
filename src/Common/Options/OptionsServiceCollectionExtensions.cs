using Hub256.Common.Options;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OptionsServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static void ConfigureAppOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<EndpointOptions>(configuration.GetSection(EndpointOptions.SectionName));
        }

        /// <summary>
        /// 
        /// </summary>
        public static EndpointOptions GetEndpointOptions(this IConfiguration configuration)
        {
            return configuration.GetSection(EndpointOptions.SectionName).Get<EndpointOptions>();
        }
    }
}
