using MyDouble;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    abstract class SubGrid
    {
        public abstract GridCell this[LimitedDouble i] { get; set; } 
    }
}
