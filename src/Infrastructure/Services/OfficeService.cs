using System;
using System.Collections.Generic;
using System.Text;
using Application.Interfaces;
using Bogus;

namespace Infrastructure.Services
{
    public class OfficeService : IOfficeOperations
    {
        public List<string> GetFakeOffices()
        {
            var prefix = "fake office - ";
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
