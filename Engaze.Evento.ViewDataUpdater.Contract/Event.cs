using System;

namespace Engaze.Evento.ViewDataUpdater.Contract
{
    public class Event
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string eventData { get; set; }
    }
}
