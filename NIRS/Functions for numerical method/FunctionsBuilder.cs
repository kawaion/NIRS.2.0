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
        public FunctionsBuilder()
        {

        }
        public IFunctionsParametersOfTheNextLayer FunctionsParametersOfTheNextLayerBuild(IGrid grid, IMainData mainData)
        {
            IBarrelSize barrelSize = new BarrelSize(mainData.Barrel);
            ICombustionFunctions combustionFunctions = new CombustionFunctions(mainData);
            IWaypointCalculator waypointCalculator = new WaypointCalculator(grid, barrelSize, mainData);
            IHFunctions hFunctions = new HFunctions(grid, barrelSize, combustionFunctions, mainData);

            IFunctionsParametersOfTheNextLayer functionsNewLayer = new FunctionsParametersOfTheNextLayer(
                grid,
                waypointCalculator,
                hFunctions,
                mainData);
            return functionsNewLayer;
        }
        public IProjectileFunctions ProjectileFunctionsBuild(IGrid grid, IMainData mainData)
        {
            IProjectileFunctions projectileFunctions = new ProjectileFunctions(grid, mainData);
            return projectileFunctions;
        }
    }
}
