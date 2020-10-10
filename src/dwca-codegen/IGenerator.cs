namespace DwcaCodegen
{
    public interface IGenerator
    {
        void Generate(string archive, string @namespace, bool capitalize, string output, string configName);
    }
}