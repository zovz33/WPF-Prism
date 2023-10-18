using Serilog;


namespace WPFPrism.Infrastructure.Configuration
{
    public class LoggingConfiguration
    {
        public ILogger CreateLogger()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
             
            string logFilePath = Path.Combine(documentsPath, @"ServiceApplication/ServiceAppLogs.txt");

            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
