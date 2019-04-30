using Microsoft.Extensions.Configuration;

namespace Engaze.Evento.ViewDataUpdater.Persistance
{
    public class CassandraConfiguration
    {
        public string ContactPoint { get; private set; }
        public int Port { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public string KeySpace { get; set; }

        public CassandraConfiguration(IConfigurationRoot config)
        {
            ContactPoint = "127.0.0.1";
            Port = 9042;
            UserName = "cassandra";
            Password = "password123";
            KeySpace = "engaze";
        }
    }
}
