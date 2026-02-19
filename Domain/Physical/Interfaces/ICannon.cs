using Core.Domain.Common;
using Core.Domain.Physical.ValueObjects;
using Core.Domain.Physical.ValueObjects.Main;
using Core.Domain.Points.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Physical.Interfaces;

internal interface ICannon
{
    double ChamberLength { get; }
    double Length { get; }

    double Skn { get; }
    double Wkm { get; }

    double R(double x);
    double S(double x);
    double W(double x);
}
