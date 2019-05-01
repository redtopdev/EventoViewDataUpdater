using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
        public List<ParticipantsWithStatus> Participants { get; set; }
        public string participantswithacceptancestate { get; set; }


        public class ParticipantsWithStatus
        {
            public Guid UserId { get; set; }
            public int AcceptanceState { get; set; }
        }

        public static Event Map(JObject eventoJObject)
        {
            List<Event> eventWithSeperateUser = new List<Event>();
            JToken jToken;
            if (eventoJObject.TryGetValue("Participants", out jToken))
            {

                var participants = (JArray)jToken;

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
                    trackingstartoffset = eventoJObject.Value<double>("TrackingStartOffset")

                };

                JToken value;
                if (eventoJObject.TryGetValue("ReminderTypeId", out value))
                {
                    @event.remindertypeid = (int)value;
                }

                @event.Participants = participants.ToObject<List<ParticipantsWithStatus>>();
                @event.participantswithacceptancestate = JsonConvert.SerializeObject(@event.Participants);
                //@event.participantswithacceptancestate = participants.ToString();


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

                if (eventoJObject.TryGetValue("IsRecurring", out value) && (bool)value)
                {
                    @event.isrecurring = true;
                    @event.isrecurring = eventoJObject.Value<bool>("IsRecurring");
                    @event.recurrencefrequencytypeid = eventoJObject.Value<int>("RecurrenceFrequencyTypeid");
                    @event.recurrencecount = eventoJObject.Value<int>("RecurrenceCount");
                    @event.recurrencefrequency = eventoJObject.Value<int>("RecurrenceFrequency");
                    @event.recurrencedaysofweek = eventoJObject.Value<string>("RecurrenceDaysOfweek");
                }

                @event.initiatorid = Guid.Parse(eventoJObject.Value<string>("InitiatorId"));
                //string eventString = JsonConvert.SerializeObject(@event);

                //foreach (JObject participant in participants)
                //{
                //    @event = JsonConvert.DeserializeObject<Event>(eventString);
                //    @event.userid = Guid.Parse((string)participant.GetValue("UserId"));
                //    eventWithSeperateUser.Add(@event);
                //}
                return @event;
            }

            return null;
        }
    }
}
