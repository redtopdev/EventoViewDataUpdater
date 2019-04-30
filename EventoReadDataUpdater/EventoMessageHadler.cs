using Engaze.Core.MessageBroker.Consumer;
using Engaze.Evento.ViewDataUpdater.Contract;
using Engaze.Evento.ViewDataUpdater.Persistance;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading.Tasks;

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
                var eventObject = parseMessage(JObject.Parse(message));
                
                await this.repo.PostAsync(eventObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception ocured :" + ex.ToString());
            }
        }

        private Event parseMessage(JObject eventoJObject)
        {
            string eventType = eventoJObject.Value<string>("EventType");
            switch (eventType)
            {
                case "EventoCreated":
                    return Event.Map(JObject.Parse(eventoJObject.Value<string>("Data")));

                default:
                    return null;
            }
        }
    }
}
