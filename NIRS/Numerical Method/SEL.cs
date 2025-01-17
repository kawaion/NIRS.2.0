using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;
//using NIRS.MyDouble_Folder;
using MyDouble;


namespace NIRS.Numerical_Method
{
    class SEL : INumericalMethod
    {
        private readonly IInitialParameters _initialParameters;
        private readonly IConstParameters _constParameters;

        public SEL(IInitialParameters initialParameters, IConstParameters constParameters)
        {
            _initialParameters = initialParameters;
            _constParameters = constParameters;
        }
        
        private readonly IOutputDataTransmitter outputDataTransmitter = new OutputDataTransmitter();

        public IGrid Calculate()
        {
            IGrid grid = new TimeSpaceGrid(_constParameters.tau, _constParameters.h);

            var gridWithFilledBorders = FillGridBoundaries(grid, _initialParameters, _constParameters);
            var numericalSolution = GetNumericalSolution(gridWithFilledBorders, _initialParameters, _constParameters);
            return outputDataTransmitter.GetOutputData(numericalSolution);
        }
        private IGrid FillGridBoundaries(IGrid grid,IInitialParameters initialParameters, IConstParameters constParameters)
        {
            IGridBorderFiller gridBorderFiller = new GridBorderFiller();

            return gridBorderFiller.Fill
                (grid, 
                initialParameters, constParameters);
        }
        private IGrid GetNumericalSolution(IGrid grid, IInitialParameters initialParameters, IConstParameters constParameters)
        {
            LimitedDouble n = new LimitedDouble(0);
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
        private IGrid GetNumericalSolutionUpToN(IGrid grid, LimitedDouble n, IInitialParameters initialParameters, IConstParameters constParameters)
        {
            LimitedDouble k = new LimitedDouble(0);
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

        private IGrid GetNumericalSolutionUpToK(IGrid grid, LimitedDouble n, LimitedDouble k, IInitialParameters initialParameters, IConstParameters constParameters)
        {

        }

    }
}
