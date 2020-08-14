using System;
using System.Collections.Generic;
using System.Text;
using Application.Interfaces;
using Bogus;
using Shared.Constants;

namespace Infrastructure.Services
{
    public class DepartmentService : IDepartmentOperations
    {
        public List<string> GetFakeDepartments()
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
    }
}
