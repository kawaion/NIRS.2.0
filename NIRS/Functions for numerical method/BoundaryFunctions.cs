using MyDouble;
using NIRS.Helpers;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Functions_for_numerical_method
{
    class BoundaryFunctions : IBoundaryFunctions
    {
        IConstParameters constP;
        IBarrelSize bs;
        IPowder powder;

        XGetter x;
        public BoundaryFunctions(IMainData mainData)
        {
            constP = mainData.ConstParameters;
            powder = mainData.Powder;
            bs = mainData.Barrel.BarrelSize;

            x = new XGetter(mainData.ConstParameters);
        }

        public double GetDynamic_k0(PN pn, double n)
        {
            switch (pn)
            {
                case PN.dynamic_m: return Get_dynamic_m0(n);
                case PN.M: return Get_M0(n);
                case PN.v: return Get_v0(n);
                case PN.w: return Get_w0(n);
            }
            throw new Exception();
        }
        public double GetDynamic_nMinus0Dot5(PN pn, double n)
        {
            switch (pn)
            {
                case PN.dynamic_m: return Get_dynamic_mMinus0Dot5(n);
                case PN.M: return Get_MMinus0Dot5(n);
                case PN.v: return Get_vMinus0Dot5(n);
                case PN.w: return Get_wMinus0Dot5(n);
            }
            throw new Exception();
        }
        public double GetDynamic_K(PN pn, double n)
        {
            switch (pn)
            {
                case PN.dynamic_m: return Get_dynamic_mK(n);
                case PN.M: return Get_MK(n);
                case PN.v: return Get_vK(n);
                case PN.w: return Get_wK(n);
            }
            throw new Exception();
        }
        public double GetMixture_n0(PN pn, double k)
        {
            switch (pn)
            {
                case PN.p: return Get_p0(k);
                case PN.ro: return Get_ro0(k);
                case PN.z: return Get_z0(k);
                case PN.psi: return Get_psi0(k);
                case PN.m: return Get_m0(k);
                case PN.a: return Get_a0(k);
                case PN.r: return Get_r0(k);
                case PN.e: return Get_e0(k);
            }
            throw new Exception();
        }

        public double Get_dynamic_m0(double n)
        {
            return 0;
        }
        public double Get_M0(double n)
        {
            return 0;
        }
        public double Get_v0(double n)
        {
            return 0;
        }
        public double Get_w0(double n)
        {
            return 0;
        }

        public double Get_dynamic_mMinus0Dot5(double k)
        {
            return 0;
        }
        public double Get_MMinus0Dot5(double k)
        {
            return 0;
        }
        public double Get_vMinus0Dot5(double k)
        {
            return 0;
        }
        public double Get_wMinus0Dot5(double k)
        {
            return 0;
        }

        public double Get_dynamic_mK(double n)
        {
            return 0;
        }
        public double Get_MK(double n)
        {
            return 0;
        }   
        public double Get_vK(double n)
        {
            return 0;
        }
        public double Get_wK(double n)
        {
            return 0;
        }

        public double Get_p0(double k)
        {
            return (0.3 * constP.omegaV * constP.f) /
                   (bs.Wkm - powder.Omega / powder.Delta - constP.alpha * constP.omegaV);
        }
        private double pV()
        {
            return Get_p0(0);
        }
        public double Get_ro0(double k)
        {
            return pV() /
                   (constP.alpha * pV() + constP.f);
        }
        public double Get_eps0(double k)
        {
            return constP.f /
                   constP.teta;
        }
        public double Get_z0(double k)
        {
            return 0;
        }
        public double Get_psi0(double k)
        {
            return 0;
        }
        public double Get_m0(double k)
        {
            return 1 - powder.DELTA / powder.Delta;
        }
        public double Get_a0(double k)
        {
            return powder.Omega/
                  (powder.LAMDA0 * powder.Delta * bs.Wkm);
        }
        public double Get_r0(double k)
        {
            return Get_ro0(k) * Get_m0(k) * bs.S(x[k]);
        }
        public double Get_e0(double k)
        {
            return Get_ro0(k) * Get_m0(k) * bs.S(x[k]) * Get_eps0(k);
        }

        public void Update()
        {
        }
    }
}
