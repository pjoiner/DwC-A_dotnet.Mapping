extern alias Core;
using System;
using Core.DwC_A;

namespace DwC_A.Mapping
{
    public static class MapperFactory
    {
        public static IMapper<T> CreateMapper<T>(Action<T, IRow> mapFunc) where T : new()
        {
            return new Mapper<T>(mapFunc);
        }
    }
}
