using System;
using System.Windows.Forms;

using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Numerical_Method;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;
using NIRS.Cannon_Folder.Barrel_Folder;
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
            IConstParameters constParameters = new ConstParametersCase1(0,0);
            (var newInitialParameters, var newConstParameters) = (initialParameters, constParameters);//inputDataTransmitter.GetInputData(initialParameters, constParameters);
            List<Point2D> points = new List<Point2D>();
            points.Add(new Point2D(0, 0.214));
            points.Add(new Point2D(0.85, 0.214));
            points.Add(new Point2D(0.96, 0.196));
            points.Add(new Point2D(1.015, 0.164));
            points.Add(new Point2D(1.045, 0.155));
            points.Add(new Point2D(1.1225, 0.1524));
            points.Add(new Point2D(6.322, 0.1524));

            Point2D endChamber = new Point2D(1.1225, 0.1524);

            double omega = 19;
            double d = 0.1524;

            IBarrel barrel = new Barrel(points,endChamber,Dimension.D);
            IPowder powder = new Powder_12_7(newConstParameters, barrel.BarrelSize, omega);
            IProjectile projectile = new Projectile(newConstParameters.q, d);
            IMainData mainData = new MainData(barrel, powder, newConstParameters, newInitialParameters, projectile);
            INumericalMethod numericalMethod = new SEL(mainData);
            IGrid grid = numericalMethod.Calculate();
        }
    }
}
