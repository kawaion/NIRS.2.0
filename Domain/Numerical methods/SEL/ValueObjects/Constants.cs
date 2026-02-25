using Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Numerical_methods.SEL.ValueObjects;

internal class Constants : ValueObject
{
    public double lamda0 {  get; }

    private Constants(double lamda0)
    {
        this.lamda0 = lamda0;
    }
    public static Constants Create(double lamda0)
    {
        var instance = new Constants(lamda0);

        return instance;   
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return lamda0;
    }
}
