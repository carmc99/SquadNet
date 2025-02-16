﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquadNET.LogManagement
{
    public interface ILogReaderFactory
    {
        ILogReader Create(LogReaderType type);
    }
}
