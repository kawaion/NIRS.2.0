﻿

namespace NIRS.Interfaces
{
    public interface IConstParameters
    {
        double tau { get; }
        double h { get; }
        double teta { get; }
        double alpha { get; }
        double delta { get; }
        double DELTA { get; }
        double D0 { get; }
        double d0 { get; }
        double L0 { get; }
        double mu0 { get; }
        double lamda0 { get; }
        double Q { get; }
        double e1 { get; }
        double u1 { get; }
        double omegaV { get; }
        double f { get; }
    }
}
