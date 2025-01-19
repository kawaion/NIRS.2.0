using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder
{
    internal class Cannon
    {
        public Cannon(IBarrel barrel,IPowder powder)
        {
            Barrel = barrel;
            Powder = powder;
        }

        public IBarrel Barrel { get; }
        public IPowder Powder { get; }
    }
}
