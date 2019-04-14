using Cassandra;
using Microsoft.Extensions.DependencyInjection;

namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    class DataUpdaterConfiguration
    {
        public void ConfigureServices(IServiceCollection services)
        {
            CassandraConfiguration  cassandraConfig= new CassandraConfiguration(null);
            var cluster = Cluster.Builder()
                .AddContactPoint(cassandraConfig.ContactPoint)
                .WithPort(cassandraConfig.Port)
                .WithCredentials(cassandraConfig.UserName, cassandraConfig.Password)
                .Build();
            CassandraSessionCacheManager cassandraSessionCacheManager = new CassandraSessionCacheManager(cluster);
            services.AddSingleton(cluster.GetType(), cluster);
            services.AddSingleton(cassandraSessionCacheManager.GetType(), cassandraSessionCacheManager);
        }
    }
}
