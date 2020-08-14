using System;
using System.Collections.Generic;
using System.Text;
using Application.Interfaces;
using Bogus;
using Infrastructure.Services;
using Moq;
using Xunit;

namespace Infrastructure.UnitTests.OfficeTests
{
    public class OfficeTests
    {
        private readonly OfficeService _officeService;

        public OfficeTests()
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
