using Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Exceptions
{
    public class InvalidValueObjectException : ValueObjectException
    {
        public InvalidValueObjectException(string message) : base(message) { }

        public static InvalidValueObjectException Create(string objectName, string property, string reason)
        {
            return new InvalidValueObjectException(
                $"Invalid {objectName}: {property} {reason}");
        }
    }
}
