using System.Collections.Generic;

namespace MapErrors
{
    public class MapRowError : IMapRowError
    {
        private readonly int index;
        private IEnumerable<IMapFieldError> fieldErrors;

        public int Index => index;

        public IEnumerable<IMapFieldError> FieldErrors => fieldErrors;

        public MapRowError(int index, IEnumerable<IMapFieldError> fieldErrors)
        {
            this.index = index;
            this.fieldErrors = fieldErrors;
        }

        public override string ToString()
        {
            return $"Could not map row {index}";
        }
    }
}
