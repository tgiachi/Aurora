using System.Text.RegularExpressions;

namespace Aurora.Api.MethodEx
{
    public static class EnvReplacerMethodEx
    {
        private static readonly Regex EnvReplaceRegex = new("\\{(.*?)\\}");

        public static string ReplaceEnv(this string value)
        {
            var matches = EnvReplaceRegex.Matches(value);
            for (var count = 0; count < matches.Count; count++)
            {
                if (matches[count].Groups.Count != 2)
                {
                    continue;
                }

                var env = Environment.GetEnvironmentVariable(matches[count].Groups[1].Value) ?? "";
                if (!string.IsNullOrEmpty(env))
                {
                    value = value.Replace(matches[count].Value, env);
                }
            }

            return value;
        }
    }
}
