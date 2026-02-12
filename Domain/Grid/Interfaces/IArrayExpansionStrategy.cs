using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.Interfaces;

public interface IArrayExpansionStrategy
{
    (int newY, int newZ) CalculateNewDimensions(int currentY, int currentZ, int requiredY, int requiredZ);
}
