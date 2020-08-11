using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Seeder.Entities;
using Seeder.Interfaces;
using Seeder.Options;

namespace Seeder.Services
{
    public class UserService : BaseService<UserService>, IUserOperations
    {
        private readonly UserOptions _options;

        public UserService(IServerCommandOperations papercutService, ILogger<UserService> logger, IOptions<UserOptions> options) : base(
            papercutService, logger)
        {
            _options = options.Value;
        }

        public List<string> GetAllUsers()
        {
            List<string> users = _papercutService.GetAllUsers();

            if (users.Count >= 50)
                return users;

            _logger.LogInformation("  MUST CREATE USERS");

            CreateUserFakeList();

            return _papercutService.GetAllUsers();
        }

        private void CreateUserFakeList()
        {
            List<string> departments = GetFakeDepartmentList();

            List<string> offices = GetFakeOfficeList();

            for (var i = 0; i < _options.CreateUsersMaxQuantity; i++)
            {
                var user = new User().GetFakeUser(departments, offices);

                _logger.LogInformation("   CREATING USER {de} OF {ate} - {user}", i + 1, 50, user.Login);

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

        private List<string> GetFakeOfficeList()
        {
            var prefix = "office ";
            return new List<string>()
            {
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department()
            };
        }

        private List<string> GetFakeDepartmentList()
        {
            var prefix = "department ";
            return new List<string>()
            {
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department(),
                prefix + new Bogus.Faker().Commerce.Department()
            };
        }
    }
}