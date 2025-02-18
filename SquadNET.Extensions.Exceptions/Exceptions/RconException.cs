using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Extensions.Exceptions.Exceptions
{
    using System;

    public class RconException : Exception
    {
        public ErrorCode Code { get; }

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

        public override string ToString()
        {
            return $"[Código de error {Code}] {Message}";
        }
    }

}
