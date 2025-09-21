using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyDouble;

namespace NIRS.Grid_Folder
{
    public class TimeSpaceGrid : IGrid
    {
        const int countParams = 13;
        const int maximumnNegativeN = 1;
        const int maximumnNegativeK = 1;
        
        private DataArray data = new DataArray(countParams);
        private LastKArray lastK = new LastKArray(countParams);
        private LastNArray lastN = new LastNArray(countParams);

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
                
                data[paramIndex, nIndex, kIndex] = value;

                if (n > lastN[paramIndex])
                    lastN[paramIndex] = n;
                if (k > lastK[paramIndex,nIndex])
                    lastK[paramIndex, nIndex] = k;                    
            }
        }

        private void Validation(LimitedDouble n, LimitedDouble k)
        {
            if (n.IsInt() && k.IsHalfInt())
                return;
            if (n.IsHalfInt() && k.IsInt())
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

        public LimitedDouble LastIndexK(PN pn, LimitedDouble n)
        {
            var paramIndex = (int)pn;
            var nIndex = ConvertToNIndex(n);
            return lastK[paramIndex, nIndex];
        }

        public LimitedDouble LastIndexN(PN pn)
        {
            var paramIndex = (int)pn;
            return lastN[paramIndex];
        }





        const int countParamsSn = 15;
        private DataSnArray dataSn = new DataSnArray(countParamsSn); 
        private LastNArray lastNSn = new LastNArray(countParamsSn); 

        public double GetSn(PN pn, LimitedDouble n)
        {
            if (pn == PN.One_minus_m)
                return 1.02 - GetSn(PN.m, n);

            var paramIndex = (int)pn;
            var nIndex = ConvertToNIndex(n);

            return dataSn[paramIndex, nIndex];
        }

        public void SetSn(PN pn, LimitedDouble n, double value)
        {
            var paramIndex = (int)pn;
            var nIndex = ConvertToNIndex(n);

            dataSn[paramIndex, nIndex] = value;
            if (n > lastNSn[paramIndex])
                lastNSn[paramIndex] = n;
        }
        public LimitedDouble LastIndexNSn(PN pn)
        {
            var paramIndex = (int)pn;

            return lastNSn[paramIndex];
        }        
        
        //public double[,] GetFullData(int pn)
        //{
        //    double[,] datapn = new double[data.GetLength(1),data.GetLength(2)];
        //    for (int i = 0; i < data.GetLength(1); i++)
        //        for(int j = 0; j < data.GetLength(2); j++)
        //        {
        //            datapn[i,j] = data[pn,i,j];
        //        }
        //    return datapn;
        //}
    }
}
