using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NIRS.Cannon_Folder.Barrel_Folder
{
    public partial class BarrelInitializationInterface : Form
    {
        public BarrelInitializationInterface()
        {
            InitializeComponent();     
        }

        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            var res = chart1.HitTest(e.X, e.Y);
            int index;
            if(res.PointIndex%2==0)
                index=res.PointIndex/2;
            else
                index = (res.PointIndex-1)/2;
            if (res.PointIndex != -1)
            {
                if (flagDeletePoint)
                {
                    chart1.Series[0].Points.Remove(chart1.Series[0].Points[index]);
                    chart1.Series[1].Points.Remove(chart1.Series[1].Points[index]);
                    chart1.Series[2].Points.Remove(chart1.Series[2].Points[index*2]);
                    chart1.Series[2].Points.Remove(chart1.Series[2].Points[index*2]);
                }
                if (flagSetEndChamber)
                {
                    chart1.Series[3].Points.Clear();
                    chart1.Series[3].Points.AddXY(chart1.Series[0].Points[index].XValue, chart1.Series[0].Points[index].YValues[0]);
                    chart1.Series[3].Points.AddXY(chart1.Series[1].Points[index].XValue, chart1.Series[1].Points[index].YValues[0]);
                }

            }
        }
        bool flagDeletePoint=false;
        bool flagSetEndChamber=false;
        private void DeleteCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) 
            {
                InstallInteractiveWithGraph();
                flagDeletePoint = true;
            }
            else
            {
                DisableInteractiveWithGraph();
                flagDeletePoint = false;
            }
        }
        private void SetEndChamberCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                InstallInteractiveWithGraph();
                flagSetEndChamber = true;
            }
            else
            {
                DisableInteractiveWithGraph();
                flagSetEndChamber = false;
            }

        }
        private void InstallInteractiveWithGraph()
        {
            chart1.Enabled = true;
            EnabledButtons(false);
            DrawDots();
        }

        private void DisableInteractiveWithGraph()
        {
            chart1.Enabled = false;
            EnabledButtons(true);
            DeleteDots();
        }

        private void EnabledButtons(bool enable)
        {
            button1.Enabled = enable;
            button2.Enabled = enable;
        }        
        private void DrawDots()
        {
            var points = chart1.Series[0].Points;
            foreach(var point in points)
            {
                chart1.Series[2].Points.AddXY(point.XValue, point.YValues[0]);
                chart1.Series[2].Points.AddXY(point.XValue, -point.YValues[0]);
            }
                
        }
        private void DeleteDots()
        {
            chart1.Series[2].Points.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox1.Text, out double x))
                if (double.TryParse(textBox2.Text, out double y))
                {
                    chart1.Series[0].Points.AddXY(x, y);
                    chart1.Series[1].Points.AddXY(x, -y);
                    chart1.Series[0].Points.OrderBy(t => t.XValue);
                    chart1.Series[1].Points.OrderBy(t => t.XValue);
                }
                else MessageBox.Show("неверно заданы точки");
            else MessageBox.Show("неверно заданы точки");
        }
    }
}
