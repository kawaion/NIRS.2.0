using Core.Domain.Common;
using Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Domain.Exceptions
{
    public class ValueOutOfRangeException : ValueObjectException
    {
        
        public ValueOutOfRangeException(string objectName, object value, object min, object max)
            : base($"{objectName} value '{value}' is out of range. Must be between {min} and {max}.") { }

        public ValueOutOfRangeException(string objectName, object value, ComparisonOperator comparisonDirection, object comparisonValue)
            : base($"{objectName} value '{value}' is out of range. Must be {GetComparisonText(comparisonDirection)} {comparisonValue}.") { }

        private static string GetComparisonText(ComparisonOperator comparisonDirection)
        {
            return comparisonDirection switch
            {
                ComparisonOperator.GreaterThanOrEqual => "greater than or equal to",
                ComparisonOperator.LessThanOrEqual => "less than or equal to",
                ComparisonOperator.LessThan => "less than to",
                ComparisonOperator.GreaterThan => "greater than to",
                ComparisonOperator.Equal => "equal to",
                ComparisonOperator.NotEqual => "not equal to",
                _ => throw new ArgumentOutOfRangeException(nameof(comparisonDirection))
            };
        }
    }
    
}
