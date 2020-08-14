using System;
using System.Collections.Generic;
using System.Text;
using Application.Interfaces;
using Bogus;
using Castle.Core.Logging;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Constants;
using Shared.Options;
using Xunit;

namespace Infrastructure.UnitTests.UserServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IServerCommandOperations> _serverCommandOperationsMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly Mock<IOptions<UserOptions>> _userOptions;
        private readonly Mock<IOfficeOperations> _officeOperationsMock;
        private readonly Mock<IDepartmentOperations> _departmentOperationsMock;
        private UserService _userService;


        public UserServiceTests()
        {
            _serverCommandOperationsMock = new Mock<IServerCommandOperations>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userOptions = new Mock<IOptions<UserOptions>>();
            _officeOperationsMock = new Mock<IOfficeOperations>();
            _departmentOperationsMock = new Mock<IDepartmentOperations>();

            _userService = new UserService(_serverCommandOperationsMock.Object, _loggerMock.Object, _userOptions.Object, _officeOperationsMock.Object, _departmentOperationsMock.Object);
        }


        [Fact(DisplayName = "Get a List of Users - Will Not Create New Users")]
        [Trait("Category", "User Service Tests")]
        public void User_GetAllUsers_ShouldReturnListOfStringAndNotCreateNewUsers()
        {
            //Arrange 
            var userOptions = new UserOptions() {CreateUsersMaxQuantity = 50};
            var departments = ReturnsFakeDepartments();
            var offices = ReturnFakeOffices();
            var expectedUsers = ReturnFakeUsersEqualToLimit(userOptions.CreateUsersMaxQuantity);


            _serverCommandOperationsMock.Setup(s => s.GetAllUsers()).Returns(expectedUsers);
            _userOptions.Setup(s => s.Value).Returns(userOptions);
            _userService = new UserService(_serverCommandOperationsMock.Object, _loggerMock.Object, _userOptions.Object, _officeOperationsMock.Object, _departmentOperationsMock.Object);


            //Act
            var returnedUsers = _userService.GetAllUsers();

            //Assert
            _serverCommandOperationsMock.Verify(s => s.AddFakeUsersToPapercut(ReturnsFakeDepartments(), ReturnFakeOffices(), userOptions.CreateUsersMaxQuantity), Times.Never);
            _serverCommandOperationsMock.Verify(s => s.GetAllUsers(), Times.Once);
            Assert.Equal(expectedUsers, returnedUsers);
        }

        private List<string> ReturnFakeUsersEqualToLimit(int usersThreshold)
        {
            List<string > users = new List<string>();
            for (int i = 0; i <= usersThreshold; i++)
                users.Add(new Faker().Person.UserName.ToLower());

            return users;
        }

        private List<string> ReturnFakeOffices()
        {
            return new List<string>()
            {
                DepartmentConstants.FAKE_DEPARTMENT_PREFIX + new Faker().Commerce.Department(),
                DepartmentConstants.FAKE_DEPARTMENT_PREFIX + new Faker().Commerce.Department(),
                DepartmentConstants.FAKE_DEPARTMENT_PREFIX + new Faker().Commerce.Department(),
                DepartmentConstants.FAKE_DEPARTMENT_PREFIX + new Faker().Commerce.Department(),
                DepartmentConstants.FAKE_DEPARTMENT_PREFIX + new Faker().Commerce.Department(),
                DepartmentConstants.FAKE_DEPARTMENT_PREFIX + new Faker().Commerce.Department(),
                DepartmentConstants.FAKE_DEPARTMENT_PREFIX + new Faker().Commerce.Department(),
                DepartmentConstants.FAKE_DEPARTMENT_PREFIX + new Faker().Commerce.Department()
            };
        }

        private List<string> ReturnsFakeDepartments()
        {
            return new List<string>()
            {
                OfficeConstants.FAKE_OFFICE_PREFIX + new Faker().Commerce.Department(),
                OfficeConstants.FAKE_OFFICE_PREFIX + new Faker().Commerce.Department(),
                OfficeConstants.FAKE_OFFICE_PREFIX + new Faker().Commerce.Department(),
                OfficeConstants.FAKE_OFFICE_PREFIX + new Faker().Commerce.Department(),
                OfficeConstants.FAKE_OFFICE_PREFIX + new Faker().Commerce.Department(),
                OfficeConstants.FAKE_OFFICE_PREFIX + new Faker().Commerce.Department(),
                OfficeConstants.FAKE_OFFICE_PREFIX + new Faker().Commerce.Department(),
                OfficeConstants.FAKE_OFFICE_PREFIX + new Faker().Commerce.Department()
            };
        }
    }
}
