using Core.Domain.Enums;
using Core.Domain.Grid.Aggregates;
using Core.Domain.Grid.Exceptions;
using Core.Domain.Grid.ValueObjects;
using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.Interfaces;

internal interface IGrid
{
    double this[PN pn, LimitedDouble n, LimitedDouble k] {  get; }
    void Set(PN pn, LimitedDouble n, LimitedDouble k, double value);
    ISetter At(PN pn, LimitedDouble n, LimitedDouble k);

    LimitedDouble LastIndexK(PN pn, LimitedDouble n);
    LimitedDouble LastIndexK(LimitedDouble n);

    LimitedDouble LastIndexN(PN pn);
    LimitedDouble LastIndexN();

    IGridSn sn { get; set; }
}
