using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;
using MyDouble;
using NIRS.Parameter_Type;


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
                n += 0.5;
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
                k += 0.5;
            }

            return grid;

            bool IsEndCondition()
            {

            }
        }

        private IGrid GetNumericalSolutionUpToK(IGrid grid, LimitedDouble n, LimitedDouble k, IInitialParameters initialParameters, IConstParameters constParameters)
        {
            IFunctionsParametersOfTheNextLayer functionsNewLayer = new FunctionsParametersOfTheNextLayer();
            if (ParameterTypeGetter.isDynamic(n, k))
            {
                grid[n][k].D.dynamic_m = functionsNewLayer.Calc_dynamic_m(n, k);
                grid[n][k].D.v = functionsNewLayer.Calc_v(n, k);
                grid[n][k].D.M = functionsNewLayer.Calc_M(n, k);
                grid[n][k].D.w = functionsNewLayer.Calc_w(n, k);
                
            }
            if (ParameterTypeGetter.isMixture(n, k))
            {
                grid[n][k].M.r = functionsNewLayer.Calc_r(n, k);
                grid[n][k].M.e = functionsNewLayer.Calc_e(n, k);
                grid[n][k].M.psi = functionsNewLayer.Calc_psi(n, k);
                grid[n][k].M.z = functionsNewLayer.Calc_z(n, k);
                grid[n][k].M.a = functionsNewLayer.Calc_a(n, k);
                grid[n][k].M.p = functionsNewLayer.Calc_p(n, k);
                grid[n][k].M.m = functionsNewLayer.Calc_m(n, k);
            }
            return grid;
        }

    }
}
