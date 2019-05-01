namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    internal class CassandraDML
    {
        internal static string InsertStatement = "INSERT INTO ez_event " +
            "(id, userid, name, eventtypeid, description, " +
            "starttime, endtime, " +
            "duration, initiatorid, eventstateid, trackingstateid, " +
            "trackingstoptime, " +
            "destinationlatitude, destinationlongitude, destinationname, destinationaddress, remindertypeid, reminderoffset," +
            " trackingstartoffset, isrecurring, recurrencefrequencytypeid, recurrencecount, recurrencefrequency, recurrencedaysofweek, participants) " +
            "values " +
            "(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";
    }
}
