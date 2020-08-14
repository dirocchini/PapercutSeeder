using System;
using System.Collections.Generic;
using Bogus;

namespace Domain.Entities
{
    public class User
    {
        public string Login { get;} 
        public string FirstName{ get;} 
        public string LastName{ get;} 
        public string FullName{ get;} 
        public string PrimaryCardNumber { get;} 
        public string SecondaryCardNumber { get;} 
        public string Email { get;} 
        public string UserNameAlias { get;} 
        public string Notes { get;} 
        public string Office { get;} 
        public string Department { get;} 
        public string Restricted { get;} 
        public string Home { get;} 
        public string Pin{ get;} 

        public User GetFakeUser(List<string> departments, List<string> offices)
        {
            var user = new Faker<User>()
                .StrictMode(true)
                .RuleFor(u => u.Login, f => f.Person.UserName.ToLower())
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.PrimaryCardNumber, f => f.Person.Random.AlphaNumeric(8))
                .RuleFor(u => u.SecondaryCardNumber, f => f.Person.Random.AlphaNumeric(8))
                .RuleFor(u => u.Email, (f,u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.UserNameAlias, f => f.Person.UserName.ToLower() + "-alias")
                .RuleFor(u => u.Notes, f => "Created by Papercut Seeder - Random words - " + f.Random.Words(5))
                .RuleFor(u => u.Office, f => offices[new Random().Next(offices.Count - 1)])
                .RuleFor(u => u.Department, f => departments[new Random().Next(departments.Count - 1)])
                .RuleFor(u => u.Restricted, "FALSE")
                .RuleFor(u => u.Home, (f,u) => $"/home/users/{u.Login.ToLower()}")
                .RuleFor(u => u.Pin, f => f.Random.Number(9999).ToString().PadLeft(4, '0'));

            return user;
        }
    }
}
