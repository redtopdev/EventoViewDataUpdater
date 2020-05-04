namespace Engaze.Evento.ViewDataUpdater.Service
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    using Engaze.Core.MessageBroker.Consumer;
    using Engaze.Core.Persistance.Cassandra;
    using Engaze.Evento.ViewDataUpdater.Persistance;

    class Program
    {
        private static string ServiceName = "Evento ViewUpdater service";
        public static void Main(string[] args)
        {
            Console.WriteLine(ServiceName + " is starting..");

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
                 services.ConfigureCloudCassandra(hostContext.Configuration);
                 var sp = services.BuildServiceProvider();
                 CassandraRepository repo = new CassandraRepository(sp.GetService<CassandraSessionCacheManager>(), sp.GetService<CassandraConfiguration>());
                 services.AddSingleton(typeof(IViewDataRepository), repo);
                 services.ConfigureConsumerService(hostContext.Configuration, new EventoMessageHadler(repo));
                 services.AddHostedService<KafkaConsumer>();

             })
             .RunConsoleAsync();

            Console.WriteLine(ServiceName + " started");
            Console.ReadLine();
        }
    }
}


