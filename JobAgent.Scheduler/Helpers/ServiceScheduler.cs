using Hangfire;
using JobAgent.Scheduler.Services;
using System;

namespace JobAgent.Scheduler.Helpers
{
    public class ServiceScheduler
    {
        private readonly JobsSchedulerScheuler bulkFileProcess = null;
        public ServiceScheduler()
        {
            bulkFileProcess = new JobsSchedulerScheuler();
        }

        internal static void ScheduleJobs()
        {
            ServiceScheduler objServiceScheduler = new ServiceScheduler();

            // 1. Fire and Forget job: When code need to run in the background immediately. 
            // Run only Once
            var fireAndForgetJobId = BackgroundJob.Enqueue(
                () => objServiceScheduler.bulkFileProcess.ProcessFiles("1. Fire and Forget job")
            );

            // 2. Delayed job: When code need to run in the background after some delay. Postponed background jobs.
            // Run only Once
            var delayedJobId = BackgroundJob.Schedule(
                () => objServiceScheduler.bulkFileProcess.ProcessFiles("2. Scheduled background jobs with delay"),
                TimeSpan.FromMinutes(2)
            );

            // 3. Recurring job: When code need to run recucively with time frequency.
            // Run Repeatly
            // Cron Expression can be passed for better frequency management.
            RecurringJob.AddOrUpdate(
                () => objServiceScheduler.bulkFileProcess.ProcessFiles("3. Recursive job called with 2 minutes interval"),
                Cron.MinuteInterval(1)
            );
        }
    }
}