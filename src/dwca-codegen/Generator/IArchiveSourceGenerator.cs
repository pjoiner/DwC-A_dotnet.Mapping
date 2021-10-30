using DwcaCodegen.Config;

namespace DwcaCodegen.Generator
{
    public interface IArchiveSourceGenerator
    {
        string[] GenerateSource(string fileName, IArchiveGeneratorConfiguration config);
    }
}