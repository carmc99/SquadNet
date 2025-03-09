// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using System.Diagnostics;

namespace SquadNET.Core
{
    public static class ParserLogger
    {
        public static bool EnableDebugLogging { get; set; } = false;

        public static void LogInvalidInput(string parserName, string input)
        {
            if (!EnableDebugLogging)
            {
                return;
            }

            string message = $"[Parser: {parserName}] Invalid input detected: \"{input}\"";

            Debug.WriteLine(message);
        }
    }
}