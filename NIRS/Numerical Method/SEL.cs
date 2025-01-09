using NIRS.Boundary_Interfaces;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Data_Transmitters;
using NIRS.Grid_Folder;

namespace NIRS.Numerical_Method
{
    class SEL : NumericalMethod
    {
        private readonly InitialParameters _initialParameters;
        private readonly ConstParameters _constParameters;

        public SEL(InitialParameters initialParameters, ConstParameters constParameters)
        {
            _initialParameters = initialParameters;
            _constParameters = constParameters;
        }
        
        private readonly IOutputDataTransmitter outputDataTransmitter = new OutputDataTransmitter();

        public override Grid Calculate()
        {
            Grid grid = new Grid();
            var gridWithFilledBorders = FillGridBoundaries(grid);
            var numericalSolution = GetNumericalSolution(gridWithFilledBorders);
            return outputDataTransmitter.GetOutputData(numericalSolution);
        }
        private Grid FillGridBoundaries(Grid grid)
        {

        }
        private Grid GetNumericalSolution(Grid grid)
        {

        }

    }
}
