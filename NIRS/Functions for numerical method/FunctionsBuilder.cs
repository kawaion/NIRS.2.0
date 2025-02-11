using NIRS.Cannon_Folder.Barrel_Folder;
using NIRS.Cannon_Folder.Powder_Folder;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Grid_Folder;
using NIRS.H_Functions;
using NIRS.Nabla_Functions;
using NIRS.Projectile_Folder;

namespace NIRS.Functions_for_numerical_method
{
    public class FunctionsBuilder
    {
        public FunctionsBuilder()
        {

        }
        public IFunctionsParametersOfTheNextLayer FunctionsParametersOfTheNextLayerBuild(IGrid grid,IBarrel barrel, IConstParameters constParameters, IPowder powder)
        {
            IBarrelSize barrelSize = new BarrelSize(barrel, constParameters);
            ICombustionFunctions combustionFunctions = new CombustionFunctions(powder, constParameters);
            IWaypointCalculator waypointCalculator = new WaypointCalculator(grid, constParameters, barrelSize);
            IHFunctions hFunctions = new HFunctions(grid, barrelSize, powder, combustionFunctions, constParameters);

            IFunctionsParametersOfTheNextLayer functionsNewLayer = new FunctionsParametersOfTheNextLayer(
                grid,
                waypointCalculator,
                hFunctions,
                constParameters,
                barrelSize,
                powder);
            return functionsNewLayer;
        }
        public IProjectileFunctions ProjectileFunctionsBuild(IGrid grid, IProjectile projectile, IConstParameters constParameters)
        {
            IProjectileFunctions projectileFunctions = new ProjectileFunctions(grid, projectile, constParameters);
            return projectileFunctions;
        }
    }
}
