using Engaze.Core.DataContract;
using Engaze.Core.MessageBroker.Consumer;
using Engaze.Evento.ViewDataUpdater.Persistance;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Engaze.Evento.ViewDataUpdater.Service
{
    public class EventoMessageHadler : IMessageHandler
    {
        private IViewDataRepository repo;

        public EventoMessageHadler(IViewDataRepository cassandraRepository)
        {
            this.repo = cassandraRepository;
        }
        public void OnError(string error)
        {
            Console.WriteLine(error);
        }

        public async Task OnMessageReceivedAsync(string message)
        {
            try
            {

                await ProcessMessage(JsonConvert.DeserializeObject<EventStoreEvent>(message));


            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception ocured :" + ex.ToString());
            }
        }

        private async Task ProcessMessage(EventStoreEvent  eventStoreEvent)
        {           

            switch (eventStoreEvent.EventType)
            {
                case OccuredEventType.EventoCreated:
                    JsonSerializer serializer = new JsonSerializer();
                    Event @event = JsonConvert.DeserializeObject<Event>(eventStoreEvent.Data);                    
                    if (@event != null)
                    {
                        await this.repo.InsertAsync(@event);
                        /*foreach (Participant participant in @event.Participants)
                        {
                            @event.User = participant.UserId;
                            await this.repo.InsertAsync(@event);
                        }*/
                    }
                    break;
                case OccuredEventType.EventoDeleted:
                    await this.repo.DeleteAsync(eventStoreEvent.EventId);
                    break;

                case OccuredEventType.EventoEnded:
                    await this.repo.EndEventAsync(eventStoreEvent.EventId);
                    break;

                case OccuredEventType.EventoExtended:
                    await this.repo.ExtendEventAsync(eventStoreEvent.EventId, JsonConvert.DeserializeObject<DateTime>(eventStoreEvent.Data));
                    break;

                case OccuredEventType.ParticipantLeft:
                    await this.repo.DeleteAsync(eventStoreEvent.EventId);
                    break;

                case OccuredEventType.ParticipantsListUpdated:
                    await this.repo.DeleteAsync(eventStoreEvent.EventId);
                    break;

                case OccuredEventType.ParticipantStateUpdated:
                    await this.repo.UpdateParticipantStateAsync(eventStoreEvent.EventId,
                        JsonConvert.DeserializeObject<Guid>(eventStoreEvent.Data), 0);
                    break;

                default:
                    break;

            }
        }
    }
}
