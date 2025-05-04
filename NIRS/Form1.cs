using System;
using System.Windows.Forms;

using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Numerical_Method;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;
using NIRS.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIRS.Interfaces;
using NIRS.Main_data;
using NIRS.Projectile_Folder;
using NIRS.Helpers;
using MyDouble;
using NIRS.Parameter_names;
using System.Net.NetworkInformation;
using System.Threading;

namespace NIRS
{
    public partial class Form1 : Form
    {
        IInputDataTransmitter inputDataTransmitter = new InputDataTransmitter();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IInitialParameters initialParameters = new InitialParametersCase1();
            double h = 0.0126875; //0.0025;
            double tau = 3.171875; //curantTau(h, 945);
            IConstParameters constParameters = new ConstParametersCase1(tau, h);
            (var newInitialParameters, var newConstParameters) = (initialParameters, constParameters);//inputDataTransmitter.GetInputData(initialParameters, constParameters);
            List<Point2D> points = new List<Point2D>();
            points.Add(new Point2D(0, 0.214));
            points.Add(new Point2D(0.85, 0.214));
            points.Add(new Point2D(0.96, 0.196));
            //points.Add(new Point2D(1.015, 0.164));
            //points.Add(new Point2D(1.045, 0.155));
            //points.Add(new Point2D(1.1225, 0.1524));
            //points.Add(new Point2D(6.322, 0.1524));
            points.Add(new Point2D(1.015, 0.1524));
            points.Add(new Point2D(6.322, 0.1524));

            //Point2D endChamber = new Point2D(1.1225, 0.1524);
            Point2D endChamber = new Point2D(1.015, 0.1524);

            double omega = 19;
            double d = 0.1524;

            IBarrel barrel = new Barrel(points,endChamber,Dimension.D);
            IPowder powder = new Powder_12_7(newConstParameters, barrel.BarrelSize, omega);
            IProjectile projectile = new Projectile(newConstParameters.q, d);
            IMainData mainData = new MainData(barrel, powder, newConstParameters, newInitialParameters, projectile);
            //INumericalMethod numericalMethod = new SEL(mainData,DrawGrid);
            //Task<IGrid> task = new Task<IGrid>(()=>numericalMethod.Calculate());
            //task.Start();
            //IGrid grid = task.Result;//numericalMethod.Calculate();

            INumericalMethod numericalMethod = new SEL(mainData);
            IGrid grid = numericalMethod.Calculate();
        }
        private double curantTau(double h, double v)
        {
            double c = 340;
            return h / (v + c);
        }
        private void DrawGrid(IGrid grid)
        {
            PN pn = PN.z;
            for (LimitedDouble i = grid.MinN+1;i<=grid.MaxN; i+=1)
                for(LimitedDouble j = grid[i].MinK; j <= grid[i].MaxK(pn); j += 1)
                {
                    chart1.BeginInvoke(new Action(()=>chart1.Series[0].Points.AddXY(j.Value, i.Value)));
                }
            var maxN = grid.MaxN;
            if(maxN == 7.5)
            {

            }
            for (LimitedDouble j = grid[maxN].MinK; j <= grid[maxN].MaxK(pn); j += 1)
            {
                chart2.BeginInvoke(new Action(() => chart2.Series[0].Points.AddXY(j.Value, grid[maxN][j][pn])));
                //chart2.Series[0].Points.AddXY(j.Value, grid[maxN][j][pn]);
            }

        }
    }
    delegate void Draw(IGrid grid); 
}
