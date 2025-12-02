using Core.Domain.Common;
using Core.Domain.Exceptions;
using Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;

namespace Core.Domain.ValueObjects
{
    [DebuggerDisplay("Points: {BendingPoints.Count}, Chamber: {EndChamber}, Length: {Length}")]
    internal sealed class Barrel : ValueObject
    {
        public IReadOnlyCollection<Point2D> BendingPoints { get; }
        public double EndChamber { get; }
        public double Length { get; }

        private Barrel(List<Point2D> bendingPoints, double endChamber, double length)
        {
            BendingPoints = bendingPoints.AsReadOnly();
            EndChamber = endChamber;
            Length = length;
        }
        public static Barrel Create(
            List<Point2D> bendingPoints,
            double endChamber)
        {
            Validation(bendingPoints, endChamber);
            
            double length = bendingPoints.Last().X;
            return new Barrel(bendingPoints, endChamber, length);
        }
        private static void Validation(List<Point2D> bendingPoints, double endChamber)
        {
            if(bendingPoints is null)
                throw new ValueObjectRequiredException(nameof(bendingPoints));

            if (bendingPoints.Count < 2)
                throw new InvalidValueObjectException("At least 2 bending points are required");
            if (bendingPoints.Any(p => p.X < 0 || p.Y < 0))
                throw new InvalidValueObjectException("All coordinates must be non-negative"); 
            if (IsSortedAscending(bendingPoints, out var point1, out var point2))
                throw new InvalidValueObjectException($"the x-value of point {point1} must be less than the value of point {point2}");
            if (bendingPoints[0].X != 0)
                throw new ValueOutOfRangeException($"first {nameof(bendingPoints[0])}", bendingPoints[0].X, ComparisonOperator.Equal, 0);
            
            if (endChamber < bendingPoints[0] || endChamber > bendingPoints.Last()) 
                throw new ValueOutOfRangeException(nameof(endChamber), endChamber, bendingPoints[0].X, bendingPoints.Last().X);
        }
        public static bool IsSortedAscending(List<Point2D> list, out Point2D point1, out Point2D point2)
        {            
            point1 = null;
            point2 = null;
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i - 1].X.CompareTo(list[i].X) > 0) // предыдущий > текущего
                { 
                    point1 = list[i-1];
                    point2 = list[i];
                    return false;
                }
                    
            }


            return true;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return BendingPoints;
            yield return EndChamber;
            yield return Length;
        }
    }
}
