using System.Collections.Generic;

namespace MapErrors
{
    public interface IMapFileError
    {
        string FileName { get; }

        IEnumerable<IMapRowError> RowErrors { get; }

        string ToString();
    }
}