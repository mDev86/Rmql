using System;
using System.Threading.Tasks;
using Rmql.Extensions;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Rmql
{
    /// <summary>
    /// Client for sending message in queue
    /// </summary>
    public class RmqlClient
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public RmqlClient(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }
        
        /// <summary>
        /// Send message in queue
        /// </summary>
        /// <param name="message">Message for sending</param>
        /// <typeparam name="T">Worker configuration class</typeparam>
        /// <typeparam name="TModel">Message DTO class</typeparam>
        /// <returns></returns>
        public async Task PushAsync<T, TModel>(TModel message)
            where T: RmqlWorker<TModel> where TModel : class
        {
            var config = Activator.CreateInstance<T>();
            await _redisCacheClient
                .GetDb(config.RedisDb)
                .ListAddToLeftAsync(config.Key, message);
        }
    }
}