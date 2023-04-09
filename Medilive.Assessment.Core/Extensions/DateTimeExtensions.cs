namespace Medilive.Assessment.Core.Extensions
{
    /// <summary>
    /// Assuming this is a world wide application , we should implement utc and timezone support also
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts DateTime to Unix/Epoch date time milliseconds in numeric format
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Unix date time milliseconds in number format</returns>
        public static long ToUnixTimeLong(this DateTime dateTime)
        {
            var epochNow = (dateTime.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks) / 10000;
            return epochNow;
        }
        /// <summary>
        /// Converts nullable DateTime to Unix/Epoch date time milliseconds in numeric format
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Unix date time milliseconds in number format</returns>
        public static long ToUnixTimeMilisecondsAsLong(this DateTime? dateTime)
        {
            var epochNow = 0l;
            if (dateTime.HasValue)
            {
                epochNow = (dateTime.Value.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks) / 10000;
            }
            return epochNow;
        }
    }
}
