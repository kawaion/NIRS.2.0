using NIRS.Parameter_Type;
using NIRS.Projectile_Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class IGridCellProjectile : IGridCell
    {
        public DynamicCharacteristicsFlow D { get; set; }
        public MixtureStateParameters M { get; set; }
        public ProjectileParameters P { get; set; }
    }
}
