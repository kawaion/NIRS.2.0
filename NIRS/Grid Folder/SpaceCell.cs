using MyDouble;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class SpaceCell : IGridCell
    {
        public double? dynamic_m { get; set; } = null;
        public double? M { get; set; } = null;
        public double? v { get; set; } = null;
        public double? w { get; set; } = null;
        public double? r { get; set; } = null;
        public double? e { get; set; } = null;
        public double? eps { get; set; } = null;
        public double? psi { get; set; } = null;
        public double? z { get; set; } = null;
        public double? a { get; set; } = null;
        public double? m { get; set; } = null;
        public double? p { get; set; } = null;
        public double? ro { get; set; } = null;

        public double? One_minus_m 
        { 
            get 
            {
                if (m == null)
                    return null;
                return 1 - m; 
            } 
        }

        public double? this[PN pn]
        {
            get
            {
                return (double?)GetType().GetProperty(pn.ToString()).GetValue(this);
            }
            set
            {
                GetType().GetProperty(pn.ToString()).SetValue(this, value);
            }
        }
    }

}
