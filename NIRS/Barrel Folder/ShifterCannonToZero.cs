using NIRS.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    static class ShifterCannonToZero
    {
        public static (List<Point2D> bendingPoints, Point2D endChamber) Shift(List<Point2D> bendingPoints, Point2D endChamber)
        {
            double bottomBorePoint = GetBottomBorePoint(bendingPoints);
            (var shiftedBendingPoints, var shiftedEndChamber) = ShiftCannonToZero(bendingPoints, endChamber, bottomBorePoint);
            return (shiftedBendingPoints, shiftedEndChamber);
        }

        private static (List<Point2D> bendingPoints, Point2D endChamber) ShiftCannonToZero(List<Point2D> bendingPoints, Point2D endChamber, double bottomBorePoint)
        {
            if (bottomBorePoint != 0)
            {
                endChamber.X -= bottomBorePoint;
                foreach (var point in bendingPoints)
                    point.X -= bottomBorePoint;
            }
            return (bendingPoints, endChamber);
        }

        private static double GetBottomBorePoint(List<Point2D> bendingPoints)
        {
            return bendingPoints[0].X;
        }
    }
}
