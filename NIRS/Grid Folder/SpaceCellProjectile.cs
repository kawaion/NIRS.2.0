using MyDouble;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    public class SpaceCellProjectile : SpaceCell
    {
        public double vSn {get; set; }
        public double x { get; set; }
    }
}
