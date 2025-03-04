// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SquadNET.MonitoringService
{
    public class CustomConsoleFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            StringBuilder sb = new();

            string color = logEvent.Level switch
            {
                LogEventLevel.Information => "\x1b[32m",
                LogEventLevel.Warning => "\x1b[33m",
                LogEventLevel.Error => "\x1b[31m",
                LogEventLevel.Debug => "\x1b[36m",
                LogEventLevel.Verbose => "\x1b[35m",
                _ => "\x1b[0m"
            };

            string levelShort = logEvent.Level switch
            {
                LogEventLevel.Information => "INF",
                LogEventLevel.Warning => "WRN",
                LogEventLevel.Error => "ERR",
                LogEventLevel.Debug => "DBG",
                LogEventLevel.Verbose => "VRB",
                _ => "UNK"
            };

            sb.Append("\x1b[37m[");
            sb.Append(logEvent.Timestamp.ToString("HH:mm:ss"));
            sb.Append("] \x1b[0m");

            sb.Append(color);
            sb.Append($"[{levelShort}] ");
            sb.Append("\x1b[0m");

            string message = logEvent.RenderMessage();

            message = Regex.Replace(message, @"""(.*?)""", "\x1b[36m\"$1\"\x1b[0m");

            message = message
                .Replace("Verbose", "\x1b[35mVerbose\x1b[0m")
                .Replace("Warning", "\x1b[33mWarning\x1b[0m")
                .Replace("Error", "\x1b[31mError\x1b[0m");

            sb.Append(message);
            sb.AppendLine();

            output.Write(sb.ToString());
        }
    }
}