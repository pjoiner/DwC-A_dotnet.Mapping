using System;
using System.Runtime.Serialization;

namespace MapErrors
{
    public class MapFieldException : Exception
    {
        private readonly IMapFieldError fieldError;

        public IMapFieldError FieldError => fieldError;

        public MapFieldException(string message) : base(message)
        {
        }

        public MapFieldException(IMapFieldError fieldError) : base(fieldError.ToString())
        {
            this.fieldError = fieldError;
        }

        public MapFieldException(IMapFieldError fieldError, Exception innerException) 
            : base(fieldError.ToString(), innerException)
        {
            this.fieldError = fieldError;
        }

        public MapFieldException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MapFieldException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
