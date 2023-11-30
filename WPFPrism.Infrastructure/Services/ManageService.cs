using WPFPrism.Infrastructure.Services.Interface;

namespace WPFPrism.Infrastructure.Services
{
    public class ManageService : IManageService
    {
        private readonly ILoggerService _logger;

        public ManageService(ILoggerService logger)
        {
            _logger = logger;
        }


    }
}
