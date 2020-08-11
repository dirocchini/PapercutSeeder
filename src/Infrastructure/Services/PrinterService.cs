using System;
using System.Collections.Generic;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class PrinterService : BaseService<PrinterService>, IPrinterOperations
    {

        public PrinterService(IServerCommandOperations papercutService, ILogger<PrinterService> logger) : base(
            papercutService, logger)
        {
        }

        public List<string> GetAllPrinters()
        {
            throw new NotImplementedException();
        }
    }
}