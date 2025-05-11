using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using MyDouble;

namespace NIRS.Interfaces
{
    public interface IHFunctions
    {
        double H1(double n, double k);
        double H2(double n, double k);
        double H3(double n, double k);
        double H4(double n, double k);
        double H5(double n, double k);
        double HPsi(double n, double k);

        IHFunctionsProjectile sn { get; set; }

        void Update(IGrid grid);
    }
}
