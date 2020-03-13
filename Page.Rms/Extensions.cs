using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Page.Rms
{
    public static class Extensions
    {
        /// <summary>
        /// 注入RMS告警服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRms(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddSingleton(new RmsOptions
            {
                ProjectCode = configuration["Rms:ProjectCode"],
                ProjectName = configuration["Rms:ProjectName"],
                Key = configuration["Rms:Key"],
                RestoreUrl = configuration["Rms:RestoreUrl"],
                NoticeUrl = configuration["Rms:NoticeUrl"]
            });

            serviceCollection.AddSingleton(provider =>
            {
                var options = provider.GetService<RmsOptions>();
                var clientFactory = provider.GetService<IHttpClientFactory>();
                return new Rms(options, clientFactory);
            });
            return serviceCollection;
        }

        /// <summary>
        /// 注入RMS告警服务
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="options"></param>
        /// <param name="clientFactory"></param>
        /// <returns></returns>
        public static IServiceCollection AddRms(this IServiceCollection serviceCollection, RmsOptions options,
            IHttpClientFactory clientFactory)
        {
            serviceCollection.AddSingleton(options);
            serviceCollection.AddSingleton(new Rms(options, clientFactory));
            return serviceCollection;
        }
    }
}