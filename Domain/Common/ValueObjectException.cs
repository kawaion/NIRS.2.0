
namespace Core.Domain.Common
{
    public abstract class ValueObjectException : Exception
    {
        protected ValueObjectException(string message) : base(message) { }
    }
}
