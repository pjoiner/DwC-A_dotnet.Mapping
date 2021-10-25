using System.Collections.Generic;

namespace MapErrors
{
    public class MapFileError : IMapFileError
    {
        private readonly string fileName;
        private readonly IEnumerable<IMapRowError> rowErrors;

        public string FileName => fileName;

        public IEnumerable<IMapRowError> RowErrors => rowErrors;

        public MapFileError(string fileName, IEnumerable<IMapRowError> rowErrors)
        {
            this.fileName = fileName;
            this.rowErrors = rowErrors;
        }

        public override string ToString()
        {
            return $"Could not map file {fileName}";
        }
    }
}
