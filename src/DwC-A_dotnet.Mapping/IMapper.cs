extern alias Core;

using Core.DwC_A;

namespace DwC_A.Mapping
{
    public interface IMapper<T> where T : new()
    {
        T MapRow(IRow row);
    }
}