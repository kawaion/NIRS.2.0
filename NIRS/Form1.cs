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
using NIRS.For_chart;

namespace NIRS
{
    public partial class Form1 : Form
    {
        IInputDataTransmitter inputDataTransmitter = new InputDataTransmitter();
        public Form1()
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            //chart2.ChartAreas[0].AxisY.Interval = 100000000;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IInitialParameters initialParameters = new InitialParametersCase1();
            double h = 0.0126875; //0.0025;
            double tau = 3.171875e-6; //curantTau(h, 945);
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
            grid = numericalMethod.Calculate();
            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = (int)grid.LastIndexN(PN.m);
            var tmp = grid.GetSn(PN.vSn, grid.LastIndexN(PN.v));
            var maxN = grid.LastIndexN(PN.p);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chart2);
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataT = resultExtractor.GetT(PN.p, mainData);
            var dataPkn = resultExtractor.GetPKn();
            var dataPSn = resultExtractor.GetPSn();
            chartPlaceholder.Add(dataT, dataPkn);
            chartPlaceholder.SetIntervalY(100);
            chartPlaceholder.Add(dataT, dataPSn);
            chart2 = chartPlaceholder.GetChart;
        }
        IGrid grid;
        private void ShowLayer(double n,List<PN> pns)
        {
            for (int j = 0; j < pns.Count; j++)
                chart1.Series[j].Points.Clear();
            var last = grid.LastIndexK(pns[0], n);
            for (double i = 0; i <= last; i++)
                for(int j = 0;j<pns.Count;j++)
                    chart1.Series[j].Points.AddXY(i, grid[pns[j],n,i]);
        }
        private double curantTau(double h, double v)
        {
            double c = 340;
            return h / (v + c);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var h = GetStep();
            textBox1.Text = (Convert.ToDouble(textBox1.Text) - h).ToString();
            //Visualise();
        }

        private double GetStep()
        {
            if (radioButton1.Checked) return 1;
            else if (radioButton2.Checked) return 10;
            else if (radioButton3.Checked) return 100;
            throw new Exception();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var h = GetStep();
            textBox1.Text = (Convert.ToDouble(textBox1.Text) + h).ToString();
            //Visualise();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Visualise();
        }

        private void Visualise(PN pn)
        {
            var pns = new List<PN>() {pn};
            var n = Convert.ToDouble(textBox1.Text);
            if (n > grid.LastIndexN(pns[0])-1)
            {
                n = grid.LastIndexN(pns[0])-1;
                textBox1.Text = n.ToString();
            }
            ShowLayer(n, pns);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            var n = hScrollBar1.Value;
            textBox1.Text = Convert.ToString(n);
            var pn = GetPNFromComboBox();
            Visualise(pn);
        }
        private PN GetPNFromComboBox()
        {
            var pn = comboBox1.Text;
            switch (pn)
            {
                case "p":return PN.p;
                case "psi": return PN.psi;
                default: throw new Exception();
            }
        }
        //private void DrawGrid(IGrid grid)
        //{
        //    PN pn = PN.z;
        //    for (LimitedDouble i = grid.MinN+1;i<=grid.MaxN; i+=1)
        //        for(LimitedDouble j = grid[i].MinK; j <= grid[i].MaxK(pn); j += 1)
        //        {
        //            chart1.BeginInvoke(new Action(()=>chart1.Series[0].Points.AddXY(j.Value, i.Value)));
        //        }
        //    var maxN = grid.MaxN;
        //    if(maxN == 7.5)
        //    {

        //    }
        //    for (LimitedDouble j = grid[maxN].MinK; j <= grid[maxN].MaxK(pn); j += 1)
        //    {
        //        chart2.BeginInvoke(new Action(() => chart2.Series[0].Points.AddXY(j.Value, grid[maxN][j][pn])));
        //        //chart2.Series[0].Points.AddXY(j.Value, grid[maxN][j][pn]);
        //    }

        //}
    }
    delegate void Draw(IGrid grid); 
}
