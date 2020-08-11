namespace Shared.Options
{
    public class RetryPolicyOptions
    {
        public int MaxRetries { get; set; }
        public int TimeBetweenRetries { get; set; }
    }
}
