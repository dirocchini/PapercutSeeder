using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Services;
using Xunit;

namespace Infrastructure.UnitTests.DepartmentTests
{
    public class DepartmentTests
    {
        private readonly DepartmentService _departmentService;

        public DepartmentTests()
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
