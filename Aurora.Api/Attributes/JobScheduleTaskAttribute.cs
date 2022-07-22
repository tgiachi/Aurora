namespace Aurora.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class JobScheduleTaskAttribute : Attribute
    {
        public JobScheduleTaskAttribute(string cronTab,
            string jobName,
            bool exposeApi = true,
            bool enableConcurrency = true)
        {
            CronTab = cronTab;
            JobName = jobName;
            ExposeApi = exposeApi;
            EnableConcurrency = enableConcurrency;
        }

        public string CronTab { get; set; }

        public string JobName { get; set; }

        public bool ExposeApi { get; set; }

        public bool EnableConcurrency { get; set; }
    }
}
