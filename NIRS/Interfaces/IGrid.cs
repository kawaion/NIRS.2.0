using MyDouble;
using NIRS.Parameter_names;

namespace NIRS.Interfaces
{
    public interface IGrid
    {
        double this[PN pn, double n, double k] { get;set; }
        double LastIndexK(PN pn, double n);
        double LastIndexN(PN pn);

        double GetSn(PN pn, double n);
        void SetSn(PN pn, double n, double value);
    }
}
