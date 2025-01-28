using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;
using MyDouble;
using NIRS.Parameter_Type;
using NIRS.Parameter_names;
using NIRS.Nabla_Functions;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.H_Functions;

namespace NIRS.Numerical_Method
{
    class SEL : INumericalMethod
    {
        private readonly IBarrel _barrel;
        private readonly IPowder _powder;
        private readonly IInitialParameters _initialParameters;
        private readonly IConstParameters _constParameters;
        private readonly IBarrelSize _barrelSize;
        private readonly ICombustionFunctions _combustionFunctions;

        public SEL(IBarrel barrel, IPowder powder, IInitialParameters initialParameters, IConstParameters constParameters)
        {
            _barrel = barrel;
            _powder = powder;
            _initialParameters = initialParameters;
            _constParameters = constParameters;

            _combustionFunctions = new CombustionFunctions(powder, constParameters);
            _barrelSize = new BarrelSize(barrel,constParameters);
        }
        
        private readonly IOutputDataTransmitter outputDataTransmitter = new OutputDataTransmitter();

        public IGrid Calculate()
        {
            IGrid grid = new TimeSpaceGrid(_constParameters.tau, _constParameters.h);

            var gridWithFilledBorders = FillGridBoundaries(grid, _initialParameters, _constParameters);
            var numericalSolution = GetNumericalSolution(gridWithFilledBorders);
            return outputDataTransmitter.GetOutputData(numericalSolution);
        }
        private IGrid FillGridBoundaries(IGrid grid,IInitialParameters initialParameters, IConstParameters constParameters)
        {
            IGridBorderFiller gridBorderFiller = new GridBorderFiller();

            return gridBorderFiller.Fill
                (grid, 
                initialParameters, constParameters);
        }
        private IGrid GetNumericalSolution(IGrid grid)
        {
            LimitedDouble n = new LimitedDouble(0);

            while (!IsEndCondition())
            {
                grid = GetNumericalSolutionUpToN(grid, n );
                n += 0.5;
            }
            return grid;

            bool IsEndCondition()
            {

            }
        }
        private IGrid GetNumericalSolutionUpToN(IGrid grid, LimitedDouble n)
        {
            LimitedDouble k = new LimitedDouble(0);

            while (!IsEndCondition())
            {
                if(ParameterTypeGetter.IsDynamic(n, k) || ParameterTypeGetter.IsMixture(n, k))
                    grid = GetNumericalSolutionUpToK(grid, n, k );
                k += 0.5;
            }
            return grid;

            bool IsEndCondition()
            {

            }
        }

        private IGrid GetNumericalSolutionUpToK(IGrid grid, LimitedDouble n, LimitedDouble k)
        {
            var functionsNewLayer = GetFunctionsNewLayer(grid);

            if (ParameterTypeGetter.IsDynamic(n, k))
                grid = GetDynamicParametersOfNextLayer(grid, n, k, functionsNewLayer);

            else if (ParameterTypeGetter.IsMixture(n, k))
                grid = GetMixtureParametersOfNextLayer(grid, n, k, functionsNewLayer);

            return grid;
        }



        private IFunctionsParametersOfTheNextLayer GetFunctionsNewLayer(IGrid grid)
        {
            (var waypointCalculator, var hFunctions) = GetHAndDiffFunctions(grid);

            IFunctionsParametersOfTheNextLayer functionsNewLayer = new FunctionsParametersOfTheNextLayer(
                grid,
                waypointCalculator,
                hFunctions,
                _constParameters,
                _barrelSize,
                _powder);
            return functionsNewLayer;
        }

        private (IWaypointCalculator waypointCalculator, IHFunctions hFunctions) GetHAndDiffFunctions(IGrid grid)
        {
            IWaypointCalculator waypointCalculator = new WaypointCalculator(grid, _constParameters, _barrelSize);
            IHFunctions hFunctions = new HFunctions(grid, _barrelSize, _powder, _combustionFunctions, _constParameters);
            return (waypointCalculator, hFunctions);
        }


        private IGrid GetDynamicParametersOfNextLayer(IGrid grid, LimitedDouble n, LimitedDouble k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {
            grid[n][k].D.dynamic_m = functionsNewLayer.Get(PN.dynamic_m, n, k);
            grid[n][k].D.v = functionsNewLayer.Get(PN.v, n, k);
            grid[n][k].D.M = functionsNewLayer.Get(PN.M, n, k);
            grid[n][k].D.w = functionsNewLayer.Get(PN.w, n, k);

            return grid;
        }        
        private IGrid GetMixtureParametersOfNextLayer(IGrid grid, LimitedDouble n, LimitedDouble k, IFunctionsParametersOfTheNextLayer functionsNewLayer)
        {
            grid[n][k].M.r = functionsNewLayer.Get(PN.r, n, k);
            grid[n][k].M.e = functionsNewLayer.Get(PN.e, n, k);
            grid[n][k].M.psi = functionsNewLayer.Get(PN.psi, n, k);
            grid[n][k].M.z = functionsNewLayer.Get(PN.z, n, k);
            grid[n][k].M.a = functionsNewLayer.Get(PN.a, n, k);
            grid[n][k].M.p = functionsNewLayer.Get(PN.p, n, k);
            grid[n][k].M.m = functionsNewLayer.Get(PN.m, n, k);

            return grid;
        }
    }
}
