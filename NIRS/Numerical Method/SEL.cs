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

namespace NIRS.Numerical_Method
{
    class SEL : INumericalMethod
    {
        private readonly IBarrel _barrel;
        private readonly IPowder _powder;
        private readonly IInitialParameters _initialParameters;
        private readonly IConstParameters _constParameters;

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
                grid = GetNumericalSolutionAtNodeN(grid, n);
                grid = GetNumericalSolutionInProjectile(grid, n);
                grid = GetNumericalSolutionAtInaccessibleNodes(grid, n);
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
            var functionsNewLayer = functionsBuilder.FunctionsParametersOfTheNextLayerBuild(grid, _barrel, _constParameters, _powder);
            INumericalSolutionInNodes numericalSolutionInNodes = new NumericalSolutionInNodes(functionsNewLayer);

            grid = numericalSolutionInNodes.Get(grid, n, k);

            return grid;
        }
        private IGrid GetNumericalSolutionInProjectile(IGrid grid, LimitedDouble n)
        {
            FunctionsBuilder functionsBuilder = new FunctionsBuilder();
            IProjectile projectile = new Projectile(q);
            var projectileFunctions = functionsBuilder.ProjectileFunctionsBuild(grid, projectile, _constParameters);
            INumericalSolutionProjectile numericalSolutionProjectile = new NumericalSolutionProjectile(projectileFunctions);

            grid = numericalSolutionProjectile.Get(grid, n);

            return grid;
        }
    }
}
