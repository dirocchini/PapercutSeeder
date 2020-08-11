using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Seeder.Interfaces;

namespace Seeder
{
    public class App
    {
        private readonly IUserOperations _userService;
        private readonly IPrinterOperations _printerService;
        private readonly ILogger<App> _logger;

        public App(IUserOperations userService, IPrinterOperations printerService, ILogger<App> logger)
        {
            _userService = userService;
            _printerService = printerService;
            _logger = logger;
        }


        public void ProcessJobs()
        {
            _logger.LogInformation("STARTING PROCESS");
            _logger.LogInformation("");


            _logger.LogInformation(" GETTING USERS");
            var users = _userService.GetAllUsers();

            _logger.LogInformation(" GETTING PRINTERS");
            var printers = _printerService.GetAllPrinters();
        }
    }
}
