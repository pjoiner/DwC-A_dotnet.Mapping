extern alias Core;

using Core.DwC_A;
using System.Collections.Generic;

namespace DwC_A.Mapping
{
    public static class MapperExtensions
    {
        public static T Map<T>(this IRow row, IMapper<T> mapper) where T : class, new()
        {
            return mapper.MapRow(row);
        }

        public static IEnumerable<T> Map<T>(this IEnumerable<IRow> rows, IMapper<T> mapper) where T : class, new()
        {
            foreach(var row in rows)
            {
                yield return mapper.MapRow(row);
            }
        }

        public static IEnumerable<T> Map<T>(this IFileReader fileReader, IMapper<T> mapper) where T : class, new()
        {
            foreach(var row in fileReader.DataRows)
            {
                yield return mapper.MapRow(row);
            }
        }

    }
}
