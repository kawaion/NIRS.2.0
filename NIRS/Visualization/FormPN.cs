using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NIRS.Visualization
{
    public partial class FormPN: Form
    {
        private PNDataArrayByN _data;

        private double firstN;
        public FormPN(PNDataArrayByN data)
        {
            InitializeComponent();
            firstN = data.firstN;

            _data = data;
            this.Text = "значение " + data.pn.ToString();

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = data.maxX;
            chart1.ChartAreas[0].AxisX.Interval = 1;

            hScrollBar1.Maximum = data.Array.Count()-1;
            hScrollBar1.LargeChange = 1;
            hScrollBar1.SmallChange = 1;

            Show(hScrollBar1.Value);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            var value = hScrollBar1.Value;

            Show(value);
        }

        private void Show(int value)
        {
            chart1.Series[0].Points.Clear();

            labelN.Text = (firstN + value).ToString();
            labelT.Text = _data.time(firstN + value).ToString();

            var massive = _data.Array[value];
            var size = massive.Count();

            for (int i = 0; i < size; i++)
            {
                chart1.Series[0].Points.AddXY(_data.Xs[i], massive[i]);
            }
        }
    }
}
