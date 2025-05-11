using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Interfaces;
using System;

namespace NIRS.Helpers
{
    public class KGetter
    {
        double h;
        double eps = 0.00001;
        public KGetter(IConstParameters constParameters)
        {
            h = constParameters.h;
        }
        public double this[double x] 
        {
            get {
                var res = x / h;
                var res2 = Math.Round(res);
                if (Math.Abs(res2 - res) < eps)
                    return res2;
                else
                    return res;
                } 
        }
    }
}
