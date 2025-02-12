using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;

namespace NIRS.Helpers
{
    public class XGetter
    {
        double h;
        public XGetter(IConstParameters constParameters)
        {
            h = constParameters.h;
        }
        public double this[LimitedDouble k] => k.Value * h;
        public double this[double k] => k * h;
    }
}
