﻿using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Interfaces;

namespace NIRS.Helpers
{
    public class KGetter
    {
        double h;
        public KGetter(IConstParameters constParameters)
        {
            h = constParameters.h;
        }
        public double this[double x] => x / h;
    }
}
