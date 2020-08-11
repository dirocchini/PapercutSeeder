using System;
using System.Collections.Generic;
using System.Text;

namespace Seeder.Options
{
    public class RetryPolicyOptions
    {
        public int MaxRetries { get; set; }
        public int TimeBetweenRetries { get; set; }
    }
}
