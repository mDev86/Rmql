using Rmql.Enums;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Rmql.Extensions
{
    public static class RedisExtensions
    {
        public static IRedisDatabase GetDb(this IRedisCacheClient cacheClient, RedisDbEnum redisDb) =>
            redisDb switch
            {
                RedisDbEnum.Db0 => cacheClient.Db0,
                RedisDbEnum.Db1 => cacheClient.Db1,
                RedisDbEnum.Db2 => cacheClient.Db2,
                RedisDbEnum.Db3 => cacheClient.Db3,
                RedisDbEnum.Db4 => cacheClient.Db4,
                RedisDbEnum.Db5 => cacheClient.Db5,
                RedisDbEnum.Db6 => cacheClient.Db6,
                RedisDbEnum.Db7 => cacheClient.Db7,
                RedisDbEnum.Db8 => cacheClient.Db8,
                RedisDbEnum.Db9 => cacheClient.Db9,
                RedisDbEnum.Db10 => cacheClient.Db10,
                RedisDbEnum.Db11 => cacheClient.Db11,
                RedisDbEnum.Db12 => cacheClient.Db12,
                RedisDbEnum.Db13 => cacheClient.Db13,
                RedisDbEnum.Db14 => cacheClient.Db14,
                RedisDbEnum.Db15 => cacheClient.Db15,
                _ => cacheClient.Db0
            };
    }
}