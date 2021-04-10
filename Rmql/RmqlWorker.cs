using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rmql.Enums;

namespace Rmql
{
    /// <summary>
    /// Worker configuration
    /// </summary>
    /// <typeparam name="T">Message DTO class</typeparam>
    public abstract class RmqlWorker<T>
        where T : class
    {
        /// <summary>
        /// Unique list name for queue message storage
        /// </summary>
        public abstract string Key { get; }

        /// <summary>
        /// Type of queue. Default FIFO
        /// </summary>
        public virtual QueueType Type { get; } = QueueType.FIFO;

        /// <summary>
        /// Delay in milliseconds before check new message in queue. Default 1000ms
        /// </summary>
        public virtual int Frequency { get; } = 1000;

        /// <summary>
        /// Redis db number for queue storage. Default Db0
        /// </summary>
        public virtual RedisDbEnum RedisDb { get; } = RedisDbEnum.Db0;

        /// <summary>
        /// Handling queue message
        /// </summary>
        /// <param name="serviceScope">Scope for DI</param>
        /// <param name="message">Message from queue</param>
        /// <returns></returns>
        public abstract Task ActionAsync(IServiceScope serviceScope, T message);

        /// <summary>
        /// Handling thrown exception
        /// </summary>
        /// <param name="e">Thrown exception</param>
        /// <returns></returns>
        public virtual Task LogErrorAsync(Exception e)
        {
            Debug.WriteLine(e);
            return Task.CompletedTask;
        }
    }
}