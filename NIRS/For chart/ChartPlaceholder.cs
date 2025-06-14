using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace NIRS.For_chart
{
    class ChartPlaceholder
    {
        Chart _chart;

        private double? intervalY = null;
        private double? intervalYLeft = null;
        private double? intervalX = null;
        public ChartPlaceholder(Chart chart)
        {
            _chart = chart;
            _chart.Series.Clear();
            _chart.ChartAreas[0].AxisX.Minimum = 0;
            _chart.ChartAreas[0].AxisY.Minimum = 0;
            _chart.ChartAreas[0].AxisX.Maximum = double.NaN;
            _chart.ChartAreas[0].AxisY.Maximum = double.NaN;
            _chart.ChartAreas[0].AxisY2.Maximum = double.NaN;
            _chart.ChartAreas[0].AxisX.Interval = double.NaN;
            _chart.ChartAreas[0].AxisY2.Minimum = 0;
        }        
        public void SetIntervalCount(int value)
        {
            _chart.ChartAreas[0].AxisY.Interval = (_chart.ChartAreas[0].AxisY.Maximum - _chart.ChartAreas[0].AxisY.Minimum) / value;
            _chart.ChartAreas[0].AxisY2.Interval = (_chart.ChartAreas[0].AxisY2.Maximum - _chart.ChartAreas[0].AxisY2.Minimum) / value;
        }
        public void SetIntervalX(double value)
        {
            _chart.ChartAreas[0].AxisX.Interval = value;
        }

        public void SetIntervalY(double value)
        {
            _chart.ChartAreas[0].AxisY.Interval = value;
        }
        public void SetIntervalYLeft(double value)
        {
            _chart.ChartAreas[0].AxisY2.Interval = value;
        }
        public void SetMaxX(double value)
        {
            _chart.ChartAreas[0].AxisX.Maximum = value;
        }
        public void SetMaxY(double value)
        {
            _chart.ChartAreas[0].AxisY.Maximum = value;
        }
        public void SetMaxYLeft(double value)
        {
            _chart.ChartAreas[0].AxisY2.Maximum = value;
        }
        public void SubAdd(DataForChart X, DataForChart Y)
        {
            AddNewSeries();

            var massiveX = X.Massive;
            var massiveY = Y.Massive;
            CheckSizeMatching(massiveX, massiveY);

            for (int i = 0; i < massiveX.Count; i++)
                _chart.Series.Last().Points.AddXY(massiveX[i], massiveY[i]);

            _chart.Series.Last().Name = Y.LineName;
            if (_chart.Series.Count == 1)
            {
                _chart.ChartAreas[0].AxisX.Title = X.Title;
            }
        }

        private void AddNewSeries()
        {
            _chart.Series.Add(new Series());
            _chart.Series.Last().ChartType = SeriesChartType.Line;
        }

        public void Add(DataForChart X, DataForChart Y)
        {
            SubAdd(X, Y);
            _chart.ChartAreas[0].AxisY.Title = Y.Title;
        }
        public void AddLeft(DataForChart X, DataForChart Y)
        {
            SubAdd(X, Y);
            _chart.ChartAreas[0].AxisY2.Title = Y.Title;
            _chart.Series.Last().YAxisType = AxisType.Secondary;
        }
        public Chart GetChart => _chart;
        private void CheckSizeMatching(List<double> X, List<double> Y)
        {
            if (X.Count != Y.Count)
                throw new Exception();
        }

    }
}
