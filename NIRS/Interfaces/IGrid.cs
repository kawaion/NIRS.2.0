using MyDouble;

namespace NIRS.Interfaces
{
    public interface IGrid
    {
        ISubGrid this[LimitedDouble i] { get;set; }
        LimitedDouble MinN { get; }
        LimitedDouble MaxN { get; }
    }
}
