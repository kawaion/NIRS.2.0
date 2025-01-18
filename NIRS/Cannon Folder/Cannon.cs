using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.PowderFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder
{
    internal class Cannon
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
