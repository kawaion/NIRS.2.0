using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface IGrid
    {
        double this[PN pn, LimitedDouble n, LimitedDouble k] { get;set; }
        double LastIndexK(PN pn, LimitedDouble n);
        double LastIndexN(PN pn);

        double GetSn(PN pn, double n);
        void SetSn(PN pn, double n, double value);
        double LastIndexNSn(PN pn);

        double[,] GetFullData(int pn);
    }
}
