using NIRS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Main_data
{
    class MainData : IMainData
    {
        public IBarrel Barrel { get; }
        public IBarrelSize BarrelSize { get; }
        public IPowder Powder { get;}
        public IBurningPowdersSize BurningPowdersSize { get; }
        public IConstParameters ConstParameters { get; }
        public IInitialParameters InitialParameters { get; }
        public IProjectile Projectile { get; }

        public MainData(IBarrel barrel, IBarrelSize barrelSize, IPowder powder, IBurningPowdersSize burningPowdersSize, IConstParameters constParameters, IInitialParameters initialParameters, IProjectile projectile)
        {
            Barrel = barrel;
            BarrelSize = barrelSize;
            Powder = powder;
            BurningPowdersSize = burningPowdersSize;
            ConstParameters = constParameters;
            InitialParameters = initialParameters;
            Projectile = projectile;
        }
    }
}
