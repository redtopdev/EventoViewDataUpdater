using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Engaze.Evento.ViewDataUpdater.Contract
{
    public class Event
    {
        public Guid id { get; set; }
        public Guid userid { get; set; }
        public Guid initiatorid { get; set; }
        public string name { get; set; }
        public int? eventtypeid { get; set; } = null;
        public string description { get; set; } = null;
        public DateTime? starttime { get; set; } = null;
        public DateTime? endtime { get; set; } = null;
        public int? duration { get; set; } = null;
        public int? eventstateid { get; set; } = null;
        public int? trackingstateid { get; set; } = null;
        public DateTime? trackingstoptime { get; set; } = null;
        public double? destinationlatitude { get; set; } = null;
        public double? destinationlongitude { get; set; } = null;
        public string destinationname { get; set; } = null;
        public string destinationaddress { get; set; } = null;
        public int? remindertypeid { get; set; } = null;
        public double? reminderoffset { get; set; } = null;
        public double? trackingstartoffset { get; set; } = null;
        public bool? isrecurring { get; set; } = null;
        public int? recurrencefrequencytypeid { get; set; } = null;
        public int? recurrencecount { get; set; } = null;
        public int? recurrencefrequency { get; set; } = null;
        public string recurrencedaysofweek { get; set; } = null;
        public List<string> participantswithacceptancestate { get; set; }

        public static Event Map(JObject eventoJObject)
        {

            var @event = new Event()
            {
                id = Guid.NewGuid(),


                name = eventoJObject.Value<string>("Description"),
                description = eventoJObject.Value<string>("Description"),
                eventtypeid = eventoJObject.Value<int>("EventTypeId"),
                eventstateid = eventoJObject.Value<int>("EventType"),

                starttime = eventoJObject.Value<DateTime>("StartTime"),
                endtime = eventoJObject.Value<DateTime>("EndTime"),
                trackingstateid = eventoJObject.Value<int>("TrackingStateId"),
                trackingstoptime = eventoJObject.Value<DateTime>("TrackingStopTime"),
                trackingstartoffset = eventoJObject.Value<double>("TrackingStartOffset"),

                isrecurring = eventoJObject.Value<bool>("IsRecurring"),
                recurrencefrequencytypeid = eventoJObject.Value<int>("RecurrenceFrequencyTypeid"),
                recurrencecount = eventoJObject.Value<int>("RecurrenceCount"),
                recurrencefrequency = eventoJObject.Value<int>("RecurrenceFrequency"),
                recurrencedaysofweek = eventoJObject.Value<string>("RecurrenceDaysOfweek")

            };
            JToken value;
            if (eventoJObject.TryGetValue("ReminderTypeId", out value))
            {
                @event.remindertypeid = (int)value;
            }

            if (eventoJObject.TryGetValue("Destination", out value))
            {
                var destination = (JObject)value;
                if (destination.TryGetValue("Latitude", out value))
                {
                    @event.destinationlatitude = (double)value;
                }
                if (destination.TryGetValue("Longitude", out value))
                {
                    @event.destinationlongitude = (double)value;
                }
                if (destination.TryGetValue("Name", out value))
                {
                    @event.destinationname = value.ToString();
                }
                if (destination.TryGetValue("Address", out value))
                {
                    @event.destinationaddress = value.ToString();
                }
            }
            //reminderoffset = eventoJObject.Value<double?>("ReminderOffset"),





            @event.userid = Guid.Parse(eventoJObject.Value<string>("InitiatorId"));
            @event.initiatorid = Guid.Parse(eventoJObject.Value<string>("InitiatorId"));

            return @event;

            //   duration = eventoJObject.Value<string>("EventType"),
            //   destinationlatitude = eventoJObject.Value<string>("EventType"),
            //  destinationlongitude = eventoJObject.Value<string>("EventType"),
            //   destinationname = eventoJObject.Value<string>("EventType"),
            //   destinationaddress = eventoJObject.Value<string>("EventType"),
        }
    }
}
