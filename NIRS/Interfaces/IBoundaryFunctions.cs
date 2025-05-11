using MyDouble;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace NIRS.Interfaces
{
    public interface IBoundaryFunctions
    {
        double GetDynamic_k0(PN pn, double n);
        double GetDynamic_nMinus0Dot5(PN pn, double k);
        double GetDynamic_K(PN pn, double n);
        double GetMixture_n0(PN pn, double k);

        void Update();
    }
}
