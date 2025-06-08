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
        int series;

        private double? intervalY = null;
        private double? intervalYLeft = null;
        private double? intervalX = null;
        public ChartPlaceholder(Chart chart)
        {
            _chart = chart;
            series = 0;
        }

        public void SetIntervalY(double value)
        {
            intervalY = value;
        }
        public void SetIntervalYLeft(double value)
        {
            intervalYLeft = value;
        }
        public void SubAdd(DataForChart X, DataForChart Y)
        {
            if (_chart.Series.Count < series + 1)
            {
                _chart.Series.Add(new Series());
                _chart.Series[series].ChartType = SeriesChartType.Line;
            }


            var massiveX = X.Massive;
            var massiveY = Y.Massive;
            CheckSizeMatching(massiveX, massiveY);

            for (int i = 0; i < massiveX.Count; i++)
                _chart.Series[series].Points.AddXY(massiveX[i], massiveY[i]);

            _chart.Series[series].Name = Y.LineName;
            if (series == 0)
            {
                _chart.ChartAreas[0].AxisX.Title = X.Title;
                if (intervalX != null)
                    _chart.ChartAreas[0].AxisX.Interval = intervalX.Value;
            }

            series++;
        }

        public void Add(DataForChart X, DataForChart Y)
        {
            SubAdd(X, Y);
            _chart.ChartAreas[0].AxisY.Title = Y.Title;
            if (intervalY != null)
                _chart.ChartAreas[0].AxisY.Interval = intervalY.Value;
        }
        public void AddLeft(DataForChart X, DataForChart Y)
        {
            SubAdd(X, Y);
            _chart.ChartAreas[0].AxisY2.Title = Y.Title;
            if (intervalYLeft != null)
                _chart.ChartAreas[0].AxisY2.Interval = intervalY.Value;
        }
        public Chart GetChart => _chart;
        private void CheckSizeMatching(List<double> X, List<double> Y)
        {
            if (X.Count != Y.Count)
                throw new Exception();
        }

    }
}
