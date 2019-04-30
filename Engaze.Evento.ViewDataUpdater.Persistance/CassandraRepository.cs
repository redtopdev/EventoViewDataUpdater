using Cassandra;
using Cassandra.Mapping;
using Engaze.Evento.ViewDataUpdater.Contract;
using System;
using System.Threading.Tasks;

namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    public class CassandraRepository : IViewDataRepository
    {
        private CassandraSessionCacheManager sessionCacheManager;
        private Mapper mapper;
        private string keySpace;
        private PreparedStatement ips;

        public CassandraRepository(CassandraSessionCacheManager sessionCacheManager, string keySpace)
        {
            this.sessionCacheManager = sessionCacheManager;
            this.keySpace = keySpace;
        }


        public async Task<string> GetAsync(string id, string keySpace)
        {
            SetSessionAndMapper();

            return await mapper.FirstOrDefaultAsync<string>("SELECT * FROM \"Test\" WHERE id = ?", id);
        }


        public async Task PostAsync(Event @event)
        {
            var session = sessionCacheManager.GetSession(keySpace);
            var ips = session.Prepare(CassandraDML.InsertStatement);
            var statement = ips.Bind(@event.id, @event.userid, @event.name, @event.eventtypeid, @event.description,
                @event.starttime, @event.endtime,
                @event.duration, @event.initiatorid, @event.eventstateid, @event.trackingstateid,
                @event.trackingstoptime, @event.destinationlatitude,
                @event.destinationlongitude, @event.destinationname, @event.destinationaddress, @event.remindertypeid, @event.reminderoffset,
                @event.trackingstartoffset, @event.isrecurring, @event.recurrencefrequencytypeid, @event.recurrencecount, @event.recurrencefrequency,
                @event.recurrencedaysofweek, @event.participantswithacceptancestate);

            await session.ExecuteAsync(statement);
        }

        public async Task DeleteAsync(Guid id)
        {
            SetSessionAndMapper();

            await mapper.DeleteAsync<string>("WHERE id = ?", id);
        }

        private void SetSessionAndMapper()
        {
            var session = sessionCacheManager.GetSession(keySpace);
            var ips = session.Prepare(CassandraDML.InsertStatement);

            mapper = new Mapper(session);
        }
    }
}
