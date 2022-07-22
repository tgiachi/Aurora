namespace Aurora.Turbine.Api.Data
{
    public class TurbineConfig
    {
        public bool UseSwagger { get; set; }

        public bool IsMapControllers { get; set; } = true;

        public bool EnableRestDateTimeMills { get; set; } = false;
        public string IamAuthorizationUrl { get; set; } = "https://iam.isendu.com";
    }
}
