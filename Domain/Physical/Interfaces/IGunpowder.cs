using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Physical.Interfaces;

internal interface IGunpowder
{
    /// <summary>
    /// ω (Омега) - Масса порохового заряда (кг) 
    /// </summary>
    double Omega { get; }

    /// <summary>
    /// δ (Дельта) - Плотность пороха (кг/м³) 
    /// </summary>
    double Delta { get; }

    /// <summary>
    /// S₀ - Начальная площадь поверхности порохового зерна (м²) 
    /// </summary>
    double S0 { get; }

    /// <summary>
    /// Λ₀ (Лямбда_0) - Начальный объем порохового заряда (м³)
    /// </summary>
    double LAMBDA0 { get; }

    /// <summary>
    /// θ = κ - 1
    /// </summary>
    double Theta { get; }

    /// <summary>
    /// f - Сила пороха 
    /// </summary>
    double f { get; }

    /// <summary>
    /// α - Коволюм 
    /// </summary>
    double alpha { get; }

    /// <summary>
    /// κ - Показатель адиабаты 
    /// </summary>
    double kappa { get; }

    /// <summary>
    /// Q - Теплотворная способность пороха 
    /// </summary>
    double Q { get; }
}
