using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Node_point_interpolation
{
    class Interpolator
    {
        private readonly IGrid g;
        private readonly IConstParameters constP;

        public Interpolator(IGrid grid, IConstParameters constParameters)
        {
            g = grid;
            constP = constParameters; 
        }

        public IGrid Interpolate(LimitedDouble n)
        {
            var k = GetKLastNode(n);

            while (isNodeToProjectile(n,k+1))
            {
                
            }
        }
        // сделать класс checker
        private double GetTheProjectileNodeCoordinate(LimitedDouble n)
        {
            var x = g[n].sn.P.x;
            var value = x / constP.h;
            return value;
        }
        public bool isNodeToProjectile(LimitedDouble n, LimitedDouble k)
        {
            var kStroke = GetTheProjectileNodeCoordinate(n);

            return k.Value < kStroke;
        }
        private LimitedDouble GetKLastNode(LimitedDouble n)
        {
            return g[n].Count();
        }
    }
}
