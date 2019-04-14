using Engaze.Core.MessageBroker.Consumer;
using System;

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
            Console.WriteLine(message);
        }
    }
}
