using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;
using MyDouble;
using NIRS.Parameter_Type;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.Functions_for_numerical_method;
using NIRS.Projectile_Folder;
using NIRS.Numerical_solution;
using NIRS.Interfaces;
using NIRS.Verifer;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Net.Http.Headers;
using System.Diagnostics;
using NIRS.Parameter_names;
using NIRS.Helpers;
using System.Runtime.InteropServices.ComTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIRS.Visualization.Progress;
using NIRS.Numerical_Method.Method_SEL.Solution_Calculator_After_The_Belt_Breaks;
using NIRS.Numerical_Method.Method_SEL.Solution_Calculator_Before_The_Belt_Breaks;
using NIRS.Numerical_Method.Method_SEL.Solution.Helper;
using NIRS.Numerical_Method.Method_SEL.Solution;

namespace NIRS.Numerical_Method
{
    class SEL : INumericalMethod
    {       
        private readonly IMainData _mainData;

        Progresser _progresser = null;
        public void ProgressActivate(Progresser progresser)
        {
            _progresser = progresser;
        }

        public SEL(IMainData mainData)
        {
            //3.1718749999999996E-06
            //0.012687499999999999
            _mainData = mainData;
        }
        
        private readonly IOutputDataTransmitter outputDataTransmitter = new OutputDataTransmitter();
        FunctionsBuilder functionsBuilder;

        public IGrid Calculate()
        {
            IGrid grid = new TimeSpaceGrid();

            functionsBuilder = new FunctionsBuilder(_mainData);
            functionsBuilder.Build(grid);

            var functionsNewLayer = functionsBuilder.FunctionsParametersOfTheNextLayerBuild(grid);
            var projectileFunctions = functionsBuilder.ProjectileFunctionsBuild(grid);
            var parameterInterpolationFunctions = functionsBuilder.ParameterInterpolationFunctionsBuild(grid);
            var boundaryFunctions = functionsBuilder.BoundaryFunctionsBuild();

            Visualizator visualizator = new Visualizator(_progresser);

            NumericalSolution numericalSolution = new NumericalSolution(_mainData, boundaryFunctions, functionsNewLayer, projectileFunctions, parameterInterpolationFunctions, visualizator);
            grid = numericalSolution.Calculate(grid);
            return grid;
        }
    }
}
