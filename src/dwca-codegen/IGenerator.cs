namespace DwcaCodegen
{
    public interface IGenerator
    {
        void Generate(string archive, string @namespace, bool? pascalCase, bool? termAttribute, string output, string configName);
    }
}