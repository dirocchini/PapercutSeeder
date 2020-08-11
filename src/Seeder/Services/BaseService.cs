using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Seeder.Interfaces;

namespace Seeder.Services
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
