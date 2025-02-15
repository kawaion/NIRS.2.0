using NIRS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    class FinderPointsBetweenCurrent
    {
        private readonly List<Point2D> _points;
        private int _MaxIndex;

        public FinderPointsBetweenCurrent(List<Point2D> points)
        {
            _points = points;
            _MaxIndex = GetTheIndexSoAsNotToCauseTheArrayToBeOverrun(points);
        }


        public (Point2D, Point2D) Find(double x)
        {
            int i = _MaxIndex;
            while (_points[i].X > x)
                i--;
            return (_points[i], _points[i + 1]);
        }        
        
        
        private int GetTheIndexSoAsNotToCauseTheArrayToBeOverrun(List<Point2D> points)
        {
            return points.Count - 2;
        }
    }
}
