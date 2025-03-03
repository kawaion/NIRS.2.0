using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.H_Functions;
using NIRS.Interfaces;
using NIRS.Nabla_Functions;
using NIRS.Projectile_Folder;

namespace NIRS.Functions_for_numerical_method
{
    public class FunctionsBuilder
    {
        private readonly IMainData _mainData;

        public FunctionsBuilder(IMainData mainData)
        {
            _mainData = mainData;
        }
        public IFunctionsParametersOfTheNextLayer FunctionsParametersOfTheNextLayerBuild(IGrid grid)
        {
            IWaypointCalculator waypointCalculator = new WaypointCalculator(grid, _mainData);
            IHFunctions hFunctions = new HFunctions(grid, _mainData);

            IFunctionsParametersOfTheNextLayer functionsNewLayer = new FunctionsParametersOfTheNextLayer(
                grid,
                waypointCalculator,
                hFunctions,
                _mainData);
            return functionsNewLayer;
        }
        public IProjectileFunctions ProjectileFunctionsBuild(IGrid grid)
        {
            IWaypointCalculator waypointCalculator = new WaypointCalculator(grid, _mainData);
            IHFunctions hFunctions = new HFunctions(grid, _mainData);

            IProjectileFunctions projectileFunctions = new ProjectileFunctions(
                grid,
                waypointCalculator,
                hFunctions,
                _mainData);
            return projectileFunctions;
        }
        public IBoundaryFunctions BoundaryFunctionsBuild()
        {
            IBoundaryFunctions boundaryFunctions = new BoundaryFunctions(_mainData);
            return boundaryFunctions;
        }
        public IParameterInterpolationFunctions ParameterInterpolationFunctionsBuild(IGrid grid)
        {
            IParameterInterpolationFunctions parameterInterpolationFunctions = new ParameterInterpolationFunctions(grid, _mainData);
            return parameterInterpolationFunctions;
        }
    }
}
