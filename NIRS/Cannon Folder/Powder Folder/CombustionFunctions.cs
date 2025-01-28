﻿using NIRS.Data_Parameters.Input_Data_Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder.Powder_Folder
{
    class CombustionFunctions : ICombustionFunctions
    {
        private readonly IPowder _powder;
        private readonly IConstParameters _constP;

        public CombustionFunctions(IPowder powder, IConstParameters constP)
        {
            _powder = powder;
            _constP = constP;
        }
        public double Psi(double z)
        {
            var psi = _powder.Kappa * z * (1 + _powder.Lamda * z + _powder.Mu * Math.Pow(z, 2));
            psi = PsiValidation(psi);
            return psi;
        }
        public double Sigma(double z, double psi)
        {
            if (z < 1)
                return 1.0 + 2.0 * _powder.Lamda * z + 3.0 * _powder.Mu * Math.Pow(z, 2);

            if (psi <= 1)
                return (1.0 + 2.0 * _powder.Lamda + 3.0 * _powder.Mu) 
                        * Math.Sqrt( Math.Abs( 
                                                (1.0 - psi) / (1.0 - _powder.PsiS) 
                                             ) 
                                   );

            throw new ArgumentException();
        }

        private static double PsiValidation(double psi)
        {
            if (psi > 1) 
                psi = 1;
            return psi;
        }

        public double z(double z)
        {
            throw new NotImplementedException();
        }

        public double Uk(double p)
        {
            return _constP.u1 * p;
        }
    }
}
