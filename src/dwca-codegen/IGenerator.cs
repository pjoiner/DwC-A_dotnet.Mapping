using DwC_A.Config;

namespace DwcaCodegen;

public interface IGenerator
{
    void Generate(string archive, string @namespace, bool pascalCase, TermAttributeType termAttribute, string output, bool mapMetho);
}