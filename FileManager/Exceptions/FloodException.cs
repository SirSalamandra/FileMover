using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FileMover.FileManager.Exceptions
{
    public class FloodException : Exception
    {
        public FloodException()
        {
        }

        public FloodException(string message) : base(message)
        {
        }

        public FloodException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FloodException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
