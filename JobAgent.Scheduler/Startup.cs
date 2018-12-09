using Hangfire;
using Hangfire.SqlServer;
using JobAgent.Scheduler.Helpers;
using Microsoft.Owin;
using Owin;
using Serilog;
using System;

[assembly: OwinStartup(typeof(JobAgent.Scheduler.Startup))]

namespace JobAgent.Scheduler
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbCS"].ConnectionString;

            var options = new SqlServerStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(30)// Default value
            };


            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                .WriteTo.Console(Serilog.Events.LogEventLevel.Error)
                .WriteTo.Console(Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                .WriteTo.File(@"C:\tempLog\JobAgentLog.txt")
                .CreateLogger();

            GlobalConfiguration.Configuration
                .UseSqlServerStorage(connectionString, options);

            app.UseHangfireDashboard("/jobs");

            ServiceScheduler.ScheduleJobs();
        }
    }
}
