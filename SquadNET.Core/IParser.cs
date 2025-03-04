// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
namespace SquadNET.Core
{
    public interface IParser<out T>
    {
        public T Parse(string input);
    }
}