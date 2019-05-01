namespace Engaze.Evento.ViewDataUpdater.Service
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using Engaze.Core.MessageBroker.Consumer;
    using Cassandra;
    using Engaze.Evento.ViewDataUpdater.Persistance;

    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Service is starting..");

            new HostBuilder().ConfigureHostConfiguration(configHost =>
            {
                configHost.AddCommandLine(args);
                configHost.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureAppConfiguration((hostContext, configApp) =>
            {
                hostContext.HostingEnvironment.EnvironmentName = System.Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            })
             .ConfigureLogging((hostContext, configLogging) =>
             {
                 if (hostContext.HostingEnvironment.IsDevelopment())
                 {
                     configLogging.AddConsole();
                     configLogging.AddDebug();
                 }

             }).ConfigureServices((hostContext, services) =>
             {
                 services.AddLogging();
                 ConfigureCassandra(services);
                 services.AddSingleton(typeof(IMessageHandler), typeof(EventoMessageHadler));
                 services.AddHostedService<EventoConsumer>();

             })
             .RunConsoleAsync();

            Console.ReadLine();
        }

        private static void ConfigureCassandra(IServiceCollection services)
        {
            CassandraConfiguration cc = new CassandraConfiguration(null);
            var cluster = Cluster.Builder()
            .AddContactPoint(cc.ContactPoint)
            .WithPort(cc.Port)
            .WithCredentials(cc.UserName, cc.Password)
            .Build();

            CassandraRepository cr = new CassandraRepository(
                new CassandraSessionCacheManager(cluster), cc.KeySpace);
            services.AddSingleton(cr.GetType(), cr);

            Console.WriteLine("Service running");
        }
    }
}


