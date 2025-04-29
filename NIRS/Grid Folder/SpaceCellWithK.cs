using MyDouble;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    public class SpaceCellWithK : SpaceCell
    {
        private LimitedDouble K;
        public LimitedDouble k
        {
            get
            {
                return K;
            }
            set
            {
                if (isNullCell)
                {
                    K = value;
                    isNullCell = false;
                }
                else if (K == value)
                    return;
                else
                    throw new Exception("нельзя задать новое значение k");
            }
        }
        private bool isNullCell = true;
    }
}
