using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;
using MyDouble;
using NIRS.Parameter_Type;
using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;

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

            var gridWithFilledBorders = gridBorderFiller.Fill(grid, initialParameters, constParameters);
            return gridWithFilledBorders;
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
                {
                    grid = GetNumericalSolutionUpToK(grid, n, k );
                    grid = GetNumericalSolutionInProjectile(grid, n);
                }


                k += 0.5;
            }
            return grid;

            bool IsEndCondition()
            {

            }
        }

        private IGrid GetNumericalSolutionUpToK(IGrid grid, LimitedDouble n, LimitedDouble k)
        {
            IFunctionsParametersOfTheNextLayer functionsNewLayer = FunctionsNewLayerBuilder.Build(grid, _barrel, _constParameters, _powder);
            INumericalSolutionInNodes numericalSolutionInNodes = new NumericalSolutionInNodes(functionsNewLayer);

            grid = numericalSolutionInNodes.Get(grid, n, k);

            return grid;
        }
        private IGrid GetNumericalSolutionInProjectile(IGrid grid, LimitedDouble n, LimitedDouble k)
        {
            IFunctionsParametersOfTheNextLayer functionsNewLayer = FunctionsNewLayerBuilder.Build(grid, _barrel, _constParameters, _powder);
            INumericalSolutionInNodes numericalSolutionInNodes = new NumericalSolutionInNodes(functionsNewLayer);

            grid = numericalSolutionInNodes.Get(grid, n, k);

            return grid;
        }
    }
}
