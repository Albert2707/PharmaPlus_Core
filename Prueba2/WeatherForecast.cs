using log4net.Config;
using log4net;
using System.Reflection;

namespace Prueba2
{
    public class WeatherForecast
    {
        private static readonly ILog _logger;

        static WeatherForecast()
        {
            // Configurar log4net una vez en el constructor estático
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                throw new InvalidOperationException("GetEntryAssembly() returned null. Unable to configure log4net.");
            }

            var logRepository = LogManager.GetRepository(entryAssembly);
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            _logger = LogManager.GetLogger(typeof(WeatherForecast));
        }

        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }


    }
}
