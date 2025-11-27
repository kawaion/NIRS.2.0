using Core.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Core.Domain.ValueObjects
{
    [DebuggerDisplay("Points: {BendingPoints.Count}, Chamber: {EndChamber}, Length: {Length}")]
    internal sealed class BarrelCoordinate : IEquatable<BarrelCoordinate>
    {
        public IReadOnlyCollection<Point2D> BendingPoints { get; }
        public double EndChamber { get; }
        public double Length { get; }

        private BarrelCoordinate(List<Point2D> bendingPoints, double endChamber, double length)
        {
            BendingPoints = bendingPoints.AsReadOnly();
            EndChamber = endChamber;
            Length = length;
        }
        public static BarrelCoordinate Create(
            List<Point2D> bendingPoints,
            double endChamber)
        {
            //Validation(bendingPoints, endChamber);
            
            double length = bendingPoints.Last().X;
            return new BarrelCoordinate(bendingPoints, endChamber, length);
        }
        private void Validation(List<Point2D> bendingPoints, double endChamber)
        {
            if (BendingPoints.Count < 2)
                throw new DomainException("At least 2 bending points are required");
            if (EndChamber <= 0) throw new DomainException("End chamber must be positive");
            if (Length <= 0) throw new DomainException("Length must be positive");
            if (BendingPoints.Any(p => p.X < 0 || p.Y < 0))
                throw new DomainException("All coordinates must be non-negative");
        }
        public bool Equals(BarrelCoordinate? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return EndChamber.Equals(other.EndChamber) &&
                   Length.Equals(other.Length) &&
                   BendingPoints.SequenceEqual(other.BendingPoints);
        }
    }
}
