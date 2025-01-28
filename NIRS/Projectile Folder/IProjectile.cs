using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Projectile_Folder
{
    public interface IProjectile
    {
        double q { get; }
        double x { get; set; }
        double v { get; set; }
    }
}
