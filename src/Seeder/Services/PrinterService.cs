using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Seeder.Interfaces;

namespace Seeder.Services
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