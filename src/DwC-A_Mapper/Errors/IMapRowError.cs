using System.Collections.Generic;

namespace MapErrors
{
    public interface IMapRowError
    {
        int Index { get; }

        IEnumerable<IMapFieldError> FieldErrors { get; }

        string ToString();
    }
}