using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;

namespace NIRS.Numerical_Method
{
    class SEL : NumericalMethod
    {
        private readonly InitialParameters _initialParameters;
        private readonly ConstParameters _constParameters;

        public SEL(InitialParameters initialParameters, ConstParameters constParameters)
        {
            _initialParameters = initialParameters;
            _constParameters = constParameters;
        }
        
        private readonly IOutputDataTransmitter outputDataTransmitter = new OutputDataTransmitter();

        public override Grid Calculate()
        {
            Grid grid = new Grid();
            var gridWithFilledBorders = FillGridBoundaries(grid, _initialParameters, _constParameters);
            var numericalSolution = GetNumericalSolution(gridWithFilledBorders, _initialParameters, _constParameters);
            return outputDataTransmitter.GetOutputData(numericalSolution);
        }
        private Grid FillGridBoundaries(Grid grid,InitialParameters initialParameters, ConstParameters constParameters)
        {
            return GridBorderFiller.Fill
                (grid, 
                initialParameters, constParameters);
        }
        private Grid GetNumericalSolution(Grid grid, InitialParameters initialParameters, ConstParameters constParameters)
        {
            double n = 0;
            while (!IsEndCondition())
            {
                grid = GetNumericalSolutionUpToN(
                    grid, 
                    n, 
                    initialParameters, constParameters
                    );
                n += constParameters.tau;
            }

            return grid;

            bool IsEndCondition()
            {

            }
        }
        private Grid GetNumericalSolutionUpToN(Grid grid, double n, InitialParameters initialParameters, ConstParameters constParameters)
        {
            double k = 0;
            while (!IsEndCondition())
            {
                grid = GetNumericalSolutionUpToK(
                    grid, 
                    n, k, 
                    initialParameters, constParameters);
                k += constParameters.h;
            }

            return grid;

            bool IsEndCondition()
            {

            }
        }

        private Grid GetNumericalSolutionUpToK(Grid grid, double n, double k, InitialParameters initialParameters, ConstParameters constParameters)
        {

        }

    }
}
