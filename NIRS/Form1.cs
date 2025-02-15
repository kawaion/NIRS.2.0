using System;
using System.Windows.Forms;

using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Numerical_Method;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;
using NIRS.Cannon_Folder.Barrel_Folder;
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
            IInitialParameters initialParameters = new InitialParametersCase1();
            IConstParameters constParameters = new ConstParametersCase1();
            (var newInitialParameters, var newConstParameters) = inputDataTransmitter.GetInputData(initialParameters, constParameters);
            IBarrel barrel = new Barrel();
            IPowder powder = new Powder_12_7(newConstParameters.D0, 
                                             newConstParameters.d0, 
                                             newConstParameters.L0, 
                                             newConstParameters.u1, 
                                             newConstParameters.e1);
            IProjectile projectile = new Projectile(q, d);
            IMainData mainData = new MainData(barrel, powder, constParameters, initialParameters, projectile);
            INumericalMethod numericalMethod = new SEL(mainData);
            IGrid grid = numericalMethod.Calculate();
        }
    }
}
