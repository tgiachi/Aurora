namespace Aurora.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class JobTaskAttribute : Attribute
    {
        public JobTaskAttribute(string jobName, bool exposeApi = true, bool enableConcurrency = true)
        {
            JobName = jobName;
            ExposeApi = exposeApi;
            EnableConcurrency = enableConcurrency;
        }

        public string JobName { get; set; }

        public string JobDescription { get; set; }

        public bool ExposeApi { get; set; }

        public bool EnableConcurrency { get; set; }
    }
}
