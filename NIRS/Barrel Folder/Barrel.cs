using System;
using System.Collections.Generic;
using System.Drawing;
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
        public Barrel(List<Point2D> bendingPoints, Point2D endChamberPoint, Dimension dim)
        {
            BendingPoints = bendingPoints;
            EndChamberPoint = endChamberPoint;
            if (dim == Dimension.D)
            {
                BendingPoints.ForEach(point => point.Y /= 2);
                EndChamberPoint.Y /= 2;
            }
            (BendingPoints, EndChamberPoint) = ShifterCannonToZero.Shift(BendingPoints, EndChamberPoint);
            Length = BendingPoints.Last().X;

            BarrelSize = new BarrelSize(this);
        }


    }
    enum Dimension
    {
        R,
        D
    }
}
