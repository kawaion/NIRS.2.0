using NIRS.Helpers;
using System;
using System.Collections.Generic;

namespace NIRS.Barrel_Folder
{
    static class VBarrelParts
    {
        public static Dictionary<Point2D, double> GetListOfBarrelVFromBottomToBendingPoint(this List<Point2D> bendingPoints)
        {
            var listVBarrelSegments = GetListVBarrelSegments(bendingPoints);
            var listOfVBarrelFromBottomToBendingPoint = GetListOfVBarrelFromBottomToBendingPoin(bendingPoints, listVBarrelSegments);
            return listOfVBarrelFromBottomToBendingPoint;
        }



        private static List<double> GetListVBarrelSegments(List<Point2D> bendingPoints)
        {
            List<double> listVBarrelSegments = new List<double>();

            for (int i = 1; i < bendingPoints.Count; i++)
            {
                var vBarrelSegment = GetVBarrelSegmentBetweenTwoPoints(bendingPoints[i - 1], bendingPoints[i]);
                listVBarrelSegments = AddVBarrelSegmentInList(listVBarrelSegments, vBarrelSegment);
            }

            return listVBarrelSegments;
        }        
        // перенести во что то отдельное
        public static double GetVBarrelSegmentBetweenTwoPoints(Point2D p1 ,Point2D p2)
        {
            (var h,var r1, var r2) = GetTheParametersOfTheBarrelPart(p1, p2);

            double vBarrelSegment = 1.0 / 3 * Math.PI * h * (Math.Pow(r1, 2) + Math.Pow(r2, 2) + r1 * r2);
            return vBarrelSegment;
        }         
        private static (double h, double r1, double r2) GetTheParametersOfTheBarrelPart(Point2D p1, Point2D p2)
        {
            double h = Math.Abs(p2.X - p1.X);
            double r1 = p1.Y;
            double r2 = p2.Y;

            return (h, r1, r2);
        }       
        private static List<double> AddVBarrelSegmentInList(List<double> vBarrelSegments, double vBarrelSegment)
        {
            vBarrelSegments.Add(vBarrelSegment);
            return vBarrelSegments;
        }



        private static Dictionary<Point2D, double> GetListOfVBarrelFromBottomToBendingPoin(List<Point2D> bendingPoints, List<double> listVBarrelSegments)
        {
            Dictionary<Point2D, double> listOfVBarrelFromBottomToBendingPoint = new Dictionary<Point2D, double>();
            for (int i = 0; i < bendingPoints.Count; i++)
            {
                var vBarrelFromBottomToBendingPoint = SumPartsOfVFromZeroToMaxIndex(i, listVBarrelSegments);
                listOfVBarrelFromBottomToBendingPoint = AddVBarrelFromBottomToBendingPointInList(listOfVBarrelFromBottomToBendingPoint, bendingPoints[i], vBarrelFromBottomToBendingPoint);
            }

            return listOfVBarrelFromBottomToBendingPoint;
        }
        private static double SumPartsOfVFromZeroToMaxIndex(int maxIndex, List<double> listVBarrelSegments)
        {
            double v = 0;
            for(int i = 0; i < maxIndex; i++)
            {
                v += listVBarrelSegments[i];
            }
            return v;
        }
        private static Dictionary<Point2D,double> AddVBarrelFromBottomToBendingPointInList(Dictionary<Point2D, double> listOfBarrelVFromBottomToBendingPoint, Point2D bendingPoint, double barrelVFromBottomToBendingPoint)
        {
            listOfBarrelVFromBottomToBendingPoint.Add(bendingPoint, barrelVFromBottomToBendingPoint);
            return listOfBarrelVFromBottomToBendingPoint;
        }
    }
}
