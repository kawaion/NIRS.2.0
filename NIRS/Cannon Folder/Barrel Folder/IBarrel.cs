using NIRS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    interface IBarrel
    {
        List<Point2D> BendingPoints { get; }
        Point2D EndChamberPoint { get; }
        Dictionary<Point2D, double> VFromBottomBoreToBendingPoints { get; }
    }
}
