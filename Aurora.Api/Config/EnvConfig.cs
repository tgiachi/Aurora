namespace Aurora.Api.Config
{
    public class EnvConfig
    {
        public static string Mode => Environment.GetEnvironmentVariable("MODE") ?? "alpha";
    }
}
