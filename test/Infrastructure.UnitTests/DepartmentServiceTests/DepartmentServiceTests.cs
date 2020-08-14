using System.Collections.Generic;
using Infrastructure.Services;
using Xunit;

namespace Infrastructure.UnitTests.DepartmentServiceTests
{
    public class DepartmentServiceTests
    {
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _departmentService = new DepartmentService();
        }

        [Fact(DisplayName = "Create a List of Fake Departments")]
        [Trait("Category", "Department Tests")]
        public void Department_GetFakeDepartments_ShouldReturnListOfString()
        {
            //Arrange + Act
            var departmentsReturned = _departmentService.GetFakeDepartments();

            //Assert
            Assert.IsType<List<string>>(departmentsReturned);
        }
    }
}
