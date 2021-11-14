extern alias Core;

using Core.DwC_A;
using System.Collections.Generic;

namespace DwC_A.Mapping
{
    public static class MapperExtensions
    {
        /// <summary>
        /// Map this row to an object of type T using a predefined mapper
        /// </summary>
        /// <typeparam name="T">Type to map to</typeparam>
        /// <param name="row">Current row</param>
        /// <param name="mapper">Predefined mapper. <seealso cref="Mapping.MapperFactory"/></param>
        /// <returns>Class of type T with properties mapped to fields in the current row</returns>
        public static T Map<T>(this IRow row, IMapper<T> mapper) where T : class, new()
        {
            return mapper.MapRow(row);
        }

        /// <summary>
        /// Enumerate over rows and return mapped classes of type T
        /// </summary>
        /// <typeparam name="T">Type to map to</typeparam>
        /// <param name="rows">Enumeration of IRow</param>
        /// <param name="mapper">Predefined mapper. <seealso cref="Mapping.MapperFactory"/></param>
        /// <returns>Enumeration of classes of type T with properties mapped</returns>
        public static IEnumerable<T> Map<T>(this IEnumerable<IRow> rows, IMapper<T> mapper) where T : class, new()
        {
            foreach(var row in rows)
            {
                yield return mapper.MapRow(row);
            }
        }

        /// <summary>
        /// Enumerte over the DataRows in the current file and map fields using a predefined mapper
        /// </summary>
        /// <typeparam name="T">Type to map to</typeparam>
        /// <param name="fileReader">Current file reader</param>
        /// <param name="mapper">Predefined mapper</param>
        /// <returns>Enumeration of mapped classes of type T</returns>
        public static IEnumerable<T> Map<T>(this IFileReader fileReader, IMapper<T> mapper) where T : class, new()
        {
            foreach(var row in fileReader.DataRows)
            {
                yield return mapper.MapRow(row);
            }
        }

    }
}
