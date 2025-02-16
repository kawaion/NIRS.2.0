using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Interfaces
{
    public interface IMainData
    {
        IBarrel Barrel { get; }
        IBarrelSize BarrelSize { get; }
        IPowder Powder { get; }
        IBurningPowdersSize BurningPowdersSize { get; }
        IConstParameters ConstParameters { get; }
        IInitialParameters InitialParameters { get; }
        IProjectile Projectile { get; }
    }
}
