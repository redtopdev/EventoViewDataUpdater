using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Engaze.Evento.ViewDataUpdater.Contract
{
    public class Event
    {
        public Guid id { get; set; }
        public Guid userid { get; set; }
        public string name { get; set; }
        public int eventtypeid { get; set; }
        public string description { get; set; }
        public DateTime starttime { get; set; }
        public DateTime endtime { get; set; }
        public int duration { get; set; }
        public int initiatorid { get; set; }
        public int eventstateid { get; set; }
        public int trackingstateid { get; set; }
        public DateTime trackingstoptime { get; set; }
        public double destinationlatitude { get; set; }
        public double destinationlongitude { get; set; }
        public string destinationname { get; set; }
        public string destinationaddress { get; set; }
        public int remindertypeid { get; set; }
        public double reminderoffset { get; set; }
        public double trackingstartoffset { get; set; }
        public bool isrecurring { get; set; }
        public int recurrencefrequencytypeid { get; set; }
        public int recurrencecount { get; set; }
        public int recurrencefrequency { get; set; }
        public string recurrencedaysofweek { get; set; }
        List<Guid> participants { get; set; }

        public static Event Map(JObject eventoJObject)
        {
          
            var @event = new Event()
            {

                id = Guid.NewGuid(),
                userid = eventoJObject.Value<Guid>("InitiatorId "),
                name = eventoJObject.Value<string>("Description"),
                eventtypeid = eventoJObject.Value<int>("EventTypeId"),
                description = eventoJObject.Value<string>("Description"),
                starttime = eventoJObject.Value<DateTime>("StartTime "),
                endtime = eventoJObject.Value<DateTime>("EndTime "),
            };



            //   duration = eventoJObject.GetValue("EventType").ToString(),
            //initiatorid = eventoJObject.GetValue("EventType").ToString(),
            //  eventstateid = eventoJObject.GetValue("EventType").ToString(),
            //  trackingstateid = eventoJObject.GetValue("EventType").ToString(),
            //   trackingstoptime = eventoJObject.GetValue("EventType").ToString(),
            //   destinationlatitude = eventoJObject.GetValue("EventType").ToString(),
            //  destinationlongitude = eventoJObject.GetValue("EventType").ToString(),
            //   destinationname = eventoJObject.GetValue("EventType").ToString(),
            //   destinationaddress = eventoJObject.GetValue("EventType").ToString(),
            //   remindertypeid = eventoJObject.GetValue("EventType").ToString(),
            //    reminderoffset = eventoJObject.GetValue("EventType").ToString(),
            //    trackingstartoffset = eventoJObject.GetValue("EventType").ToString(),
            //    isrecurring = eventoJObject.GetValue("EventType").ToString(),
            //  recurrencefrequencytypeid = eventoJObject.GetValue("EventType").ToString(),
            //  recurrencecount = eventoJObject.GetValue("EventType").ToString(),
            //   recurrencefrequency = eventoJObject.GetValue("EventType").ToString(),
            // recurrencedaysofweek = eventoJObject.GetValue("EventType").ToString()


            return @event;
        }
    }
}
