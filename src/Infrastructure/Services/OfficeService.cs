using System;
using System.Collections.Generic;
using System.Text;
using Application.Interfaces;
using Bogus;
using Shared.Constants;

namespace Infrastructure.Services
{
    public class OfficeService : IOfficeOperations
    {
        public List<string> GetFakeOffices()
        {
            var prefix = "fake office - ";
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
