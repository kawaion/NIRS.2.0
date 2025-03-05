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
        public IPowder Powder { get;}
        public IConstParameters ConstParameters { get; }
        public IInitialParameters InitialParameters { get; }
        public IProjectile Projectile { get; }

        public MainData(IBarrel barrel, IPowder powder, IConstParameters constParameters, IInitialParameters initialParameters, IProjectile projectile)
        {
            Barrel = barrel;
            Powder = powder;
            ConstParameters = constParameters;
            InitialParameters = initialParameters;
            Projectile = projectile;
        }
    }
}
