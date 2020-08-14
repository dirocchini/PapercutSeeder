using System.Collections.Generic;
using Infrastructure.Services;
using Xunit;

namespace Infrastructure.UnitTests.OfficeServiceTests
{
    public class OfficeServiceTests
    {
        private readonly OfficeService _officeService;

        public OfficeServiceTests()
        {
            _officeService = new OfficeService();
        }

        [Fact(DisplayName = "Create a List of Fake Offices")]
        [Trait("Category", "Office Tests")]
        public void Department_GetFakeOffices_ShouldReturnListOfString()
        {
            //Arrange + Act
            var officesReturned = _officeService.GetFakeOffices();

            //Assert
            Assert.IsType<List<string>>(officesReturned);
        }
    }
}
