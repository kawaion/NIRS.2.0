using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Projectile_Folder
{
    class Projectile : IProjectile
    {
        public double q { get; }
        public double x { get; set; }
        public double v { get; set; }
        public Projectile(double q)
        {
            this.q = q;
        }
    }
}
