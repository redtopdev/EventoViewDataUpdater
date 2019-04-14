using Cassandra.Mapping;
using System;
using System.Threading.Tasks;

namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    public class CassandraRepository :IViewDataRepository
    {
        private CassandraSessionCacheManager sessionCacheManager;
        private Mapper mapper;

        public CassandraRepository(CassandraSessionCacheManager sessionCacheManager)
        {
            this.sessionCacheManager = sessionCacheManager;
        }

        public async Task<string> GetAsync(string id, string keySpace)
        {
            SetSessionAndMapper(keySpace);

            return await mapper. FirstOrDefaultAsync<string>("SELECT * FROM \"Test\" WHERE id = ?", id);
        }

        public async Task PostAsync(string data, string keySpace)
        {
            SetSessionAndMapper(keySpace);

            await mapper.InsertAsync(data);
        }

        public async Task DeleteAsync(Guid id, string keySpace)
        {
            SetSessionAndMapper(keySpace);

            await mapper.DeleteAsync<string>("WHERE id = ?", id);
        }

        private void SetSessionAndMapper(string keySpace)
        {
            var session = sessionCacheManager.GetSession(keySpace);
            mapper = new Mapper(session);
        }
    }
}
