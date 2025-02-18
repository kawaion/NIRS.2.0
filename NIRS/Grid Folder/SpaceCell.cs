using NIRS.Interfaces;
using NIRS.Parameter_Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class SpaceCell : IGridCell
    {
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
    }
}
