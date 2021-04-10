using Microsoft.Extensions.DependencyInjection;

namespace Rmql.Utils
{
    public class RmqlConfigurations
    {
        private readonly IServiceCollection _serviceCollection;
        
        public RmqlConfigurations(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public void UseWorker<T, TModel>() 
            where T : RmqlWorker<TModel>
            where TModel: class
        {
            _serviceCollection.AddHostedService<RmqlBackgroundService<T, TModel>>();
        }
    }
}