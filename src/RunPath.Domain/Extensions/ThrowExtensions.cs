using System;

namespace RunPath.Domain.Extensions
{
    public static class ThrowExtensions
    {
        public static T ThrowIfNull<T>(this T value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }

            return value;
        }
    }
}