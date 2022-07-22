namespace Aurora.Api.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime ParseEpochMillsToDateTime(this long value)
        {
            try
            {
                var initialDate = DateTime.Parse("01/01/1970");
                initialDate = initialDate.AddMilliseconds(value);
                return initialDate;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
