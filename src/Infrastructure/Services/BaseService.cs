using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
   public abstract class BaseService<T>
    {
        protected readonly IServerCommandOperations _papercutService;
        protected readonly ILogger<T> _logger;

        protected BaseService(IServerCommandOperations papercutService, ILogger<T> logger)
        {
            _papercutService = papercutService;
            _logger = logger;
        }
    }
}
