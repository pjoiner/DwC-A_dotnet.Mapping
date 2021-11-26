using DwC_A.Config;

namespace DwcaCodegen.Generator
{
    public interface IArchiveSourceGenerator
    {
        string[] GenerateSource(string fileName, IGeneratorConfiguration config);
    }
}