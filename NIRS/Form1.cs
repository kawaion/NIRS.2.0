using System;
using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Numerical_Method;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NIRS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitialParameters initialParameters = new InitialParametersCase1();
            ConstParameters constParameters = new ConstParametersCase1();
            NumericalMethod numericalMethod = new SEL();
        }
    }
}
