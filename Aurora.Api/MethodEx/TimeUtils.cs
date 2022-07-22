namespace Aurora.Api.MethodEx
{
    public static class TimeUtils
    {
        public static long GetUnixTimestamp(this DateTime date)
        {
            var zero = new DateTime(1970, 1, 1);
            var span = date.Subtract(zero);

            return (long)span.TotalMilliseconds;
        }

        public static long GetUnixTimestamp()
        {
            return DateTime.UtcNow.GetUnixTimestamp();
        }

        public static long GetMills(this DateTime date)
        {
            return date.GetUnixTimestamp() / 1000;
        }
    }
}
