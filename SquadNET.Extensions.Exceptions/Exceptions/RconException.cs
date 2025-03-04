// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Extensions.Exceptions.Exceptions
{
    using System;

    public class RconException : Exception
    {
        public RconException(ErrorCode code, string message)
            : base(message)
        {
            Code = code;
        }

        public RconException(ErrorCode code, string message, Exception innerException)
            : base(message, innerException)
        {
            Code = code;
        }

        public ErrorCode Code { get; }

        public override string ToString()
        {
            return $"[Error Code: {Code}] {Message}";
        }
    }
}