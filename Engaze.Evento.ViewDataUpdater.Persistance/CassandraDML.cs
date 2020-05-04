namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    internal class CassandraDML
    {
        internal static string InsertEventData = "INSERT INTO EventData " +
            "(EventId, StartTime, EndTime, EventDetails)" +
            "values " +
            "(?,?,?,?);";

        internal static string InsertEventParticipantMapping = "INSERT INTO EventParticipantMapping " +
         "(UserId ,EventId)" +
        "values " +
        "(?,?);";

        internal static string SelectEventParticipantMapping = "SELECT UserId FROM EventParticipantMapping WHERE EventId=? ALLOW FILTERING;";

        internal static string SelectUserIdStatement = "select userid from ez_event " +
            "WHEre id=? ALLOW FILTERING";

        internal static string SelectParticipantsStatement = "select participants from ez_event " +
          "WHEre id=? ALLOW FILTERING";

        internal static string eventDeleteStatement = "Delete from EventData where EventId=?;";
        internal static string DeleteEventParticipantMappings = "Delete from EventParticipantMapping where UserId = ? AND EventId=?;";

        internal static string ExtendEventStatement= "UPDATE EventData SET endtime = ? WHERE EventID=?;";
        
        internal static string EndEventStatement = "UPDATE EventData SET endtime = ? WHERE EventID=?;";

        internal static string UpdateParticipantStateStatement = "UPDATE EventData SET EventDetails = ? WHERE EventID=?;";

        internal static string eventUpdateParticipantsStatement = "update ez_event set participants =? where id=? and userid IN ?;";
    }
}
