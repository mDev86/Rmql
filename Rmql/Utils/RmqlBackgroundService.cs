using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rmql.Enums;
using Rmql.Extensions;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Rmql.Utils
{
    class RmqlBackgroundService<T, TModel>: BackgroundService
        where T: RmqlWorker<TModel>
        where TModel: class
    {
        private readonly IServiceScope _serviceScope;
        private readonly RmqlWorker<TModel> _configurations;
        private readonly IRedisDatabase _redisDatabase;
        
        public RmqlBackgroundService(IServiceProvider serviceProvider)
        {
            _configurations = Activator.CreateInstance<T>();
            
            _serviceScope = serviceProvider.CreateScope();
            _redisDatabase = _serviceScope.ServiceProvider
                .GetRequiredService<IRedisCacheClient>()
                .GetDb(_configurations.RedisDb);
        }

        protected sealed override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_configurations.Frequency, stoppingToken);
                try
                {
                    var value = GetValueByKey();
                    while (value.HasValue)
                    {
                        var model = JsonSerializer.Deserialize<TModel>(value);            
                        await _configurations.ActionAsync(_serviceScope, model);
                
                        value = GetValueByKey();
                    }
                }
                catch (Exception e)
                {
                    await _configurations.LogErrorAsync(e);
                }
            }
        }
        
        public sealed override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public sealed override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        private RedisValue GetValueByKey() => 
            _configurations.Type == QueueType.LIFO
                ? _redisDatabase.Database.ListLeftPop(_configurations.Key)
                : _redisDatabase.Database.ListRightPop(_configurations.Key);
    }
}