using Core.Domain.Limited_Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.Helper.Services;

static class OffseterK
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static LimitedDouble AppointAndOffset(LimitedDouble k, LimitedDouble offsetK)
    {
        return k - offsetK;
    }
}
