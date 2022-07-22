using Aurora.Api.Data.Job;

namespace Aurora.Api.Interfaces.Job
{
    public interface IJobTask
    {
        Task<JobResultObject> ExecuteJob(string param);
    }
}
