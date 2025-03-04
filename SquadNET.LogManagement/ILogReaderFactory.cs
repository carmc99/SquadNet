﻿// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.LogManagement
{
    /// <summary>
    /// Factory interface for creating instances of <see cref="ILogReader"/>.
    /// </summary>
    public interface ILogReaderFactory
    {
        /// <summary>
        /// Creates an instance of an <see cref="ILogReader"/> based on the specified log reader type.
        /// </summary>
        /// <param name="type">The type of log reader to create.</param>
        /// <returns>An instance of <see cref="ILogReader"/> corresponding to the given type.</returns>
        ILogReader Create(LogReaderType type);
    }
}