// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core.Squad
{
    public static class TimeExtensions
    {
        public static string NormalizeTime(this string time)
        {
            string[] parts = time.Split('-');
            if (parts.Length != 2)
            {
                return time;
            }

            string datePart = parts[0];
            string timePart = parts[1];

            return $"{datePart}-{timePart}";
        }
    }
}