namespace SquadNET.Core
{
    public interface IParser<out T>
    {
        public T Parse(string input);
    }
}
