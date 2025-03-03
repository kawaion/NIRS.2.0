using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class SpaceCellProjectile : IGridCellProjectile
    {
        public double vSn {get; set; }
        public double x { get; set; }

        public double dynamic_m { get; set; }
        public double M { get; set; }
        public double v { get; set; }
        public double w { get; set; }
        public double r { get; set; }
        public double e { get; set; }
        public double eps { get; set; }
        public double psi { get; set; }
        public double z { get; set; }
        public double a { get; set; }
        public double m { get; set; }
        public double p { get; set; }
        public double ro { get; set; }


        public double this[PN pn]
        {
            get
            {
                return (double)GetType().GetProperty(pn.ToString()).GetValue(this);
            }
            set
            {
                GetType().GetProperty(pn.ToString()).SetValue(this, value);
            }
        }
    }
}
