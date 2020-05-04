using Engaze.Core.DataContract;
using Engaze.Core.Persistance.Cassandra;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    public class CassandraRepository : IViewDataRepository
    {
        private CassandraSessionCacheManager sessionCacheManager;    
        private string keySpace;

        public CassandraRepository(CassandraSessionCacheManager sessionCacheManager, CassandraConfiguration cassandrConfig)
        {
            this.sessionCacheManager = sessionCacheManager;
            this.keySpace = cassandrConfig.KeySpace;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task InsertAsync(Event @event)
        {
            await InsertAsyncEventData(@event);
            InsertEventParticipantMapping(@event);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        private async Task InsertAsyncEventData(Event @event)
        {
            var session = sessionCacheManager.GetSession(keySpace);            
            string eventJson = JsonConvert.SerializeObject(@event);
            string insertEventData = "INSERT INTO EventData " +
            "(EventId, StartTime, EndTime, EventDetails)" +
            "values " +
            "(" + @event.EventId + "," + @event.StartTime + "," + @event.EndTime + ",'" + eventJson + "');";
            var ips = session.Prepare(insertEventData);
            var statement = ips.Bind();
            await session.ExecuteAsync(statement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        private void InsertEventParticipantMapping(Event @event)
        {
            var session = sessionCacheManager.GetSession(keySpace);
            List<Guid> participantList = GetEventParticipantsList(@event).ToList();
            participantList.ForEach(async participant =>
            {
                string insertEventParticipantMapping = "INSERT INTO EventParticipantMapping " +
                         "(UserId ,EventId)" +
                        "values " +
                        "("+ participant + ","+ @event.EventId + ");";
                var ips = session.Prepare(insertEventParticipantMapping);
                string eventJson = JsonConvert.SerializeObject(@event);
                var statement = ips.Bind();
                await session.ExecuteAsync(statement);
            }
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        private IEnumerable<Guid> GetEventParticipantsList(Event @event)
        {
            IEnumerable<Guid> participantsList = @event.Participants.Select(x=>x.UserId).Append(@event.InitiatorId);
            return participantsList.Distinct();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid eventId)
        {
            await DeleteAsyncEventData(eventId);
            DeleteEventParticipantMapping(eventId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task DeleteAsyncEventData(Guid eventId)
        {
            var session = sessionCacheManager.GetSession(keySpace);
            string eventDeleteStatement = "Delete from EventData where EventId="+ eventId + ";";
            await session.ExecuteAsync(session.Prepare(eventDeleteStatement).Bind(eventId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        public void DeleteEventParticipantMapping(Guid eventId)
        {
            List<Guid> participantList = GetEventParticipantsList(eventId).ToList();
            var session = sessionCacheManager.GetSession(keySpace);
            participantList.ForEach(async participant =>
            {
                string deleteEventParticipantMappings = "Delete from EventParticipantMapping where UserId = "+ participant + " AND EventId="+ eventId + ";";
                var ips = session.Prepare(deleteEventParticipantMappings);
                var statement = ips.Bind(participant, eventId);
                await session.ExecuteAsync(statement);
            }
            );
        }


        /// <summary>
        /// gets events by userId
        /// </summary>
        /// <param name="userid">userId</param>
        /// <returns>list of event</returns>
        private IEnumerable<Guid> GetEventParticipantsList(Guid eventId)
        {
            var session = sessionCacheManager.GetSession(keySpace);
            string query = "SELECT UserId FROM EventParticipantMapping WHERE EventId=" + eventId.ToString() + "ALLOW FILTERING;";
            var preparedStatement = session.Prepare(query);
            var resultSet = session.Execute(preparedStatement.Bind());
            return resultSet.Select(row => row.GetValue<Guid>("userid"));
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task ExtendEventAsync(Guid eventId, DateTime endTime)
        {
            string datetimeUtcIso8601 = endTime.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz");
            string extendEventStatement = "UPDATE EventData SET endtime = '"+ datetimeUtcIso8601 + "' WHERE EventID="+ eventId + ";";
            var session = sessionCacheManager.GetSession(keySpace);
            await session.ExecuteAsync(session.Prepare(extendEventStatement).Bind());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task EndEventAsync(Guid eventId)
        {
            string datetimeUtcIso8601 = DateTime.UtcNow.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz");
            string endEventStatement = "UPDATE EventData SET endtime = '" + datetimeUtcIso8601 + "' WHERE EventID=" + eventId + ";";
            var session = sessionCacheManager.GetSession(keySpace);
            await session.ExecuteAsync(session.Prepare(endEventStatement).Bind());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="participantId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public async Task UpdateParticipantStateAsync(Guid eventId, Guid participantId, int stateId)
        {
            Event @event = GetEvent(eventId);
            Participant participant = new Participant(participantId, (EventAcceptanceState)stateId);

            @event.Participants  = @event.Participants.Select(p => p.UserId == participantId ? participant : p);
            string eventJson = JsonConvert.SerializeObject(@event);

            string UpdateParticipantStateStatement = "UPDATE EventData SET EventDetails = '"+eventJson+"' WHERE EventID="+ eventId + ";";
            var session = sessionCacheManager.GetSession(keySpace);
            await session.ExecuteAsync(session.Prepare(UpdateParticipantStateStatement).Bind());

        }

        /// <summary>
        /// gets event by eventId
        /// </summary>
        /// <param name="eventId">eventId</param>
        /// <returns>event</returns>

        private Event GetEvent(Guid eventId)
        {
            string query = "SELECT EventDetails from EventData WHERE EventId=" + eventId.ToString() + ";";
            var sessionL = sessionCacheManager.GetSession(keySpace);
            var preparedStatement = sessionL.Prepare(query);
            var resultSet = sessionL.Execute(preparedStatement.Bind());
            return JsonConvert.DeserializeObject<Event>(resultSet.First().GetValue<string>("eventdetails"));
        }
    }
}
