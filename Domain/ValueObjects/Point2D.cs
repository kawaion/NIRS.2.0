using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.ValueObjects
{
    public class Point2D
    {
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }
        public double X { get; set; }
        public double Y { get; set; }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Point2D p = obj as Point2D;
            if (p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return X == p.X && Y == p.Y;
        }

        public bool Equals(Point2D other)
        {
            // If parameter is null return false:
            if (other == null)
            {
                return false;
            }

            // Return true if the fields match:
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"{X} {Y}";
        }

    }
}
