using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.For_chart
{
    class DataForChart
    {
        public List<double> Massive { get; set; }
        public string Title { get; set; }
        public string LineName { get; set; }
        private double? interval = null;
        public DataForChart(List<double> massive, string title, string lineName = null)
        {
            Massive = massive;
            Title = title;
            LineName = lineName;
        }
        public void SetInterval(double value)
        {
            interval = value;
        }
        public double GetInterval()
        {
            return interval.Value;
        }
    }
}
