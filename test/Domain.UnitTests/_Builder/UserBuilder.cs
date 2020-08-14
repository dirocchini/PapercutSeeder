using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using Domain.Entities;

namespace Domain.UnitTests._Builder
{
    public class UserBuilder
    {
        public static UserBuilder New()
        {
            return new UserBuilder();
        }
        
        public User Build()
        {
            return new User();
        }
    }
}
