#nullable enable
using Aurora.Api.Data.Job;

namespace Aurora.Api.Data.Job
{
    public class JobResultObject
    {
        public string JobName { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public Exception? Error { get; set; }
    }
}
