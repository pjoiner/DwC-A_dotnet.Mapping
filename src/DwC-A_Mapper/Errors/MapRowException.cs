using System;
using System.Runtime.Serialization;

namespace MapErrors
{
    public class MapRowException : Exception
    {
        private readonly IMapRowError rowError;

        public IMapRowError RowError => rowError;

        public MapRowException(string message) : base(message)
        {
        }

        public MapRowException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MapRowException(IMapRowError rowError) : base(rowError.ToString())
        {
            this.rowError = rowError;
        }

        public MapRowException(IMapRowError rowError, Exception innerException)
            : base(BuildMessage(rowError), innerException)
        {
            this.rowError = rowError;
        }

        protected MapRowException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string BuildMessage(IMapRowError rowError)
        {
            return $"{rowError.ToString()}. See inner exception for more details.";
        }
    }
}
