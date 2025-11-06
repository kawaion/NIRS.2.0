using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyDouble;
using NIRS.Parameter_Type;

namespace NIRS.Grid_Folder
{
    public class TimeSpaceGrid : IGrid
    {
        const int countParams = 13;
        const int maximumnNegativeN = 1;
        const int maximumnNegativeK = 1;

        private DataArray data = new DataArray(countParams);
        private LastKArray lastK = new LastKArray(countParams, new LimitedDouble(-1));
        private LastNArray lastN = new LastNArray(countParams,new LimitedDouble(-1));

        // Словари для кэширования конвертации LimitedDouble в int
        private Dictionary<LimitedDouble, int> nIndexCache = new Dictionary<LimitedDouble, int>();
        private Dictionary<LimitedDouble, int> kIndexCache = new Dictionary<LimitedDouble, int>();
        private Dictionary<LimitedDouble, int> nSnIndexCache = new Dictionary<LimitedDouble, int>();

        public TimeSpaceGrid()
        {
        }

        public double this[PN pn, LimitedDouble n, LimitedDouble k]
        {
            get
            {
                Validation(n, k);

                if (pn == PN.One_minus_m)
                    return 1.02 - this[PN.m, n, k];

                var paramIndex = (int)pn;
                var nIndex = ConvertToNIndex(n);
                var kIndex = ConvertToKIndex(k);

                return data[paramIndex, nIndex, kIndex];
            }
            set
            {
                Validation(n, k);

                var paramIndex = (int)pn;
                var nIndex = ConvertToNIndex(n);
                var kIndex = ConvertToKIndex(k);

                data[paramIndex, nIndex, kIndex] = ValidationValue(value);

                if (n > lastN[paramIndex])
                    lastN[paramIndex] = n;

                if (lastK.IsNewLayer(nIndex))
                    lastK[paramIndex, nIndex] = k;
                else if (k > lastK[paramIndex, nIndex])
                    lastK[paramIndex, nIndex] = k;
            }
        }

        private void Validation(LimitedDouble n, LimitedDouble k)
        {
            if (ParameterTypeGetter.IsMixture(n, k))
                return;
            if (ParameterTypeGetter.IsDynamic(n, k))
                return;
            throw new Exception();
        }

        private int ConvertToNIndex(LimitedDouble n)
        {
            return (n + maximumnNegativeN).GetInt();
        }

        private int ConvertToKIndex(LimitedDouble k)
        {
            return (k + maximumnNegativeK).GetInt();
        }
        static int DynamicParamIndexForLastK = (int)PN.dynamic_m;
        static int MixtureParamIndexForLastK = (int)PN.r;
        public LimitedDouble LastIndexK(PN pn, LimitedDouble n)
        {
            var paramIndex = (int)pn;
            var nIndex = ConvertToNIndex(n);
            return lastK[paramIndex, nIndex];
        }
        public LimitedDouble LastIndexK(LimitedDouble n)
        {
            var nIndex = ConvertToNIndex(n);
            if (n.IsHalfInt())
                return lastK[DynamicParamIndexForLastK, nIndex];
            else 
                return lastK[MixtureParamIndexForLastK, nIndex];
        }

        public LimitedDouble LastIndexN(PN pn)
        {
            var paramIndex = (int)pn;
            return lastN[paramIndex];
        }

        private double ValidationValue(double value)
        {
            if (Math.Abs(value) < 1e-6)
                return 0;
            return value;
        }

        const int countParamsSn = 15;
        private DataSnArray dataSn = new DataSnArray(countParamsSn);
        private LastNArray lastNSn = new LastNArray(countParamsSn, new LimitedDouble(-1));

        public double GetSn(PN pn, LimitedDouble n)
        {
            if (pn == PN.One_minus_m)
                return 1.02 - GetSn(PN.m, n);
            if (pn == PN.v || pn == PN.w)
                return GetSn(PN.vSn, n);

            var paramIndex = (int)pn;
            var nIndex = ConvertToNIndexSn(n);

            return dataSn[paramIndex, nIndex];
        }

        public void SetSn(PN pn, LimitedDouble n, double value)
        {
            var paramIndex = (int)pn;
            var nIndex = ConvertToNIndexSn(n);

            dataSn[paramIndex, nIndex] = ValidationValue(value);
            if (n > lastNSn[paramIndex])
                lastNSn[paramIndex] = n;
        }

        public LimitedDouble LastIndexNSn(PN pn)
        {
            var paramIndex = (int)pn;
            return lastNSn[paramIndex];
        }

        private int ConvertToNIndexSn(LimitedDouble n)
        {
            return (n + maximumnNegativeN).GetIndex();
        }
    }
}
