using NIRS.Helpers;
using NIRS.Data_Parameters.Input_Data_Parameters;
using System;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    class BarrelSize : IBarrelSize
    {        
        public double h { get; } // нужен для класса BarrelSizeByIndex
        private readonly IBarrel _barrel;
        FinderPointsBetweenCurrent _finderPointsBetweenCurrent;
        public BarrelSize(IBarrel barrel,IConstParameters constParameters)
        {
            _barrel = barrel;
            _finderPointsBetweenCurrent = new FinderPointsBetweenCurrent(barrel.BendingPoints);
            Skn = GetSkn();
            Wkm = GetWkm();
            h = constParameters.h;
        }

        public double Skn { get; }
        public double Wkm { get; }


        public double R(double x)
        {
            (var p1, var p2) = FindBendingPointsBetweenPoint(x);
            var RWhereX = GetRWhereX(x, p1, p2);
            return RWhereX;
        }
        public double D(double x) => 2 * R(x);
        public double S(double x) => Math.Pow(R(x), 2) * Math.PI;//pi*r^2
        //сделать поясняющие методы
        public double W(double x)
        {
            (var p1, var p2) = FindBendingPointsBetweenPoint(x);
            var r = GetRWhereX(x, p1, p2);
            var px = new Point2D(x, r);

            var VSegment = VBarrelParts.GetVBarrelSegmentBetweenTwoPoints(p1, px);
            var VUpToX = GetVUpToX(p1, VSegment);
            return VUpToX;
        }
        public double W(double x1, double x2) => Math.Abs(W(x1) - W(x2));


        private double GetSkn()
        {
            var xkn = _barrel.BendingPoints[0].X;
            return S(xkn);
        }
        private double GetWkm()
        {
            return _barrel.VFromBottomBoreToBendingPoints[_barrel.EndChamberPoint];
        }


        private double GetVUpToX(Point2D p1, double VSegment)
        {
            return _barrel.VFromBottomBoreToBendingPoints[p1] + VSegment;
        }
        private (Point2D,Point2D) FindBendingPointsBetweenPoint(double x)
        {
            return  _finderPointsBetweenCurrent.Find(x);
        }
        private static double GetRWhereX(double x, Point2D p1, Point2D p2)
        {
            var equationOfLine = GetLineFunctionFromTwoPoints(p1, p2);
            return equationOfLine.GetY(x);
        }
        private static EquationOfLineFromTwoPoints GetLineFunctionFromTwoPoints(Point2D p1, Point2D p2)
        {
            return new EquationOfLineFromTwoPoints(p1, p2);
        } 
    }
}
