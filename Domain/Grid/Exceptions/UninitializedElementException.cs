using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Grid.Exceptions;

internal class UninitializedElementException : Exception
{
    public UninitializedElementException(string message) : base(message) { }
}
