using System.Diagnostics;

namespace DigiKey.Api.Extensions
{
    public static class StringExtensions
    {
        [DebuggerStepThrough]
        public static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        public static bool IsPresent(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        [DebuggerStepThrough]
        public static string EnsureTrailingSlash(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (!input.EndsWith("/"))
            {
                return input + "/";
            }

            return input;
        }
    }
}
