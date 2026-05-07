using DevOpsProject.CommunicationControl.Logic.Services.Interfaces;
using DevOpsProject.CommunicationControl.Logic.Services;
using DevOpsProject.Shared.Configuration;
using StackExchange.Redis;

namespace DevOpsProject.CommunicationControl.API.DI
{
    public static class RedisConfiguration
    {
                public static IServiceCollection AddRedis(this IServiceCollection serviceCollection, IConfiguration configuration, string? connectionString = null)
        {
            var redisConfiguration = configuration.GetSection("Redis").Get<RedisOptions>();
            var connStr = connectionString ?? redisConfiguration.ConnectionString;
            
            Console.WriteLine($"CONNECTING TO REDIS: {connStr}");
            
            var redis = ConnectionMultiplexer.Connect(connStr);

            serviceCollection.AddSingleton<IConnectionMultiplexer>(redis);

            serviceCollection.Configure<RedisOptions>(
                configuration.GetSection("Redis"));

            serviceCollection.Configure<RedisKeys>(
                configuration.GetSection("RedisKeys"));

            serviceCollection.AddTransient<IRedisKeyValueService, RedisKeyValueService>();
            serviceCollection.AddTransient<IPublishService, RedisPublishService>();

            return serviceCollection;
        }
    }
}
