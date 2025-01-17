using NIRS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    class BarrelSize : IBarrelSize
    {        
        private readonly IBarrel _barrel;
        FinderPointsBetweenCurrent _pointsBetween;
        public BarrelSize(IBarrel barrel)
        {
            _barrel = barrel;
            _pointsBetween = new FinderPointsBetweenCurrent(barrel.BendingPoints);
        }

        public double Skn { get; }
        public double Wkm { get; }


        public double R(double x)
        {
            (Point2D p1, Point2D p2) = _pointsBetween.Find(x);
            EquationOfLineFromTwoPoints equationOfLine = new EquationOfLineFromTwoPoints(p1, p2);
            return equationOfLine.Y(x);
        }        
        public double D(double x) => 2 * R(x);


        public double S(double x) => Math.Pow(R(x), 2) * Math.PI;
        //сделать поясняющие методы
        public double W(double x)
        {
            (Point2D p1, Point2D p2) = _pointsBetween.Find(x);
            EquationOfLineFromTwoPoints equationOfLine = new EquationOfLineFromTwoPoints(p1, p2);
            double y = equationOfLine.Y(x);
            Point2D px = new Point2D(x, y);
            double VSegment = VBarrelParts.CalcVBarrelSegment(p1, px);
            return _barrel.VFromBottomBoreToBendingPoints[p1] + VSegment;
        }
        public double W(double x1, double x2) => Math.Abs(W(x1) - W(x2));
    }
}
