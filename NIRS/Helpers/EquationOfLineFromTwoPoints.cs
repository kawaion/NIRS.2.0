using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Helpers
{
    class EquationOfLineFromTwoPoints
    {
        private readonly Point2D _p1;
        private readonly Point2D _p2;

        public EquationOfLineFromTwoPoints(Point2D p1, Point2D p2)
        {
            _p1 = p1;
            _p2 = p2;
        }
        public double Y(double x) => (x - _p1.X) * (_p2.Y - _p1.Y) / (_p2.X - _p1.X) + _p1.Y;
    }
}
