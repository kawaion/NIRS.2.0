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

namespace NIRS.Numerical_Method
{
    class SEL : INumericalMethod
    {       
        private readonly IMainData _mainData;
        public SEL(IMainData mainData)
        {
            _mainData = mainData;
        }
        
        private readonly IOutputDataTransmitter outputDataTransmitter = new OutputDataTransmitter();


        public IGrid Calculate()
        {
            IGrid grid = new TimeSpaceGrid();

            var gridWithFilledBorders = FillGridBoundaries(grid);
            var numericalSolution = GetNumericalSolution(gridWithFilledBorders);
            return outputDataTransmitter.GetOutputData(numericalSolution);
        }

        private IGrid FillGridBoundaries(IGrid grid)
        {
            IGridBorderFiller gridBorderFiller = new GridBorderFiller();

            var gridWithFilledBorders = gridBorderFiller.FillAtZeroTime(grid, _mainData);
            return gridWithFilledBorders;
        }


        private IGrid GetNumericalSolution(IGrid grid)
        {
            LimitedDouble n = new LimitedDouble(0);

            while (!IsEndCondition())
            {
                grid = GetNumericalSolutionAtNodeN(grid, n);
                grid = GetNumericalSolutionInProjectile(grid, n);
                grid = GetInterpolateSolutionAtInaccessibleNodes(grid, n);
                n += 0.5;
            }
            return grid;

            bool IsEndCondition()
            {

            }
        }
        private IGrid GetNumericalSolutionAtNodeN(IGrid grid, LimitedDouble n)
        {
            LimitedDouble k = new LimitedDouble(0);

            while (!IsEndCondition())
            {
                if(ParameterTypeGetter.IsDynamic(n, k) || ParameterTypeGetter.IsMixture(n, k))
                {
                    grid = GetNumericalSolutionAtNodeNK(grid, n, k );
                }


                k += 0.5;
            }
            return grid;

            bool IsEndCondition()
            {

            }
        }

        private IGrid GetNumericalSolutionAtNodeNK(IGrid grid, LimitedDouble n, LimitedDouble k)
        {
            FunctionsBuilder functionsBuilder = new FunctionsBuilder();
            var functionsNewLayer = functionsBuilder.FunctionsParametersOfTheNextLayerBuild(grid, _mainData);
            INumericalSolutionInNodes numericalSolutionInNodes = new NumericalSolutionInNodes(functionsNewLayer);

            grid = numericalSolutionInNodes.Get(grid, n, k);

            return grid;
        }
        private IGrid GetNumericalSolutionInProjectile(IGrid grid, LimitedDouble n)
        {
            FunctionsBuilder functionsBuilder = new FunctionsBuilder();
            var projectileFunctions = functionsBuilder.ProjectileFunctionsBuild(grid, _mainData);
            INumericalSolutionProjectile numericalSolutionProjectile = new NumericalSolutionProjectile(projectileFunctions);

            grid = numericalSolutionProjectile.Get(grid, n);

            return grid;
        }

        private IGrid GetInterpolateSolutionAtInaccessibleNodes(IGrid grid, LimitedDouble n)
        {
            FunctionsBuilder functionsBuilder = new FunctionsBuilder();

            grid = 

            return grid;
        }
    }
}
