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

        internal static string SelectUserIdStatement = "select userid from ez_event " +
            "WHEre id=? ALLOW FILTERING";

        internal static string eventDeleteStatement = "Delete from ez_event where id=? and userid=? IF EXISTS";
    }
}
