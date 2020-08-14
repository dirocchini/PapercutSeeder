using System.Collections.Generic;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Constants;
using Shared.Options;

namespace Infrastructure.Services
{
    public class UserService : BaseService<UserService>, IUserOperations
    {
        private readonly IOfficeOperations _officeService;
        private readonly IDepartmentOperations _departmentService;
        private readonly UserOptions _options;

        public UserService(IServerCommandOperations papercutService, ILogger<UserService> logger, IOptions<UserOptions> options, IOfficeOperations officeService, IDepartmentOperations departmentService) : base(
            papercutService, logger)
        {
            _officeService = officeService;
            _departmentService = departmentService;
            _options = options.Value;
        }

        public List<string> GetAllUsers()
        {
            List<string> users = _papercutService.GetAllUsers();

            if (users.Count >= _options.CreateUsersMaxQuantity)
                return users;

            LoggerExtensions.LogInformation(_logger, "  MUST CREATE USERS");

            _papercutService.AddFakeUsersToPapercut(_departmentService.GetFakeDepartments(), _officeService.GetFakeOffices(), _options.CreateUsersMaxQuantity);

            return _papercutService.GetAllUsers();
        }

    
    }
}