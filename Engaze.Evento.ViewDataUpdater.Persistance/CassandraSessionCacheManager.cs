using Cassandra;
using System;
using System.Collections.Concurrent;

namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    public class CassandraSessionCacheManager
    {
        private Cluster cassandraCluster;
        private ConcurrentDictionary<string, Lazy<ISession>> sessions = new ConcurrentDictionary<string, Lazy<ISession>>();

        public  CassandraSessionCacheManager(Cluster cassandraCluster)
        {
            this.cassandraCluster = cassandraCluster;
        }

        public ISession GetSession(string keyspaceName)
        {
            if (!sessions.ContainsKey(keyspaceName))
                sessions.GetOrAdd(keyspaceName, key => new Lazy<ISession>(() =>
            cassandraCluster.Connect(key)));

            var result = sessions[keyspaceName];

            return result.Value;
        }
    }
}
