using System;
using System.Runtime.Serialization;

namespace MapErrors
{
    public class MapFileException : Exception
    {
        private readonly IMapFileError fileError;

        public IMapFileError FileError => fileError;

        public MapFileException(IMapFileError fileError) : base(fileError.ToString())
        {
            this.fileError = fileError;
        }

        public MapFileException(IMapFileError fileError, Exception innerException) 
            : base(BuildMessage(fileError), innerException)
        {
        }

        public MapFileException(string message) : base(message)
        {
        }

        public MapFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MapFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string BuildMessage(IMapFileError fileError)
        {
            return $"{fileError.ToString()}. See inner exception for more details";
        }
    }
}
