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

internal interface IGridProjectile
{
    double this[PNsn pn, LimitedDouble n] {  get; }
    void Set(PNsn pn, LimitedDouble n, double value);
    ISetter At(PNsn pn, LimitedDouble n, LimitedDouble k);


    public LimitedDouble LastIndexN(PN pn);
    public LimitedDouble LastIndexN();
}
