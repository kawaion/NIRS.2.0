using NIRS.Parameter_Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    class SpaceCell : GridCell
    {
        public override DynamicCharacteristicsFlow D { get; set; }
        public override MixtureStateParameters M { get; set; }
    }
}
