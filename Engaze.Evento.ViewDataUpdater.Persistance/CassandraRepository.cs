using Cassandra.Mapping;
using System;
using System.Threading.Tasks;

namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    public class CassandraRepository :IViewDataRepository
    {
        private CassandraSessionCacheManager sessionCacheManager;
        private Mapper mapper;
        private string keySpace;

        public CassandraRepository(CassandraSessionCacheManager sessionCacheManager, string keySpace)
        {
            this.sessionCacheManager = sessionCacheManager;
            this.keySpace = keySpace;
        }

        public async Task<string> GetAsync(string id, string keySpace)
        {
            SetSessionAndMapper();

            return await mapper. FirstOrDefaultAsync<string>("SELECT * FROM \"Test\" WHERE id = ?", id);
        }

        public async Task PostAsync(string data)
        {
            SetSessionAndMapper();

            await mapper.InsertAsync(data);
        }

        public async Task DeleteAsync(Guid id)
        {
            SetSessionAndMapper();

            await mapper.DeleteAsync<string>("WHERE id = ?", id);
        }

        private void SetSessionAndMapper()
        {
            var session = sessionCacheManager.GetSession(keySpace);
            mapper = new Mapper(session);
        }
    }
}
