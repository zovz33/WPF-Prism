namespace WPFPrism.Infrastructure.Services.Interface
{
    public interface ILoggerService
    {
        void LogEvent(string eventName, Dictionary<string, object> eventData);
        void Verbose(string messageTemplate, params object[] propertyValues);
        void Information(string messageTemplate, params object[] propertyValues);
        void Warning(string messageTemplate, params object[] propertyValues);
        void Error(Exception exception, string messageTemplate, params object[] propertyValues);
        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);
    }
}
