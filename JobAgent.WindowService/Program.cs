using Hangfire;
using Hangfire.SqlServer;
using Serilog;
using System;
using Topshelf;

namespace JobAgent.WindowService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbCS"].ConnectionString;

            var options = new SqlServerStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(30)// Default value
            };

            // Serilog configuration: Log to file
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole(Serilog.Events.LogEventLevel.Debug)
                .WriteTo.ColoredConsole(Serilog.Events.LogEventLevel.Information)
                .WriteTo.ColoredConsole(Serilog.Events.LogEventLevel.Error)
                .WriteTo.File(@"C:\tempLog\JobAgentServerLog.txt")
                .CreateLogger();

            // Hangfire job storage
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString, options);

            HostFactory.Run(x =>
            {
                x.UseSerilog();

                x.Service<JobAgentService>(s =>
                {
                    s.ConstructUsing(name => new JobAgentService());
                    s.WhenStarted(servie => servie.Start());
                    s.WhenStopped(service => service.Stop());
                });

                x.RunAsLocalService();

                x.SetServiceName("HangfireJobAgent");
                x.SetDisplayName("HangfireJobAgent");
                x.SetDescription("Hangfire job agent to process background jobs.");

                x.StartAutomatically();

            });
        }
    }
}
