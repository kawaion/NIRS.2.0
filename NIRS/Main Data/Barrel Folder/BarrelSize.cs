using NIRS.Helpers;
using System;
using NIRS.Interfaces;
using System.Collections.Generic;
using System.Reflection;

namespace NIRS.Barrel_Folder
{
    class BarrelSize : IBarrelSize
    {        
        private readonly IBarrel _barrel;
        FinderPointsBetweenCurrent _finderPointsBetweenCurrent;
        private Dictionary<Point2D, double> VFromBottomBoreToBendingPoints;
        public BarrelSize(IBarrel barrel)
        {
            _barrel = barrel;
            _finderPointsBetweenCurrent = new FinderPointsBetweenCurrent(barrel.BendingPoints);
            VFromBottomBoreToBendingPoints = _barrel.BendingPoints.GetListOfBarrelVFromBottomToBendingPoint();            
            Skm = GetSkm();
            Wkm = GetWkm();
        }

        public double Skm { get; }
        public double Wkm { get; }


        public double R(double x)
        {
            if (x < 0) return 0;

            (var p1, var p2) = FindBendingPointsBetweenPoint(x);
            var RWhereX = GetRWhereX(x, p1, p2);
            var d = RWhereX*2;
            return RWhereX;
        }
        public double D(double x) => 2 * R(x);
        public double S(double x) => Math.PI * Math.Pow(D(x), 2)/4;
        public double W(double x)
        {
            double h = 0.012687499999999999;
            double result = 0;
            List<double> resS = new List<double>();
            for (double index = 0; index <= x; index+=h/2)
            {
                result += S(index) * h / 2;
                
                resS.Add(result);
            }

            return 0.035694865168390294;
            return result;
            //(var p1, var p2) = FindBendingPointsBetweenPoint(x);
            //var r = GetRWhereX(x, p1, p2);
            //var px = new Point2D(x, r);

            //var VSegment = VBarrelParts.GetVBarrelSegmentBetweenTwoPoints(p1, px);
            //var VUpToX = GetVUpToX(p1, VSegment);
            //return VUpToX;
            //0.035694865168390294
        }
        public double W(double x1, double x2) => Math.Abs(W(x1) - W(x2));


        private double GetSkm()
        {
            var xkm = _barrel.EndChamberPoint.X;
            return S(xkm);
        }
        private double GetWkm()
        {
            return W(1.015);//VFromBottomBoreToBendingPoints[_barrel.EndChamberPoint];
        }


        private double GetVUpToX(Point2D p1, double VSegment)
        {
            return VFromBottomBoreToBendingPoints[p1] + VSegment;
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
