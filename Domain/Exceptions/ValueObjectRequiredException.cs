using Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Exceptions
{
    public class ValueObjectRequiredException : ValueObjectException
    {
        public ValueObjectRequiredException(string objectName)
            : base($"{objectName} is required and cannot be empty") { }
    }
}
