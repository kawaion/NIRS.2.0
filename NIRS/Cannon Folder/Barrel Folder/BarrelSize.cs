using NIRS.Helpers;
using System;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    class BarrelSize : IBarrelSize
    {        
        private readonly IBarrel _barrel;
        FinderPointsBetweenCurrent _finderPointsBetweenCurrent;
        public BarrelSize(IBarrel barrel)
        {
            _barrel = barrel;
            _finderPointsBetweenCurrent = new FinderPointsBetweenCurrent(barrel.BendingPoints);
        }

        public double Skn { get; }
        public double Wkm { get; }


        public double R(double x)
        {
            (Point2D p1, Point2D p2) = _finderPointsBetweenCurrent.Find(x);
            var equationOfLine = GetLineFunctionFromTwoPoints(p1, p2);
            var RWhereX = equationOfLine.GetY(x);
            return RWhereX;
        }        
        public double D(double x) => 2 * R(x);


        public double S(double x) => Math.Pow(R(x), 2) * Math.PI;//pi*r^2
        //сделать поясняющие методы
        public double W(double x)
        {
            (Point2D p1, Point2D p2) = _finderPointsBetweenCurrent.Find(x);
            var equationOfLine = GetLineFunctionFromTwoPoints(p1, p2);
            double y = equationOfLine.GetY(x);
            Point2D px = new Point2D(x, y);
            var VSegment = VBarrelParts.CalcVBarrelSegmentBetweenTwoPoints(p1, px);
            var VUpToX = _barrel.VFromBottomBoreToBendingPoints[p1] + VSegment;
            return VUpToX;
        }

        private static EquationOfLineFromTwoPoints GetLineFunctionFromTwoPoints(Point2D p1, Point2D p2)
        {
            return new EquationOfLineFromTwoPoints(p1, p2);
        }

        public double W(double x1, double x2) => Math.Abs(W(x1) - W(x2));
    }
}
