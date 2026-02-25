using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Services;

static class OffseterNK
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (LimitedDouble n, LimitedDouble k) AppointAndOffset(LimitedDouble n, LimitedDouble offsetN, LimitedDouble k, LimitedDouble offsetK)
    {
        return (n - offsetN,
                k - offsetK);
    }
}
static class OffseterN
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LimitedDouble AppointAndOffset(LimitedDouble n, LimitedDouble offsetN)
    {
        return n - offsetN;
    }
}
