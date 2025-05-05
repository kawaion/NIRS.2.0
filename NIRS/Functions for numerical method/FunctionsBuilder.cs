using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.H_Functions;
using NIRS.Interfaces;
using NIRS.Nabla_Functions;
using NIRS.Projectile_Folder;
using System.Windows.Forms.DataVisualization.Charting;

namespace NIRS.Functions_for_numerical_method
{
    public class FunctionsBuilder
    {
        private readonly IMainData _mainData;

        public FunctionsBuilder(IMainData mainData)
        {
            _mainData = mainData;
        }

        IWaypointCalculator waypointCalculator;
        IHFunctions hFunctions;

        IFunctionsParametersOfTheNextLayer functionsNewLayer;
        IProjectileFunctions projectileFunctions;
        IBoundaryFunctions boundaryFunctions;
        IParameterInterpolationFunctions parameterInterpolationFunctions;

        public void Build(IGrid grid)
        {
            FunctionsParametersOfTheNextLayerBuild(grid);
            ProjectileFunctionsBuild(grid);
            BoundaryFunctionsBuild();
            ParameterInterpolationFunctionsBuild(grid);
        }

        public IFunctionsParametersOfTheNextLayer FunctionsParametersOfTheNextLayerBuild(IGrid grid)
        {
            waypointCalculator = new WaypointCalculator(grid, _mainData);
            hFunctions = new HFunctions(grid, _mainData);

            functionsNewLayer = new FunctionsParametersOfTheNextLayer(
                grid,
                waypointCalculator,
                hFunctions,
                _mainData);
            return functionsNewLayer;
        }
        public IProjectileFunctions ProjectileFunctionsBuild(IGrid grid)
        {
            waypointCalculator = new WaypointCalculator(grid, _mainData);
            hFunctions = new HFunctions(grid, _mainData);

            projectileFunctions = new ProjectileFunctions(
                grid,
                waypointCalculator,
                hFunctions,
                _mainData);
            return projectileFunctions;
        }
        public IBoundaryFunctions BoundaryFunctionsBuild()
        {
            boundaryFunctions = new BoundaryFunctions(_mainData);
            return boundaryFunctions;
        }
        public IParameterInterpolationFunctions ParameterInterpolationFunctionsBuild(IGrid grid)
        {
            parameterInterpolationFunctions = new ParameterInterpolationFunctions(grid, _mainData);
            return parameterInterpolationFunctions;
        }


        public IFunctionsParametersOfTheNextLayer FunctionsParametersOfTheNextLayerUpdate(IGrid grid)
        {
            functionsNewLayer.Update(grid);
            return functionsNewLayer;
        }
        public IProjectileFunctions ProjectileFunctionsUpdate(IGrid grid)
        {
            projectileFunctions.Update(grid);
            return projectileFunctions;
        }
        public IBoundaryFunctions BoundaryFunctionsUpdate()
        {
            boundaryFunctions.Update();
            return boundaryFunctions;
        }
        public IParameterInterpolationFunctions ParameterInterpolationFunctionsUpdate(IGrid grid)
        {
            parameterInterpolationFunctions.Update(grid);
            return parameterInterpolationFunctions;
        }
    }
}
