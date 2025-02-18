using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Core
{
    public static class ParserExtensions
    {
        /// <summary>
        /// Sanitizes a string by trimming unnecessary spaces, replacing tab characters, 
        /// and normalizing line breaks to a consistent format.
        /// </summary>
        /// <param name="input">The string to sanitize.</param>
        /// <returns>A sanitized string with trimmed spaces and normalized line breaks.</returns>
        public static string SanitizeInput(this string input)
        {
            return input?.Trim().Replace("\r\n", "\n").Replace("\t", " ") ?? string.Empty;
        }

        /// <summary>
        /// Attempts to parse a string into an integer, returning null if the conversion fails.
        /// </summary>
        /// <param name="value">The string to convert.</param>
        /// <returns>An integer if the conversion is successful, otherwise null.</returns>
        public static int? TryParseOrNull(this string value)
        {
            return int.TryParse(value, out int result) ? result : (int?)null;
        }

        /// <summary>
        /// Attempts to parse a string into a generic numeric type (int, ulong, bool, etc.).
        /// Returns false if the conversion fails.
        /// </summary>
        /// <typeparam name="T">The numeric type to convert to.</typeparam>
        /// <param name="value">The string to convert.</param>
        /// <param name="result">The parsed value, or default if the conversion fails.</param>
        /// <returns>True if conversion is successful, otherwise false.</returns>
        public static bool TryParse<T>(this string value, out T result) where T : struct
        {
            result = default;

            return typeof(T) switch
            {
                Type t when t == typeof(int) => int.TryParse(value, out Unsafe.As<T, int>(ref result)),
                Type t when t == typeof(ulong) => ulong.TryParse(value, out Unsafe.As<T, ulong>(ref result)),
                Type t when t == typeof(bool) => bool.TryParse(value, out Unsafe.As<T, bool>(ref result)),
                _ => throw new NotSupportedException($"Conversion not supported for type: {typeof(T).Name}")
            };
        }
    }
}
