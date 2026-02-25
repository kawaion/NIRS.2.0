using Core.Domain.Enums;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Numerical_methods.SEL.Services;
using Core.Domain.Physical.Interfaces;
using Core.Domain.Services;
using static Core.Domain.Limited_Double.LimitedDouble;

namespace Core.Domain.Numerical_methods.SEL.Functions.Services;

internal class WaypointCalculator : IWaypointCalculator
{
    private readonly IGrid g;

    private INumericalMethodSettings _settings;
    private readonly ICannon _c;
    private IPseudoViscosityMechanism _q;

    private readonly IXGetter _x;

    public WaypointCalculator(IGrid grid, 
                              INumericalMethodSettings settings,
                              ICannon cannon,
                              IPseudoViscosityMechanism q,
                              IXGetter xGetter)
    {
        g = grid;
        _settings = settings;
        _c = cannon;
        _q = q;
        _x = xGetter;

        sn = new WaypointCalculatorProjectile(g, settings, cannon, xGetter);
    }


    public double NablaWithS(PN param1, PN param2, LimitedDouble N, LimitedDouble K)
    {
        (var n, var k) = OffseterNK.AppointAndOffset(N, + ld0_5, K, - ld0_5);

        return (AverageWithS(param1, param2, n + ld0_5, k) - AverageWithS(param1, param2, n + ld0_5, k - ld1)) / _settings.h;
    }
    private double AverageWithS(PN mu, PN v, LimitedDouble n, LimitedDouble k)
    {
        // формула преобразуется в значение на n,k
        double V = g[v, n, k];

        if (V >= 0)
        {
            return g[mu, n - ld0_5, k - ld0_5] * _c.S(_x[k - ld0_5]) * V;
        }

        else
        {
            return g[mu, n - ld0_5, k + ld0_5] * _c.S(_x[k + ld0_5]) * V;
        }

    }
    private double AverageWithS(double mu, PN v, LimitedDouble n, LimitedDouble k)
    {
        // формула преобразуется в значение на n,k
        double V = g[v, n, k];

        if (V >= 0)
            return V * _c.S(_x[k - ld0_5]);
        else
            return V * _c.S(_x[k + ld0_5]);
    }
    //
    private double Get_m(LimitedDouble n, LimitedDouble k, PN mu)
    {
        if (mu == PN.One_minus_m)
            return 1 - g[PN.m, n, k];
        return g[mu, n, k];
    }


    public double Nabla(PN param1, PN v, LimitedDouble N, LimitedDouble K)
    {
        if (ParameterTypeGetter.IsDynamic(param1))
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, - ld0_5, K, ld0);

            return (DynamicAverage(param1, v, n - ld0_5, k + ld0_5, NablaType.plus) - DynamicAverage(param1, v, n - ld0_5, k - ld0_5, NablaType.minus)) / _settings.h;
        }

        if (ParameterTypeGetter.IsMixture(param1))
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + ld0_5, K, - ld0_5);

            return (MixtureAverage(param1, v, n + ld0_5, k, NablaType.plus) - MixtureAverage(param1, v, n + ld0_5, k - ld1, NablaType.minus)) / _settings.h;
        }

        throw new Exception($"неизвестные параметры {param1} и {v} на слое {N} {K}");
    }
    private double DynamicAverage(PN mu, PN v, LimitedDouble N, LimitedDouble K, NablaType type)
    {
        if (type == NablaType.plus)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, - ld0_5, K, + ld0_5);

            double sum_v = g[v, n - ld0_5, k] + g[v, n - ld0_5, k + ld1];
            if (sum_v >= 0)
                return sum_v / 2 * g[mu, n - ld0_5, k];
            else
                return sum_v / 2 * g[mu, n - ld0_5, k + ld1];
        }
        if (type == NablaType.minus)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, - ld0_5, K, - ld0_5);

            double sum_v = g[v, n - ld0_5, k - ld1] + g[v, n - ld0_5, k];
            if (sum_v >= 0)
                return sum_v / 2 * g[mu, n - ld0_5, k - ld1];
            else
                return sum_v / 2 * g[mu, n - ld0_5, k];
        }
        throw new Exception();
    }
    private double MixtureAverage(PN fi, PN V, LimitedDouble N, LimitedDouble K, NablaType type)
    {
        if (type == NablaType.plus)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + ld0_5, K, ld0);

            double v = g[V, n + ld0_5, k];

            if (v >= 0)
            {
                if (v == 0) return 0;
                return v * g[fi, n, k - ld0_5];
            }

            else
                return v * g[fi, n, k + ld0_5];

        }
        if (type == NablaType.minus)
        {
            (var n, var k) = OffseterNK.AppointAndOffset(N, + ld0_5, K, - ld1);

            double v = g[V, n + ld0_5, k - ld1];
            if (v >= 0)
            {
                if (v == 0) return 0;
                return v * g[fi, n, k - ld1_5];
            }
            else
                return v * g[fi, n, k - ld0_5];
        }
        throw new Exception();
    }


    public double Nabla(PN v, LimitedDouble N, LimitedDouble K)
    {
        (var n, var k) = OffseterNK.AppointAndOffset(N, + ld0_5, K, - ld0_5);

        return (g[v, n + ld0_5, k] - g[v, n + ld0_5, k - ld1]) / _settings.h;
    }

    public IWaypointCalculatorProjectile sn { get; set; }    
    
    enum NablaType
    {
        plus,
        minus
    }
}
