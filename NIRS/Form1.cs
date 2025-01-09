using System;
using System.Windows.Forms;

using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Numerical_Method;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS
{
    public partial class Form1 : Form
    {
        IInputDataTransmitter inputDataTransmitter = new InputDataTransmitter();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitialParameters initialParameters = new InitialParametersCase1();
            ConstParameters constParameters = new ConstParametersCase1();
            (var newInitialParameters, var newConstParameters) = inputDataTransmitter.GetInputData(initialParameters, constParameters);
            NumericalMethod numericalMethod = new SEL(newInitialParameters, newConstParameters);
            Grid grid = numericalMethod.Calculate();
        }
    }
}
