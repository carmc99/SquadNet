using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.Extensions.Exceptions.Exceptions
{
    public enum ErrorCode
    {
        UnknownError = 1000,
        RconConnectionFailed = 2001,
        RconCommandExecutionFailed = 2002,
        RconDisconnectionFailed = 2003,
        InvalidCommand = 3001
    }
}
