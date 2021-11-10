extern alias Core;

using DwC_A.Mapping;
using System;
using Core.DwC_A;

namespace DwC_A
{
    public static class MapperFactory
    {
        public static IMapper<T> LambdaMapper<T>(Action<T, IRow> mapFunc) where T : new()
        {
            return new LambdaMapper<T>(mapFunc);
        }
    }
}
