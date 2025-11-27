using NIRS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Main_Data.Projectile_Folder
{
    internal class Projectile : IProjectile
    {
        public double q {  get; set; }
        public double d { get; set; }
        public double r => d / 2;

        public Projectile(double q, double d)
        {
            this.q = q;
            this.d = d;
        }
    }
}
