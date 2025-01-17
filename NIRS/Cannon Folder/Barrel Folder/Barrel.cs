using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIRS.Helpers;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    internal class Barrel : IBarrel
    {
        public List<Point2D> BendingPoints { get; }
        public Point2D EndChamber { get; }
        public Dictionary<Point2D, double> VFromBottomBoreToBendingPoints { get; }
        public Barrel(List<Point2D> points, Point2D endChamber)
        {
            (BendingPoints, EndChamber) = ShifterCannonToZero.Shift(points, endChamber);
            VFromBottomBoreToBendingPoints = BendingPoints.CalculateListOfBarrelVFromBottomToBendingPoint();
        }
    }
}
