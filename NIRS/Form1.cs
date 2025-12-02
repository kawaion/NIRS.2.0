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

namespace NIRS
{
    public partial class Form1 : Form
    {
        IInputDataTransmitter inputDataTransmitter = new InputDataTransmitter();
        public Form1()
        {
            InitializeComponent();

            //ConfigureDataGridView();


            chart1.ChartAreas[0].AxisX.Minimum = 0;
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

            //int maxN = 1100;

            //var dataSheets = new Dictionary<string, double[,]>
            //{
            //    //{"dynamic_m", grid.GetFullData(PN.dynamic_m, maxN)},
            //    //{"v", grid.GetFullData(PN.v, maxN)},
            //    //{"M", grid.GetFullData(PN.M)},
            //    //{"w", grid.GetFullData(PN.w)},
            //    //{"a", grid.GetFullData(PN.a)},
            //    {"e", grid.GetFullData(PN.e, maxN)},
            //    //{"m_", grid.GetFullData(PN.m, maxN)},
            //    //{"p", grid.GetFullData(PN.p, maxN)},
            //    //{"r", grid.GetFullData(PN.r)},
            //    //{"rho", grid.GetFullData(PN.rho)},
            //    //{"z", grid.GetFullData(PN.z)},
            //    //{"psi", grid.GetFullData(PN.psi)}
            //};
            //string programFolder = Application.StartupPath;
            //string parentFolder = Directory.GetParent(programFolder).FullName;
            //string fileName = "multi_sheet_data.xlsx";

            //try
            //{
            //    ExcelHelper.CreateExcelFileWithSheets(dataSheets, fileName);

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
            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = grid.LastIndexN(PN.m).GetInt();
            var tmp = grid.GetSn(PN.vSn, grid.LastIndexN(PN.v));
            var maxN = grid.LastIndexN(PN.p);
            nForMaxP = FindNPMax();
        }

        private IMainData InitializeMainData()
        {
            IInitialParameters initialParameters = new InitialParametersCase1();
            double h = 1.015 / 80; //0.0025;
            double tau = 1.015 / (80 * (2500 + 1500)); //curantTau(h, 945);

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

            IConstParameters constParameters = new ConstParametersCase1(tau, 80, endChamber);
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
        private void ShowLayer(LimitedDouble n, List<PN> pns)
        {
            for (int j = 0; j < pns.Count; j++)
                chart1.Series[j].Points.Clear();
            var last = grid.LastIndexK(pns[0], n);
            for (LimitedDouble i = new LimitedDouble(0); i <= last; i++)
                for (int j = 0; j < pns.Count; j++)
                    chart1.Series[j].Points.AddXY(i, grid[pns[j], n, i]);
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
            var pns = new List<PN>() { pn };
            var n = new LimitedDouble(Convert.ToDouble(textBox1.Text));
            if (n > grid.LastIndexN(pns[0]) - 1)
            {
                n = grid.LastIndexN(pns[0]) - 1;
                textBox1.Text = n.ToString();
            }
            ShowLayer(n, pns);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            var n = hScrollBar1.Value;
            textBox1.Text = Convert.ToString(n);
            var pn = DictPN.Get(comboBoxPN.Text);
            Visualise(pn);
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
            chartPlaceholder.SetIntervalX(1);
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
            chartPlaceholder.SetIntervalX(0.5);
            chartPlaceholder.SetMaxY(400);
            chartPlaceholder.SetMaxYLeft(4000);
            chartPlaceholder.SetIntervalCount(4);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw4()
        {
            LimitedDouble n = nForMaxP;
            ResultExtractor resultExtractor = new ResultExtractor(grid);
            var dataXForP = resultExtractor.GetX(PN.p, n, mainData);
            var dataXForV = resultExtractor.GetX(PN.v, n-0.5, mainData);
            var dataP = resultExtractor.GetP(n);
            var dataVtw = resultExtractor.GetV(n - 0.5);
            var dataWgas = resultExtractor.GetW(n - 0.5);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataXForP, dataP);
            chartPlaceholder.AddLeft(dataXForV, dataVtw);
            chartPlaceholder.AddLeft(dataXForV, dataWgas);
            chartPlaceholder.SetIntervalX(0.25);
            //chartPlaceholder.SetMaxY(500);
            //chartPlaceholder.SetMaxYLeft(500);
            //chartPlaceholder.SetIntervalCount(5);

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
            var dataWgas = resultExtractor.GetW(n - 0.5);
            ChartPlaceholder chartPlaceholder = new ChartPlaceholder(chartForDraw);
            chartPlaceholder.Add(dataXForP, dataP);
            chartPlaceholder.AddLeft(dataXForV, dataVtw);
            chartPlaceholder.AddLeft(dataXForV, dataWgas);
            chartPlaceholder.SetIntervalX(0.25);
            //chartPlaceholder.SetMaxY(500);
            //chartPlaceholder.SetMaxYLeft(1000);
            //chartPlaceholder.SetIntervalCount(5);

            chartForDraw = chartPlaceholder.GetChart;
        }
        private void Draw6()
        {
            LimitedDouble n = grid.LastIndexN(PN.a); //nForMaxP;
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
            chartPlaceholder.SetIntervalX(0.25);
            chartPlaceholder.SetMaxY(500);
            chartPlaceholder.SetIntervalCount(5);

            chartForDraw = chartPlaceholder.GetChart;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<double> result = new List<double>();
            IInitialParameters initialParameters = new InitialParametersCase1();
            double hFinal = 0.0126875; //0.0025;
            double tauFinal = 3.171875e-6; //curantTau(h, 945);
            for (int i = 3; i >= 0; i--)
            {
                double h = hFinal * Math.Pow(2, i);
                double tau = tauFinal * Math.Pow(2, i);
//inputDataTransmitter.GetInputData(initialParameters, constParameters);
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

                IConstParameters constParameters = new ConstParametersCase1(tau, 80, endChamber);
                (var newInitialParameters, var newConstParameters) = (initialParameters, constParameters);

                double omega = 19;
                double d = 0.1524;

                IBarrel barrel = new Barrel(points, endChamber, Dimension.D);
                IPowder powder = new Powder_12_7(newConstParameters, barrel.BarrelSize, omega);
                IProjectile projectile = new Projectile(newConstParameters.q, d);
                mainData = new MainData(barrel, powder, newConstParameters, newInitialParameters, projectile);
                //INumericalMethod numericalMethod = new SEL(mainData,DrawGrid);
                //Task<IGrid> task = new Task<IGrid>(()=>numericalMethod.Calculate());
                //task.Start();
                //IGrid grid = task.Result;//numericalMethod.Calculate();

                INumericalMethod numericalMethod = new SEL(mainData);
                grid = numericalMethod.Calculate();
                var lastN = grid.LastIndexNSn(PN.vSn);
                result.Add(grid.GetSn(PN.vSn, lastN));
            }

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
