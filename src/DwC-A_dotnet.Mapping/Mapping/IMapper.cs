extern alias Core;

using Core.DwC_A;

namespace DwC_A.Mapping
{
    /// <summary>
    /// Mapper interface that may be used to map IRow to an instance of type T
    /// </summary>
    /// <typeparam name="T">Type to map row to</typeparam>
    public interface IMapper<T> where T : new()
    {
        /// <summary>
        /// Maps an IRow instance to an instance of type T
        /// </summary>
        /// <param name="row">An IRow</param>
        /// <returns>Type to map row to</returns>
        T MapRow(IRow row);
    }
}