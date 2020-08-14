using System.Collections.Generic;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

            if (users.Count >= 50)
                return users;

            LoggerExtensions.LogInformation(_logger, "  MUST CREATE USERS");

            CreateUserFakeList();

            return _papercutService.GetAllUsers();
        }

        private void CreateUserFakeList()
        {
            for (var i = 0; i < _options.CreateUsersMaxQuantity; i++)
            {
                var user = new User().GetFakeUser(_departmentService.GetFakeDepartments(), _officeService.GetFakeOffices());

                LoggerExtensions.LogInformation(_logger, "   CREATING USER {de} OF {ate} - {user}", i + 1, 50, user.Login);

                _papercutService.AddNewUser(user.Login);

                string[] primaryCarNumber = {"primary-card-number", user.PrimaryCardNumber};
                string[] secondaryCardNumber = {"secondary-card-number", user.SecondaryCardNumber};
                string[] userDepartment = {"department", user.Department};
                string[] email = {"email", user.Email};
                string[] fullName = {"full-name", user.FullName};
                string[] userNameAlias = {"username-alias", user.UserNameAlias};
                string[] notes = {"notes", user.Notes};
                string[] userOffice = {"office", user.Office};
                string[] restricted = {"restricted", "FALSE"};
                string[] home = {"home", user.Home};
                string[] cardPin = {"card-pin", user.Pin};

                _papercutService.SetUserProperties(user.Login,
                    new string[][]
                    {
                        primaryCarNumber, secondaryCardNumber, userDepartment, email, fullName, userNameAlias, notes,
                        userOffice, restricted, home, cardPin
                    });
            }
        }
    }
}