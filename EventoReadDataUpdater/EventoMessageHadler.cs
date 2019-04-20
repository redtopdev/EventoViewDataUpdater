using Engaze.Core.MessageBroker.Consumer;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace Engaze.Evento.ViewDataUpdater.Service
{
    public class EventoMessageHadler : IMessageHandler
    {
        public void OnError(string error)
        {
            Console.WriteLine(error);
        }

        public void OnMessageReceived(string message)
        {
            //string data = Encoding.ASCII.GetString();
            JObject eventoJObject = JObject.Parse(message);
            string eventType = eventoJObject.GetValue("EventType").ToString();
            if (eventType.Equals("4"))
            {

            }

        }
    }
}
