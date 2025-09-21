using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface IGrid
    {
        double this[PN pn, LimitedDouble n, LimitedDouble k] { get;set; }
        LimitedDouble LastIndexK(PN pn, LimitedDouble n);
        LimitedDouble LastIndexN(PN pn);

        double GetSn(PN pn, LimitedDouble n);
        void SetSn(PN pn, LimitedDouble n, double value);
        LimitedDouble LastIndexNSn(PN pn);

        //double[,] GetFullData(int pn);
    }
}
