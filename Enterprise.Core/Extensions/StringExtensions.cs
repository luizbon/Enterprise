using System.Diagnostics;

namespace System
{
    public static class StringExtensions
    {
        [DebuggerStepThrough]
        public static string NullSafe(this string value)
        {
            return (value ?? string.Empty).Trim();
        }

        [DebuggerStepThrough]
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value.NullSafe());
        }

        [DebuggerStepThrough]
        public static string IsEmpty(this string value, string defaultValue)
        {
            return string.IsNullOrEmpty(value.NullSafe()) ? defaultValue : value;
        }
    }
}