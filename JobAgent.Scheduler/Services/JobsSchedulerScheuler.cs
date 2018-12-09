using Serilog;

namespace JobAgent.Scheduler.Services
{
    public class JobsSchedulerScheuler
    {
        public void ProcessFiles(string msg)
        {
            Log.Logger.Error(msg);
        }
    }
}