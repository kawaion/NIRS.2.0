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
using NIRS.Main_Data;
using NIRS.Projectile_Folder;
using NIRS.Helpers;
using MyDouble;
using NIRS.Parameter_names;
using System.Net.NetworkInformation;
using System.Threading;
using NIRS.For_chart;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using NIRS.Visualization.Progress;
using NIRS.Visualization;
using NIRS.Parameter_Type;
using System.IO;
using OfficeOpenXml;
using System.Diagnostics;
using NIRS.Main_Data.Projectile_Folder;
using NIRS.Main_Data.Input_Data_Parameters;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;

namespace NIRS
{
    public partial class Form1 : Form
    {
        IInputDataTransmitter inputDataTransmitter = new InputDataTransmitter();
        public Form1()
        {
            InitializeComponent();
            int N = 80;
            Point2D endChamber = new Point2D(1.015, 0.1524);
            IInitialParameters initialParameters = new InitialParametersCase1();
            double h = 1.015 / N; //0.0025;
            //double tau = 1.015 / (N * (2500 + 1500)); //curantTau(h, 945);
            double tau = 1.015 / (N * (1000 + 1500));
            var _parameters = new ConstParametersCase2(
            tau,                // 1 микросекунда
            N,
            endChamber  // Камера 0.5 метра
            );
            propertyGrid1.SelectedObject = _parameters;

            //ConfigureDataGridView();
            //chart2.ChartAreas[0].AxisY.Interval = 100000000;
        }
        IMainData mainData;
        private async void button1_Click(object sender, EventArgs e)
        {
            mainData = InitializeMainData();
            //INumericalMethod numericalMethod = new SEL(mainData,DrawGrid);
            //Task<IGrid> task = new Task<IGrid>(()=>numericalMethod.Calculate());
            //task.Start();
            //IGrid grid = task.Result;//numericalMethod.Calculate();
            progressBar1.Maximum = (int)(mainData.Barrel.Length / mainData.ConstParameters.h * 2);
            var progress = new Progress<ProgressInfo>(info =>
            {
                if (progressBar1.Maximum < info.progressbarValue)
                    progressBar1.Value = progressBar1.Maximum;
                else
                    progressBar1.Value = info.progressbarValue;
                labelN.Text = info.layerValue.ToString();
                labelTime.Text = info.time.ToString();
            });

            INumericalMethod numericalMethod = new SEL(mainData);
            Progresser progresser = new Progresser(progress);
            numericalMethod.ProgressActivate(progresser);

            grid = await Task.Run(() => numericalMethod.Calculate());
            //grid = numericalMethod.Calculate();

            //int minN = 1200;
            //int maxN = 1300;

            //var dataSheets = new Dictionary<string, double[,]>
            //{
            //    {"dynamic_m", grid.GetFullData(PN.dynamic_m, maxN)},
            //    {"v", grid.GetFullData(PN.v, maxN)},
            //    {"M", grid.GetFullData(PN.M, maxN)},
            //    {"w", grid.GetFullData(PN.w, maxN)},
            //    {"a", grid.GetFullData(PN.a, maxN)},
            //    {"e", grid.GetFullData(PN.e, maxN)},
            //    {"m_", grid.GetFullData(PN.m, maxN)},
            //    {"p", grid.GetFullData(PN.p, maxN)},
            //    {"r", grid.GetFullData(PN.r, maxN)},
            //    {"rho", grid.GetFullData(PN.rho, maxN)},
            //    {"z", grid.GetFullData(PN.z, maxN)},
            //    {"psi", grid.GetFullData(PN.psi, maxN)}
            //};
            //string programFolder = Application.StartupPath;
            //string parentFolder = Directory.GetParent(programFolder).FullName;
            //string fileName = "multi_sheet_data.xlsx";

            //try
            //{
            //    ExcelHelper.CreateExcelFileWithSheets(dataSheets, fileName, minN);

            //    // Показать сообщение о успешном сохранении
            //    MessageBox.Show($"✅ Файл первой программы успешно сохранен!\n\nПуть: {fileName}",
            //        "Сохранено", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    // Открыть папку с файлом
            //    if (MessageBox.Show("Открыть расположение файла?", "Файл сохранен",
            //        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        System.Diagnostics.Process.Start("explorer.exe", $"/select, \"{fileName}\"");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"❌ Ошибка при сохранении файла: {ex.Message}",
            //        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}


            InitializePostData();

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

        List<double> DymamicX;
        List<double> MixtureX;
        private void InitializePostData()
        {
            var tmp = grid.GetSn(PN.vSn, grid.LastIndexN(PN.v));
            var maxN = grid.LastIndexN(PN.p);
            nForMaxP = FindNPMax();
        }

        private IMainData InitializeMainData(int N = 80)
        {
            IInitialParameters initialParameters = new InitialParametersCase1();
            double h = 1.015 / N; //0.0025;
            //double tau = 1.015 / (N * (2500 + 1500)); //curantTau(h, 945);
            double tau = 1.015 / (N * (1000 + 1500));

            ;//inputDataTransmitter.GetInputData(initialParameters, constParameters);
            List<Point2D> points = new List<Point2D>();
            points.Add(new Point2D(0, 0.214));
            points.Add(new Point2D(0.85, 0.214));
            points.Add(new Point2D(0.96, 0.196));
            //points.Add(new Point2D(1.015, 0.164));
            //points.Add(new Point2D(1.045, 0.155));
            //points.Add(new Point2D(1.1225, 0.1524));
            //points.Add(new Point2D(6.322, 0.1524));
            points.Add(new Point2D(1.015, 0.1524));
            points.Add(new Point2D(6.322 + 1.015, 0.1524));

            //Point2D endChamber = new Point2D(1.1225, 0.1524);
            Point2D endChamber = new Point2D(1.015, 0.1524);

            IConstParameters constParameters = new ConstParametersCase2(tau, N, endChamber);
            (var newInitialParameters, var newConstParameters) = (initialParameters, constParameters);

            double omega = 19;
            double d = 0.1524;

            IBarrel barrel = new Barrel(points, endChamber, Dimension.D);
            IPowder powder = new Powder_12_7(newConstParameters, barrel.BarrelSize, omega);
            IProjectile projectile = new Projectile(newConstParameters.q, d);
            return new MainData(barrel, powder, newConstParameters, newInitialParameters, projectile);
        }

        LimitedDouble nForMaxP;
        private LimitedDouble FindNPMax()
        {
            LimitedDouble nForMaxP = new LimitedDouble(double.MinValue);
            double maxP = double.MinValue;
            var N = grid.LastIndexN(PN.p);
            for (LimitedDouble n = new LimitedDouble(0); n <= N; n++)
            {
                for (LimitedDouble k = new LimitedDouble(0.5); k <= grid.LastIndexK(PN.p, n); k++)
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
        private double curantTau(double h, double v)
        {
            double c = 340;
            return h / (v + c);
        }


        private void button4_Click(object sender, EventArgs e)
        {
            //Visualise();
        }

        Chart chartForDraw;
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            chartForDraw = chart3;
            switch (comboBox2.Text[0])
            {
                case '1': Draw1(); break;
                case '2': Draw2(); break;
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
            chartPlaceholder.SetIntervalX(2);
            chartPlaceholder.SetMaxY(500);
            chartPlaceholder.SetMaxYLeft(2000);
            chartPlaceholder.SetIntervalCount(5);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw2()
        {
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataX = resultExtractor.GetX(PN.rho, nForMaxP, mainData);
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
            LimitedDouble n = grid.LastIndexN(PN.rho);
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataX = resultExtractor.GetX(PN.rho, n, mainData);
            var dataRo = resultExtractor.GetRo(n);
            var dataTemperature = resultExtractor.GetTemperature(n, mainData);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataX, dataRo);
            chartPlaceholder.AddLeft(dataX, dataTemperature);
            chartPlaceholder.SetIntervalX(1);
            chartPlaceholder.SetMaxY(200);
            chartPlaceholder.SetMaxYLeft(3000);
            chartPlaceholder.SetIntervalCount(5);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw4()
        {
            LimitedDouble n = nForMaxP;
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
            chartPlaceholder.SetMaxY(600);
            chartPlaceholder.SetMaxYLeft(900);
            chartPlaceholder.SetIntervalCount(6);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw5()
        {
            LimitedDouble n = grid.LastIndexN(PN.p);
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataXForP = resultExtractor.GetX(PN.p, n, mainData);
            var dataXForV = resultExtractor.GetX(PN.v, n - 0.5, mainData);
            var dataP = resultExtractor.GetP(n);
            var dataVtw = resultExtractor.GetV(n - 0.5);
            //var dataWgas = resultExtractor.GetW(n - 0.5);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataXForP, dataP);
            chartPlaceholder.AddLeft(dataXForV, dataVtw);
            //chartPlaceholder.AddLeft(dataXForV, dataWgas);
            chartPlaceholder.SetIntervalX(0.5);
            chartPlaceholder.SetMaxY(150);
            chartPlaceholder.SetMaxYLeft(2000);
            chartPlaceholder.SetIntervalCount(5);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw6()
        {
            LimitedDouble n = nForMaxP;
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
            LimitedDouble n = grid.LastIndexN(PN.p);
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataX = resultExtractor.GetX(PN.p, n, mainData);
            var dataEpure = resultExtractor.GetEpure(mainData);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataX, dataEpure);
            chartPlaceholder.SetIntervalX(0.5);
            chartPlaceholder.SetMaxY(500);
            chartPlaceholder.SetIntervalCount(5);

            chartForDraw = chartPlaceholder.GetChart;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            chartVerification1.ChartAreas[0].AxisX.Interval = 0.5;
            chartVerification1.ChartAreas[0].AxisX.Minimum = -5;
            chartVerification1.ChartAreas[0].AxisX.Maximum = 0;
            chartVerification2.ChartAreas[0].AxisX.Interval = 0.01;
            chartVerification2.ChartAreas[0].AxisX.Minimum = 0;
            chartVerification2.ChartAreas[0].AxisX.Maximum = 0.1;

            chartVerification1.ChartAreas[0].AxisX.Title = "ln(h), м";
            chartVerification1.ChartAreas[0].AxisY.Title = "ln(Vд error), %";

            chartVerification2.ChartAreas[0].AxisX.Title = "1/N";
            chartVerification2.ChartAreas[0].AxisY.Title = "ln(Vд error), %";
            LimitedDouble lastN;
            LimitedDouble lastK;
            XGetter xGetter;

            List<int> numbersOfSplits = new List<int> { 10, 30, 40, 50, 60, 80 };
            List<double> Vd = new List<double>();
            List<double> h = new List<double>();
            List<double> errors = new List<double>();
            for (int i = 0; i < numbersOfSplits.Count; i++)
            {
                var mainData = InitializeMainData(numbersOfSplits[i]);
                xGetter = new XGetter(mainData.ConstParameters);
                INumericalMethod numericalMethod = new SEL(mainData);
                grid = numericalMethod.Calculate();
                lastN = grid.LastIndexN(PN.v);
                lastK = grid.LastIndexK(PN.v, lastN);
                Point2D p1 = new Point2D(xGetter[lastK], grid[PN.v, lastN, lastK]);
                Point2D p2 = new Point2D(grid.GetSn(PN.x, lastN), grid.GetSn(PN.vSn, lastN));
                EquationOfLineFromTwoPoints equationOfLineFromTwoPoints = new EquationOfLineFromTwoPoints(p1, p2);
                var res = equationOfLineFromTwoPoints.GetY(6.322 + 1.015);
                Vd.Add(res);
                h.Add(mainData.ConstParameters.h);
            }
            label4.Text = Vd.Last().ToString();
            for (int i = 0; i < Vd.Count; i++)
            {
                errors.Add(Math.Abs(Vd[i] - Vd.Last()) / Vd.Last() * 100);
            }
            List<double> hLog = new List<double>();
            List<double> errorsLog = new List<double>();
            for (int i = 0; i < errors.Count - 1; i++)
            {
                hLog.Add(Math.Log(h[i]));
                errorsLog.Add(Math.Log(errors[i]));
            }
            LeastSquaresSolver leastSquaresSolver = new LeastSquaresSolver(hLog.ToArray(), errorsLog.ToArray());
            (var p, var a, _) = leastSquaresSolver.CalculateRegression();
            for (int i = 0; i < hLog.Count; i++)
            {
                chartVerification1.Series[0].Points.AddXY(hLog[i], errorsLog[i]);
            }
            chartVerification1.Series[1].Points.AddXY(hLog[0], a + p * hLog[0]);
            chartVerification1.Series[1].Points.AddXY(hLog.Last(), a + p * hLog.Last());

            for (int i = 0; i < numbersOfSplits.Count; i++)
            {
                chartVerification2.Series[0].Points.AddXY(1.0 / numbersOfSplits[i], errors[i]);
                chartVerification2.Series[1].Points.AddXY(1.0 / numbersOfSplits[i], errors[i]);
            }
            label3.Text = p.ToString();

            dataGridView1.ColumnCount = 3;
            dataGridView1.RowCount = 2;

            LimitedDouble firstN;
            lastN = grid.LastIndexN(PN.r);
            if (lastN.IsInt())
            {
                firstN = new LimitedDouble(1);
            }
            else
            {
                firstN = new LimitedDouble(0.5);
            }
            double initialRSum = 0;
            double lastRSum = 0;
            double initialESum = 0;
            double lastESum = 0;

            LimitedDouble firstK;
            lastK = grid.LastIndexK(PN.r, firstN);
            if (lastK.IsInt())
            {
                firstK = new LimitedDouble(0);
            }
            else
            {
                firstK = new LimitedDouble(0.5);
            }
            mainData = InitializeMainData(numbersOfSplits.Last());
            xGetter = new XGetter(mainData.ConstParameters);
            var delta = mainData.Powder.Delta;
            var Q = mainData.ConstParameters.Q;
            //var eps = mainData.ConstParameters.f/mainData.ConstParameters.teta;

            var bs = mainData.Barrel.BarrelSize;
            for (var k = firstK; k <= lastK; k++)
            {
                initialRSum += grid[PN.r, firstN, k] + delta * (1 - grid[PN.m, firstN, k]) * bs.S(xGetter[k]);
                //eps = grid[PN.e, firstN, k] / (grid[PN.rho, firstN, k] * grid[PN.m, firstN, k] * bs.S(xGetter[k]));
                initialESum += grid[PN.e, firstN, k] + grid[PN.r, firstN, k] * grid[PN.v, firstN - 0.5, k - 0.5] * Math.Abs(grid[PN.v, firstN - 0.5, k - 0.5]) / 2
                                + delta * (1 - grid[PN.m, firstN, k]) * bs.S(xGetter[k]) * (Q + grid[PN.w, firstN - 0.5, k - 0.5] * Math.Abs(grid[PN.w, firstN - 0.5, k - 0.5]) / 2);
            }
            lastK = grid.LastIndexK(PN.r, lastN);
            for (var k = firstK; k <= lastK; k++)
            {
                lastRSum += grid[PN.r, lastN, k] + delta * (1 - grid[PN.m, lastN, k]) * bs.S(xGetter[k]);
                //eps = grid[PN.e, lastN, k] / (grid[PN.rho, lastN, k] * grid[PN.m, lastN, k] * bs.S(xGetter[k]));
                lastESum += grid[PN.e, lastN, k] + grid[PN.r, lastN, k] * grid[PN.v, lastN - 0.5, k - 0.5] * Math.Abs(grid[PN.v, lastN - 0.5, k - 0.5]) / 2
                                + delta * (1 - grid[PN.m, lastN, k]) * bs.S(xGetter[k]) * (Q + grid[PN.w, lastN - 0.5, k - 0.5] * Math.Abs(grid[PN.w, lastN - 0.5, k - 0.5]) / 2);
            }
            dataGridView2.ColumnCount = 3;
            dataGridView2.RowCount = 2;
            // Заполняем заголовки столбцов
            dataGridView2.Columns[0].HeaderText = "в начале";
            dataGridView2.Columns[1].HeaderText = "в конце";
            dataGridView2.Columns[2].HeaderText = "разница";

            // Заполняем данные
            dataGridView2.Rows[0].Cells[0].Value = initialRSum;
            dataGridView2.Rows[0].Cells[1].Value = lastRSum;
            dataGridView2.Rows[0].Cells[2].Value = Math.Abs(lastRSum - initialRSum);

            dataGridView2.Rows[1].Cells[0].Value = initialESum;
            dataGridView2.Rows[1].Cells[1].Value = lastESum;
            dataGridView2.Rows[1].Cells[2].Value = Math.Abs(lastESum - initialESum);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var pn = DictPN.Get(comboBoxPN.Text);
            PNDataArrayByN data = new PNDataArrayByN(pn, grid, mainData);
            FormPN form = new FormPN(data);
            form.Show();
        }









        //// Константы для настройки внешнего вида
        //private const int MIN_COLUMN_WIDTH = 80; // Минимальная ширина столбца
        //private const int ROW_HEIGHT = 25;       // Высота строки
        //private const int MAX_VISIBLE_ROWS = 30; // Максимальное количество строк перед включением прокрутки
        //private const int MAX_VISIBLE_COLS = 15; // Максимальное количество столбцов перед включением прокрутки
        ////private void ConfigureDataGridView()
        ////{
        ////    // Базовая конфигурация DataGridView
        ////    dataGridView1.AutoGenerateColumns = false;
        ////    dataGridView1.AllowUserToAddRows = false;
        ////    dataGridView1.AllowUserToDeleteRows = false;
        ////    dataGridView1.ReadOnly = true;
        ////    dataGridView1.RowHeadersVisible = true;
        ////    dataGridView1.ColumnHeadersVisible = true;
        ////    dataGridView1.ScrollBars = ScrollBars.Both;
        ////    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // Полностью ручное управление
        ////    dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        ////}
        //public void DisplayMatrix(double[,] matrix)
        //{
        //    if (matrix == null) return;

        //    dataGridView1.SuspendLayout();
        //    try
        //    {
        //        ClearDataGridView();

        //        int rowCount = matrix.GetLength(0);
        //        int colCount = matrix.GetLength(1);

        //        // 1. Создаем столбцы с фиксированной шириной
        //        CreateColumns(colCount);

        //        // 2. Находим диапазон значений для цветовой схемы
        //        CalculateValueRange(matrix, out double min, out double max);

        //        // 3. Заполняем данными
        //        PopulateData(matrix, rowCount, colCount, min, max);

        //        // 4. Настраиваем размеры
        //        AdjustGridSizes(rowCount, colCount);
        //    }
        //    finally
        //    {
        //        dataGridView1.ResumeLayout();
        //    }
        //}

        //private void ClearDataGridView()
        //{
        //    dataGridView1.Columns.Clear();
        //    dataGridView1.Rows.Clear();
        //}

        //private void ConfigureDataGridView()
        //{
        //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        //    dataGridView1.AllowUserToAddRows = false;
        //    dataGridView1.AllowUserToDeleteRows = false;
        //    dataGridView1.ReadOnly = true;
        //}

        //private void CreateColumns(int colCount)
        //{
        //    dataGridView1.Columns.Clear();

        //    for (int j = 0; j < colCount; j++)
        //    {
        //        dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
        //        {
        //            Name = $"Column_{j}",
        //            HeaderText = $"Col {j}",
        //            Width = MIN_COLUMN_WIDTH,
        //            FillWeight = 100,
        //            DefaultCellStyle = new DataGridViewCellStyle
        //            {
        //                Alignment = DataGridViewContentAlignment.MiddleCenter,
        //                Format = "F2",
        //                BackColor = Color.White
        //            }
        //        });
        //    }
        //}

        //private void CalculateValueRange(double[,] matrix, out double min, out double max)
        //{
        //    min = double.MaxValue;
        //    max = double.MinValue;

        //    foreach (double value in matrix)
        //    {
        //        if (value < min) min = value;
        //        if (value > max) max = value;
        //    }

        //    if (min == max) // Если все значения одинаковые
        //    {
        //        min = max - 1; // Чтобы избежать деления на ноль
        //    }
        //}

        //private void PopulateData(double[,] matrix, int rowCount, int colCount, double min, double max)
        //{
        //    for (int i = 0; i < rowCount; i++)
        //    {
        //        dataGridView1.Rows.Add();
        //        dataGridView1.Rows[i].HeaderCell.Value = $"Row {i}";

        //        for (int j = 0; j < colCount; j++)
        //        {
        //            var cell = dataGridView1.Rows[i].Cells[j];
        //            cell.Value = matrix[i, j];
        //            cell.Style.BackColor = GetValueColor(matrix[i, j], min, max);
        //        }
        //    }
        //}

        //private Color GetValueColor(double value, double min, double max)
        //{
        //    double normalized = (value - min) / (max - min);
        //    normalized = Math.Max(0, Math.Min(1, normalized));

        //    // Градиент от синего (мин) к красному (макс)
        //    int red = (int)(255 * normalized);
        //    int blue = (int)(255 * (1 - normalized));
        //    int green = 0;

        //    return Color.FromArgb(red, green, blue);
        //}

        //private void AdjustGridSizes(int rowCount, int colCount)
        //{
        //    // Настраиваем ширину столбцов
        //    int availableWidth = dataGridView1.ClientSize.Width - dataGridView1.RowHeadersWidth;
        //    int columnWidth = CalculateColumnWidth(availableWidth, colCount);

        //    foreach (DataGridViewColumn column in dataGridView1.Columns)
        //    {
        //        column.Width = columnWidth;
        //    }

        //    // Настраиваем высоту строк
        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        row.Height = ROW_HEIGHT;
        //    }
        //}

        //private int CalculateColumnWidth(int availableWidth, int colCount)
        //{
        //    int calculatedWidth = availableWidth / colCount;

        //    // Если столбцы слишком узкие, включаем горизонтальную прокрутку
        //    if (calculatedWidth < MIN_COLUMN_WIDTH)
        //    {
        //        return MIN_COLUMN_WIDTH;
        //    }

        //    // Если столбцов немного, можно сделать их шире
        //    if (colCount <= 5)
        //    {
        //        return Math.Max(MIN_COLUMN_WIDTH, availableWidth / colCount);
        //    }

        //    return calculatedWidth;
        //}

        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    if (dataGridView1.Columns.Count > 0)
        //    {
        //        AdjustGridSizes(dataGridView1.Rows.Count, dataGridView1.Columns.Count);
        //    }
        //}


        //private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int pnNumber = comboBox3.SelectedIndex;
        //    var Array = grid.GetFullData(pnNumber);
        //    DisplayMatrix(Array);
        //}
    }
    delegate void Draw(IGrid grid);
}
