using NIRS.Parameter_Type;
using NIRS.Projectile_Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Interfaces
{
    public interface IGridCellProjectile : IGridCell
    {
        double v_sn { get; set; }
        double x { get; set; }
    }
}
