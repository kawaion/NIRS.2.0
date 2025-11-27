using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Exceptions
{
    [DebuggerDisplay("{Message}")]
    internal class DomainException : Exception
    {
        public string ErrorCode { get; }
        public string UserMessage { get; }

        public DomainException(string message)
            : base(message)
        {
            UserMessage = message;
        }

        public DomainException(string message, string errorCode)
            : base($"{errorCode}: {message}")
        {
            ErrorCode = errorCode;
            UserMessage = message;
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
            UserMessage = message;
        }

        public DomainException(string message, string errorCode, Exception innerException)
            : base($"{errorCode}: {message}", innerException)
        {
            ErrorCode = errorCode;
            UserMessage = message;
        }
    }
}
