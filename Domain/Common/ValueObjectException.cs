using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Common
{
    public abstract class ValueObjectException : Exception
    {
        protected ValueObjectException(string message) : base(message) { }
    }
}
