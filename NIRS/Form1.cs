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
using System.Windows.Forms.DataVisualization.Charting;

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
        IMainData mainData;
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
            points.Add(new Point2D(6.322+1.015, 0.1524));

            //Point2D endChamber = new Point2D(1.1225, 0.1524);
            Point2D endChamber = new Point2D(1.015, 0.1524);

            double omega = 19;
            double d = 0.1524;

            IBarrel barrel = new Barrel(points,endChamber,Dimension.D);
            IPowder powder = new Powder_12_7(newConstParameters, barrel.BarrelSize, omega);
            IProjectile projectile = new Projectile(newConstParameters.q, d);
            mainData = new MainData(barrel, powder, newConstParameters, newInitialParameters, projectile);
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
            nForMaxP = FindNPMax();
            
            //ResultExtractor resultExtractor = new ResultExtractor(grid);
            //var dataT = resultExtractor.GetT(PN.p, mainData);
            //var dataPkn = resultExtractor.GetPKn();
            //var dataPSn = resultExtractor.GetPSn();
            //ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chart3);
            //chartPlaceholder.Add(dataT, dataPkn);
            //chartPlaceholder.Add(dataT, dataPSn);
            //chartPlaceholder.SetIntervalY(100);
            //chart3 = chartPlaceholder.GetChart;
        }
        double nForMaxP;
        private double FindNPMax()
        {
            double nForMaxP = double.MinValue;
            double maxP = double.MinValue;
            var N = grid.LastIndexN(PN.p);
            for (double n = 0; n <= N; n++)
            {
                for (double k = 0.5; k <= grid.LastIndexK(PN.p, n); k++)
                {
                    double currentP = grid[PN.p, n, k];
                    if (currentP > maxP)
                    {
                        maxP = currentP;
                        nForMaxP = n;

                    }
                }
                //if(n > 2092)
                //{
                //    int c = 0;
                //}
                    
            }
            return nForMaxP;
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
        Chart chartForDraw;
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            chartForDraw = chart3;
            switch (comboBox2.Text[0])
            {
                case '1': Draw1();break;
                case '2': Draw2();break;
                case '3': Draw3(); break;
                case '4': Draw4(); break;
                case '5': Draw5(); break;
                case '6': Draw6(); break;
                case '7': Draw7(); break;
            }
        }
        
        private void Draw1()
        {
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataTForP = resultExtractor.GetT(PN.p, mainData);
            var dataPkn = resultExtractor.GetPKn();
            var dataPSn = resultExtractor.GetPSn();
            var dataTForVsn = resultExtractor.GetT(PN.v, mainData);
            var dataVsn = resultExtractor.GetVSn();
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataTForP, dataPkn);
            chartPlaceholder.Add(dataTForP, dataPSn);
            chartPlaceholder.AddLeft(dataTForVsn, dataVsn);
            chartPlaceholder.SetIntervalX(1);
            chartPlaceholder.SetMaxY(500);
            chartPlaceholder.SetMaxYLeft(2000);
            chartPlaceholder.SetIntervalCount(5);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw2()
        {
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataX = resultExtractor.GetX(PN.ro, nForMaxP, mainData);
            var dataRo = resultExtractor.GetRo(nForMaxP);
            var dataTemperature = resultExtractor.GetTemperature(nForMaxP, mainData);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataX, dataRo);
            chartPlaceholder.AddLeft(dataX, dataTemperature);
            chartPlaceholder.SetIntervalX(0.25);
            chartPlaceholder.SetMaxY(400);
            chartPlaceholder.SetMaxYLeft(4000);
            chartPlaceholder.SetIntervalCount(4);

            chartForDraw = chartPlaceholder.GetChart;
        }        
        private void Draw3()
        {
            double n = grid.LastIndexN(PN.ro);
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataX = resultExtractor.GetX(PN.ro, n, mainData);
            var dataRo = resultExtractor.GetRo(n);
            var dataTemperature = resultExtractor.GetTemperature(n, mainData);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataX, dataRo);
            chartPlaceholder.AddLeft(dataX, dataTemperature);
            chartPlaceholder.SetIntervalX(0.5);
            chartPlaceholder.SetMaxY(400);
            chartPlaceholder.SetMaxYLeft(4000);
            chartPlaceholder.SetIntervalCount(4);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw4()
        {
            double n = nForMaxP;
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataXForP = resultExtractor.GetX(PN.p, n, mainData);
            var dataXForV = resultExtractor.GetX(PN.v, n, mainData);
            var dataP = resultExtractor.GetP(n);
            var dataVtw = resultExtractor.GetV(n);
            var dataWgas = resultExtractor.GetW(n);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataXForP, dataP);
            chartPlaceholder.AddLeft(dataXForV, dataVtw);
            chartPlaceholder.AddLeft(dataXForV, dataWgas);
            chartPlaceholder.SetIntervalX(0.25);
            chartPlaceholder.SetMaxY(500);
            chartPlaceholder.SetMaxYLeft(500);
            chartPlaceholder.SetIntervalCount(5);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw5()
        {
            double n = grid.LastIndexN(PN.p);
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataXForP = resultExtractor.GetX(PN.p, n, mainData);
            var dataXForV = resultExtractor.GetX(PN.v, n - 0.5, mainData);
            var dataP = resultExtractor.GetP(n);
            var dataVtw = resultExtractor.GetV(n - 0.5);
            var dataWgas = resultExtractor.GetW(n - 0.5);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataXForP, dataP);
            chartPlaceholder.AddLeft(dataXForV, dataVtw);
            chartPlaceholder.AddLeft(dataXForV, dataWgas);
            chartPlaceholder.SetIntervalX(0.25);
            chartPlaceholder.SetMaxY(500);
            chartPlaceholder.SetMaxYLeft(500);
            chartPlaceholder.SetIntervalCount(5);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw6()
        {
            double n = nForMaxP;
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataX = resultExtractor.GetX(PN.a, n, mainData);
            var dataA = resultExtractor.GetA(n, mainData);
            var dataPsi = resultExtractor.GetPsi(n, mainData);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataX, dataA);
            chartPlaceholder.AddLeft(dataX, dataPsi);
            chartPlaceholder.SetIntervalX(0.25);
            chartPlaceholder.SetMaxY(1);
            chartPlaceholder.SetMaxYLeft(1);
            chartPlaceholder.SetIntervalCount(4);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw7()
        {
            double n = grid.LastIndexN(PN.p);
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataX = resultExtractor.GetX(PN.p, n, mainData);
            var dataEpure = resultExtractor.GetEpure(mainData);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataX, dataEpure);
            chartPlaceholder.SetIntervalX(0.25);
            chartPlaceholder.SetMaxY(500);
            chartPlaceholder.SetIntervalCount(5);

            chartForDraw = chartPlaceholder.GetChart;
        }
    }
    delegate void Draw(IGrid grid); 
}
