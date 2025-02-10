extern alias Core;

using Core.DwC_A;
using System;

namespace DwC_A.Mapping
{
    internal class Mapper<T>(Action<T, IRow> rowMapFunc) : IMapper<T> where T : new()
    {
        private readonly Action<T, IRow> rowMapFunc = rowMapFunc;

        public T MapRow(IRow row)
        {
            T obj = new();
            rowMapFunc?.Invoke(obj, row);
            return obj;
        }
    }
}
