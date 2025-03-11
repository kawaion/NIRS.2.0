using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using NIRS.Helpers;
using NIRS.Interfaces;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    internal class Barrel : IBarrel
    {
        public List<Point2D> BendingPoints { get; }
        public Point2D EndChamberPoint { get; }
        public IBarrelSize BarrelSize { get; }
        public double Length { get; }


        //сделать функции Clone
        public Barrel(List<Point2D> bendingPoints, Point2D endChamberPoint)
        {
            BendingPoints = bendingPoints;
            EndChamberPoint = endChamberPoint;
            (BendingPoints, EndChamberPoint) = ShifterCannonToZero.Shift(BendingPoints, EndChamberPoint);
            Length = BendingPoints.Last().X;

            BarrelSize = new BarrelSize(this);
        }


    }
}
