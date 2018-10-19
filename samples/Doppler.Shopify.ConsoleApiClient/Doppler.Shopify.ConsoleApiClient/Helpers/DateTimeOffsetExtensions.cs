using System;

namespace Doppler.Shopify.ConsoleApiClient.Helpers
{
    static class DateTimeOffsetHelper
    {
        public static DateTimeOffset? ParseCustom(string date)
        {
            if (string.IsNullOrEmpty(date) || !DateTimeOffset.TryParse(date, out DateTimeOffset dto))
            {
                return null;
            }

            return dto;
        }
    }
}
