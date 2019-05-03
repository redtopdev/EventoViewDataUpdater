using Cassandra.Mapping;
using Engaze.Evento.ViewDataUpdater.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    public class CassandraRepository : IViewDataRepository
    {
        private CassandraSessionCacheManager sessionCacheManager;
        private Mapper mapper;
        private string keySpace;

        public CassandraRepository(CassandraSessionCacheManager sessionCacheManager, string keySpace)
        {
            this.sessionCacheManager = sessionCacheManager;
            this.keySpace = keySpace;
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
            var ids = await GetAffectedUserIdList(eventId);
            var session = sessionCacheManager.GetSession(keySpace);
            await session.ExecuteAsync(session.Prepare(CassandraDML.eventDeleteStatement).Bind(eventId, ids));
        }

        public async Task ExtendEventAsync(Guid eventId, DateTime endTime)
        {
            var ids = await GetAffectedUserIdList(eventId);
            var session = sessionCacheManager.GetSession(keySpace);
            await session.ExecuteAsync(session.Prepare(CassandraDML.eventUpdateEndDateStatement).Bind(endTime, eventId, ids));
        }

        public async Task EndEventAsync(Guid eventId)
        {
            var ids = await GetAffectedUserIdList(eventId);
            DateTime endTime = DateTime.Now;
            var session = sessionCacheManager.GetSession(keySpace);
            await session.ExecuteAsync(session.Prepare(CassandraDML.eventUpdateEndDateStatement).Bind(endTime, eventId, ids));
        }

        public Task ParticipantStateUpdated(Guid eventId, Guid participantId, int stateId)
        {
            throw new NotImplementedException();
        }

        private void SetSessionAndMapper()
        {
            var session = sessionCacheManager.GetSession(keySpace);
            var ips = session.Prepare(CassandraDML.InsertStatement);

            mapper = new Mapper(session);
        }

        private async Task<IEnumerable<Guid>> GetAffectedUserIdList(Guid eventId)
        {
            var session = sessionCacheManager.GetSession(keySpace);
            var result = await session.ExecuteAsync(session.Prepare(CassandraDML.SelectUserIdStatement).Bind(eventId));
            return result.GetRows().Select(row => row.GetValue<Guid>("userid"));
        }
    }
}
