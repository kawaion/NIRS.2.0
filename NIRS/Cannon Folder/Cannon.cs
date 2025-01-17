using NIRS.BarrelFolder;
using NIRS.CannonFolder.PowderFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.CannonFolder
{
    class Cannon
    {
        public Cannon(Barrel barrel,Powder powder)
        {
            Barrel = barrel;
            Powder = powder;
        }

        public Barrel Barrel { get; }
        public Powder Powder { get; }
    }
}
