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
        public DynamicCharacteristicsFlow D { get; set; }
        public MixtureStateParameters M { get; set; }
    }
}
