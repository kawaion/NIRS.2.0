﻿using NIRS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Interfaces
{
    public interface IBarrel
    {
        List<Point2D> BendingPoints { get; }
        Point2D EndChamberPoint { get; }
        double Length { get; }
        IBarrelSize BarrelSize { get; }
    }
}
