using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface IGrid
    {
        double this[PN pn, LimitedDouble n, LimitedDouble k] { get;set; }
        LimitedDouble LastIndex(PN pn, LimitedDouble n);

        double GetSn(PN pn, LimitedDouble n);
        void SetSn(PN pn, LimitedDouble n, double value);
    }
}
