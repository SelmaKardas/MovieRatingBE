using System;

namespace MistralMovieRating.Common
{
    public static class Extensions
    {
        public static Guid ToGuid(this string guidString)
        {
            if (Guid.TryParse(guidString, out var guid))
            {
                return guid;
            }

            return Guid.Empty;
        }
    }
}
