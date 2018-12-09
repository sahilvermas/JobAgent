using Hangfire;
using Serilog;

namespace JobAgent.WindowService
{
    public class JobAgentService
    {

        private BackgroundJobServer _server;

        public void Start()
        {
            _server = new BackgroundJobServer();

            Log.Logger.Information(string.Format("##### :: Hangfire server started at: {0} :: #####", System.DateTime.UtcNow.ToString("dd MMM yyyy, hh:mm tt")));
        }

        public void Stop()
        {
            if (_server != null) _server.Dispose();

            Log.Logger.Information(string.Format("##### :: Hangfire server stopped at: {0} :: #####", System.DateTime.UtcNow.ToString("dd MMM yyyy, hh:mm tt")));
        }
    }
}
