using Engaze.Core.MessageBroker.Consumer;
using Engaze.Evento.ViewDataUpdater.Contract;
using Engaze.Evento.ViewDataUpdater.Persistance;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using static Engaze.Evento.ViewDataUpdater.Contract.Event;

namespace Engaze.Evento.ViewDataUpdater.Service
{
    public class EventoMessageHadler : IMessageHandler
    {
        private CassandraRepository repo;

        public EventoMessageHadler(CassandraRepository cassandraRepository)
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
                await ProcessMessage(JObject.Parse(message));


            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception ocured :" + ex.ToString());
            }
        }

        private async Task ProcessMessage(JObject msgJObject)
        {
            string eventType = msgJObject.Value<string>("EventType");
            JObject eventoObject = msgJObject.Value<JObject>("Data");
            Guid eventId = Guid.Empty;

            switch (eventType)
            {
                case "EventoCreated":
                    Event @event = Event.Map(eventoObject);
                    if (@event != null)
                    {
                        foreach (ParticipantsWithStatus participnat in @event.Participants)
                        {
                            @event.userid = participnat.UserId;
                            await this.repo.InsertAsync(@event);
                        }
                    }
                    break;
                case "EventoDeleted":
                    await this.repo.DeleteAsync(Guid.Parse(eventoObject.Value<string>("EventoId")));
                    break;

                case "EventoEnded":                   
                    await this.repo.EndEventAsync(Guid.Parse(eventoObject.Value<string>("EventoId")));
                    break;

                case "EventoExtended":
                    await this.repo.ExtendEventAsync(Guid.Parse(eventoObject.Value<string>("EventoId")), eventoObject.Value<DateTime>("EndTime"));
                    break;

                case "ParticipantLeft":
                    await this.repo.DeleteAsync(Guid.Parse(eventoObject.Value<string>("EventoId")));
                    break;

                case "ParticipantsListUpdated":
                    await this.repo.DeleteAsync(Guid.Parse(eventoObject.Value<string>("EventoId")));
                    break;

                case "ParticipantStateUpdated":
                    await this.repo.DeleteAsync(Guid.Parse(eventoObject.Value<string>("EventoId")));
                    break;

                default:
                    break;

            }
        }
    }
}
