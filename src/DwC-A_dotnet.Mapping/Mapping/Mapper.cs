extern alias Core;

using Core.DwC_A;
using System;

namespace DwC_A.Mapping
{
    internal class Mapper<T> : IMapper<T> where T : new()
    {
        private readonly Action<T, IRow> rowMapFunc;

        public Mapper(Action<T, IRow> rowMapFunc)
        {
            this.rowMapFunc = rowMapFunc;
        }

        public T MapRow(IRow row)
        {
            T obj = new T();
            rowMapFunc?.Invoke(obj, row);
            return obj;
        }
    }
}
