using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;

namespace Infrastructure.Data.Configurations
{
    public static class Logs
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Logs));
        public static readonly Logger demo = new();
        static Logs()
        {
            // Configure log4net once during static initialization
            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public static void Info(string msg)
        {
            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4netconfig.config"));
            //var demo = new Logger();
            //demo.Info("Starting the console application");
            log.Info(msg);
        }

        public static void Error(string msg, Exception ex = null)
        {
            if (ex != null)
            {
                log.Error(msg, ex);
            }
            else
            {
                log.Error(msg);
            }
        }

        public static void Debug(string msg)
        {
            log.Debug(msg);
        }
    }
}
