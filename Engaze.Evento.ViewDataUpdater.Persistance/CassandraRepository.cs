using Cassandra;
using Cassandra.Mapping;
using Engaze.Evento.ViewDataUpdater.Contract;
using System;
using System.Linq;
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


        public async Task InsertAsync(Event @event)
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

        public async Task DeleteAsync(Guid eventId)
        {
            var session = sessionCacheManager.GetSession(keySpace);
            var ips = session.Prepare(CassandraDML.SelectUserIdStatement);
            var statement = ips.Bind(eventId);

            var result = await session.ExecuteAsync(statement);
            var rows = result.GetRows().ToList();
            if (rows.Count > 0)
            {
                var batch = new BatchStatement();
                PreparedStatement dps = null;
               
                foreach (var row in rows)
                {
                    dps = session.Prepare(CassandraDML.eventDeleteStatement);                  
                    batch.Add(dps.Bind(eventId, row.GetValue<Guid>("userid")));                   
                }

                session.Execute(batch);
            }
        }

        public Task ExtendEventAsync(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public Task ParticipantStateUpdated(Guid eventId)
        {
            throw new NotImplementedException();
        }



        private void SetSessionAndMapper()
        {
            var session = sessionCacheManager.GetSession(keySpace);
            var ips = session.Prepare(CassandraDML.InsertStatement);

            mapper = new Mapper(session);
        }
    }
}
