using System;
using System.Collections.Generic;
using System.Text;
using Application.Interfaces;
using Bogus;

namespace Infrastructure.Services
{
    public class DepartmentService : IDepartmentOperations
    {
        public List<string> GetFakeDepartments()
        {
            var prefix = "fake department - ";
            return new List<string>()
            {
                prefix + new Faker().Commerce.Department(),
                prefix + new Faker().Commerce.Department(),
                prefix + new Faker().Commerce.Department(),
                prefix + new Faker().Commerce.Department(),
                prefix + new Faker().Commerce.Department(),
                prefix + new Faker().Commerce.Department(),
                prefix + new Faker().Commerce.Department(),
                prefix + new Faker().Commerce.Department()
            };
        }
    }
}
