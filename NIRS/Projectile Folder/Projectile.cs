using NIRS.Interfaces;
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
        public double d { get; }
        public double r { get; }
        public Projectile(double q, double d)
        {
            this.q = q;
            this.d = d;
            this.r = d / 2;
        }
    }
}
