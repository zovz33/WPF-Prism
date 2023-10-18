using Serilog;
using WPFPrism.Infrastructure.Configuration;
using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.Infrastructure.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger _logger;

        public LoggerService(LoggingConfiguration loggingConfig)
        {
            _logger = loggingConfig.CreateLogger();
        }

        /// <summary>
        /// Логирование события с данными.
        /// </summary>
        /// <param name="eventName">Имя события.</param>
        /// <param name="eventData">Словарь данных события.</param>
        public void LogEvent(string eventName, Dictionary<string, object> eventData)
        {
            if (eventName == null)
                throw new ArgumentNullException(nameof(eventName));

            if (eventData == null)
                throw new ArgumentNullException(nameof(eventData));

            _logger.Information("Event: {EventName}. Data: {@EventData}", eventName, eventData);
        }

        /// <summary>
        /// Логирование подробных данных для отладки.
        /// </summary>
        public void Verbose(string messageTemplate, params object[] propertyValues)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            _logger.Verbose(messageTemplate, propertyValues);
        }

        /// <summary>
        /// Логирование информационных событий.
        /// </summary>
        public void Information(string messageTemplate, params object[] propertyValues)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            _logger.Information(messageTemplate, propertyValues);
        }

        /// <summary>
        /// Логирование предупреждений.
        /// </summary>
        public void Warning(string messageTemplate, params object[] propertyValues)
        {
            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            _logger.Warning(messageTemplate, propertyValues);
        }

        /// <summary>
        /// Логирование ошибок.
        /// </summary>
        public void Error(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            _logger.Error(exception, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Логирование фатальных ошибок.
        /// </summary>
        public void Fatal(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            if (messageTemplate == null)
                throw new ArgumentNullException(nameof(messageTemplate));

            _logger.Fatal(exception, messageTemplate, propertyValues);
        }
    }
}

