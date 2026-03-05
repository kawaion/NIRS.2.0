using Core.Domain.Enums;
using Core.Domain.Grid.Aggregates;
using Core.Domain.Grid.Interfaces;
using Core.Domain.Limited_Double;
using Core.Domain.Numerical_methods.SEL.Formulas.Interfaces;
using Core.Domain.Numerical_methods.SEL.Functions.Interfaces;
using Core.Domain.Numerical_methods.SEL.Interfaces;
using Core.Domain.Physical.ValueObjects.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Core.Domain.Limited_Double.LimitedDouble;

namespace Core.Domain.Numerical_methods.SEL.Aggregate;

internal class SELSolverFSM
{
    public enum TypeParameterState { Mixture, Dynamic }
    public enum BeltState { Intact, Broke }
    public enum TimeState { Zero, Half, OnePlus}
    public enum SolveState { Yes, No }    
    
    [Flags]
    public enum LayerFlag : byte
    {
        None = 0,                    // 0000 0000
        Calculated = 1 << 0,         // 0000 0001
        Projectile = 1 << 1,         // 0000 0010
        Interpolated = 1 << 2,       // 0000 0100
        Boundary = 1 << 3,           // 0000 1000
    }

    private TypeParameterState _typeParameterState;
    private BeltState _beltState;
    private TimeState _timeState;
    private SolveState _solveState;
    private LayerFlag _layerflag;

    private LimitedDouble n;
    private LimitedDouble kSnInStart;
    private IGrid grid;

    private IBoundaryCalculator _boundaryCalculator;
    private IZeroTimeCalculator _zeroTimeCalculator;
    private ICalculatorValuesInNodesNequal0_5 _calculatorValuesInNodesNEqual0_5;
    private ICalculatorValuesInNodes _calculatorValuesInNodes;

    public SELSolverFSM(INumericalMethodSettings settings)
    {
        _typeParameterState = TypeParameterState.Dynamic;
        _beltState = BeltState.Intact;
        _timeState = TimeState.Zero;
        _solveState = SolveState.No;
        _layerflag = LayerFlag.None;

        n = new LimitedDouble(0);
        kSnInStart = new LimitedDouble(settings.KSnInStart);
        grid = new TimeSpaceGrid();
    }

    private void RemoveAllFlags()
    {
        _layerflag = LayerFlag.None;
    }
    private void AddNodeFlag(LayerFlag flag)
    {
        _layerflag |= flag;
    }
    private bool HasNodeFlag(LayerFlag flag)
    {
        return (_layerflag & flag) == flag;
    }
    private bool HasAnyFlag(LayerFlag flags)
    {
        return (_layerflag & flags) != 0;
    }



    public void CalculateBoundary()
    {
        if (IsStateNEqualZero())
        {
            AddNodeFlag(LayerFlag.Boundary);
        }
        if (IsStateNEqual05())
        {
            grid = _boundaryCalculator.CalculateInLeft(grid, n, ld0);
            grid = _boundaryCalculator.CalculateInRight(grid, n, kSnInStart);
        }
    }
    public void CalculateNode()
    {
        if (IsStateNEqualZero())
        {
            grid = _zeroTimeCalculator.CalculateMixture(grid, ld0_5, kSnInStart);
        }
        if (IsStateNEqual05())
        {
            grid = _calculatorValuesInNodesNEqual0_5.CalculateDynamic(grid, n, ld1, kSnInStart);
        }
    }
    public void InterpolateNode()
    {
        if (IsStateNEqualZero())
        {

        }
        if (IsStateNEqual05())
        {

        }
    }
    public void CalculateProjectileNode()
    {
        if (IsStateNEqualZero())
        {

        }
        if (IsStateNEqual05())
        {

        }
    }
    public void GoToNextLayer()
    {
        EnsureAllNodesSolved();

        UpdateTimeState();

        UpdateTypeParameterAndN();

        ResetForNextLayer();
    }
    #region GoToNextLayerMethods
    private void EnsureAllNodesSolved()
    {
        if (_typeParameterState == TypeParameterState.Dynamic && !HasNodeFlag(LayerFlag.Boundary | LayerFlag.Projectile | LayerFlag.Calculated | LayerFlag.Interpolated))
            throw new Exception($"moving to a new layer is not possible because the nodes have not been solved {GetNotSolvedFlags()}");
        if (_typeParameterState == TypeParameterState.Mixture && !HasNodeFlag(LayerFlag.Projectile | LayerFlag.Calculated | LayerFlag.Interpolated))
            throw new Exception($"moving to a new layer is not possible because the nodes have not been solved {GetNotSolvedFlags()}");
    }
    
    private void UpdateTimeState()
    {
        _timeState = _timeState switch
        {
            TimeState.Zero => TimeState.Half,
            TimeState.Half => TimeState.OnePlus,
            TimeState.OnePlus => TimeState.OnePlus, 
            _ => throw new ArgumentOutOfRangeException($"Unknown TimeState: {_timeState}")
        };

        //Console.WriteLine($"Time state changed to: {_timeState}");
    }

    private void UpdateTypeParameterAndN()
    {
        _typeParameterState = _typeParameterState == TypeParameterState.Mixture
            ? TypeParameterState.Dynamic
            : TypeParameterState.Mixture;

        n += new LimitedDouble(0.5);

        //Console.WriteLine($"TypeParameter changed to: {_typeParameterState}, n = {n.ToString()}");
    }

    private void ResetForNextLayer()
    {
        RemoveAllFlags();
    }

    #endregion
    public bool IsEndSolution()
    {
        if( _solveState == SolveState.No)
        {
            if (IsProjectileFlyOutOfCannon())
                _solveState = SolveState.Yes;
            else
                return false;
        }
            

        if(_solveState == SolveState.Yes)
        {
            InterpolateNodesToMomentFlyOut();
            return true;
        }
            

        throw new Exception($"unknown state of SolveState {_solveState}");
    }

    private string GetNotSolvedFlags()
    {
        var notSolved = new List<string>();

        if (!HasNodeFlag(LayerFlag.Calculated))
            notSolved.Add("Calculated");
        if (!HasNodeFlag(LayerFlag.Projectile))
            notSolved.Add("Projectile");
        if (!HasNodeFlag(LayerFlag.Interpolated))
            notSolved.Add("Interpolated");
        if (!HasNodeFlag(LayerFlag.Boundary) && _typeParameterState == TypeParameterState.Dynamic)
            notSolved.Add("Boundary");

        return notSolved.Count > 0 ? string.Join(", ", notSolved) : "None";
    }

    private string GetCurrentFlags()
    {
        var flags = new List<string>();
        if (HasNodeFlag(LayerFlag.Calculated)) flags.Add("Calculated");
        if (HasNodeFlag(LayerFlag.Projectile)) flags.Add("Projectile");
        if (HasNodeFlag(LayerFlag.Interpolated)) flags.Add("Interpolated");
        if (HasNodeFlag(LayerFlag.Boundary)) flags.Add("Boundary");

        return flags.Count > 0 ? string.Join(", ", flags) : "None";
    }
    private bool IsStateNEqualZero()
    {
        return _timeState == TimeState.Zero &&
               _typeParameterState == TypeParameterState.Mixture &&
               _beltState == BeltState.Intact;
    }
    private bool IsStateNEqual05()
    {
        return _timeState == TimeState.Half &&
               _typeParameterState == TypeParameterState.Dynamic &&
               _beltState == BeltState.Intact;
    }

}
